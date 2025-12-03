namespace Sudoku.Generation.Symmetries;

public class RotationalTwoFold : Symmetry
{
    private RotationalTwoFold() { }

    static RotationalTwoFold()
    {
        Symmetry = new RotationalTwoFold();
    }

    public static readonly Symmetry Symmetry;

    public override SymmetryType Type => SymmetryType.RotationalTwoFold;

    public override int[] GetReflections(int cellIndex)
    {
        var row = cellIndex.GetRowIndex();
        var col = cellIndex.GetColIndex();
        var axis = Puzzle.ReflectiveIndex;

        var reflections = new List<int> { cellIndex };

        var lastIndex = Puzzle.UnitSize - 1;
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
