using System.Collections.Generic;
using MuirDev.ConsoleTools;

namespace Sudoku.Console
{
    public static class PuzzleMenu
    {
        private static readonly FluentConsole _console = new();
        private static readonly Dictionary<char, string> _options = new()
        {
            { '1', "Save" },
            { '2', "Analyze" },
            { '3', "Solve" },
            { '4', "Print" },
            { '5', "Clear" },
            { '0', "Quit" },
        };
        private static readonly Menu _menu = new(_options, "Puzzle Menu");

        public static Puzzle Run(Puzzle puzzle) => _menu.Run() switch
        {
            '1' => Save(puzzle),
            '2' => Analyze(puzzle),
            '3' => Solve(puzzle),
            '4' => Print(puzzle),
            '5' => Clear(puzzle),
            '0' => throw new MenuExitException(),
            _ => throw new SudokuException("Invalid option"),
        };

        private static Puzzle Save(Puzzle puzzle)
        {
            FilePuzzle.Save(puzzle);
            return puzzle;
        }

        private static Puzzle Analyze(Puzzle puzzle)
        {
            AnalyzePuzzle.Run(puzzle);
            return puzzle;
        }

        private static Puzzle Solve(Puzzle puzzle) => SolvePuzzle.Run(puzzle);

        private static Puzzle Print(Puzzle puzzle)
        {
            PrintPuzzle.Run(puzzle);
            return puzzle;
        }

        private static Puzzle Clear(Puzzle puzzle)
        {
            _console.Warning("Puzzle has been cleared from memory!");
            return null;
        }
    }
}
