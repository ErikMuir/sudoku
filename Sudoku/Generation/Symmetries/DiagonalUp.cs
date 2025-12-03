namespace Sudoku.Generation.Symmetries;

public class DiagonalUp : Symmetry
{
    private DiagonalUp() { }

    static DiagonalUp()
    {
        Symmetry = new DiagonalUp();
    }

    public static readonly Symmetry Symmetry;

    public override SymmetryType Type => SymmetryType.DiagonalUp;

    public override int[] GetReflections(int cellIndex)
    {
        var row = cellIndex.GetRowIndex();
        var col = cellIndex.GetColIndex();

        var reflections = new List<int> { cellIndex };

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
