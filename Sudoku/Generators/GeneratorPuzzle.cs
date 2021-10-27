using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku.Generators
{
    public class GeneratorPuzzle
    {
        public int Length;
        public int[][] Cells;
        public int BoxSize => (int)Math.Sqrt(Length);

        private static readonly Random _rand = new();
        private static readonly Dictionary<Tuple<int, int>, int[]> _savedPeers = new();
        private static readonly SymmetryType[] _supportedSymmetryTypes = new[]
        {
            SymmetryType.Horizontal,
            SymmetryType.Vertical,
            // SymmetryType.DiagonalUp,
            // SymmetryType.DiagonalDown,
            // SymmetryType.Rotational,
        };
        private static SymmetryType GetRandomSymmetry() => _supportedSymmetryTypes[_rand.Next(_supportedSymmetryTypes.Length)];

        public GeneratorPuzzle(int length)
        {
            Length = length;
            Cells = Enumerable.Repeat(Enumerable.Range(1, length).ToArray(), length * length).ToArray();
        }

        public GeneratorPuzzle(GeneratorPuzzle puzzle)
        {
            Length = puzzle.Length;
            Cells = new int[puzzle.Cells.Length][];
            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i] = new int[puzzle.Cells[i].Length];
                Buffer.BlockCopy(puzzle.Cells[i], 0, this.Cells[i], 0, puzzle.Cells[i].Length * sizeof(int));
            }
        }

        public int[] Peers(int cell)
        {
            Tuple<int, int> key = new(this.Length, cell);
            if (!_savedPeers.ContainsKey(key))
                _savedPeers.Add(key, Enumerable.Range(0, Length * Length).Where(c => IsPeer(cell, c)).ToArray());
            return _savedPeers[key];
        }

        private bool IsPeer(int c1, int c2) => c1 != c2 && (IsSameRow(c1, c2) || IsSameColumn(c1, c2) || IsSameBox(c1, c2));
        private bool IsSameRow(int c1, int c2) => c1 / Length == c2 / Length;
        private bool IsSameColumn(int c1, int c2) => c1 % Length == c2 % Length;
        private bool IsSameBox(int c1, int c2) => c1 / Length / BoxSize == c2 / Length / BoxSize && c1 % Length / BoxSize == c2 % Length / BoxSize;
        private int Row(int i) => i / Length;
        private int Col(int i) => i % Length;
        private int Box(int i) => ((Row(i) / BoxSize) * BoxSize) + (Col(i) / BoxSize);

        public static GeneratorPuzzle PlaceValue(GeneratorPuzzle input, int cellIndex, int value, bool recursive = true)
        {
            GeneratorPuzzle puzzle = new(input);

            if (!puzzle.Cells[cellIndex].Contains(value))
                return null;

            puzzle.Cells[cellIndex] = new int[] { value };

            Dictionary<int, int> cellsToPlace = new();
            foreach (int peerIndex in puzzle.Peers(cellIndex))
            {
                int[] newPeers = puzzle.Cells[peerIndex].Except(new int[] { value }).ToArray();

                if (!newPeers.Any())
                    return null;

                if (newPeers.Length == 1 && puzzle.Cells[peerIndex].Length > 1)
                {
                    if (recursive)
                        cellsToPlace.Add(peerIndex, newPeers.Single());
                    else
                        return null;
                }

                puzzle.Cells[peerIndex] = newPeers;
            }

            if (recursive)
            {
                foreach (KeyValuePair<int, int> cell in cellsToPlace)
                {
                    if ((puzzle = PlaceValue(puzzle, cell.Key, cell.Value)) is null)
                        return null;
                }
            }

            return puzzle;
        }

        public static int FindWorkingCell(GeneratorPuzzle puzzle)
        {
            int minCandidates = puzzle.Cells.Where(cands => cands.Length >= 2).Min(cands => cands.Length);
            return Array.FindIndex(puzzle.Cells, c => c.Length == minCandidates);
        }

        public static GeneratorPuzzle Solve(GeneratorPuzzle input, Func<GeneratorPuzzle, bool> solutionFunc = null)
        {
            if (input.Cells.All(cell => cell.Length == 1))
                return (solutionFunc != null && solutionFunc(input)) ? null : input;

            int activeCell = FindWorkingCell(input);
            foreach (int guess in input.Cells[activeCell])
            {
                GeneratorPuzzle puzzle;
                if ((puzzle = PlaceValue(input, activeCell, guess)) is not null)
                    if ((puzzle = Solve(puzzle, solutionFunc)) is not null)
                        return puzzle;
            }
            return null;
        }

        public static List<GeneratorPuzzle> MultiSolve(GeneratorPuzzle input, int maxSolutions = -1)
        {
            List<GeneratorPuzzle> solutions = new();
            Solve(input, p =>
            {
                solutions.Add(p);
                return solutions.Count() < maxSolutions || maxSolutions == -1;
            });
            return solutions;
        }

        public static GeneratorPuzzle Generate(int size)
        {
            GeneratorPuzzle puzzle = new(size);

            while (true)
            {
                int[] unsolvedCellIndexes = GetUnsolvedCells(puzzle);
                int cellIndex = unsolvedCellIndexes[_rand.Next(unsolvedCellIndexes.Length)];
                int candidateValue = puzzle.Cells[cellIndex][_rand.Next(puzzle.Cells[cellIndex].Length)];
                GeneratorPuzzle workingPuzzle = PlaceValue(puzzle, cellIndex, candidateValue, recursive: false);
                if (workingPuzzle is null) continue;
                List<GeneratorPuzzle> solutions = MultiSolve(workingPuzzle, 2);
                switch (solutions.Count)
                {
                    case 0: continue;
                    case 1: return workingPuzzle;
                    default: puzzle = workingPuzzle; break;
                }
            }
        }

        public static GeneratorPuzzle Generate(int size, SymmetryType symmetry)
        {
            if (symmetry == SymmetryType.None)
                return Generate(size);

            if (symmetry == SymmetryType.Random)
                symmetry = GetRandomSymmetry();

            while (true)
            {
                GeneratorPuzzle puzzle = new(size);
                int puzzleIterations = 0;

                while (puzzleIterations < 100)
                {
                    puzzleIterations++;
                    GeneratorPuzzle workingPuzzle = new(puzzle);

                    int[] cellsToFill = GetCellsToFill(puzzle, symmetry);
                    for (int i = 0; i < cellsToFill.Length && workingPuzzle is not null; i++)
                    {
                        int cellIndex = cellsToFill[i];
                        int value = workingPuzzle.Cells[cellIndex][_rand.Next(workingPuzzle.Cells[cellIndex].Length)];
                        workingPuzzle = PlaceValue(workingPuzzle, cellIndex, value, recursive: false);
                    }

                    if (workingPuzzle is null) continue;

                    List<GeneratorPuzzle> solutions = MultiSolve(workingPuzzle, 2);
                    switch (solutions.Count)
                    {
                        case 0: continue;
                        case 1: return workingPuzzle;
                        default: puzzle = workingPuzzle; break;
                    }
                }
            }
        }

        private static int[] GetCellsToFill(GeneratorPuzzle puzzle, SymmetryType symmetry)
        {
            int[] unsolvedCellIndexes = GetUnsolvedCells(puzzle);
            int randomUnsolvedCell = unsolvedCellIndexes[_rand.Next(unsolvedCellIndexes.Length)];
            int[] cellsToFill = GetReflections(puzzle, randomUnsolvedCell, symmetry);
            return cellsToFill;
        }

        private static int[] GetUnsolvedCells(GeneratorPuzzle puzzle)
            => puzzle.Cells
                .Select((cands, index) => new { cands, index })
                .Where(t => t.cands.Length >= 2)
                .Select(u => u.index)
                .ToArray();

        private static int GetReflectiveIndex(GeneratorPuzzle puzzle)
            => puzzle.Length % 2 == 0 ? -1 : puzzle.Length / 2;

        public static int[] GetReflections(GeneratorPuzzle puzzle, int cell, SymmetryType symmetry)
        {
            int[] symmetricCells = symmetry switch
            {
                SymmetryType.Horizontal => GetHorizontalReflection(puzzle, cell),
                SymmetryType.Vertical => GetVerticalReflection(puzzle, cell),
                SymmetryType.DiagonalUp => GetDiagonalUpReflection(puzzle, cell),
                SymmetryType.DiagonalDown => GetDiagonalDownReflection(puzzle, cell),
                SymmetryType.Rotational => GetRotationalReflection(puzzle, cell),
                _ => new int[] { },
            };
            return symmetricCells.Concat(new[] { cell }).ToArray();
        }

        private static int[] GetHorizontalReflection(GeneratorPuzzle puzzle, int cell)
        {
            int row = puzzle.Row(cell);
            int col = puzzle.Col(cell);
            if (row == GetReflectiveIndex(puzzle))
                return new int[] { };
            int reflectedRow = (puzzle.Length - 1) - row;
            int reflectedIndex = (reflectedRow * puzzle.Length) + col;
            return new[] { reflectedIndex };
        }

        private static int[] GetVerticalReflection(GeneratorPuzzle puzzle, int cell)
        {
            int row = puzzle.Row(cell);
            int col = puzzle.Col(cell);
            if (col == GetReflectiveIndex(puzzle))
                return new int[] { };
            int reflectedCol = (puzzle.Length - 1) - col;
            int reflectedIndex = (row * puzzle.Length) + reflectedCol;
            return new[] { reflectedIndex };
        }

        private static int[] GetDiagonalUpReflection(GeneratorPuzzle puzzle, int cell)
        {
            throw new NotImplementedException();
            int row = puzzle.Row(cell);
            int col = puzzle.Col(cell);
            if (row + col == puzzle.Length - 1)
                return new int[] { };
            int reflectedCol = 0; // TODO
            int reflectedRow = 0; // TODO
            int reflectedIndex = (reflectedRow * puzzle.Length) + reflectedCol;
            return new[] { reflectedIndex };
        }

        private static int[] GetDiagonalDownReflection(GeneratorPuzzle puzzle, int cell)
        {
            // TODO : test this
            int row = puzzle.Row(cell);
            int col = puzzle.Col(cell);
            if (row == col)
                return new int[] { };
            int reflectedCol = row;
            int reflectedRow = col;
            int reflectedIndex = (reflectedRow * puzzle.Length) + reflectedCol;
            return new[] { reflectedIndex };
        }

        private static int[] GetRotationalReflection(GeneratorPuzzle puzzle, int cell)
        {
            throw new NotImplementedException();
        }
    }
}
