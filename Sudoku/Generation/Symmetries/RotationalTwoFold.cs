namespace Sudoku.Generation.Symmetries;

public class RotationalTwoFold : ISymmetry
{
    private RotationalTwoFold() { }

    public static readonly ISymmetry Symmetry;

    static RotationalTwoFold()
    {
        Symmetry = new RotationalTwoFold();
    }

    public SymmetryType Type => SymmetryType.RotationalTwoFold;

    public int[] GetReflections(int cellIndex)
    {
        List<int> reflections = [cellIndex];
        var row = cellIndex.GetRowIndex();
        var col = cellIndex.GetColIndex();
        var lastIndex = Puzzle.UnitSize - 1;
        var axis = Puzzle.ReflectiveIndex;
        if (row != axis || col != axis)
        {
            var reflectedRow = lastIndex - row;
            var reflectedCol = lastIndex - col;
            var reflectedIndex = (reflectedRow * Puzzle.UnitSize) + reflectedCol;
            reflections.Add(reflectedIndex);
        }
        return [.. reflections];
    }
}
