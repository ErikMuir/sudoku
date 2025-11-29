namespace Sudoku.Serialization;

public class Sdm : ISerializer
{
    private static readonly Regex _sdmPattern = new("^.{81}$");

    private Sdm() { }

    public static readonly ISerializer Serializer;

    static Sdm()
    {
        Serializer = new Sdm();
    }

    public string FileExtension => "sdm";

    public Puzzle Deserialize(string puzzleString)
    {
        if (!_sdmPattern.SafeIsMatch(puzzleString))
            throw new SudokuException("Invalid sdm file format");

        var puzzle = new Puzzle();
        for (var i = 0; i < Puzzle.TotalCells; i++)
        {
            var row = i.GetRowIndex();
            var col = i.GetColIndex();
            var _ = int.TryParse($"{puzzleString[i]}", out var val);
            if (val > 0) puzzle.Cells[i] = new Clue(row, col, val);
        }
        return puzzle;
    }

    public string Serialize(Puzzle puzzle)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < Puzzle.TotalCells; i++)
            sb.Append(SerializeCell(puzzle.Cells[i]));
        return sb.ToString();
    }

    public List<Puzzle> DeserializeMultiple(string puzzleString)
    {
        if (puzzleString == null)
            throw new SudokuException("Invalid sdm file format");

        return [.. puzzleString
            .Split(SerializationUtils.NewLines, StringSplitOptions.None)
            .Select(x => Deserialize(x))];
    }

    public string SerializeMultiple(List<Puzzle> puzzles)
    {
        var sb = new StringBuilder();
        foreach (var puzzle in puzzles)
        {
            var serializedPuzzle = Serialize(puzzle);
            sb.AppendLine(serializedPuzzle);
        }
        return sb.ToString();
    }

    private static string SerializeCell(Cell cell) => cell.Value != null ? $"{cell.Value}" : "0";
}
