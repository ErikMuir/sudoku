using System.Collections.Generic;
using Sudoku.Extensions;
using Sudoku.Logic;

namespace Sudoku.Generation
{
    public class Horizontal : ISymmetry
    {
        private Horizontal() { }

        public static readonly ISymmetry Symmetry;

        static Horizontal()
        {
            Symmetry = new Horizontal();
        }

        public SymmetryType Type => SymmetryType.Horizontal;

        public int[] GetReflections(int cellIndex)
        {
            List<int> reflections = new() { cellIndex };
            int row = cellIndex.GetRowIndex();
            int col = cellIndex.GetColIndex();
            if (row != Puzzle.ReflectiveIndex)
            {
                int reflectedRow = (Puzzle.UnitSize - 1) - row;
                int reflectedIndex = (reflectedRow * Puzzle.UnitSize) + col;
                reflections.Add(reflectedIndex);
            }
            return reflections.ToArray();
        }
    }
}
