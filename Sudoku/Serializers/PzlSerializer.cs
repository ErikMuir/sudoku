using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sudoku.Serializers
{
    public class PzlSerializer : ISerializer
    {
        private static readonly Regex _pzlPattern = new("^([0-9][0-1]1?2?3?4?5?6?7?8?9?,?){81}$");
        private static readonly Regex _cluePattern = new("^[1-9]1$");
        private static readonly Regex _filledPattern = new("^[1-9]0$");
        private static readonly Regex _emptyPattern = new("^001?2?3?4?5?6?7?8?9?$");

        public string FileExtension => "pzl";

        public string Serialize(Puzzle puzzle)
        {
            List<string> cellStrings = puzzle.Cells.Select(cell => Serialize(cell)).ToList();
            return string.Join(",", cellStrings);
        }

        private string Serialize(Cell cell)
        {
            int value = cell.Value ?? 0;
            int isClue = cell.IsClue ? 1 : 0;
            string candidates = string.Join("", cell.Candidates.Select(c => $"{c}"));
            return $"{value}{isClue}{candidates}";
        }

        public Puzzle Deserialize(string puzzleString)
        {
            if (!_pzlPattern.SafeIsMatch(puzzleString))
                throw new SudokuException("Invalid pzl file format");

            Puzzle puzzle = new();
            string[] cells = puzzleString.Split(',');
            Utils.Loop(Constants.TotalCells, i => puzzle.Cells[i] = DeserializeCell(cells[i], i));
            return puzzle;
        }

        private Cell DeserializeCell(string cellString, int index)
        {
            CellType cellType = GetCellType(cellString);
            if (cellType == CellType.Invalid)
                throw new SudokuException("Invalid pzl file format");

            int col = index % Constants.UnitSize;
            int row = index / Constants.UnitSize;
            int val = int.Parse($"{cellString[0]}");

            Cell cell = cellType switch
            {
                CellType.Clue => new Clue(col, row, val),
                CellType.Filled => new Cell(col, row, val),
                CellType.Empty => new Cell(col, row),
                _ => throw new NotImplementedException("Unsupported cell type"),
            };

            if (cellType == CellType.Empty)
            {
                cellString.Skip(2).ToList().ForEach(x => cell.AddCandidate(int.Parse($"{x}")));
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
