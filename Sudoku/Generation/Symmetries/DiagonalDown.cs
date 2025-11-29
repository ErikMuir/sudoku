namespace Sudoku.Generation.Symmetries;

public class DiagonalDown : ISymmetry
{
    private DiagonalDown() { }

    public static readonly ISymmetry Symmetry;

    static DiagonalDown()
    {
        Symmetry = new DiagonalDown();
    }

    public SymmetryType Type => SymmetryType.DiagonalDown;

    public int[] GetReflections(int cellIndex)
    {
        List<int> reflections = new() { cellIndex };
        int row = cellIndex.GetRowIndex();
        int col = cellIndex.GetColIndex();
        if (row != col)
        {
            int reflectedIndex = (col * Puzzle.UnitSize) + row;
            reflections.Add(reflectedIndex);
        }
        return reflections.ToArray();
    }
}
