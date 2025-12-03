namespace Sudoku.Console;

public static class SolvePuzzle
{
    private static readonly FluentConsole _console = new();

    public static Puzzle Run(Puzzle input)
    {
        var puzzle = new Puzzle(input);
        puzzle = Solver.Solve(puzzle);
        var isSolved = puzzle?.IsSolved ?? false;
        var message = isSolved ? "Puzzle was successfully solved!" : "Failed to solve puzzle!";
        var type = isSolved ? LogType.Success : LogType.Failure;
        _console.Log(message, type);
        return isSolved ? puzzle ?? input : input;
    }
}
