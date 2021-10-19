using System;
using System.Collections.Generic;
using System.Text;
using MuirDev.ConsoleTools;

namespace Sudoku.CommandLine
{
    public static class InputPuzzle
    {
        private static readonly FluentConsole _console = new();

        public static Puzzle Run()
        {
            List<string> rows = new();
            do
            {
                _console.WriteLine("Input a Sudoku puzzle one line at a time. Enter 0 for empty cells.");
                for (int i = 1; i < 10; i++)
                {
                    string row = InputRow(i);
                    rows.Add(row);
                }
            } while (!Confirm(rows));
            return ParsePuzzle(rows);
        }

        private static string InputRow(int i)
        {
            StringBuilder sb = new();
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
            StringBuilder sb = new();
            for (int row = 0; row < 9; row++)
            {
                for (int col = 0; col < 9; col++)
                {
                    int val = rows[row][col];
                    int clue = val == 0 ? 0 : 1;
                    sb.Append($"{col}{row}{val}{clue},");
                }
            }
            sb.Length--;
            string puzzleString = sb.ToString();
            return Puzzle.Parse(puzzleString);
        }
    }
}
