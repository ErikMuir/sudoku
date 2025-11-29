namespace Sudoku.Console;

public static class InputPuzzle
{
    private static readonly Confirm _confirm = new("Is this correct?", true);
    private static readonly FluentConsole _console = new();

    public static Puzzle Run()
    {
        List<string> rows = new();
        do
        {
            _console.WriteLine("Input a Sudoku puzzle one line at a time. Enter a period (.) for empty cells.");
            for (int i = 0; i < Puzzle.UnitSize; i++)
                rows.Add(_inputRow(i));
        } while (!_confirm.Run());
        Puzzle puzzle = _parsePuzzle(rows);
        PrintPuzzle.Run(puzzle);
        _console.Success("Puzzle is now in memory.");
        return puzzle;
    }

    private static string _inputRow(int i)
    {
        StringBuilder sb = new();
        _console.Write($"row {i + 1}: ");
        while (sb.Length < Puzzle.UnitSize)
            _inputCell(ref sb);
        _console.LineFeed();
        return sb.ToString();
    }

    private static void _inputCell(ref StringBuilder sb)
    {
        ConsoleKeyInfo key = _console.ReadKey(true);
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
                char[] allowedChars = ".123456789".ToCharArray();
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
        StringBuilder sb = new();
        rows.ForEach(row => sb.AppendLine(row));
        string puzzleString = sb.ToString();
        return Sdk.Serializer.Deserialize(puzzleString);
    }
}
