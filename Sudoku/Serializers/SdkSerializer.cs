using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sudoku.Serializers
{
    public class SdkSerializer : ISerializer
    {
        public string FileExtension => "sdk";

        private static readonly Regex _sdkLinePattern = new(@"^[1-9\.]{9}$");

        public Puzzle Deserialize(string input)
        {
            if (input is null)
                throw new SudokuException("Invalid sdk file format");

            string[] lines = input
                .Trim()
                .Split(SerializerUtils.NewLines, StringSplitOptions.None)
                .Where(x => x.Length is not 0) // ignore empty lines
                .Where(x => x.Substring(0, 1) != "#") // ignore metadata
                .ToArray();

            int length = lines[0].Length;
            if (lines.Length != length || lines.Any(x => !_sdkLinePattern.IsMatch(x)))
                throw new SudokuException("Invalid sdk file format");

            string puzzleString = string.Join("", lines);
            Puzzle puzzle = new(length);
            for (int i = 0; i < puzzleString.Length; i++)
            {
                int.TryParse($"{puzzleString[i]}", out int val);
                if (val > 0) puzzle = Puzzle.SetGiven(puzzle, i, val);
            }
            return puzzle;
        }

        public string Serialize(Puzzle puzzle)
        {
            StringBuilder sb = new();
            for (int i = 0; i < puzzle.Cells.Length; i++)
            {
                sb.Append(Serialize(puzzle.Cells[i]));
                if ((i + 1) % puzzle.Length == 0)
                    sb.AppendLine();
            }
            return sb.ToString();
        }

        private string Serialize(int[] cell) => cell.Length == 1 ? $"{Math.Abs(cell[0])}" : ".";
    }
}
