namespace Sudoku.Generation
{
    public class NoSymmetry : Symmetry
    {
        public override SymmetryType Type => SymmetryType.None;

        public override Cell[] GetReflections(Puzzle puzzle, Cell cell)
        {
            return new[] { cell };
        }
    }
}
