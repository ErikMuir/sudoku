using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sudoku.Serializers
{
    public class SdxSerializer : ISerializer
    {
        public string FileExtension => "sdx";

        private static readonly Regex _sdxPattern = new("^(u?[1-9]* ){8}u?[1-9]*(\r\n?|\n)?$", RegexOptions.Multiline);
        private static readonly Regex _givenPattern = new("^[1-9]$");
        private static readonly Regex _filledPattern = new("^u[1-9]$");
        private static readonly Regex _emptyPattern = new("^1?2?3?4?5?6?7?8?9?$");

        public Puzzle Deserialize(string input)
        {
            if (input is null || !_sdxPattern.IsMatch(input))
                throw new SudokuException("Invalid sdx file format");

            string[] lines = input
                .Trim()
                .Split(SerializerUtils.NewLines, StringSplitOptions.None)
                .Where(x => x.Length is not 0) // ignore empty lines
                .ToArray();

            int length = lines[0].Length;
            Puzzle puzzle = new(length);
            string[] cells = string.Join(" ", lines).Split(" ");
            for (int i = 0; i < cells.Length; i++)
                puzzle.Cells[i] = DeserializeCell(cells[i]);
            return puzzle;
        }

        private int[] DeserializeCell(string cellString)
            => GetCellType(cellString) switch
            {
                CellType.Given => new[] { int.Parse(cellString) * -1 },
                CellType.Filled => new[] { int.Parse(cellString.Replace("u", "")) },
                CellType.Empty => cellString.ToCharArray().Select(x => int.Parse($"{x}")).ToArray(),
                _ => throw new SudokuException("Invalid sdx format"),
            };

        private CellType GetCellType(string cell)
        {
            if (_givenPattern.IsMatch(cell))
                return CellType.Given;
            if (_filledPattern.IsMatch(cell))
                return CellType.Filled;
            if (_emptyPattern.IsMatch(cell))
                return CellType.Empty;
            return CellType.Invalid;
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

        private string Serialize(int[] cell)
        {
            if (cell.Length > 1)
                return string.Join("", cell.Select(x => $"{x}"));
            int val = cell[0];
            return val > 0 ? $"u{val}" : $"{Math.Abs(val)}";
        }
    }
}
