namespace Sudoku.Generation.Symmetries;

public class Asymmetric : Symmetry
{
    private Asymmetric() { }

    static Asymmetric()
    {
        Symmetry = new Asymmetric();
    }

    public static readonly Symmetry Symmetry;

    public override SymmetryType Type => SymmetryType.Asymmetric;

    public override int[] GetReflections(int cellIndex)
    {
        return [cellIndex];
    }
}
