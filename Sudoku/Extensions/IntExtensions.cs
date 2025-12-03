namespace Sudoku.Extensions;

public static class IntExtensions
{
    public static void Loop(this int count, Action<int> action)
    {
        for (var i = 0; i < count; i++)
            action(i);
    }

    public static bool LoopAnd(this int count, Func<int, bool> func)
    {
        for (var i = 0; i < count; i++)
            if (!func?.Invoke(i) ?? false)
                return false;
        return true;
    }

    public static bool LoopOr(this int count, Func<int, bool> func)
    {
        for (var i = 0; i < count; i++)
            if (func?.Invoke(i) ?? false)
                return true;
        return false;
    }

    public static int GetRowIndex(this int cellIndex) => cellIndex / Puzzle.UnitSize;

    public static int GetColIndex(this int cellIndex) => cellIndex % Puzzle.UnitSize;
}
