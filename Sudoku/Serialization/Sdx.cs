namespace Sudoku.Serialization;

public class Sdx : ISerializer
{
    private static readonly Regex _sdxPattern = new("^(u?[1-9]* ){8}u?[1-9]*(\r\n?|\n)?$", RegexOptions.Multiline);
    private static readonly Regex _cluePattern = new("^[1-9]$");
    private static readonly Regex _filledPattern = new("^u[1-9]$");
    private static readonly Regex _emptyPattern = new("^1?2?3?4?5?6?7?8?9?$");

    private Sdx() { }

    public static readonly ISerializer Serializer;

    static Sdx()
    {
        Serializer = new Sdx();
    }

    public string FileExtension => "sdx";

    public Puzzle Deserialize(string puzzleString)
    {
        if (!_sdxPattern.SafeIsMatch(puzzleString))
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

    public string Serialize(Puzzle puzzle)
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
        if (_cluePattern.SafeIsMatch(cell))
            return CellType.Clue;
        if (_filledPattern.SafeIsMatch(cell))
            return CellType.Filled;
        if (_emptyPattern.SafeIsMatch(cell))
            return CellType.Empty;
        return CellType.Invalid;
    }
}
