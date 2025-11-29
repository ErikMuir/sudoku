namespace Sudoku.Generation.Symmetries;

public class DiagonalUp : ISymmetry
{
    private DiagonalUp() { }

    public static readonly ISymmetry Symmetry;

    static DiagonalUp()
    {
        Symmetry = new DiagonalUp();
    }

    public SymmetryType Type => SymmetryType.DiagonalUp;

    public int[] GetReflections(int cellIndex)
    {
        List<int> reflections = [cellIndex];
        var row = cellIndex.GetRowIndex();
        var col = cellIndex.GetColIndex();
        var lastIndex = Puzzle.UnitSize - 1;
        if (row + col != lastIndex)
        {
            var reflectedRow = lastIndex - col;
            var reflectedCol = lastIndex - row;
            var reflectedIndex = (reflectedRow * Puzzle.UnitSize) + reflectedCol;
            reflections.Add(reflectedIndex);
        }
        return [.. reflections];
    }
}
