using System;
using System.Collections.Generic;
using System.Text;
using MuirDev.ConsoleTools;

namespace Sudoku.CommandLine
{
    public static class InputPuzzle
    {
        private static readonly FluentConsole _console = new FluentConsole();

        public static Puzzle Run()
        {
            var rows = new List<string>();
            do
            {
                _console.WriteLine("Input a Sudoku puzzle one line at a time. Enter 0 for empty cells.");
                for (var i = 1; i < 10; i++)
                {
                    var row = InputRow(i);
                    rows.Add(row);
                }
            } while (!Confirm(rows));
            return ParsePuzzle(rows);
        }

        private static string InputRow(int i)
        {
            var sb = new StringBuilder();
            ConsoleKeyInfo key;
            _console.Write($"row {i}: ");
            do
            {
                key = _console.ReadKey(true);
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
                    default:
                        if (short.TryParse(key.KeyChar.ToString(), out _))
                        {
                            sb.Append(key.KeyChar);
                            _console.Write(key.KeyChar);
                        }
                        break;
                }
            }
            while (sb.Length < 9);
            _console.LineFeed();
            return sb.ToString();
        }

        private static bool Confirm(List<string> rows) => new Confirm("Is this correct?", true).Run();

        private static Puzzle ParsePuzzle(List<string> rows)
        {
            var sb = new StringBuilder();
            for (var row = 0; row < 9; row++)
            {
                for (var col = 0; col < 9; col++)
                {
                    var val = rows[row][col];
                    var clue = $"{val}" == "0" ? "0" : "1";
                    sb.Append($"{col}{row}{val}{clue},");
                }
            }
            sb.Length--;
            var puzzleString = sb.ToString();
            return Puzzle.Parse(puzzleString);
        }
    }
}
