using System.Collections.Generic;

namespace Sudoku.Generation
{
    public class HorizontalSymmetry : Symmetry
    {
        public override SymmetryType Type => SymmetryType.Horizontal;

        public override Cell[] GetReflections(Puzzle puzzle, Cell cell)
        {
            List<Cell> reflections = new List<Cell> { cell };
            if (cell.Row == Puzzle.ReflectiveIndex)
                return reflections.ToArray();
            int row = (Puzzle.UnitSize - 1) - cell.Row;
            int index = (row * Puzzle.UnitSize) + cell.Col;
            reflections.Add(puzzle.Cells[index]);
            return reflections.ToArray();
        }
    }
}
