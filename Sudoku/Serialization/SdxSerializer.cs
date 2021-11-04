using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sudoku.Serialization
{
    public class SdxSerializer : ISerializer
    {
        private static readonly Regex _sdxPattern = new("^(u?[1-9]* ){8}u?[1-9]*(\r\n?|\n)?$", RegexOptions.Multiline);
        private static readonly Regex _cluePattern = new("^[1-9]$");
        private static readonly Regex _filledPattern = new("^u[1-9]$");
        private static readonly Regex _emptyPattern = new("^1?2?3?4?5?6?7?8?9?$");

        public string FileExtension => "sdx";

        public string Serialize(Puzzle puzzle)
        {
            StringBuilder sb = new();
            Utils.Loop(row =>
            {
                List<string> cells = new();
                Utils.Loop(col => cells.Add(Serialize(puzzle.GetCell(col, row))));
                sb.AppendLine(string.Join(" ", cells));
            });
            return sb.ToString();
        }

        private string Serialize(Cell cell)
            => cell.Value is not null
                ? $"{(cell.IsClue ? "" : "u")}{cell.Value}"
                : string.Join("", cell.Candidates.Select(x => $"{x}"));

        public Puzzle Deserialize(string puzzleString)
        {
            if (!_sdxPattern.SafeIsMatch(puzzleString))
                throw new SudokuException("Invalid sdx file format");

            string[] lines = puzzleString
                .Split(Constants.NewLines, StringSplitOptions.None)
                .ToArray();

            Puzzle puzzle = new();
            Utils.Loop(row =>
            {
                string[] cells = lines[row].Split(' ');
                Utils.Loop(col =>
                {
                    int index = (row * Constants.UnitSize) + col;
                    string cellString = cells[col];
                    puzzle.Cells[index] = DeserializeCell(cellString, col, row);
                });
            });
            return puzzle;
        }

        private Cell DeserializeCell(string cellString, int col, int row)
        {
            CellType cellType = GetCellType(cellString);
            if (cellType == CellType.Invalid)
                throw new SudokuException("Invalid sdx file format");

            Cell cell = cellType switch
            {
                CellType.Clue => new Clue(col, row, int.Parse(cellString)),
                CellType.Filled => new Cell(col, row, int.Parse(cellString.Replace("u", ""))),
                CellType.Empty => new Cell(col, row),
                _ => throw new NotImplementedException("Unsupported cell type"),
            };

            if (cellType == CellType.Empty)
            {
                cellString.ToList().ForEach(x => cell.AddCandidate(int.Parse($"{x}")));
            }

            return cell;
        }

        private CellType GetCellType(string cell)
        {
            if (_cluePattern.SafeIsMatch(cell))
                return CellType.Clue;
            if (_filledPattern.SafeIsMatch(cell))
                return CellType.Filled;
            if (_emptyPattern.SafeIsMatch(cell))
                return CellType.Empty;
            return CellType.Invalid;
        }
    }
}
