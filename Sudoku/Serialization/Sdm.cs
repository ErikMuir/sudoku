using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sudoku.Serialization
{
    public static class Sdm
    {
        private static Regex _sdmPattern = new("^.{81}$");

        public static string Serialize(List<Puzzle> puzzles)
        {
            StringBuilder sb = new();
            foreach (Puzzle puzzle in puzzles)
            {
                string serializedPuzzle = Serialize(puzzle);
                sb.AppendLine(serializedPuzzle);
            }
            return sb.ToString();
        }

        private static string Serialize(Puzzle puzzle)
        {
            StringBuilder sb = new();
            for (int row = 0; row < Constants.Size; row++)
            {
                for (int col = 0; col < Constants.Size; col++)
                {
                    Cell cell = puzzle.GetCell(row, col);
                    string serializedCell = Sdm.Serialize(cell);
                    sb.Append(serializedCell);
                }
            }
            return sb.ToString();
        }

        private static string Serialize(Cell cell) => cell.IsClue ? $"{cell.Value}" : "0";

        public static List<Puzzle> DeserializePuzzles(string puzzleString)
        {
            if (puzzleString is null) return null;

            string[] serializedPuzzles = puzzleString
                .Split(Constants.NewLines, StringSplitOptions.None)
                .ToArray();

            if (serializedPuzzles.Any(x => !_sdmPattern.IsMatch(x)))
                throw new SudokuException("Invalid sdm file format");

            List<Puzzle> puzzles = new();
            foreach (string serializedPuzzle in serializedPuzzles)
            {
                Puzzle puzzle = new();
                for (int row = 0; row < Constants.Size; row++)
                {
                    for (int col = 0; col < Constants.Size; col++)
                    {
                        int index = (row * 9) + col;
                        int.TryParse($"{serializedPuzzle[index]}", out int value);
                        puzzle.Cells[index] = new Clue(col, row, value);
                    }
                }
                puzzles.Add(puzzle);
            }
            return puzzles;
        }
    }
}
