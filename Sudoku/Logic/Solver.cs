namespace Sudoku.Logic;

public static class Solver
{
    private static int _iterationCount = 0;

    public static Puzzle? Solve(Puzzle input)
    {
        _iterationCount = 0;
        input.FillCandidates();
        input.ReduceCandidates();
        return DoMultiSolve(input);
    }

    public static List<Puzzle> MultiSolve(Puzzle input, int maxSolutions = -1)
    {
        _iterationCount = 0;
        input.FillCandidates();
        input.ReduceCandidates();
        var solutions = new List<Puzzle>();
        DoMultiSolve(input, p =>
        {
            _iterationCount = 0;
            solutions.Add(p);
            return solutions.Count < maxSolutions || maxSolutions == -1;
        });
        return solutions;
    }

    private static Puzzle? DoSolve(Puzzle input)
    {
        if (input.IsSolved) return input;
        if (++_iterationCount >= 1000) return null;
        var activeCell = FindWorkingCell(input);
        if (activeCell == null) return null;
        foreach (var guess in activeCell.Candidates)
        {
            Puzzle? puzzle;
            if ((puzzle = PlaceValue(input, activeCell.Index, guess)) != null)
                if ((puzzle = DoSolve(puzzle)) != null)
                    return puzzle;
        }
        return null;
    }

    private static Puzzle? DoMultiSolve(Puzzle input, Func<Puzzle, bool>? solutionFunc = null)
    {
        if (input.IsSolved)
            return (solutionFunc != null && solutionFunc(input)) ? null : input;
        if (++_iterationCount >= 1000) return null;
        var activeCell = FindWorkingCell(input);
        if (activeCell == null) return null;
        foreach (var guess in activeCell.Candidates)
        {
            Puzzle? puzzle;
            if ((puzzle = PlaceValue(input, activeCell.Index, guess)) != null)
                if ((puzzle = DoMultiSolve(puzzle, solutionFunc)) != null)
                    return puzzle;
        }
        return null;
    }

    private static Puzzle? PlaceValue(Puzzle input, int cellIndex, int value)
    {
        var puzzle = new Puzzle(input);
        var cell = puzzle.Cells[cellIndex];
        if (!cell.Candidates.Contains(value))
            return null;
        cell.Value = value;
        var cellsToPlace = new Dictionary<int, int>();
        foreach (var peer in puzzle.Peers(cell))
        {
            if (peer.Value != null)
                continue;
            var newCandidates = new SortedSet<int>(peer.Candidates.Except([value]));
            if (newCandidates.Count == 0)
                return null;
            if (newCandidates.Count == 1 && peer.Candidates.Count > 1)
                cellsToPlace.Add(peer.Index, newCandidates.Single());
            peer.RemoveCandidate(value);
        }
        foreach (var pair in cellsToPlace)
        {
            if ((puzzle = PlaceValue(puzzle, pair.Key, pair.Value)) == null)
                return null;
        }
        return puzzle;
    }

    private static Cell? FindWorkingCell(Puzzle puzzle)
    {
        return puzzle.Cells
            .EmptyCells()
            .OrderBy(cell => cell.Candidates.Count)
            .FirstOrDefault();
    }
}
