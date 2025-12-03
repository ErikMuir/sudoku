namespace Sudoku.Serialization;

public partial class Sdx : Serializer
{
    public static readonly Serializer Serializer;

    private Sdx() { }

    static Sdx()
    {
        Serializer = new Sdx();
    }

    public override string FileExtension => "sdx";

    public override Puzzle Deserialize(string puzzleString)
    {
        if (!SdxPattern().SafeIsMatch(puzzleString))
            throw new SudokuException("Invalid sdx file format");

        var lines = puzzleString
            .Split(SerializationUtils.NewLines, StringSplitOptions.None)
            .ToArray();

        var puzzle = new Puzzle();
        for (var row = 0; row < Puzzle.UnitSize; row++)
        {
            var cells = lines[row].Split(' ');
            for (var col = 0; col < Puzzle.UnitSize; col++)
            {
                var index = (row * Puzzle.UnitSize) + col;
                var cellString = cells[col];
                puzzle.Cells[index] = DeserializeCell(cellString, col, row);
            }
        }
        return puzzle;
    }

    public override string Serialize(Puzzle puzzle)
    {
        var sb = new StringBuilder();
        for (var row = 0; row < Puzzle.UnitSize; row++)
        {
            var cells = new List<string>();
            for (var col = 0; col < Puzzle.UnitSize; col++)
            {
                cells.Add(SerializeCell(puzzle.GetCell(row, col)));
            }
            sb.AppendLine(string.Join(" ", cells));
        }
        return sb.ToString();
    }

    private static string SerializeCell(Cell cell)
        => cell.Value != null
            ? $"{(cell.IsClue ? "" : "u")}{cell.Value}"
            : string.Join("", cell.Candidates.Select(x => $"{x}"));

    private Cell DeserializeCell(string cellString, int col, int row)
    {
        var cellType = GetCellType(cellString);
        if (cellType == CellType.Invalid)
            throw new SudokuException("Invalid sdx file format");

        var cell = cellType switch
        {
            CellType.Clue => new Clue(row, col, int.Parse(cellString)),
            CellType.Filled => new Cell(row, col, int.Parse(cellString.Replace("u", ""))),
            CellType.Empty => new Cell(row, col),
            _ => throw new NotImplementedException("Unsupported cell type"),
        };

        if (cellType == CellType.Empty)
        {
            cellString.ToList().ForEach(x => cell.AddCandidate(int.Parse($"{x}")));
        }

        return cell;
    }

    private static CellType GetCellType(string cell)
    {
        if (CluePattern().SafeIsMatch(cell))
            return CellType.Clue;
        if (FilledPattern().SafeIsMatch(cell))
            return CellType.Filled;
        if (EmptyPattern().SafeIsMatch(cell))
            return CellType.Empty;
        return CellType.Invalid;
    }

    [GeneratedRegex(@"^(u?[1-9]* ){8}u?[1-9]*(\r\n?|\n)?$", RegexOptions.Multiline)]
    private static partial Regex SdxPattern();

    [GeneratedRegex(@"^[1-9]$")]
    private static partial Regex CluePattern();

    [GeneratedRegex(@"^u[1-9]$")]
    private static partial Regex FilledPattern();

    [GeneratedRegex(@"^1?2?3?4?5?6?7?8?9?$")]
    private static partial Regex EmptyPattern();
}
