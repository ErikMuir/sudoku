using System.Collections.Generic;
using System.Text;

namespace Sudoku.Serialization
{
    public static class Sdm
    {
        public static string ToSdmString(this Puzzle puzzle)
        {
            var sb = new StringBuilder();
            for (var row = 0; row < Constants.Size; row++)
            {
                for (var col = 0; col < Constants.Size; col++)
                {
                    var serializedCell = puzzle.GetCell(col, row).ToSdkString('0');
                    sb.Append(serializedCell);
                }
            }
            return sb.ToString();
        }

        public static string ToSdmString(this Puzzle puzzle, char placeholder)
        {
            var sb = new StringBuilder();
            for (var row = 0; row < Constants.Size; row++)
            {
                for (var col = 0; col < Constants.Size; col++)
                {
                    var serializedCell = puzzle.GetCell(col, row).ToSdkString(placeholder);
                    sb.Append(serializedCell);
                }
            }
            return sb.ToString();
        }

        public static string ToSdmString(this List<Puzzle> puzzles)
        {
            var sb = new StringBuilder();
            foreach (var puzzle in puzzles)
            {
                var serializedPuzzle = puzzle.ToSdmString();
                sb.AppendLine(serializedPuzzle);
            }
            return sb.ToString();
        }

        public static string ToSdmString(this List<Puzzle> puzzles, char placeholder)
        {
            var sb = new StringBuilder();
            foreach (var puzzle in puzzles)
            {
                var serializedPuzzle = puzzle.ToSdmString(placeholder);
                sb.AppendLine(serializedPuzzle);
            }
            return sb.ToString();
        }
    }
}
