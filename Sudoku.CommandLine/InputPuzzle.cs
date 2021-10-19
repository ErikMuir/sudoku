using System;
using System.Collections.Generic;
using System.Text;
using MuirDev.ConsoleTools;
using Sudoku.Serializers;

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
                Utils.Loop(i => rows.Add(InputRow(i)));
            } while (!Confirm(rows));
            return ParsePuzzle(rows);
        }

        private static string InputRow(int i)
        {
            StringBuilder sb = new();
            ConsoleKeyInfo key;
            _console.Write($"row {i + 1}: ");
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
            Utils.Loop(Constants.TotalCells, i =>
            {
                int col = i % Constants.UnitSize;
                int row = i / Constants.UnitSize;
                int.TryParse($"{rows[row][col]}", out int val);
                int clue = val == 0 ? 0 : 1;
                sb.Append($"{val}{clue},");
            });
            sb.Length--;
            string puzzleString = sb.ToString();
            return PzlSerializer.Deserialize(puzzleString);
        }
    }
}
