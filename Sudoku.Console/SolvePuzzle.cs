using MuirDev.ConsoleTools;

namespace Sudoku.CommandLine
{
    public static class SolvePuzzle
    {
        private static readonly FluentConsole _console = new();

        public static void Run(Puzzle puzzle)
        {
            Puzzle solution = Solver.Solve(puzzle);
            PrintPuzzle.Run(solution);
            if (solution is null)
                _console.Failure("Failed to solve puzzle!");
            else
                _console.Success("Puzzle was successfully solved!");
        }
    }
}
