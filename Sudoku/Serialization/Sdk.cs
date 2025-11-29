namespace Sudoku.Serialization;

public class Sdk : ISerializer
{
    private static readonly Regex _sdkLinePattern = new(@"^[1-9\.]{9}$");

    private Sdk() { }

    public static readonly ISerializer Serializer;

    static Sdk()
    {
        Serializer = new Sdk();
    }

    public string FileExtension => "sdk";

    public Puzzle Deserialize(string puzzleString)
    {
        if (puzzleString is null) return null;

        var rows = puzzleString
            .Trim()
            .Split(SerializationUtils.NewLines, StringSplitOptions.None)
            .Where(x => x.Substring(0, 1) != MetadataTokens.Prefix)
            .ToArray();

        if (rows.Length != Puzzle.UnitSize || rows.Any(row => !_sdkLinePattern.IsMatch(row)))
            throw new SudokuException("Invalid sdk file format");

        var puzzle = new Puzzle
        {
            Metadata = DeserializeMetadata(puzzleString)
        };
        for (var row = 0; row < Puzzle.UnitSize; row++)
        {
            var line = rows[row];
            for (var col = 0; col < Puzzle.UnitSize; col++)
            {
                var _ = int.TryParse($"{line[col]}", out int val);
                if (val > 0) puzzle.Cells[(row * Puzzle.UnitSize) + col] = new Clue(row, col, val);
            }
        }
        return puzzle;
    }

    public string Serialize(Puzzle puzzle)
    {
        var sb = new StringBuilder();
        var serializedMetadata = SerializeMetadata(puzzle.Metadata);
        sb.Append(serializedMetadata);
        for (var row = 0; row < Puzzle.UnitSize; row++)
        {
            for (var col = 0; col < Puzzle.UnitSize; col++)
            {
                sb.Append(SerializeCell(puzzle.GetCell(row, col)));
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    private static string SerializeMetadata(Metadata metadata)
    {
        if (metadata == null) return null;

        var sb = new StringBuilder();
        if (metadata.Author != null) sb.AppendLine(metadata.Author.SerializeMetadataEntry(MetadataTokens.Author));
        if (metadata.Description != null) sb.AppendLine(metadata.Description.SerializeMetadataEntry(MetadataTokens.Description));
        if (metadata.Comment != null) sb.AppendLine(metadata.Comment.SerializeMetadataEntry(MetadataTokens.Comment));
        if (metadata.DatePublished != default) sb.AppendLine(metadata.DatePublished.SerializeMetadataEntry(MetadataTokens.DatePublished));
        if (metadata.Source != null) sb.AppendLine(metadata.Source.SerializeMetadataEntry(MetadataTokens.Source));
        if (metadata.Level != Level.Uninitialized) sb.AppendLine(metadata.Level.SerializeMetadataEntry());
        if (metadata.SourceUrl != null) sb.AppendLine(metadata.SourceUrl.SerializeMetadataEntry(MetadataTokens.SourceUrl));
        if (metadata.Symmetry != SymmetryType.Uninitialized) sb.AppendLine(metadata.Symmetry.SerializeMetadataEntry());
        return sb.ToString();
    }

    private static string SerializeCell(Cell cell) => cell.Value != null ? $"{cell.Value}" : ".";

    private static Metadata DeserializeMetadata(string puzzleString)
    {
        if (puzzleString == null) return null;

        var metadata = new Metadata();
        var lines = puzzleString
            .Split(SerializationUtils.NewLines, StringSplitOptions.None)
            .Where(x => x.Length >= 2)
            .Where(x => x.Substring(0, 1) == MetadataTokens.Prefix);

        foreach (var line in lines)
        {
            var token = line.Substring(1, 1);
            var value = line.Substring(2);
            try
            {
                switch (token)
                {
                    case MetadataTokens.Author:
                        metadata.Author = value;
                        break;
                    case MetadataTokens.Description:
                        metadata.Description = value;
                        break;
                    case MetadataTokens.Comment:
                        metadata.Comment = value;
                        break;
                    case MetadataTokens.DatePublished:
                        metadata.DatePublished = DateTime.Parse(value);
                        break;
                    case MetadataTokens.Source:
                        metadata.Source = value;
                        break;
                    case MetadataTokens.Level:
                        metadata.Level = (Level)Enum.Parse(typeof(Level), value, true);
                        break;
                    case MetadataTokens.SourceUrl:
                        metadata.SourceUrl = new Uri(value);
                        break;
                    case MetadataTokens.Symmetry:
                        metadata.Symmetry = (SymmetryType)Enum.Parse(typeof(SymmetryType), value, true);
                        break;
                }
            }
            catch (Exception) { }
        }
        return metadata;
    }
}
