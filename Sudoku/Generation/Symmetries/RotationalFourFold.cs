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
        List<int> reflections = new() { cellIndex };
        int row = cellIndex.GetRowIndex();
        int col = cellIndex.GetColIndex();
        int axis = Puzzle.ReflectiveIndex;
        if (row != axis || col != axis)
        {
            int reflectedIndex = cellIndex;
            for (int i = 0; i < 3; i++)
            {
                reflectedIndex = _rotateCell(reflectedIndex);
                reflections.Add(reflectedIndex);
            }
        }
        return reflections.ToArray();
    }

    private int _rotateCell(int cellIndex)
    {
        int row = cellIndex.GetRowIndex();
        int col = cellIndex.GetColIndex();
        int targetRow = col;
        int targetCol = (Puzzle.UnitSize - 1) - row;
        int targetIndex = (targetRow * Puzzle.UnitSize) + targetCol;
        return targetIndex;
    }
}
