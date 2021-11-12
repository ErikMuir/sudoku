using MuirDev.ConsoleTools;
using Sudoku.Logic;

namespace Sudoku.Console
{
    public static class SolvePuzzle
    {
        private static readonly FluentConsole _console = new();

        public static Puzzle Run(Puzzle input)
        {
            Puzzle puzzle = new(input);
            puzzle = Solver.Solve(puzzle);
            string message = puzzle.IsSolved() ? "Puzzle was successfully solved!" : "Failed to solve puzzle!";
            LogType type = puzzle.IsSolved() ? LogType.Success : LogType.Failure;
            _console.Log(message, type);
            return puzzle.IsSolved() ? puzzle : input;
        }
    }
}
