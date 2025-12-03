namespace Sudoku.Generation.Symmetries;

public class DiagonalDown : Symmetry
{
    private DiagonalDown() { }

    static DiagonalDown()
    {
        Symmetry = new DiagonalDown();
    }

    public static readonly Symmetry Symmetry;

    public override SymmetryType Type => SymmetryType.DiagonalDown;

    public override int[] GetReflections(int cellIndex)
    {
        var row = cellIndex.GetRowIndex();
        var col = cellIndex.GetColIndex();

        var reflections = new List<int> { cellIndex };

        if (row != col)
        {
            var reflectedIndex = (col * Puzzle.UnitSize) + row;
            reflections.Add(reflectedIndex);
        }

        return [.. reflections];
    }
}
