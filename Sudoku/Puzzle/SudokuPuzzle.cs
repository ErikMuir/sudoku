using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku
{
    public class SudokuPuzzle
    {
        public int Length;
        public int[][] Cells;
        public int BoxSize => (int)Math.Sqrt(Length);

        public SudokuPuzzle(int n)
        {
            Length = n;
            Cells = Enumerable.Repeat(Enumerable.Range(1, n).ToArray(), n * n).ToArray();
        }

        public SudokuPuzzle(SudokuPuzzle p)
        {
            Length = p.Length;
            Cells = new int[p.Cells.Length][];
            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i] = new int[p.Cells[i].Length];
                Buffer.BlockCopy(p.Cells[i], 0, this.Cells[i], 0, p.Cells[i].Length * sizeof(int));
            }
        }

        public SudokuPuzzle(int[] input) : this((int)Math.Sqrt(input.Length))
        {
            for (int i = 0; i < input.Length; i++)
                if (input[i] > 0 && input[i] <= Length)
                {
                    SudokuPuzzle puzzle = PlaceValue(this, i, input[i]);
                    if (puzzle is null)
                        throw new ArgumentException("This puzzle is unsolvable!");
                    this.Cells = puzzle.Cells;
                }
        }

        public SudokuPuzzle(string input) : this(input.Select(c => char.IsDigit(c) ? c - '0' : 0).ToArray())
        {

        }

        private static Dictionary<Tuple<int, int>, int[]> _savedPeers = new();

        public int[] Peers(int cell)
        {
            Tuple<int, int> key = new(this.Length, cell);
            if (!_savedPeers.ContainsKey(key))
                _savedPeers.Add(key, Enumerable.Range(0, Length * Length).Where(c => IsPeer(cell, c)).ToArray());
            return _savedPeers[key];
        }

        private bool IsPeer(int c1, int c2) => c1 != c2 && (IsInSameRow(c1, c2) || IsInSameColumn(c1, c2) || IsInSameBox(c1, c2));
        private bool IsInSameRow(int c1, int c2) => c1 / Length == c2 / Length;
        private bool IsInSameColumn(int c1, int c2) => c1 % Length == c2 % Length;
        private bool IsInSameBox(int c1, int c2) => c1 / Length / BoxSize == c2 / Length / BoxSize && c1 % Length / BoxSize == c2 % Length / BoxSize;

        public static SudokuPuzzle PlaceValue(SudokuPuzzle input, int cellIndex, int value)
        {
            SudokuPuzzle puzzle = new(input);

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
                    cellsToPlace.Add(peerIndex, newPeers.Single());

                puzzle.Cells[peerIndex] = newPeers;
            }

            foreach (KeyValuePair<int, int> cell in cellsToPlace)
            {
                if ((puzzle = PlaceValue(puzzle, cell.Key, cell.Value)) is null)
                    return null;
            }

            return puzzle;
        }

        public static int FindWorkingCell(SudokuPuzzle puzzle)
        {
            int minCandidates = puzzle.Cells.Where(cands => cands.Length >= 2).Min(cands => cands.Length);
            return Array.FindIndex(puzzle.Cells, c => c.Length == minCandidates);
        }

        public static SudokuPuzzle Solve(SudokuPuzzle input, Func<SudokuPuzzle, bool> solutionFunc = null)
        {
            if (input.Cells.All(cell => cell.Length == 1))
                return (solutionFunc != null && solutionFunc(input)) ? null : input;

            int activeCell = FindWorkingCell(input);
            foreach (int guess in input.Cells[activeCell])
            {
                SudokuPuzzle puzzle;
                if ((puzzle = PlaceValue(input, activeCell, guess)) is not null)
                    if ((puzzle = Solve(puzzle, solutionFunc)) is not null)
                        return puzzle;
            }
            return null;
        }

        public static List<SudokuPuzzle> MultiSolve(SudokuPuzzle input, int MaximumSolutions = -1)
        {
            List<SudokuPuzzle> solutions = new();
            Solve(input, p =>
            {
                solutions.Add(p);
                return solutions.Count() < MaximumSolutions || MaximumSolutions == -1;
            });
            return solutions;
        }

        public static SudokuPuzzle RandomGrid(int size)
        {
            SudokuPuzzle puzzle = new(size);
            Random rand = new();
            int iterations = 0;

            while (true)
            {
                iterations++;
                int[] unsolvedCellIndexes = puzzle.Cells
                    .Select((cands, index) => new { cands, index })
                    .Where(t => t.cands.Length >= 2)
                    .Select(u => u.index)
                    .ToArray();

                int cellIndex = unsolvedCellIndexes[rand.Next(unsolvedCellIndexes.Length)];
                int candidateValue = puzzle.Cells[cellIndex][rand.Next(puzzle.Cells[cellIndex].Length)];

                SudokuPuzzle workingPuzzle = PlaceValue(puzzle, cellIndex, candidateValue);
                if (workingPuzzle != null)
                {
                    List<SudokuPuzzle> solutions = MultiSolve(workingPuzzle, 2);
                    switch (solutions.Count)
                    {
                        case 0: continue;
                        case 1:
                            Console.WriteLine($"Iterations: {iterations}");
                            return workingPuzzle; //solutions.Single();
                        default:
                            puzzle = workingPuzzle;
                            break;
                    }
                }
            }
        }

        public static void Output(SudokuPuzzle puzzle)
        {
            StringBuilder sb = new();
            sb.AppendLine();
            sb.AppendLine(" ----------------------------- ");
            for (int row = 0; row < puzzle.Length; row++)
            {
                if (row > 0 && row % puzzle.BoxSize == 0)
                    sb.AppendLine("|---------+---------+---------|");
                int startIndex = puzzle.Length * row;
                sb.Append('|');
                for (int cellIndex = startIndex; cellIndex < startIndex + puzzle.Length; cellIndex++)
                {
                    if (cellIndex > startIndex && cellIndex % puzzle.BoxSize == 0)
                        sb.Append('|');
                    int[] cell = puzzle.Cells[cellIndex];
                    sb.Append(cell.Length == 1 ? $" {Math.Abs(cell[0])} " : "   ");
                }
                sb.AppendLine("|");
            }
            sb.AppendLine(" ----------------------------- ");
            Console.WriteLine(sb.ToString());
        }
    }
}
