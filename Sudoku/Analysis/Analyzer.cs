namespace Sudoku.Analysis;

public class Analyzer
{
    private readonly Puzzle _puzzle;
    private readonly Stopwatch _timer;

    public Analyzer(Puzzle puzzle)
    {
        _puzzle = new Puzzle(puzzle);
        _timer = new Stopwatch();
        Analyze();
    }

    public TimeSpan SolveDuration => _timer.Elapsed;
    public int SolveDepth { get; set; } = 0;
    public Level Level { get; set; } = Level.Uninitialized;
    public List<ConstraintLog> Logs { get; set; } = [];

    private void SetLevel(Level level) => Level = (level > Level) ? level : Level;

    private void Analyze()
    {
        _timer.Start();
        _puzzle.ResetFilledCells();
        _puzzle.FillCandidates();
        _puzzle.ReduceCandidates();

        bool isChanged;
        do
        {
            SolveDepth++;
            isChanged = NakedSingle();
            if (!isChanged) isChanged = HiddenSingle();
            if (!isChanged) isChanged = NakedSet(CandidateSets.Doubles);
            if (!isChanged) isChanged = NakedSet(CandidateSets.Triples);
            if (!isChanged) isChanged = HiddenSet(CandidateSets.Doubles);
            if (!isChanged) isChanged = HiddenSet(CandidateSets.Triples);
            if (!isChanged) isChanged = NakedSet(CandidateSets.Quadruples);
            if (!isChanged) isChanged = HiddenSet(CandidateSets.Quadruples);
            if (!isChanged) isChanged = PointingSet();
            if (!isChanged) isChanged = BoxLineReduction();
            if (!isChanged) isChanged = XWing();
            if (!isChanged) isChanged = YWing();
        }
        while (isChanged);
        _timer.Stop();
        if (!_puzzle.IsSolved)
            Level = Level.Unsolvable;
    }

    #region Constraint Methods

    private bool NakedSingle()
    {
        var cell = _puzzle.Cells
            .EmptyCells()
            .Where(x => x.Candidates.Count == 1)
            .FirstOrDefault();

        if (cell == null) return false;

        cell.Value = cell.Candidates[0];
        Logs.Add(new ConstraintLog(ConstraintType.NakedSingle, cell, (int)cell.Value));
        _puzzle.ReduceCandidates();
        SetLevel(Level.Easy);
        return true;
    }

    private bool HiddenSingle()
    {
        foreach (var cell in _puzzle.Cells.EmptyCells())
        {
            foreach (var candidate in cell.Candidates)
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
                    SetLevel(Level.Easy);
                    return true;
                }
            }
        }
        return false;
    }

    private bool NakedSet(List<CandidateSet> sets)
    {
        for (var i = 0; i < Puzzle.UnitSize; i++)
        {
            if (NakedSetUnitCheck(_puzzle.GetCol(i), sets))
                return true;
            if (NakedSetUnitCheck(_puzzle.GetRow(i), sets))
                return true;
            if (NakedSetUnitCheck(_puzzle.GetBox(i), sets))
                return true;
        }
        return false;
    }

    private bool NakedSetUnitCheck(IEnumerable<Cell> unit, List<CandidateSet> sets)
    {
        var setLength = sets.First().Count;

        foreach (var set in sets)
        {
            var matches = unit.Where(x => x.ContainsOnlyMatches(set)).ToList();

            if (matches.Count != setLength) continue;

            var actions = new List<Action>();
            var nonMatches = unit.Where(x => !matches.Any(y => y.Col == x.Col && y.Row == x.Row));
            foreach (var cell in nonMatches)
            {
                foreach (var candidate in set)
                {
                    if (cell.Candidates.Contains(candidate))
                    {
                        cell.RemoveCandidate(candidate);
                        actions.Add(Action.RemoveCandidate(cell, candidate));
                    }
                }
            }

            if (actions.Count > 0)
            {
                var constraint = setLength switch
                {
                    2 => ConstraintType.NakedDouble,
                    3 => ConstraintType.NakedTriple,
                    4 => ConstraintType.NakedQuadruple,
                    _ => throw new SudokuException($"Unsupported naked set length: {setLength}."),
                };
                Logs.Add(new ConstraintLog(constraint, actions));
                SetLevel(Level.Medium);
                return true;
            }
        }

        return false;
    }

    private bool HiddenSet(List<CandidateSet> sets)
    {
        for (var i = 0; i < Puzzle.UnitSize; i++)
        {
            if (HiddenSetUnitCheck(_puzzle.GetCol(i), sets))
                return true;
            if (HiddenSetUnitCheck(_puzzle.GetRow(i), sets))
                return true;
            if (HiddenSetUnitCheck(_puzzle.GetBox(i), sets))
                return true;
        }
        return false;
    }

    private bool HiddenSetUnitCheck(IEnumerable<Cell> unit, List<CandidateSet> sets)
    {
        var setLength = sets.First().Count;

        foreach (var set in sets)
        {
            var matches = unit.Where(x => x.ContainsAtLeastOneMatch(set)).ToArray();
            if (matches.Length != setLength || !matches.ContainsEveryCandidate(set))
                continue;

            var actions = new List<Action>();
            foreach (var cell in matches)
            {
                foreach (var candidate in cell.Candidates)
                {
                    if (!set.Contains(candidate))
                    {
                        cell.RemoveCandidate(candidate);
                        actions.Add(Action.RemoveCandidate(cell, candidate));
                    }
                }
            }

            if (actions.Count > 0)
            {
                var constraint = setLength switch
                {
                    2 => ConstraintType.HiddenDouble,
                    3 => ConstraintType.HiddenTriple,
                    4 => ConstraintType.HiddenQuadruple,
                    _ => throw new SudokuException($"Unsupported hidden set length: {setLength}."),
                };
                Logs.Add(new ConstraintLog(constraint, actions));
                SetLevel(Level.Difficult);
                return true;
            }
        }

        return false;
    }
    
    private bool PointingSet()
    {
        for (var i = 0; i < Puzzle.UnitSize; i++)
        {
            var box = _puzzle.GetBox(i);
            for (var candidate = 1; candidate < 10; candidate++)
            {
                var matches = box.Where(cell => cell.Candidates.Contains(candidate)).ToList();
                if (matches.Count < 2) continue;

                var col = matches.AllInSameCol() ? matches.First().Col : null as int?;
                var row = matches.AllInSameRow() ? matches.First().Row : null as int?;
                if (col == null && row == null) continue;

                var actions = new List<Action>();
                var unit = col != null ? _puzzle.GetCol(col.Value) : _puzzle.GetRow(row!.Value);
                unit.Where(cell => cell.Candidates.Contains(candidate))
                    .Where(cell => cell.Box != i)
                    .ToList()
                    .ForEach(cell =>
                    {
                        cell.RemoveCandidate(candidate);
                        actions.Add(Action.RemoveCandidate(cell, candidate));
                    });

                if (actions.Count > 0)
                {
                    Logs.Add(new ConstraintLog(ConstraintType.PointingSet, actions));
                    SetLevel(Level.Difficult);
                    return true;
                }
            }
        }

        return false;
    }

    private bool BoxLineReduction()
    {
        for (var i = 0; i < Puzzle.UnitSize; i++)
        {
            var col = _puzzle.GetCol(i);
            var row = _puzzle.GetRow(i);

            for (var candidate = 1; candidate < 10; candidate++)
            {
                if (reduce(col, candidate)) return true;
                if (reduce(row, candidate)) return true;
            }
        }

        return false;

        bool reduce(IEnumerable<Cell> unit, int candidate)
        {
            var matches = unit.Where(x => x.Candidates.Contains(candidate)).ToList();

            if (matches.Count < 2 || !matches.AllInSameBox())
                return false;

            var actions = new List<Action>();
            _puzzle.GetBox(matches[0].Box)
                .Where(cell => cell.Candidates.Contains(candidate))
                .Where(cell => !matches.Contains(cell))
                .ToList()
                .ForEach(cell =>
                {
                    cell.RemoveCandidate(candidate);
                    actions.Add(Action.RemoveCandidate(cell, candidate));
                });

            if (actions.Count > 0)
            {
                Logs.Add(new ConstraintLog(ConstraintType.BoxLineReduction, actions));
                SetLevel(Level.Difficult);
                return true;
            }

            return false;
        }
    }

    private bool XWing()
    {
        for (var candidate = 1; candidate < 10; candidate++)
        {
            if (reduce(candidate, true)) return true;
            if (reduce(candidate, false)) return true;
        }

        return false;

        bool reduce(int candidate, bool isCol)
        {
            for (var iUnit = 0; iUnit < Puzzle.UnitSize; iUnit++)
            {
                var unit = isCol ? _puzzle.GetCol(iUnit) : _puzzle.GetRow(iUnit);
                var matches = unit.GetCandidateMatches(candidate).ToArray();
                if (matches.Length != 2) continue;

                for (var iTestUnit = iUnit + 1; iTestUnit < Puzzle.UnitSize; iTestUnit++)
                {
                    var testUnit = isCol ? _puzzle.GetCol(iTestUnit) : _puzzle.GetRow(iTestUnit);
                    var testMatches = testUnit.GetCandidateMatches(candidate).ToArray();
                    if (testMatches.Length != 2) continue;
                    if (isCol && matches[0].Row != testMatches[0].Row) continue;
                    if (isCol && matches[1].Row != testMatches[1].Row) continue;
                    if (!isCol && matches[0].Col != testMatches[0].Col) continue;
                    if (!isCol && matches[1].Col != testMatches[1].Col) continue;

                    var actions = new List<Action>();
                    for (var iActionUnit = 0; iActionUnit < 2; iActionUnit++)
                    {
                        var actionUnit = isCol
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

                    if (actions.Count > 0)
                    {
                        Logs.Add(new ConstraintLog(ConstraintType.XWing, actions));
                        SetLevel(Level.Extreme);
                        return true;
                    }
                }
            }

            return false;
        }
    }

    private bool YWing()
    {
        foreach (var hinge in _puzzle.Cells.Where(x => x.Candidates.Count == 2))
        {
            var hingeCol = _puzzle.GetCol(hinge.Col);
            var hingeRow = _puzzle.GetRow(hinge.Row);
            var hingeBox = _puzzle.GetBox(hinge.Box);
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
            foreach (var wing1 in wing1Unit
                    .Where(x => x.Candidates.Count == 2)
                    .Where(x => x.Candidates.Contains(wing1Candidate))
                    .Where(x => !x.Candidates.Contains(wing2Candidate)))
            {
                int confluence = wing1.GetNonMatchingCandidates(hinge.Candidates)[0];
                foreach (var wing2 in wing2Unit.Where(x =>
                    x.Candidates.Count != 2 ||
                    !x.Candidates.Contains(wing2Candidate) ||
                    !x.Candidates.Contains(confluence)))
                {
                    var actions = new List<Action>();
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

                    if (actions.Count > 0)
                    {
                        Logs.Add(new ConstraintLog(ConstraintType.YWing, actions));
                        SetLevel(Level.Extreme);
                        return true;
                    }
                }
            }

            return false;
        }
    }

    #endregion
}
