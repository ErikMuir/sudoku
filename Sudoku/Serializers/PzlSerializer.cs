using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sudoku.Serializers
{
    public static class PzlSerializer
    {
        public static string Serialize(Puzzle puzzle)
        {
            List<string> cellStrings = puzzle.Cells.Select(cell => Serialize(cell)).ToList();
            return string.Join(",", cellStrings);
        }

        private static string Serialize(Cell cell)
        {
            int value = cell.Value ?? 0;
            int isClue = cell.IsClue ? 1 : 0;
            string candidates = string.Join("", cell.Candidates.Select(c => $"{c}"));
            return $"{value}{isClue}{candidates}";
        }

        public static Puzzle Deserialize(string puzzleString)
        {
            if (puzzleString is null)
                throw new SudokuException("Invalid pzl file format");

            string[] cells = puzzleString.Split(',');
            if (cells.Length != Constants.TotalCells)
                throw new SudokuException("Invalid pzl file format");

            Puzzle puzzle = new();
            Utils.Loop(Constants.TotalCells, i => puzzle.Cells[i] = DeserializeCell(cells[i], i));
            return puzzle;
        }

        private static Cell DeserializeCell(string cellString, int index)
        {
            CellType cellType = GetCellType(cellString);
            if (cellType == CellType.Invalid)
                throw new SudokuException("Invalid pzl file format");

            int col = index % Constants.UnitSize;
            int row = index / Constants.UnitSize;
            int val = int.Parse($"{cellString[0]}");

            Cell cell;
            switch (cellType)
            {
                case CellType.Clue:
                    cell = new Clue(col, row, val);
                    break;
                case CellType.Filled:
                    cell = new Cell(col, row, val);
                    break;
                case CellType.Empty:
                    cell = new Cell(col, row);
                    cellString.Skip(2).ToList().ForEach(x => cell.AddCandidate(int.Parse($"{x}")));
                    break;
                default:
                    throw new NotImplementedException("Unsupported cell type");
            }
            return cell;
        }

        private static Regex _cluePattern = new("^[1-9]1$");
        private static Regex _filledPattern = new("^[1-9]0$");
        private static Regex _emptyPattern = new("^001?2?3?4?5?6?7?8?9?$");

        private static CellType GetCellType(string cell)
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
