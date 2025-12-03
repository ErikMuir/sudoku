namespace Sudoku.Extensions;

public static class DateTimeExtensions
{
    public static bool IsNullOrDefault(this DateTime? value) => value == null || value == default;
}
