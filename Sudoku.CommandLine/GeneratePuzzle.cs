using System.Collections.Generic;
using MuirDev.ConsoleTools;
using Sudoku.Generators;

namespace Sudoku.CommandLine
{
    public static class GeneratePuzzle
    {
        private static readonly FluentConsole _console = new();
        private static readonly Dictionary<char, string> _menuOptions = new()
        {
            { '1', "None" },
            { '2', "Horizontal" },
            { '3', "Vertical" },
            { '4', "Diagonal Up" },
            { '5', "Diagonal Down" },
            { '6', "Rotational" },
            { '7', "Random" },
            { '0', "Go back" },
        };
        private static readonly Menu _menu = new(_menuOptions, "Choose a symmetry:");
        
        public static Puzzle Run()
        {
            _console.LineFeed();
            SymmetryType symmetryType = _menu.Run() switch
            {
                '1' => SymmetryType.None,
                '2' => SymmetryType.Horizontal,
                '3' => SymmetryType.Vertical,
                '4' => SymmetryType.DiagonalUp,
                '5' => SymmetryType.DiagonalDown,
                '6' => SymmetryType.Rotational,
                '7' => SymmetryType.Random,
                '0' => throw new MenuExitException(),
                _ => throw new SudokuException("Invalid option"),
            };

            GeneratorPuzzle generator = GeneratorPuzzle.Generate(9, symmetryType);
            Puzzle puzzle = new Puzzle(generator);
            PrintPuzzle.Run(puzzle);
            _console.Success("Puzzle is now in memory.");
            return puzzle;
        }
    }
}
