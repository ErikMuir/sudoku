namespace Sudoku.Generation.Symmetries;

public abstract class Symmetry
{
    public abstract SymmetryType Type { get; }

    public abstract int[] GetReflections(int cellIndex);
}
