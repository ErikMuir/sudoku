namespace Sudoku.Generation;

public interface ISymmetry
{
    SymmetryType Type { get; }

    int[] GetReflections(int cellIndex);
}
