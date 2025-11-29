namespace Sudoku.Console;

public static class InputPuzzle
{
    private static readonly Confirm _confirm = new("Is this correct?", true);
    private static readonly FluentConsole _console = new();

    public static Puzzle Run()
    {
        var rows = new List<string>();
        do
        {
            _console.WriteLine("Input a Sudoku puzzle one line at a time. Enter a period (.) for empty cells.");
            for (var i = 0; i < Puzzle.UnitSize; i++)
                rows.Add(InputRow(i));
        } while (!_confirm.Run());
        var puzzle = _parsePuzzle(rows);
        PrintPuzzle.Run(puzzle);
        _console.Success("Puzzle is now in memory.");
        return puzzle;
    }

    private static string InputRow(int i)
    {
        var sb = new StringBuilder();
        _console.Write($"row {i + 1}: ");
        while (sb.Length < Puzzle.UnitSize)
            InputCell(ref sb);
        _console.LineFeed();
        return sb.ToString();
    }

    private static void InputCell(ref StringBuilder sb)
    {
        var key = _console.ReadKey(true);
        switch (key.Key)
        {
            case ConsoleKey.Escape:
                throw new SudokuException("Puzzle input terminated");
            case ConsoleKey.Backspace:
                if (sb.Length > 0)
                {
                    sb.Length--;
                    _console.Write("\b \b");
                }
                break;
            case ConsoleKey.OemPeriod:
            case ConsoleKey.Decimal:
                sb.Append(key.KeyChar);
                _console.Write(key.KeyChar);
                break;
            default:
                var allowedChars = ".123456789".ToCharArray();
                if (allowedChars.Contains(key.KeyChar))
                {
                    sb.Append(key.KeyChar);
                    _console.Write(key.KeyChar);
                }
                break;
        }
    }

    private static Puzzle _parsePuzzle(List<string> rows)
    {
        var sb = new StringBuilder();
        rows.ForEach(row => sb.AppendLine(row));
        var puzzleString = sb.ToString();
        return Sdk.Serializer.Deserialize(puzzleString);
    }
}
