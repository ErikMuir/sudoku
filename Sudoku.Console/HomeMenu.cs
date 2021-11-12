using System.Collections.Generic;
using MuirDev.ConsoleTools;
using Sudoku.Logic;

namespace Sudoku.Console
{
    public static class HomeMenu
    {
        private static readonly FluentConsole _console = new();
        private static readonly Dictionary<char, string> _options = new()
        {
            { '1', "Input" },
            { '2', "Load" },
            { '3', "Generate" },
            // { '4', "** debug **" },
            { '0', "Quit" },
        };
        private static readonly Menu _menu = new(_options, "Home Menu");

        public static Puzzle Run() => _menu.Run() switch
        {
            '1' => Input(),
            '2' => Load(),
            '3' => Generate(),
            '4' => Debug(),
            '0' => throw new MenuExitException(),
            _ => throw new SudokuException("Invalid option"),
        };

        private static Puzzle Input() => InputPuzzle.Run();

        private static Puzzle Load() => FilePuzzle.Load();

        private static Puzzle Generate() => GeneratePuzzle.Run();

        private static Puzzle Debug()
        {
            _console.Warning("Not implemented!");
            return null;
        }
    }
}
