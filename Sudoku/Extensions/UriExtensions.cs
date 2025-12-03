namespace Sudoku.Extensions;

public static class UriExtensions
{
    public static bool IsNullOrDefault(this Uri? value) => value == null || value == default;
}
