using System.Collections.Generic;
using MuirDev.ConsoleTools;
using Sudoku.Exceptions;
using Sudoku.Generation;
using Sudoku.Logic;

namespace Sudoku.Console;

    public static class GeneratePuzzle
    {
        private static readonly FluentConsole _console = new();
        private static readonly Dictionary<char, string> _symmetryMenuOptions = new()
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
        private static readonly Dictionary<char, string> _levelMenuOptions = new()
        {
            { '1', "Any" },
            { '2', "Easy" },
            { '3', "Medium" },
            { '4', "Difficult" },
            { '5', "Extreme" },
            { '0', "Go back" },
        };
        private static readonly Menu _symmetryMenu = new(_symmetryMenuOptions, "Choose a symmetry:");
        private static readonly Menu _levelMenu = new(_levelMenuOptions, "Choose a level:");

        public static Puzzle Run()
        {
            GenerationOptions options = new()
            {
                Level = _getLevel(),
                Symmetry = _getSymmetry(),
                MaxClues = _getMaxClues(),
            };
            Puzzle puzzle = Generator.Generate(options);
            PrintPuzzle.Run(puzzle);
            _console.Success("Puzzle is now in memory.");
            return puzzle;
        }

        private static Level _getLevel()
        {
            _console.LineFeed();
            return _levelMenu.Run() switch
            {
                '1' => Level.Uninitialized,
                '2' => Level.Easy,
                '3' => Level.Medium,
                '4' => Level.Difficult,
                '5' => Level.Extreme,
                '0' => throw new MenuExitException(),
                _ => throw new SudokuException("Invalid option"),
            };
        }

        private static ISymmetry _getSymmetry()
        {
            _console.LineFeed();
            return _symmetryMenu.Run() switch
            {
                '1' => Asymmetric.Symmetry,
                '2' => Horizontal.Symmetry,
                '3' => Vertical.Symmetry,
                '4' => DiagonalUp.Symmetry,
                '5' => DiagonalDown.Symmetry,
                '6' => RotationalTwoFold.Symmetry,
                '7' => RotationalFourFold.Symmetry,
                '8' => null,
                '0' => throw new MenuExitException(),
                _ => throw new SudokuException("Invalid option"),
            };
        }

        private static int _getMaxClues()
        {
            string input = _console
                .LineFeed()
                .Write("Max Clues (default none): ")
                .ReadLine();
            int.TryParse(input, out int maxClues);
            return maxClues;
    }
}
