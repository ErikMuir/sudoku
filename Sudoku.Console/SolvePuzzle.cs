namespace Sudoku.Console;

public static class SolvePuzzle
{
    private static readonly FluentConsole _console = new();

    public static Puzzle Run(Puzzle input)
    {
        var puzzle = new Puzzle(input);
        puzzle = Solver.Solve(puzzle);
        var message = puzzle.IsSolved ? "Puzzle was successfully solved!" : "Failed to solve puzzle!";
        var type = puzzle.IsSolved ? LogType.Success : LogType.Failure;
        _console.Log(message, type);
        return puzzle.IsSolved ? puzzle : input;
    }
}
