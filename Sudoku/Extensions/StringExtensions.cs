namespace Sudoku.Extensions;

public static class StringExtensions
{
    public static bool IsNullOrWhiteSpace(this string? value) => value == null || value.Trim() == string.Empty;
}
