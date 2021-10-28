using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku
{
    public class Puzzle
    {
        public int Length;
        public int[][] Cells;
        public int BoxSize => (int)Math.Sqrt(Length);

        private static readonly Random _rand = new();
        private static readonly Dictionary<Tuple<int, int>, int[]> _savedPeers = new();
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

        public Puzzle(int length)
        {
            Length = length;
            Cells = Enumerable.Repeat(Enumerable.Range(1, length).ToArray(), length * length).ToArray();
        }

        public Puzzle(Puzzle puzzle)
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
        public int Row(int i) => i / Length;
        public int Col(int i) => i % Length;
        public int Box(int i) => ((Row(i) / BoxSize) * BoxSize) + (Col(i) / BoxSize);

        public static Puzzle SetGiven(Puzzle input, int cellIndex, int value)
        {
            Puzzle puzzle = new(input);
            puzzle.Cells[cellIndex] = new int[] { value * -1 };
            foreach (int peerIndex in puzzle.Peers(cellIndex))
            {
                int[] newPeers = puzzle.Cells[peerIndex].Except(new int[] { value }).ToArray();
                puzzle.Cells[peerIndex] = newPeers;
            }
            return puzzle;
        }

        public static Puzzle PlaceValue(Puzzle input, int cellIndex, int value, bool recursive = true)
        {
            Puzzle puzzle = new(input);

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
    }
}
