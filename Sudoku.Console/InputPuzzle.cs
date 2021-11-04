using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MuirDev.ConsoleTools;
using Sudoku.Serialization;

namespace Sudoku.Console
{
    public static class InputPuzzle
    {
        private static readonly SdkSerializer _serializer = new();
        private static readonly Confirm _confirm = new("Is this correct?", true);
        private static readonly FluentConsole _console = new();

        public static Puzzle Run()
        {
            List<string> rows = new();
            do
            {
                _console.WriteLine("Input a Sudoku puzzle one line at a time. Enter a period (.) for empty cells.");
                Utils.Loop(i => rows.Add(InputRow(i)));
            } while (!_confirm.Run());
            Puzzle puzzle = ParsePuzzle(rows);
            PrintPuzzle.Run(puzzle);
            _console.Success("Puzzle is now in memory.");
            return puzzle;
        }

        private static string InputRow(int i)
        {
            StringBuilder sb = new();
            _console.Write($"row {i + 1}: ");
            while (sb.Length < Constants.UnitSize)
                InputCell(ref sb);
            _console.LineFeed();
            return sb.ToString();
        }

        private static void InputCell(ref StringBuilder sb)
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

        private static Puzzle ParsePuzzle(List<string> rows)
        {
            StringBuilder sb = new();
            rows.ForEach(row => sb.AppendLine(row));
            string puzzleString = sb.ToString();
            return _serializer.Deserialize(puzzleString);
        }
    }
}
