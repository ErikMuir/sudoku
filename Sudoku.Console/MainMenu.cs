using System.Collections.Generic;
using MuirDev.ConsoleTools;
using Sudoku.Exceptions;
using Sudoku.Logic;

namespace Sudoku.Console;

    public static class MainMenu
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
        private static readonly Menu _menu = new(_options, "Main Menu");

        public static Puzzle Run() => _menu.Run() switch
        {
            '1' => _input(),
            '2' => _load(),
            '3' => _generate(),
            '4' => _debug(),
            '0' => throw new ProgramExitException(),
            _ => throw new SudokuException("Invalid option"),
        };

        private static Puzzle _input() => InputPuzzle.Run();

        private static Puzzle _load() => FilePuzzle.Load();

        private static Puzzle _generate() => GeneratePuzzle.Run();

        private static Puzzle _debug()
        {
            _console.Warning("Not implemented!");
            return null;
    }
}
