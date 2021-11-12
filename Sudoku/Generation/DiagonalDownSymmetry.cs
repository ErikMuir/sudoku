using System.Collections.Generic;

namespace Sudoku.Generation
{
    public class DiagonalDownSymmetry : Symmetry
    {
        public override SymmetryType Type => SymmetryType.DiagonalDown;

        public override Cell[] GetReflections(Puzzle puzzle, Cell cell)
        {
            List<Cell> reflections = new List<Cell> { cell };
            if (cell.Row == cell.Col)
                return reflections.ToArray();
            int index = (cell.Col * Puzzle.UnitSize) + cell.Row;
            reflections.Add(puzzle.Cells[index]);
            return reflections.ToArray();
        }
    }
}
