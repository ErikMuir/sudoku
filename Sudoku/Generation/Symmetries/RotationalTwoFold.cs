using System.Collections.Generic;
using Sudoku.Extensions;
using Sudoku.Logic;

namespace Sudoku.Generation;

    public class RotationalTwoFold : ISymmetry
    {
        private RotationalTwoFold() { }

        public static readonly ISymmetry Symmetry;

        static RotationalTwoFold()
        {
            Symmetry = new RotationalTwoFold();
        }

        public SymmetryType Type => SymmetryType.RotationalTwoFold;

        public int[] GetReflections(int cellIndex)
        {
            List<int> reflections = new() { cellIndex };
            int row = cellIndex.GetRowIndex();
            int col = cellIndex.GetColIndex();
            int axis = Puzzle.ReflectiveIndex;
            if (row != axis || col != axis)
            {
                int reflectedRow = (Puzzle.UnitSize - 1) - row;
                int reflectedCol = (Puzzle.UnitSize - 1) - col;
                int reflectedIndex = (reflectedRow * Puzzle.UnitSize) + reflectedCol;
                reflections.Add(reflectedIndex);
            }
            return reflections.ToArray();
    }
}
