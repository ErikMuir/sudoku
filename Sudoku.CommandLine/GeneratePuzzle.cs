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
            { '4', "Diagonal (up)" },
            { '5', "Diagonal (down)" },
            { '6', "Rotational (two-fold)" },
            { '7', "Rotational (four-fold)" },
            { '8', "Random" },
            { '0', "Go back" },
        };
        private static readonly Menu _menu = new(_menuOptions, "Choose a symmetry:");
        
        public static Puzzle Run()
        {
            _console.LineFeed();
            Symmetry symmetry = _menu.Run() switch
            {
                '1' => Symmetry.None,
                '2' => Symmetry.Horizontal,
                '3' => Symmetry.Vertical,
                '4' => Symmetry.DiagonalUp,
                '5' => Symmetry.DiagonalDown,
                '6' => Symmetry.RotationalTwoFold,
                '7' => Symmetry.RotationalFourFold,
                '8' => Symmetry.Random,
                '0' => throw new MenuExitException(),
                _ => throw new SudokuException("Invalid option"),
            };

            GeneratorPuzzle generator = GeneratorPuzzle.Generate(9, symmetry);
            Puzzle puzzle = new Puzzle(generator);
            PrintPuzzle.Run(puzzle);
            _console.Success("Puzzle is now in memory.");
            return puzzle;
        }
    }
}
