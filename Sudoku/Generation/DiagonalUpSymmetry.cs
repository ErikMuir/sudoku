using System.Collections.Generic;
using Sudoku.Logic;

namespace Sudoku.Generation
{
    public class DiagonalUpSymmetry : Symmetry
    {
        public override SymmetryType Type => SymmetryType.DiagonalUp;

        public override Cell[] GetReflections(Puzzle puzzle, Cell cell)
        {
            List<Cell> reflections = new List<Cell> { cell };
            if (cell.Row + cell.Col == Puzzle.UnitSize - 1)
                return reflections.ToArray();
            int col = (Puzzle.UnitSize - 1) - cell.Row;
            int row = (Puzzle.UnitSize - 1) - cell.Col;
            int index = (row * Puzzle.UnitSize) + col;
            reflections.Add(puzzle.Cells[index]);
            return reflections.ToArray();
        }
    }
}
