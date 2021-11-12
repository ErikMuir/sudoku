using System.Collections.Generic;
using Sudoku.Logic;

namespace Sudoku.Generation
{
    public class VerticalSymmetry : Symmetry
    {
        public override SymmetryType Type => SymmetryType.Vertical;

        public override Cell[] GetReflections(Puzzle puzzle, Cell cell)
        {
            List<Cell> reflections = new List<Cell> { cell };
            if (cell.Col == Puzzle.ReflectiveIndex)
                return reflections.ToArray();
            int col = (Puzzle.UnitSize - 1) - cell.Col;
            int index = (cell.Row * Puzzle.UnitSize) + col;
            reflections.Add(puzzle.Cells[index]);
            return reflections.ToArray();
        }
    }
}
