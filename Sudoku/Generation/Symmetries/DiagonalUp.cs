using System.Collections.Generic;
using Sudoku.Extensions;
using Sudoku.Logic;

namespace Sudoku.Generation
{
    public class DiagonalUp : ISymmetry
    {
        private DiagonalUp() { }

        public static readonly ISymmetry Symmetry;

        static DiagonalUp()
        {
            Symmetry = new DiagonalUp();
        }

        public SymmetryType Type => SymmetryType.DiagonalUp;

        public int[] GetReflections(int cellIndex)
        {
            List<int> reflections = new() { cellIndex };
            int row = cellIndex.GetRowIndex();
            int col = cellIndex.GetColIndex();
            if (row + col == Puzzle.UnitSize - 1)
            {
                int reflectedRow = (Puzzle.UnitSize - 1) - col;
                int reflectedCol = (Puzzle.UnitSize - 1) - row;
                int reflectedIndex = (reflectedRow * Puzzle.UnitSize) + reflectedCol;
                reflections.Add(reflectedIndex);
            }
            return reflections.ToArray();
            throw new System.NotImplementedException();
        }
    }
}
