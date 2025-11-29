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
        List<int> reflections = [cellIndex];
        var row = cellIndex.GetRowIndex();
        var col = cellIndex.GetColIndex();
        if (row != col)
        {
            var reflectedIndex = (col * Puzzle.UnitSize) + row;
            reflections.Add(reflectedIndex);
        }
        return [.. reflections];
    }
}
