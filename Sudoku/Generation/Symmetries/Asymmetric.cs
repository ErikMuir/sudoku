namespace Sudoku.Generation.Symmetries;

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
        return [cellIndex];
    }
}
