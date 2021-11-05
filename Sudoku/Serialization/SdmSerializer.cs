using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sudoku.Serialization
{
    public class SdmSerializer : ISerializer
    {
        private static readonly Regex _sdmPattern = new("^.{81}$");

        public string FileExtension => "sdm";

        public string Serialize(Puzzle puzzle)
        {
            StringBuilder sb = new();
            Utils.Loop(Puzzle.TotalCells, i => sb.Append(Serialize(puzzle.Cells[i])));
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

        private string Serialize(Cell cell) => cell.Value is not null ? $"{cell.Value}" : "0";

        public Puzzle Deserialize(string puzzleString)
        {
            if (!_sdmPattern.SafeIsMatch(puzzleString))
                throw new SudokuException("Invalid sdm file format");

            Puzzle puzzle = new();
            Utils.Loop(Puzzle.TotalCells, i =>
            {
                int col = i % Puzzle.UnitSize;
                int row = i / Puzzle.UnitSize;
                int.TryParse($"{puzzleString[i]}", out int val);
                if (val > 0) puzzle.Cells[i] = new Clue(row, col, val);
            });
            return puzzle;
        }

        public List<Puzzle> DeserializePuzzles(string puzzleString)
        {
            if (puzzleString is null)
                throw new SudokuException("Invalid sdm file format");

            return puzzleString
                .Split(Constants.NewLines, StringSplitOptions.None)
                .Select(x => Deserialize(x))
                .ToList();
        }
    }
}
