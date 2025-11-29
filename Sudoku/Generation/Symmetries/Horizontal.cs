namespace Sudoku.Generation.Symmetries;

public class Horizontal : ISymmetry
{
    private Horizontal() { }

    public static readonly ISymmetry Symmetry;

    static Horizontal()
    {
        Symmetry = new Horizontal();
    }

    public SymmetryType Type => SymmetryType.Horizontal;

    public int[] GetReflections(int cellIndex)
    {
        List<int> reflections = [cellIndex];
        var row = cellIndex.GetRowIndex();
        var col = cellIndex.GetColIndex();
        var lastIndex = Puzzle.UnitSize - 1;
        if (row != Puzzle.ReflectiveIndex)
        {
            var reflectedRow = lastIndex - row;
            var reflectedIndex = (reflectedRow * Puzzle.UnitSize) + col;
            reflections.Add(reflectedIndex);
        }
        return [.. reflections];
    }
}
