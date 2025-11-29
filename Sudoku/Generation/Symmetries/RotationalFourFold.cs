namespace Sudoku.Generation.Symmetries;

public class RotationalFourFold : ISymmetry
{
    private RotationalFourFold() { }

    public static readonly ISymmetry Symmetry;

    static RotationalFourFold()
    {
        Symmetry = new RotationalFourFold();
    }

    public SymmetryType Type => SymmetryType.RotationalFourFold;

    public int[] GetReflections(int cellIndex)
    {
        List<int> reflections = [cellIndex];
        var row = cellIndex.GetRowIndex();
        var col = cellIndex.GetColIndex();
        var axis = Puzzle.ReflectiveIndex;
        if (row != axis || col != axis)
        {
            var reflectedIndex = cellIndex;
            for (var i = 0; i < 3; i++)
            {
                reflectedIndex = RotateCell(reflectedIndex);
                reflections.Add(reflectedIndex);
            }
        }
        return [.. reflections];
    }

    private static int RotateCell(int cellIndex)
    {
        var row = cellIndex.GetRowIndex();
        var col = cellIndex.GetColIndex();
        var lastIndex = Puzzle.UnitSize - 1;
        var targetRow = col;
        var targetCol = lastIndex - row;
        var targetIndex = (targetRow * Puzzle.UnitSize) + targetCol;
        return targetIndex;
    }
}
