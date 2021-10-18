using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sudoku.Serialization
{
    public static class Sdm
    {
        private static Regex _sdmPattern = new Regex("^.{81}$");

        public static string Serialize(List<Puzzle> puzzles)
        {
            var sb = new StringBuilder();
            foreach (var puzzle in puzzles)
            {
                var serializedPuzzle = Serialize(puzzle);
                sb.AppendLine(serializedPuzzle);
            }
            return sb.ToString();
        }

        private static string Serialize(Puzzle puzzle)
        {
            var sb = new StringBuilder();
            for (var row = 0; row < Constants.Size; row++)
            {
                for (var col = 0; col < Constants.Size; col++)
                {
                    var cell = puzzle.GetCell(row, col);
                    var serializedCell = Sdm.Serialize(cell);
                    sb.Append(serializedCell);
                }
            }
            return sb.ToString();
        }

        private static string Serialize(Cell cell) => cell.IsClue ? $"{cell.Value}" : "0";

        public static List<Puzzle> DeserializePuzzles(string puzzleString)
        {
            if (puzzleString == null) return null;

            var serializedPuzzles = puzzleString
                .Split(Constants.NewLines, StringSplitOptions.None)
                .ToArray();

            if (serializedPuzzles.Any(x => !_sdmPattern.IsMatch(x)))
                throw new SudokuException("Invalid sdm file format");

            var puzzles = new List<Puzzle>();
            foreach (var serializedPuzzle in serializedPuzzles)
            {
                var puzzle = new Puzzle();
                for (var row = 0; row < Constants.Size; row++)
                {
                    for (var col = 0; col < Constants.Size; col++)
                    {
                        var index = (row * 9) + col;
                        int.TryParse($"{serializedPuzzle[index]}", out var value);
                        puzzle.Cells[index] = new Clue(col, row, value);
                    }
                }
                puzzles.Add(puzzle);
            }
            return puzzles;
        }
    }
}
