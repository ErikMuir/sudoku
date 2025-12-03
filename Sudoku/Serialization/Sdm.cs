namespace Sudoku.Serialization;

public partial class Sdm : Serializer
{
    public static readonly Serializer Serializer;

    private Sdm() { }

    static Sdm()
    {
        Serializer = new Sdm();
    }

    public override string FileExtension => "sdm";

    public override Puzzle Deserialize(string puzzleString)
    {
        if (!SdmPattern().SafeIsMatch(puzzleString))
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

    public override string Serialize(Puzzle puzzle)
    {
        var sb = new StringBuilder();
        for (var i = 0; i < Puzzle.TotalCells; i++)
            sb.Append(SerializeCell(puzzle.Cells[i]));
        return sb.ToString();
    }

    public List<Puzzle> DeserializeMultiple(string puzzleString)
    {
        if (puzzleString.IsNullOrWhiteSpace())
            throw new SudokuException("Invalid sdm file format");

        return [.. puzzleString
            .Split(SerializationUtils.NewLines, StringSplitOptions.None)
            .Select(Deserialize)];
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

    [GeneratedRegex("^.{81}$")]
    private static partial Regex SdmPattern();
}
