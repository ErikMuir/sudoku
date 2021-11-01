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

        private static readonly Dictionary<Tuple<int, int>, int[]> _savedPeers = new();

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
        private bool IsGiven(int[] cell) => cell.Length == 1 && cell[0] < 0;
        public int Row(int i) => i / Length;
        public int Col(int i) => i % Length;
        public int Box(int i) => ((Row(i) / BoxSize) * BoxSize) + (Col(i) / BoxSize);
        private int[] GetRowIndexes(int i) => Enumerable.Range(i / Length, Length).ToArray();
        private int[] GetColIndexes(int i) => Enumerable.Range(0, Length).Select(x => (x * Length) + i).ToArray();
        private int[] GetBoxIndexes(int i)
        {
            int startRow = (i / BoxSize) * BoxSize;
            int startCol = (i % BoxSize) * BoxSize;
            List<int> indexes = new();
            for (int row = startRow; row < startRow + BoxSize; startRow++)
                for (int col = startCol; col < startCol + BoxSize; startCol++)
                    indexes.Add((row * Length) + col);
            return indexes.ToArray();
        }

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

        public bool IsSolved() => Cells.All(x => x.Length is 1);

        public void CalculateCandidates()
        {
            // first fill all candidates of empty cells
            Cells.Where(x => x.Length > 1).ToList().ForEach(x => x = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });

            // then reduce candidates by col/row/box constraints
            this.ReduceCandidates();
        }

        public void ReduceCandidates()
        {
            int getCellValue(int index, int[] unit)
            {
                int cellIndex = unit[index];
                int[] cell = Cells[cellIndex];
                return cell.Length == 1 ? Math.Abs(cell[0]) : 0;
            }

            for (int iUnit = 0; iUnit < Length; iUnit++)
            {
                int[] rowIndexes = GetRowIndexes(iUnit);
                int[] colIndexes = GetColIndexes(iUnit);
                int[] boxIndexes = GetBoxIndexes(iUnit);

                for (int iCell = 0; iCell < Length; iCell++)
                {
                    int rowCellValue = getCellValue(iCell, rowIndexes);
                    int colCellValue = getCellValue(iCell, colIndexes);
                    int boxCellValue = getCellValue(iCell, boxIndexes);

                    for (int iPeer = 0; iPeer < Length; iPeer++)
                    {
                        if (iPeer == iCell) continue;

                        int rowPeerIndex = rowIndexes[iPeer];
                        int colPeerIndex = colIndexes[iPeer];
                        int boxPeerIndex = boxIndexes[iPeer];

                        int[] rowPeer = Cells[rowPeerIndex];
                        int[] colPeer = Cells[colPeerIndex];
                        int[] boxPeer = Cells[boxPeerIndex];

                        if (rowCellValue > 0 && rowPeer.Length > 1)
                            Cells[rowPeerIndex] = rowPeer.Except(new[] { rowCellValue }).ToArray();

                        if (colCellValue > 0 && colPeer.Length > 1)
                            Cells[colPeerIndex] = colPeer.Except(new[] { colCellValue }).ToArray();

                        if (boxCellValue > 0 && boxPeer.Length > 1)
                            Cells[boxPeerIndex] = boxPeer.Except(new[] { boxCellValue }).ToArray();
                    }
                }
            }
        }
    }
}
