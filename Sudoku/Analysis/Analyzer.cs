namespace Sudoku.Analysis;

public class Analyzer
{
    private readonly Puzzle _puzzle;
    private readonly Stopwatch _timer;

    public Analyzer(Puzzle puzzle)
    {
        _puzzle = new Puzzle(puzzle);
        _timer = new Stopwatch();
        _analyze();
    }

    public TimeSpan SolveDuration => _timer.Elapsed;
    public int SolveDepth { get; set; } = 0;
    public Level Level { get; set; } = Level.Uninitialized;
    public List<ConstraintLog> Logs { get; set; } = new();

    private void _analyze()
    {
        _timer.Start();
        _puzzle.ResetFilledCells();
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
        if (!_puzzle.IsSolved)
            Level = Level.Unsolvable;
    }

    private void _setLevel(Level level)
        => this.Level = level > this.Level ? level : this.Level;

    private bool _nakedSingle()
    {
        Cell cell = _puzzle.Cells
            .EmptyCells()
            .Where(x => x.Candidates.Count == 1)
            .FirstOrDefault();

        if (cell is null) return false;

        cell.Value = cell.Candidates[0];
        Logs.Add(new ConstraintLog(ConstraintType.NakedSingle, cell, (int)cell.Value));
        _puzzle.ReduceCandidates();
        _setLevel(Level.Easy);
        return true;
    }

    private bool _hiddenSingle()
    {
        foreach (Cell cell in _puzzle.Cells.EmptyCells())
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
                    _setLevel(Level.Easy);
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

    private bool _nakedSetUnitCheck(IEnumerable<Cell> unit, List<CandidateSet> sets)
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
                _setLevel(Level.Medium);
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

    private bool _hiddenSetUnitCheck(IEnumerable<Cell> unit, List<CandidateSet> sets)
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
                _setLevel(Level.Difficult);
                return true;
            }
        }

        return false;
    }

    private bool _pointingSet()
    {
        for (int i = 0; i < Puzzle.UnitSize; i++)
        {
            IEnumerable<Cell> box = _puzzle.GetBox(i);
            for (int candidate = 1; candidate < 10; candidate++)
            {
                List<Cell> matches = box.Where(cell => cell.Candidates.Contains(candidate)).ToList();
                if (matches.Count < 2) continue;

                int? col = matches.AllInSameCol() ? matches.First().Col : null as int?;
                int? row = matches.AllInSameRow() ? matches.First().Row : null as int?;
                if (col is null && row is null) continue;

                List<Action> actions = new();
                IEnumerable<Cell> unit = col is not null ? _puzzle.GetCol(col.Value) : _puzzle.GetRow(row.Value);
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
                    _setLevel(Level.Difficult);
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
            IEnumerable<Cell> col = _puzzle.GetCol(i);
            IEnumerable<Cell> row = _puzzle.GetRow(i);

            for (int candidate = 1; candidate < 10; candidate++)
            {
                if (reduce(col, candidate)) return true;
                if (reduce(row, candidate)) return true;
            }
        }

        return false;

        bool reduce(IEnumerable<Cell> unit, int candidate)
        {
            List<Cell> matches = unit.Where(x => x.Candidates.Contains(candidate)).ToList();

            if (matches.Count < 2 || !matches.AllInSameBox())
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
                _setLevel(Level.Difficult);
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
                IEnumerable<Cell> unit = isCol ? _puzzle.GetCol(iUnit) : _puzzle.GetRow(iUnit);
                Cell[] matches = unit.GetCandidateMatches(candidate).ToArray();
                if (matches.Length != 2) continue;

                for (int iTestUnit = iUnit + 1; iTestUnit < Puzzle.UnitSize; iTestUnit++)
                {
                    IEnumerable<Cell> testUnit = isCol ? _puzzle.GetCol(iTestUnit) : _puzzle.GetRow(iTestUnit);
                    Cell[] testMatches = testUnit.GetCandidateMatches(candidate).ToArray();
                    if (testMatches.Length != 2) continue;
                    if (isCol && matches[0].Row != testMatches[0].Row) continue;
                    if (isCol && matches[1].Row != testMatches[1].Row) continue;
                    if (!isCol && matches[0].Col != testMatches[0].Col) continue;
                    if (!isCol && matches[1].Col != testMatches[1].Col) continue;

                    List<Action> actions = new();
                    for (int iActionUnit = 0; iActionUnit < 2; iActionUnit++)
                    {
                        IEnumerable<Cell> actionUnit = isCol
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
                        _setLevel(Level.Extreme);
                        return true;
                    }
                }
            }

            return false;
        }
    }

    private bool _yWing()
    {
        foreach (Cell hinge in _puzzle.Cells.Where(x => x.Candidates.Count == 2))
        {
            IEnumerable<Cell> hingeCol = _puzzle.GetCol(hinge.Col);
            IEnumerable<Cell> hingeRow = _puzzle.GetRow(hinge.Row);
            IEnumerable<Cell> hingeBox = _puzzle.GetBox(hinge.Box);

            if (hingeAndWings(hinge, hingeCol, hingeRow)) return true;
            if (hingeAndWings(hinge, hingeBox, hingeCol)) return true;
            if (hingeAndWings(hinge, hingeBox, hingeRow)) return true;
        }

        return false;

        bool hingeAndWings(Cell hinge, IEnumerable<Cell> wing1Unit, IEnumerable<Cell> wing2Unit)
        {
            if (reduce(hinge, wing1Unit, wing2Unit, hinge.Candidates[0], hinge.Candidates[1]))
                return true;

            if (reduce(hinge, wing1Unit, wing2Unit, hinge.Candidates[1], hinge.Candidates[0]))
                return true;

            return false;
        }

        bool reduce(Cell hinge, IEnumerable<Cell> wing1Unit, IEnumerable<Cell> wing2Unit, int wing1Candidate, int wing2Candidate)
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
                        _setLevel(Level.Extreme);
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
