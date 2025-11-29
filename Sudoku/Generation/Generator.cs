namespace Sudoku.Generation;

public static class Generator
{
    private static readonly Random _rand = new();
    private static readonly ISymmetry[] _supportedSymmetries =
    [
        Horizontal.Symmetry,
        Vertical.Symmetry,
        DiagonalUp.Symmetry,
        DiagonalDown.Symmetry,
        RotationalTwoFold.Symmetry,
        RotationalFourFold.Symmetry,
    ];
    
    public static Puzzle Generate(GenerationOptions options)
    {
        Puzzle puzzle = null;
        var puzzleIterations = 0;

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        while (puzzle == null)
        {
            if (++puzzleIterations % 25 == 0)
                Console.WriteLine($"generation attempts: {puzzleIterations}");
            puzzle = PuzzleIteration(options);
        }

        stopwatch.Stop();
        Console.WriteLine($"Running time: {stopwatch.Elapsed:c}");

        return puzzle;
    }

    private static Puzzle PuzzleIteration(GenerationOptions options)
    {
        var maxClues = options?.MaxClues ?? 0;
        var symmetry = options?.Symmetry ?? _supportedSymmetries[_rand.Next(_supportedSymmetries.Length)];
        var metadata = new Metadata()
        {
            Source = "MuirDev.Sudoku",
            Symmetry = symmetry.Type,
        };
        var puzzle = new Puzzle(metadata);
        puzzle.FillCandidates();
        puzzle.ReduceCandidates();
        while (true)
        {
            var workingPuzzle = ApplySymmetry(puzzle, symmetry);
            if (workingPuzzle is null) return null;
            if (IsMaxClues(workingPuzzle, maxClues)) return null;
            var solutions = Solver.MultiSolve(workingPuzzle, 2);
            if (solutions.Count == 0) return null;
            if (solutions.Count == 1) return ValidatePuzzle(workingPuzzle, options);
            puzzle = workingPuzzle;
        }
    }

    private static Puzzle ApplySymmetry(Puzzle puzzle, ISymmetry symmetry)
    {
        var workingPuzzle = new Puzzle(puzzle);
        var randomEmptyCell = GetRandomEmptyCell(workingPuzzle);
        var reflections = symmetry.GetReflections(randomEmptyCell.Index);
        for (var i = 0; i < reflections.Length && workingPuzzle is not null; i++)
        {
            var cell = workingPuzzle.Cells[reflections[i]];
            var value = cell.Candidates[_rand.Next(cell.Candidates.Count)];
            workingPuzzle = PlaceValue(workingPuzzle, cell.Index, value);
        }
        return workingPuzzle;
    }

    private static Cell GetRandomEmptyCell(Puzzle puzzle)
    {
        var emptyCells = puzzle.Cells.EmptyCells().ToArray();
        return emptyCells[_rand.Next(emptyCells.Length)];
    }

    private static Puzzle PlaceValue(Puzzle input, int cellIndex, int value)
    {
        var puzzle = new Puzzle(input);
        var cell = puzzle.Cells[cellIndex];
        if (!cell.Candidates.Contains(value)) return null;
        cell.Value = value;
        var emptyPeers = puzzle.Peers(cell)
            .Where(cell => cell.Type == CellType.Empty)
            .ToArray();
        foreach (var peer in emptyPeers)
        {
            var newCandidateCount = peer.Candidates.Except([value]).Count();
            if (newCandidateCount == 0) return null;
            // TODO : is the next statement necessary? what does it do?
            if (newCandidateCount == 1 && peer.Candidates.Count > 1) return null;
            peer.RemoveCandidate(value);
        }
        return puzzle;
    }

    private static bool IsMaxClues(Puzzle puzzle, int maxClues)
        => maxClues > 0 && puzzle.Cells.FilledCells().Count() > maxClues;

    private static Puzzle ValidatePuzzle(Puzzle input, GenerationOptions options)
    {
        // clone puzzle
        var puzzle = new Puzzle(input);

        // convert filled cells into clues
        for (var i = 0; i < puzzle.Cells.Length; i++)
        {
            var cell = puzzle.Cells[i];
            if (cell.Value is null) continue;
            puzzle.Cells[i] = new Clue(cell.Row, cell.Col, (int)cell.Value);
        }

        // validate level
        var analyzer = new Analyzer(puzzle);
        if (options.Level > Level.Uninitialized && options.Level != analyzer.Level)
            return null;

        // update metadata
        puzzle.Metadata.Level = analyzer.Level;
        puzzle.Metadata.DatePublished = DateTime.Now;

        return puzzle;
    }
}
