using System;
using System.Collections.Generic;
using System.Linq;
using Sudoku.Solution;

namespace Sudoku.Generation
{
    public static class Generator
    {
        private static readonly Random _rand = new();
        private static readonly Symmetry[] _supportedSymmetryTypes = new[]
        {
            Symmetry.Horizontal,
            Symmetry.Vertical,
            Symmetry.DiagonalUp,
            Symmetry.DiagonalDown,
            Symmetry.RotationalTwoFold,
            Symmetry.RotationalFourFold,
        };
        private static Symmetry GetRandomSymmetry() => _supportedSymmetryTypes[_rand.Next(_supportedSymmetryTypes.Length)];

        public static Puzzle Generate(int size)
        {
            Puzzle puzzle = new(size);

            while (true)
            {
                int[] unsolvedCellIndexes = GetUnsolvedCells(puzzle);
                int cellIndex = unsolvedCellIndexes[_rand.Next(unsolvedCellIndexes.Length)];
                int candidateValue = puzzle.Cells[cellIndex][_rand.Next(puzzle.Cells[cellIndex].Length)];
                Puzzle workingPuzzle = Puzzle.PlaceValue(puzzle, cellIndex, candidateValue, recursive: false);
                if (workingPuzzle is null) continue;
                List<Puzzle> solutions = Solver.MultiSolve(workingPuzzle, 2);
                switch (solutions.Count)
                {
                    case 0: continue;
                    case 1: return workingPuzzle;
                    default: puzzle = workingPuzzle; break;
                }
            }
        }

        public static Puzzle Generate(int size, Symmetry symmetry)
        {
            if (symmetry == Symmetry.None)
                return Generate(size);

            if (symmetry == Symmetry.Random)
                symmetry = GetRandomSymmetry();

            while (true)
            {
                Puzzle puzzle = new(size);
                int puzzleIterations = 0;

                while (puzzleIterations < 100)
                {
                    puzzleIterations++;
                    Puzzle workingPuzzle = new(puzzle);

                    int[] cellsToFill = GetCellsToFill(puzzle, symmetry);
                    for (int i = 0; i < cellsToFill.Length && workingPuzzle is not null; i++)
                    {
                        int cellIndex = cellsToFill[i];
                        int value = workingPuzzle.Cells[cellIndex][_rand.Next(workingPuzzle.Cells[cellIndex].Length)];
                        workingPuzzle = Puzzle.PlaceValue(workingPuzzle, cellIndex, value, recursive: false);
                    }

                    if (workingPuzzle is null) continue;

                    List<Puzzle> solutions = Solver.MultiSolve(workingPuzzle, 2);
                    switch (solutions.Count)
                    {
                        case 0: continue;
                        case 1: return workingPuzzle;
                        default: puzzle = workingPuzzle; break;
                    }
                }
            }
        }

        public static int[] GetCellsToFill(Puzzle puzzle, Symmetry symmetry)
        {
            int[] unsolvedCellIndexes = GetUnsolvedCells(puzzle);
            int cell = unsolvedCellIndexes[_rand.Next(unsolvedCellIndexes.Length)];
            int[] symmetricCells = symmetry switch
            {
                Symmetry.Horizontal => GetHorizontalReflection(puzzle, cell),
                Symmetry.Vertical => GetVerticalReflection(puzzle, cell),
                Symmetry.DiagonalUp => GetDiagonalUpReflection(puzzle, cell),
                Symmetry.DiagonalDown => GetDiagonalDownReflection(puzzle, cell),
                Symmetry.RotationalTwoFold => GetRotationalTwoFoldReflection(puzzle, cell),
                Symmetry.RotationalFourFold => GetRotationalFourFoldReflection(puzzle, cell),
                _ => new int[] { },
            };
            return symmetricCells.Concat(new[] { cell }).ToArray();
        }

        public static int[] GetUnsolvedCells(Puzzle puzzle)
            => puzzle.Cells
                .Select((cands, index) => new { cands, index })
                .Where(t => t.cands.Length >= 2)
                .Select(u => u.index)
                .ToArray();

        public static int GetReflectiveAxis(Puzzle puzzle)
            => puzzle.Length % 2 == 0 ? -1 : puzzle.Length / 2;

        public static int[] GetHorizontalReflection(Puzzle puzzle, int cell)
        {
            int row = puzzle.Row(cell);
            int col = puzzle.Col(cell);
            if (row == GetReflectiveAxis(puzzle))
                return new int[] { };
            int reflectedRow = (puzzle.Length - 1) - row;
            int reflectedIndex = (reflectedRow * puzzle.Length) + col;
            return new[] { reflectedIndex };
        }

        public static int[] GetVerticalReflection(Puzzle puzzle, int cell)
        {
            int row = puzzle.Row(cell);
            int col = puzzle.Col(cell);
            if (col == GetReflectiveAxis(puzzle))
                return new int[] { };
            int reflectedCol = (puzzle.Length - 1) - col;
            int reflectedIndex = (row * puzzle.Length) + reflectedCol;
            return new[] { reflectedIndex };
        }

        public static int[] GetDiagonalUpReflection(Puzzle puzzle, int cell)
        {
            int row = puzzle.Row(cell);
            int col = puzzle.Col(cell);
            if (row + col == puzzle.Length - 1)
                return new int[] { };
            int reflectedCol = (puzzle.Length - 1) - row;
            int reflectedRow = (puzzle.Length - 1) - col;
            int reflectedIndex = (reflectedRow * puzzle.Length) + reflectedCol;
            return new[] { reflectedIndex };
        }

        public static int[] GetDiagonalDownReflection(Puzzle puzzle, int cell)
        {
            int row = puzzle.Row(cell);
            int col = puzzle.Col(cell);
            if (row == col)
                return new int[] { };
            int reflectedCol = row;
            int reflectedRow = col;
            int reflectedIndex = (reflectedRow * puzzle.Length) + reflectedCol;
            return new[] { reflectedIndex };
        }

        public static int[] GetRotationalTwoFoldReflection(Puzzle puzzle, int cell)
        {
            int row = puzzle.Row(cell);
            int col = puzzle.Col(cell);
            int reflectiveAxis = GetReflectiveAxis(puzzle);
            if (row == reflectiveAxis && col == reflectiveAxis)
                return new int[] { };
            int reflectedRow = (puzzle.Length - 1) - row;
            int reflectedCol = (puzzle.Length - 1) - col;
            int reflectedIndex = (reflectedRow * puzzle.Length) + reflectedCol;
            return new[] { reflectedIndex };
        }

        public static int[] GetRotationalFourFoldReflection(Puzzle puzzle, int cell)
        {
            List<int> reflections = new();
            int row = puzzle.Row(cell);
            int col = puzzle.Col(cell);
            int reflectiveAxis = GetReflectiveAxis(puzzle);
            if (row == reflectiveAxis && col == reflectiveAxis)
                return new int[] { };
            int reflectedIndex = cell;
            for (int i = 0; i < 3; i++)
            {
                reflectedIndex = RotateCell(puzzle, reflectedIndex);
                reflections.Add(reflectedIndex);
            }
            return reflections.ToArray();
        }

        public static int RotateCell(Puzzle puzzle, int cell)
        {
            int row = puzzle.Row(cell);
            int col = puzzle.Col(cell);
            int targetRow = col;
            int targetCol = (puzzle.Length - 1) - row;
            int targetIndex = (targetRow * puzzle.Length) + targetCol;
            return targetIndex;
        }

        public static Puzzle RotatePuzzle(Puzzle input)
        {
            Puzzle puzzle = new(input);
            for (int i = 0; i < puzzle.Cells.Length; i++)
            {
                int targetIndex = RotateCell(puzzle, i);
                puzzle.Cells[targetIndex] = input.Cells[i];
            }
            return puzzle;
        }

        public static Puzzle RotatePuzzle(Puzzle input, int quarterTurns)
        {
            Puzzle puzzle = new(input);
            for (int i = 0; i < quarterTurns; i++)
                puzzle = RotatePuzzle(puzzle);
            return puzzle;
        }
    }
}
