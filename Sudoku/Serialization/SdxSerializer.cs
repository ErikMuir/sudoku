using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Sudoku.Logic;

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
            for (int row = 0; row < Puzzle.UnitSize; row++)
            {
                List<string> cells = new();
                for (int col = 0; col < Puzzle.UnitSize; col++)
                {
                    cells.Add(_serializeCell(puzzle.GetCell(row, col)));
                }
                sb.AppendLine(string.Join(" ", cells));
            }
            return sb.ToString();
        }

        private string _serializeCell(Cell cell)
            => cell.Value is not null
                ? $"{(cell.IsClue ? "" : "u")}{cell.Value}"
                : string.Join("", cell.Candidates.Select(x => $"{x}"));

        public Puzzle Deserialize(string puzzleString)
        {
            if (!_sdxPattern.SafeIsMatch(puzzleString))
                throw new SudokuException("Invalid sdx file format");

            string[] lines = puzzleString
                .Split(SerializationUtils.NewLines, StringSplitOptions.None)
                .ToArray();

            Puzzle puzzle = new();
            for (int row = 0; row < Puzzle.UnitSize; row++)
            {
                string[] cells = lines[row].Split(' ');
                for (int col = 0; col < Puzzle.UnitSize; col++)
                {
                    int index = (row * Puzzle.UnitSize) + col;
                    string cellString = cells[col];
                    puzzle.Cells[index] = _deserializeCell(cellString, col, row);
                }
            }
            return puzzle;
        }

        private Cell _deserializeCell(string cellString, int col, int row)
        {
            CellType cellType = _getCellType(cellString);
            if (cellType == CellType.Invalid)
                throw new SudokuException("Invalid sdx file format");

            Cell cell = cellType switch
            {
                CellType.Clue => new Clue(row, col, int.Parse(cellString)),
                CellType.Filled => new Cell(row, col, int.Parse(cellString.Replace("u", ""))),
                CellType.Empty => new Cell(row, col),
                _ => throw new NotImplementedException("Unsupported cell type"),
            };

            if (cellType == CellType.Empty)
            {
                cellString.ToList().ForEach(x => cell.AddCandidate(int.Parse($"{x}")));
            }

            return cell;
        }

        private CellType _getCellType(string cell)
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
