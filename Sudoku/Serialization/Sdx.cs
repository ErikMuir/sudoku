using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku.Serialization
{
    public static class Sdx
    {
        public static string ToSdxString(this Cell cell)
            => cell.Value.HasValue
                ? $"{(cell.IsClue ? "" : "u")}{cell.Value}"
                : string.Join("", cell.Candidates.Select(x => $"{x}"));

        public static string ToSdxString(this Puzzle puzzle)
        {
            var sb = new StringBuilder();
            for (var row = 0; row < Constants.Size; row++)
            {
                var cells = new List<string>();
                for (var col = 0; col < Constants.Size; col++)
                {
                    var serializedCell = puzzle.GetCell(col, row).ToSdxString();
                    cells.Add(serializedCell);
                }
                sb.AppendLine(string.Join(" ", cells));
            }
            return sb.ToString();
        }
    }
}
