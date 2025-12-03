namespace Sudoku.Generation.Symmetries;

public class Horizontal : Symmetry
{
    private Horizontal() { }

    static Horizontal()
    {
        Symmetry = new Horizontal();
    }

    public static readonly Symmetry Symmetry;

    public override SymmetryType Type => SymmetryType.Horizontal;

    public override int[] GetReflections(int cellIndex)
    {
        var row = cellIndex.GetRowIndex();
        var col = cellIndex.GetColIndex();

        var reflections = new List<int> { cellIndex };

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
