using System.Collections.Generic;
using Sudoku.Logic;

namespace Sudoku.Generation
{
    public class RotationalTwoFoldSymmetry : Symmetry
    {
        public override SymmetryType Type => SymmetryType.RotationalTwoFold;

        public override Cell[] GetReflections(Puzzle puzzle, Cell cell)
        {
            List<Cell> reflections = new List<Cell> { cell };
            int axis = Puzzle.ReflectiveIndex;
            if (cell.Row == axis && cell.Col == axis)
                return reflections.ToArray();
            int row = (Puzzle.UnitSize - 1) - cell.Row;
            int col = (Puzzle.UnitSize - 1) - cell.Col;
            int index = (row * Puzzle.UnitSize) + col;
            reflections.Add(puzzle.Cells[index]);
            return reflections.ToArray();
        }
    }
}
