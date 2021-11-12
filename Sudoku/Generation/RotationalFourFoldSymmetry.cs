using System.Collections.Generic;

namespace Sudoku.Generation
{
    public class RotationalFourFoldSymmetry : Symmetry
    {
        public override SymmetryType Type => SymmetryType.RotationalFourFold;

        public override Cell[] GetReflections(Puzzle puzzle, Cell cell)
        {
            List<Cell> reflections = new List<Cell> { cell };
            int axis = Puzzle.ReflectiveIndex;
            if (cell.Row == axis && cell.Col == axis)
                return reflections.ToArray();
            int reflectedIndex = cell.Index;
            for (int i = 0; i < 3; i++)
            {
                reflectedIndex = RotateCell(puzzle, reflectedIndex);
                reflections.Add(puzzle.Cells[reflectedIndex]);
            }
            return reflections.ToArray();
        }

        private int RotateCell(Puzzle puzzle, int cellIndex)
        {
            int row = puzzle.Cells[cellIndex].Row;
            int col = puzzle.Cells[cellIndex].Col;
            int targetRow = col;
            int targetCol = (Puzzle.UnitSize - 1) - row;
            int targetIndex = (targetRow * Puzzle.UnitSize) + targetCol;
            return targetIndex;
        }
    }
}
