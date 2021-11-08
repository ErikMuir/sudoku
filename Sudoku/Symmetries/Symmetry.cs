namespace Sudoku.Symmetries
{
    public abstract class Symmetry
    {
        public abstract SymmetryType Type { get; }

        public abstract Cell[] GetReflections(Puzzle puzzle, Cell cell);
    }

    public enum SymmetryType
    {
        None,
        Horizontal,
        Vertical,
        DiagonalUp,
        DiagonalDown,
        RotationalTwoFold,
        RotationalFourFold,
        Random,
    }
}
