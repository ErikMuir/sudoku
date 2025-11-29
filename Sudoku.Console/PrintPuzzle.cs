namespace Sudoku.Console;

public static class PrintPuzzle
{
    const string GridBorder = " ----------------------------- ";
    const string GridDivide = "|---------+---------+---------|";
    const char VerticalLine = '|';
    private static readonly FluentConsole _console = new();
    
    public static void Run(Puzzle puzzle)
    {
        var rows = new List<string>();
        for (var i = 0; i < Puzzle.UnitSize; i++)
        {
            var rowCells = puzzle.GetRow(i).ToArray();
            var rowString = GridRow(rowCells);
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

    private static string GridRow(Cell[] row)
    {
        var cells = new List<string>();
        for (var i = 0; i < Puzzle.UnitSize; i++)
            cells.Add(GridCell(row[i]));
        var sb = new StringBuilder();
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
}
