using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sudoku.Serializers
{
    public static class SdmSerializer
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
            Utils.Loop(Constants.TotalCells, i => sb.Append(SdmSerializer.Serialize(puzzle.Cells[i])));
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
                Utils.Loop(Constants.TotalCells, i =>
                {
                    int col = i % Constants.UnitSize;
                    int row = i / Constants.UnitSize;
                    int.TryParse($"{serializedPuzzle[i]}", out int val);
                    puzzle.Cells[i] = new Clue(col, row, val);
                });
                puzzles.Add(puzzle);
            }
            return puzzles;
        }
    }
}
