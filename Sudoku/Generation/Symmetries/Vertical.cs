namespace Sudoku.Generation.Symmetries;

public class Vertical : Symmetry
{
    private Vertical() { }

    static Vertical()
    {
        Symmetry = new Vertical();
    }

    public static readonly Symmetry Symmetry;

    public override SymmetryType Type => SymmetryType.Vertical;

    public override int[] GetReflections(int cellIndex)
    {
        var row = cellIndex.GetRowIndex();
        var col = cellIndex.GetColIndex();

        var reflections = new List<int> { cellIndex };

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
