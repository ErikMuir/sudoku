using System.Collections.Generic;
using MuirDev.ConsoleTools;
using Sudoku.Generation;

namespace Sudoku.Console
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
            SymmetryType symmetry = _menu.Run() switch
            {
                '1' => SymmetryType.None,
                '2' => SymmetryType.Horizontal,
                '3' => SymmetryType.Vertical,
                '4' => SymmetryType.DiagonalUp,
                '5' => SymmetryType.DiagonalDown,
                '6' => SymmetryType.RotationalTwoFold,
                '7' => SymmetryType.RotationalFourFold,
                '8' => SymmetryType.Random,
                '0' => throw new MenuExitException(),
                _ => throw new SudokuException("Invalid option"),
            };

            Puzzle puzzle = Generator.Generate(symmetry);
            PrintPuzzle.Run(puzzle);
            _console.Success("Puzzle is now in memory.");
            return puzzle;
        }
    }
}
