using System.Collections.Generic;
using Sudoku.Extensions;
using Sudoku.Logic;

namespace Sudoku.Generation;

    public class Vertical : ISymmetry
    {
        private Vertical() { }

        public static readonly ISymmetry Symmetry;

        static Vertical()
        {
            Symmetry = new Vertical();
        }

        public SymmetryType Type => SymmetryType.Vertical;

        public int[] GetReflections(int cellIndex)
        {
            List<int> reflections = new() { cellIndex };
            int row = cellIndex.GetRowIndex();
            int col = cellIndex.GetColIndex();
            if (col != Puzzle.ReflectiveIndex)
            {
                int reflectedCol = (Puzzle.UnitSize - 1) - col;
                int reflectedIndex = (row * Puzzle.UnitSize) + reflectedCol;
                reflections.Add(reflectedIndex);
            }
            return reflections.ToArray();
    }
}
