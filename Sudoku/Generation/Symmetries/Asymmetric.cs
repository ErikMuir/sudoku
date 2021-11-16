namespace Sudoku.Generation
{
    public class Asymmetric : ISymmetry
    {
        private Asymmetric() { }

        public static readonly ISymmetry Symmetry;

        static Asymmetric()
        {
            Symmetry = new Asymmetric();
        }

        public SymmetryType Type => SymmetryType.Asymmetric;

        public int[] GetReflections(int cellIndex)
        {
            return new[] { cellIndex };
        }
    }
}
