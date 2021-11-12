using MuirDev.ConsoleTools;
using Sudoku.Analysis;
using Sudoku.Logic;

namespace Sudoku.Console
{
    public static class AnalyzePuzzle
    {
        private static readonly FluentConsole _console = new();

        public static void Run(Puzzle puzzle)
        {
            Analyzer solver = new Analyzer(puzzle);
            solver.Solve();
            _console.LineFeed().Write(solver.Statistics());
        }
    }
}
