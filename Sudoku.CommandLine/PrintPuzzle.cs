using System.Collections.Generic;
using System.Text;
using MuirDev.ConsoleTools;
using Sudoku.Serializers;

namespace Sudoku.CommandLine
{
    public static class PrintPuzzle
    {
        const string GridBorder = " ----------------------------- ";
        const string GridDivide = "|---------+---------+---------|";
        const char VerticalLine = '|';
        private static readonly SdkSerializer _serializer = new();
        private static readonly FluentConsole _console = new();
        private static readonly Dictionary<char, string> _menuOptions = new()
        {
            { '1', "Grid" },
            { '2', "Serialized" },
            { '0', "Go back" },
        };
        private static readonly Menu _menu = new(_menuOptions, "Choose a print format:");

        public static void Run(Puzzle puzzle)
        {
            _console.LineFeed();
            char choice = _menu.Run();
            _console.LineFeed();

            switch (choice)
            {
                case '1': Grid(puzzle); break;
                case '2': Serialized(puzzle); break;
                case '0': throw new MenuExitException();
                default: throw new SudokuException("Invalid option");
            }
        }

        private static void Grid(Puzzle puzzle)
        {
            List<string> rows = new();
            Utils.Loop(i =>
            {
                List<Cell> rowCells = puzzle.GetRow(i);
                string rowString = GridRow(rowCells);
                rows.Add(rowString);
            });

            StringBuilder sb = new();
            sb.AppendLine(GridBorder);
            sb.AppendLine(rows[0]);
            sb.AppendLine(rows[1]);
            sb.AppendLine(rows[2]);
            sb.AppendLine(GridDivide);
            sb.AppendLine(rows[3]);
            sb.AppendLine(rows[4]);
            sb.AppendLine(rows[5]);
            sb.AppendLine(GridDivide);
            sb.AppendLine(rows[6]);
            sb.AppendLine(rows[7]);
            sb.AppendLine(rows[8]);
            sb.AppendLine(GridBorder);

            _console.Write(sb.ToString());
        }

        private static string GridRow(List<Cell> row)
        {
            List<string> cells = new();
            Utils.Loop(i => cells.Add(GridCell(row[i])));

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

        private static string GridCell(Cell cell) => cell.Value.HasValue ? $" {cell.Value} " : "   ";

        private static void Serialized(Puzzle puzzle)
        {
            _console.WriteLine(_serializer.Serialize(puzzle));
        }
    }
}
