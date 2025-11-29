namespace Sudoku.Generation.Symmetries;

public class Vertical : ISymmetry
{
    private Vertical() { }

    public static readonly ISymmetry Symmetry;

    static Vertical()
    {
        Symmetry = new Vertical();
    }

    public SymmetryType Type => SymmetryType.Vertical;

    public int[] GetReflections(int cellIndex)
    {
        List<int> reflections = [cellIndex];
        var row = cellIndex.GetRowIndex();
        var col = cellIndex.GetColIndex();
        var lastIndex = Puzzle.UnitSize - 1;
        if (col != Puzzle.ReflectiveIndex)
        {
            var reflectedCol = lastIndex - col;
            var reflectedIndex = (row * Puzzle.UnitSize) + reflectedCol;
            reflections.Add(reflectedIndex);
        }
        return [.. reflections];
    }
}
