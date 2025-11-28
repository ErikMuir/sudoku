using System.Collections.Generic;
using MuirDev.ConsoleTools;
using Sudoku.Exceptions;
using Sudoku.Logic;

namespace Sudoku.Console;

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
            '1' => _save(puzzle),
            '2' => _analyze(puzzle),
            '3' => _solve(puzzle),
            '4' => _print(puzzle),
            '5' => _clear(puzzle),
            '0' => throw new ProgramExitException(),
            _ => throw new SudokuException("Invalid option"),
        };

        private static Puzzle _save(Puzzle puzzle)
        {
            FilePuzzle.Save(puzzle);
            return puzzle;
        }

        private static Puzzle _analyze(Puzzle puzzle)
        {
            AnalyzePuzzle.Run(puzzle);
            return puzzle;
        }

        private static Puzzle _solve(Puzzle puzzle) => SolvePuzzle.Run(puzzle);

        private static Puzzle _print(Puzzle puzzle)
        {
            PrintPuzzle.Run(puzzle);
            return puzzle;
        }

        private static Puzzle _clear(Puzzle puzzle)
        {
            _console.Warning("Puzzle has been cleared from memory!");
            return null;
    }
}
