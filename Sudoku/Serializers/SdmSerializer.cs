using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sudoku.Serializers
{
    public class SdmSerializer : ISerializer
    {
        public string FileExtension => "sdm";

        private static readonly Regex _sdmPattern = new("^.{81}$");

        public Puzzle Deserialize(string input)
        {
            if (input is null || !_sdmPattern.IsMatch(input))
                throw new SudokuException("Invalid sdm file format");

            Puzzle puzzle = new(9);
            for (int i = 0; i < input.Length; i++)
            {
                int.TryParse($"{input[i]}", out int val);
                if (val > 0) Puzzle.SetGiven(puzzle, i, val);
            }
            return puzzle;
        }

        public List<Puzzle> DeserializePuzzles(string puzzleString)
        {
            if (puzzleString is null)
                throw new SudokuException("Invalid sdm file format");

            return puzzleString
                .Split(SerializerUtils.NewLines, StringSplitOptions.None)
                .Where(x => x.Length is not 0) // ignore empty lines
                .Select(x => Deserialize(x))
                .ToList();
        }

        public string Serialize(Puzzle puzzle)
        {
            StringBuilder sb = new();
            for (int i = 0; i < puzzle.Cells.Length; i++)
                sb.Append(Serialize(puzzle.Cells[i]));
            return sb.ToString();
        }

        public string Serialize(List<Puzzle> puzzles)
        {
            StringBuilder sb = new();
            foreach (Puzzle puzzle in puzzles)
            {
                string serializedPuzzle = Serialize(puzzle);
                sb.AppendLine(serializedPuzzle);
            }
            return sb.ToString();
        }

        private string Serialize(int[] cell) => cell.Length == 1 ? $"{Math.Abs(cell[0])}" : "0";
    }
}
