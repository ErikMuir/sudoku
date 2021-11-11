using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Sudoku
{
    public class Analyzer
    {
        public Analyzer(Puzzle puzzle)
        {
            Puzzle = new Puzzle(puzzle);
            Timer = new Stopwatch();
            _initializeSets();
        }

        public Puzzle Puzzle { get; set; }
        public Stopwatch Timer { get; }
        public TimeSpan SolveDuration => Timer.Elapsed;
        public int SolveDepth { get; set; } = 0;
        public List<ConstraintLog> Logs { get; set; } = new();

        public List<CandidateSet> Doubles { get; set; } = new();
        public List<CandidateSet> Triples { get; set; } = new();
        public List<CandidateSet> Quadruples { get; set; } = new();

        public void Solve()
        {
            if (Puzzle.IsSolved()) return;

            Timer.Start();
            Puzzle.CalculateCandidates();

            bool isChanged;
            do
            {
                SolveDepth++;
                isChanged = false;
                if (!isChanged) isChanged = NakedSingle();
                if (!isChanged) isChanged = HiddenSingle();
                if (!isChanged) isChanged = NakedSet(Doubles);
                if (!isChanged) isChanged = NakedSet(Triples);
                if (!isChanged) isChanged = HiddenSet(Doubles);
                if (!isChanged) isChanged = HiddenSet(Triples);
                if (!isChanged) isChanged = NakedSet(Quadruples);
                if (!isChanged) isChanged = HiddenSet(Quadruples);
                if (!isChanged) isChanged = PointingSet();
                if (!isChanged) isChanged = BoxLineReduction();
                if (!isChanged) isChanged = XWing();
                if (!isChanged) isChanged = YWing();
            }
            while (isChanged);

            Timer.Stop();
        }

        public bool NakedSingle()
        {
            Cell cell = Puzzle.Cells
                .Where(x => !x.Value.HasValue)
                .Where(x => x.Candidates.Count == 1)
                .FirstOrDefault();

            if (cell is null) return false;

            cell.Value = cell.Candidates[0];
            Logs.Add(new ConstraintLog(ConstraintType.NakedSingle, cell, (int)cell.Value));
            Puzzle.ReduceCandidates();
            return true;
        }

        public bool HiddenSingle()
        {
            foreach (Cell cell in Puzzle.GetEmptyCells())
            {
                foreach (int candidate in cell.Candidates)
                {
                    if (
                        Puzzle.GetCol(cell.Col).IsCandidateUnique(candidate) ||
                        Puzzle.GetCol(cell.Row).IsCandidateUnique(candidate) ||
                        Puzzle.GetBox(cell.Box).IsCandidateUnique(candidate)
                        )
                    {
                        cell.Value = candidate;
                        Logs.Add(new ConstraintLog(ConstraintType.HiddenSingle, cell, candidate));
                        Puzzle.ReduceCandidates();
                        return true;
                    }
                }
            }
            return false;
        }

        public bool NakedSet(List<CandidateSet> sets)
        {
            for (int i = 0; i < Puzzle.UnitSize; i++)
            {
                if (NakedSetUnitCheck(Puzzle.GetCol(i), sets))
                    return true;
                if (NakedSetUnitCheck(Puzzle.GetRow(i), sets))
                    return true;
                if (NakedSetUnitCheck(Puzzle.GetBox(i), sets))
                    return true;
            }
            return false;
        }

        public bool NakedSetUnitCheck(Cell[] unit, List<CandidateSet> sets)
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

        public bool HiddenSet(List<CandidateSet> sets)
        {
            for (int i = 0; i < Puzzle.UnitSize; i++)
            {
                if (HiddenSetUnitCheck(Puzzle.GetCol(i), sets))
                    return true;
                if (HiddenSetUnitCheck(Puzzle.GetRow(i), sets))
                    return true;
                if (HiddenSetUnitCheck(Puzzle.GetBox(i), sets))
                    return true;
            }
            return false;
        }

        public bool HiddenSetUnitCheck(Cell[] unit, List<CandidateSet> sets)
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

        public bool PointingSet()
        {
            for (int i = 0; i < Puzzle.UnitSize; i++)
            {
                Cell[] box = Puzzle.GetBox(i);
                for (int candidate = 1; candidate < 10; candidate++)
                {
                    Cell[] matches = box.Where(cell => cell.Candidates.Contains(candidate)).ToArray();
                    if (matches.Length < 2) continue;

                    int? col = matches.AllInSameCol() ? matches.First().Col : null as int?;
                    int? row = matches.AllInSameRow() ? matches.First().Row : null as int?;
                    if (col is null && row is null) continue;

                    List<Action> actions = new();
                    Cell[] unit = col.HasValue ? Puzzle.GetCol(col.Value) : Puzzle.GetRow(row.Value);
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

        public bool BoxLineReduction()
        {
            for (int i = 0; i < Puzzle.UnitSize; i++)
            {
                Cell[] col = Puzzle.GetCol(i);
                Cell[] row = Puzzle.GetRow(i);

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
                Puzzle.GetBox(matches[0].Box)
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

        public bool XWing()
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
                    Cell[] unit = isCol ? Puzzle.GetCol(iUnit) : Puzzle.GetRow(iUnit);
                    Cell[] matches = unit.GetCandidateMatches(candidate);
                    if (matches.Length != 2) continue;

                    for (int iTestUnit = iUnit + 1; iTestUnit < Puzzle.UnitSize; iTestUnit++)
                    {
                        Cell[] testUnit = isCol ? Puzzle.GetCol(iTestUnit) : Puzzle.GetRow(iTestUnit);
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
                                ? Puzzle.GetRow(testMatches[iActionUnit].Row)
                                : Puzzle.GetCol(testMatches[iActionUnit].Col);
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

        public bool YWing()
        {
            for (int iCol = 0; iCol < Puzzle.UnitSize; iCol++)
            {
                for (int iRow = 0; iRow < Puzzle.UnitSize; iRow++)
                {
                    Cell hinge = Puzzle.GetCell(iRow, iCol);
                    if (hinge.Candidates.Count != 2) continue;

                    Cell[] hingeCol = Puzzle.GetCol(hinge.Col);
                    Cell[] hingeRow = Puzzle.GetRow(hinge.Row);
                    Cell[] hingeBox = Puzzle.GetBox(hinge.Box);

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
                        Puzzle.CommonPeers(wing1, wing2)
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

        public string Statistics()
        {
            StringBuilder sb = new();
            sb.AppendLine($"Is Solved: {Puzzle.IsSolved()}");
            sb.AppendLine($"Solve Duration (ms): {SolveDuration.Milliseconds}");
            sb.AppendLine($"Solve Depth: {SolveDepth}");
            sb.AppendLine("Constraint Actions:");
            List<ConstraintType> constraints = Logs
                .Select(x => x.Constraint)
                .Distinct()
                .OrderBy(x => x)
                .ToList();
            foreach (ConstraintType constraint in constraints)
            {
                int actionCount = Logs
                    .Where(x => x.Constraint == constraint)
                    .Select(x => x.Actions.Count())
                    .Aggregate((result, item) => result + item);
                sb.AppendLine($"  {constraint}: {actionCount}");
            }
            return sb.ToString();
        }

        private void _initializeSets()
        {
            for (int a = 0; a < Puzzle.UnitSize; a++)
            {
                for (int b = 1; b < Puzzle.UnitSize; b++)
                {
                    if (b <= a) continue;
                    Doubles.Add(new Double(a, b));
                    for (int c = 2; c < Puzzle.UnitSize; c++)
                    {
                        if (c <= a || c <= b) continue;
                        Triples.Add(new Triple(a, b, c));
                        for (int d = 3; d < Puzzle.UnitSize; d++)
                        {
                            if (d <= a || d <= b || d <= c) continue;
                            Quadruples.Add(new Quadruple(a, b, c, d));
                        }
                    }
                }
            }
        }
    }
}
