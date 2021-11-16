using System.Collections.Generic;
using MuirDev.ConsoleTools;
using Sudoku.Generation;
using Sudoku.Logic;

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
            Puzzle puzzle = _menu.Run() switch
            {
                '1' => Generator.Generate(),
                '2' => Generator.Generate(Horizontal.Symmetry),
                '3' => Generator.Generate(Vertical.Symmetry),
                '4' => Generator.Generate(DiagonalUp.Symmetry),
                '5' => Generator.Generate(DiagonalDown.Symmetry),
                '6' => Generator.Generate(RotationalTwoFold.Symmetry),
                '7' => Generator.Generate(RotationalFourFold.Symmetry),
                '8' => Generator.GenerateRandomSymmetry(),
                '0' => throw new MenuExitException(),
                _ => throw new SudokuException("Invalid option"),
            };
            PrintPuzzle.Run(puzzle);
            _console.Success("Puzzle is now in memory.");
            return puzzle;
        }
    }
}
