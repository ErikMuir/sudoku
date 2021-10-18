using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku.Serialization
{
    public static class Sdx
    {
        public static string Serialize(Puzzle puzzle)
        {
            var sb = new StringBuilder();
            for (var row = 0; row < Constants.Size; row++)
            {
                var cells = new List<string>();
                for (var col = 0; col < Constants.Size; col++)
                {
                    var serializedCell = Serialize(puzzle.GetCell(col, row));
                    cells.Add(serializedCell);
                }
                sb.AppendLine(string.Join(" ", cells));
            }
            return sb.ToString();
        }

        private static string Serialize(Cell cell)
            => cell.Value.HasValue
                ? $"{(cell.IsClue ? "" : "u")}{cell.Value}"
                : string.Join("", cell.Candidates.Select(x => $"{x}"));

        public static Puzzle Deserialize(string puzzleString)
        {
            throw new System.NotImplementedException();
            /*
            This format contains a line for each row in the grid. A blank separates the cells.
            For unsolved cells, the candidates are listed without separating space.
            Solved cells are preceded by u when they are placed by the user, the givens have no prefix.

            2 679 6789 1 46789 5 469 9 3
            389 5 4 69 689 68 7 1 29
            9 1 679 2 4679 3 4569 8 59
            6 9 2 8 u1 7 3 59 4
            3489 3479 3789 56 2456 46 u1 2579 2579
            1 47 5 3 24 9 8 27 6
            3459 2 39 7 3589 1 59 6 589
            359 8 1 569 3569 6 2 4 579
            7 369 369 4 35689 2 59 359 1
            */
        }
    }
}
