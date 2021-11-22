using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Sudoku.Exceptions;
using Sudoku.Extensions;
using Sudoku.Logic;

namespace Sudoku.Serialization
{
    public class Sdm : ISerializer
    {
        private static readonly Regex _sdmPattern = new("^.{81}$");

        private Sdm() { }

        public static readonly ISerializer Serializer;

        static Sdm()
        {
            Serializer = new Sdm();
        }

        public string FileExtension => "sdm";

        public Puzzle Deserialize(string puzzleString)
        {
            if (!_sdmPattern.SafeIsMatch(puzzleString))
                throw new SudokuException("Invalid sdm file format");

            Puzzle puzzle = new();
            for (int i = 0; i < Puzzle.TotalCells; i++)
            {
                int row = i.GetRowIndex();
                int col = i.GetColIndex();
                int.TryParse($"{puzzleString[i]}", out int val);
                if (val > 0) puzzle.Cells[i] = new Clue(row, col, val);
            }
            return puzzle;
        }

        public string Serialize(Puzzle puzzle)
        {
            StringBuilder sb = new();
            for (int i = 0; i < Puzzle.TotalCells; i++)
                sb.Append(_serializeCell(puzzle.Cells[i]));
            return sb.ToString();
        }

        public List<Puzzle> DeserializeMultiple(string puzzleString)
        {
            if (puzzleString is null)
                throw new SudokuException("Invalid sdm file format");

            return puzzleString
                .Split(SerializationUtils.NewLines, StringSplitOptions.None)
                .Select(x => Deserialize(x))
                .ToList();
        }

        public string SerializeMultiple(List<Puzzle> puzzles)
        {
            StringBuilder sb = new();
            foreach (Puzzle puzzle in puzzles)
            {
                string serializedPuzzle = Serialize(puzzle);
                sb.AppendLine(serializedPuzzle);
            }
            return sb.ToString();
        }

        private string _serializeCell(Cell cell) => cell.Value is not null ? $"{cell.Value}" : "0";
    }
}
