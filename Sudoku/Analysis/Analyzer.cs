using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Sudoku.Extensions;
using Sudoku.Logic;

namespace Sudoku.Analysis
{
    public class Analyzer
    {
        private readonly Puzzle _puzzle;
        private readonly Stopwatch _timer;

        public Analyzer(Puzzle puzzle)
        {
            _puzzle = new Puzzle(puzzle);
            _timer = new Stopwatch();
        }

        public bool IsSolved => _puzzle.IsSolved;
        public TimeSpan SolveDuration => _timer.Elapsed;
        public int SolveDepth { get; set; } = 0;
        public List<ConstraintLog> Logs { get; set; } = new();

        public void Analyze()
        {
            if (_puzzle.IsSolved) return;

            _timer.Start();
            _puzzle.FillCandidates();
            _puzzle.ReduceCandidates();

            bool isChanged;
            do
            {
                SolveDepth++;
                isChanged = _nakedSingle();
                if (!isChanged) isChanged = _hiddenSingle();
                if (!isChanged) isChanged = _nakedSet(CandidateSets.Doubles);
                if (!isChanged) isChanged = _nakedSet(CandidateSets.Triples);
                if (!isChanged) isChanged = _hiddenSet(CandidateSets.Doubles);
                if (!isChanged) isChanged = _hiddenSet(CandidateSets.Triples);
                if (!isChanged) isChanged = _nakedSet(CandidateSets.Quadruples);
                if (!isChanged) isChanged = _hiddenSet(CandidateSets.Quadruples);
                if (!isChanged) isChanged = _pointingSet();
                if (!isChanged) isChanged = _boxLineReduction();
                if (!isChanged) isChanged = _xWing();
                if (!isChanged) isChanged = _yWing();
            }
            while (isChanged);

            _timer.Stop();
        }

        private bool _nakedSingle()
        {
            Cell cell = _puzzle.EmptyCells
                .Where(x => x.Candidates.Count == 1)
                .FirstOrDefault();

            if (cell is null) return false;

            cell.Value = cell.Candidates[0];
            Logs.Add(new ConstraintLog(ConstraintType.NakedSingle, cell, (int)cell.Value));
            _puzzle.ReduceCandidates();
            return true;
        }

        private bool _hiddenSingle()
        {
            foreach (Cell cell in _puzzle.EmptyCells)
            {
                foreach (int candidate in cell.Candidates)
                {
                    if (
                        _puzzle.GetCol(cell.Col).IsCandidateUnique(candidate) ||
                        _puzzle.GetCol(cell.Row).IsCandidateUnique(candidate) ||
                        _puzzle.GetBox(cell.Box).IsCandidateUnique(candidate)
                        )
                    {
                        cell.Value = candidate;
                        Logs.Add(new ConstraintLog(ConstraintType.HiddenSingle, cell, candidate));
                        _puzzle.ReduceCandidates();
                        return true;
                    }
                }
            }
            return false;
        }

        private bool _nakedSet(List<CandidateSet> sets)
        {
            for (int i = 0; i < Puzzle.UnitSize; i++)
            {
                if (_nakedSetUnitCheck(_puzzle.GetCol(i), sets))
                    return true;
                if (_nakedSetUnitCheck(_puzzle.GetRow(i), sets))
                    return true;
                if (_nakedSetUnitCheck(_puzzle.GetBox(i), sets))
                    return true;
            }
            return false;
        }

        private bool _nakedSetUnitCheck(Cell[] unit, List<CandidateSet> sets)
        {
            int setLength = sets.First().Count;

            foreach (CandidateSet set in sets)
            {
                List<Cell> matches = unit.Where(x => x.ContainsOnlyMatches(set)).ToList();

                if (matches.Count != setLength) continue;

                List<Action> actions = new();
                IEnumerable<Cell> nonMatches = unit.Where(x => !matches.Any(y => y.Col == x.Col && y.Row == x.Row));
                foreach (Cell cell in nonMatches)
                {
                    foreach (int candidate in set)
                    {
                        if (cell.Candidates.Contains(candidate))
                        {
                            cell.RemoveCandidate(candidate);
                            actions.Add(Action.RemoveCandidate(cell, candidate));
                        }
                    }
                }

                if (actions.Any())
                {
                    ConstraintType constraint;
                    switch (setLength)
                    {
                        case 2: constraint = ConstraintType.NakedDouble; break;
                        case 3: constraint = ConstraintType.NakedTriple; break;
                        case 4: constraint = ConstraintType.NakedQuadruple; break;
                        default: throw new Exception();
                    }
                    Logs.Add(new ConstraintLog(constraint, actions));
                    return true;
                }
            }

            return false;
        }

        private bool _hiddenSet(List<CandidateSet> sets)
        {
            for (int i = 0; i < Puzzle.UnitSize; i++)
            {
                if (_hiddenSetUnitCheck(_puzzle.GetCol(i), sets))
                    return true;
                if (_hiddenSetUnitCheck(_puzzle.GetRow(i), sets))
                    return true;
                if (_hiddenSetUnitCheck(_puzzle.GetBox(i), sets))
                    return true;
            }
            return false;
        }

        private bool _hiddenSetUnitCheck(Cell[] unit, List<CandidateSet> sets)
        {
            int setLength = sets.First().Count;

            foreach (CandidateSet set in sets)
            {
                Cell[] matches = unit.Where(x => x.ContainsAtLeastOneMatch(set)).ToArray();

                if (matches.Length != setLength || !matches.ContainsEveryCandidate(set))
                    continue;

                List<Action> actions = new();
                foreach (Cell cell in matches)
                {
                    foreach (int candidate in cell.Candidates)
                    {
                        if (!set.Contains(candidate))
                        {
                            cell.RemoveCandidate(candidate);
                            actions.Add(Action.RemoveCandidate(cell, candidate));
                        }
                    }
                }

                if (actions.Any())
                {
                    ConstraintType constraint;
                    switch (setLength)
                    {
                        case 2: constraint = ConstraintType.HiddenDouble; break;
                        case 3: constraint = ConstraintType.HiddenTriple; break;
                        case 4: constraint = ConstraintType.HiddenQuadruple; break;
                        default: throw new Exception();
                    }
                    Logs.Add(new ConstraintLog(constraint, actions));
                    return true;
                }
            }

            return false;
        }

        private bool _pointingSet()
        {
            for (int i = 0; i < Puzzle.UnitSize; i++)
            {
                Cell[] box = _puzzle.GetBox(i);
                for (int candidate = 1; candidate < 10; candidate++)
                {
                    Cell[] matches = box.Where(cell => cell.Candidates.Contains(candidate)).ToArray();
                    if (matches.Length < 2) continue;

                    int? col = matches.AllInSameCol() ? matches.First().Col : null as int?;
                    int? row = matches.AllInSameRow() ? matches.First().Row : null as int?;
                    if (col is null && row is null) continue;

                    List<Action> actions = new();
                    Cell[] unit = col is not null ? _puzzle.GetCol(col.Value) : _puzzle.GetRow(row.Value);
                    unit.Where(cell => cell.Candidates.Contains(candidate))
                        .Where(cell => cell.Box != i)
                        .ToList()
                        .ForEach(cell =>
                        {
                            cell.RemoveCandidate(candidate);
                            actions.Add(Action.RemoveCandidate(cell, candidate));
                        });

                    if (actions.Any())
                    {
                        Logs.Add(new ConstraintLog(ConstraintType.PointingSet, actions));
                        return true;
                    }
                }
            }

            return false;
        }

        private bool _boxLineReduction()
        {
            for (int i = 0; i < Puzzle.UnitSize; i++)
            {
                Cell[] col = _puzzle.GetCol(i);
                Cell[] row = _puzzle.GetRow(i);

                for (int candidate = 1; candidate < 10; candidate++)
                {
                    if (reduce(col, candidate)) return true;
                    if (reduce(row, candidate)) return true;
                }
            }

            return false;

            bool reduce(Cell[] unit, int candidate)
            {
                Cell[] matches = unit.Where(x => x.Candidates.Contains(candidate)).ToArray();

                if (matches.Length < 2 || !matches.AllInSameBox())
                    return false;

                List<Action> actions = new();
                _puzzle.GetBox(matches[0].Box)
                    .Where(cell => cell.Candidates.Contains(candidate))
                    .Where(cell => !matches.Contains(cell))
                    .ToList()
                    .ForEach(cell =>
                    {
                        cell.RemoveCandidate(candidate);
                        actions.Add(Action.RemoveCandidate(cell, candidate));
                    });

                if (actions.Any())
                {
                    Logs.Add(new ConstraintLog(ConstraintType.BoxLineReduction, actions));
                    return true;
                }

                return false;
            }
        }

        private bool _xWing()
        {
            for (int candidate = 1; candidate < 10; candidate++)
            {
                if (reduce(candidate, true)) return true;
                if (reduce(candidate, false)) return true;
            }

            return false;

            bool reduce(int candidate, bool isCol)
            {
                for (int iUnit = 0; iUnit < Puzzle.UnitSize; iUnit++)
                {
                    Cell[] unit = isCol ? _puzzle.GetCol(iUnit) : _puzzle.GetRow(iUnit);
                    Cell[] matches = unit.GetCandidateMatches(candidate);
                    if (matches.Length != 2) continue;

                    for (int iTestUnit = iUnit + 1; iTestUnit < Puzzle.UnitSize; iTestUnit++)
                    {
                        Cell[] testUnit = isCol ? _puzzle.GetCol(iTestUnit) : _puzzle.GetRow(iTestUnit);
                        Cell[] testMatches = testUnit.GetCandidateMatches(candidate);
                        if (testMatches.Length != 2) continue;
                        if (isCol && matches[0].Row != testMatches[0].Row) continue;
                        if (isCol && matches[1].Row != testMatches[1].Row) continue;
                        if (!isCol && matches[0].Col != testMatches[0].Col) continue;
                        if (!isCol && matches[1].Col != testMatches[1].Col) continue;

                        List<Action> actions = new();
                        for (int iActionUnit = 0; iActionUnit < 2; iActionUnit++)
                        {
                            Cell[] actionUnit = isCol
                                ? _puzzle.GetRow(testMatches[iActionUnit].Row)
                                : _puzzle.GetCol(testMatches[iActionUnit].Col);
                            actionUnit
                                .Where(cell => cell.Candidates.Contains(candidate))
                                .Where(cell => cell != matches[iActionUnit])
                                .Where(cell => cell != testMatches[iActionUnit])
                                .ToList()
                                .ForEach(cell =>
                                {
                                    cell.RemoveCandidate(candidate);
                                    actions.Add(Action.RemoveCandidate(cell, candidate));
                                });
                        }

                        if (actions.Any())
                        {
                            Logs.Add(new ConstraintLog(ConstraintType.XWing, actions));
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        private bool _yWing()
        {
            for (int iCol = 0; iCol < Puzzle.UnitSize; iCol++)
            {
                for (int iRow = 0; iRow < Puzzle.UnitSize; iRow++)
                {
                    Cell hinge = _puzzle.GetCell(iRow, iCol);
                    if (hinge.Candidates.Count != 2) continue;

                    Cell[] hingeCol = _puzzle.GetCol(hinge.Col);
                    Cell[] hingeRow = _puzzle.GetRow(hinge.Row);
                    Cell[] hingeBox = _puzzle.GetBox(hinge.Box);

                    if (hingeAndWings(hinge, hingeCol, hingeRow)) return true;
                    if (hingeAndWings(hinge, hingeBox, hingeCol)) return true;
                    if (hingeAndWings(hinge, hingeBox, hingeRow)) return true;
                }
            }

            return false;

            bool hingeAndWings(Cell hinge, Cell[] wing1Unit, Cell[] wing2Unit)
            {
                if (reduce(hinge, wing1Unit, wing2Unit, hinge.Candidates[0], hinge.Candidates[1]))
                    return true;

                if (reduce(hinge, wing1Unit, wing2Unit, hinge.Candidates[1], hinge.Candidates[0]))
                    return true;

                return false;
            }

            bool reduce(Cell hinge, Cell[] wing1Unit, Cell[] wing2Unit, int wing1Candidate, int wing2Candidate)
            {
                foreach (Cell wing1 in wing1Unit
                        .Where(x => x.Candidates.Count == 2)
                        .Where(x => x.Candidates.Contains(wing1Candidate))
                        .Where(x => !x.Candidates.Contains(wing2Candidate)))
                {
                    int confluence = wing1.GetNonMatchingCandidates(hinge.Candidates)[0];
                    foreach (Cell wing2 in wing2Unit.Where(x =>
                        x.Candidates.Count != 2 ||
                        !x.Candidates.Contains(wing2Candidate) ||
                        !x.Candidates.Contains(confluence)))
                    {
                        List<Action> actions = new();
                        _puzzle.CommonPeers(wing1, wing2)
                            .Where(x => x.Candidates.Contains(confluence))
                            .Where(x => x != hinge)
                            .Where(x => x != wing1)
                            .Where(x => x != wing2)
                            .ToList()
                            .ForEach(x =>
                            {
                                x.RemoveCandidate(confluence);
                                actions.Add(Action.RemoveCandidate(x, confluence));
                            });

                        if (actions.Any())
                        {
                            Logs.Add(new ConstraintLog(ConstraintType.YWing, actions));
                            return true;
                        }
                    }
                }

                return false;
            }
        }
    }
}
