using System;
using System.Collections.Generic;
using System.Text;
using MuirDev.ConsoleTools;

namespace Sudoku.CommandLine
{
    public static class PrintPuzzle
    {
        const string GridBorder = " ----------------------------- ";
        const string GridDivide = "|---------+---------+---------|";
        const char VerticalLine = '|';
        private static readonly FluentConsole _console = new();

        public static void Run(Puzzle puzzle)
        {
            List<string> rows = new();
            for (int row = 0; row < puzzle.Length; row++)
            {
                List<int[]> rowCells = new();
                for (int col = 0; col < puzzle.Length; col++)
                {
                    int cellIndex = (row * puzzle.Length) + col;
                    rowCells.Add(puzzle.Cells[cellIndex]);
                }
                string rowString = GridRow(rowCells);
                rows.Add(rowString);
            }

            _console
                .LineFeed()
                .WriteLine(GridBorder)
                .WriteLine(rows[0])
                .WriteLine(rows[1])
                .WriteLine(rows[2])
                .WriteLine(GridDivide)
                .WriteLine(rows[3])
                .WriteLine(rows[4])
                .WriteLine(rows[5])
                .WriteLine(GridDivide)
                .WriteLine(rows[6])
                .WriteLine(rows[7])
                .WriteLine(rows[8])
                .WriteLine(GridBorder)
                .LineFeed();
        }

        private static string GridRow(List<int[]> row)
        {
            List<string> cells = new();
            for (int i = 0; i < row.Count; i++)
                cells.Add(GridCell(row[i]));

            StringBuilder sb = new();
            sb.Append(VerticalLine);
            sb.Append(cells[0]);
            sb.Append(cells[1]);
            sb.Append(cells[2]);
            sb.Append(VerticalLine);
            sb.Append(cells[3]);
            sb.Append(cells[4]);
            sb.Append(cells[5]);
            sb.Append(VerticalLine);
            sb.Append(cells[6]);
            sb.Append(cells[7]);
            sb.Append(cells[8]);
            sb.Append(VerticalLine);

            return sb.ToString();
        }

        private static string GridCell(int[] cell) => cell.Length == 1 ? $" {Math.Abs(cell[0])} " : "   ";
    }
}
