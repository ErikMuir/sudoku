using System;
using System.Linq;
using System.Text;

namespace Sudoku.Serialization
{
    public static class Sdk
    {
        public static string Serialize(Puzzle puzzle)
        {
            var sb = new StringBuilder();
            var serializedMetadata = Serialize(puzzle.Metadata);
            sb.Append(serializedMetadata);
            for (var row = 0; row < Constants.Size; row++)
            {
                for (var col = 0; col < Constants.Size; col++)
                {
                    var serializedCell = Serialize(puzzle.GetCell(col, row));
                    sb.Append(serializedCell);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private static string Serialize(Metadata metadata)
        {
            if (metadata == null) return null;

            var sb = new StringBuilder();
            if (metadata.Author != null) sb.AppendLine(metadata.Author.SerializeMetadataEntry(MetadataTokens.Author));
            if (metadata.Description != null) sb.AppendLine(metadata.Description.SerializeMetadataEntry(MetadataTokens.Description));
            if (metadata.Comment != null) sb.AppendLine(metadata.Comment.SerializeMetadataEntry(MetadataTokens.Comment));
            if (metadata.DatePublished != null) sb.AppendLine(metadata.DatePublished.SerializeMetadataEntry(MetadataTokens.DatePublished));
            if (metadata.Source != null) sb.AppendLine(metadata.Source.SerializeMetadataEntry(MetadataTokens.Source));
            if (metadata.Level != Level.Uninitialized) sb.AppendLine(metadata.Level.SerializeMetadataEntry(MetadataTokens.Level));
            if (metadata.SourceUrl != null) sb.AppendLine(metadata.SourceUrl.SerializeMetadataEntry(MetadataTokens.SourceUrl));
            return sb.ToString();
        }

        private static string Serialize(Cell cell) => Serialize(cell, '.');

        private static string Serialize(Cell cell, char placeholder)
        {
            if (char.IsDigit(placeholder) && placeholder != '0')
                throw new SudokuException("Invalid placeholder");
            return cell.IsClue ? $"{cell.Value}" : $"{placeholder}";
        }

        public static Puzzle Deserialize(string puzzleString)
        {
            if (puzzleString == null) return null;

            var rows = puzzleString
                .Split(Constants.NewLines, StringSplitOptions.None)
                .Where(x => x.Length == Constants.Size)
                .Where(x => x.Substring(0, 1) != MetadataTokens.Prefix)
                .ToArray();

            if (rows.Length != Constants.Size)
                throw new SudokuException("Invalid sdk file format");

            var puzzle = new Puzzle();
            puzzle.Metadata = DeserializeMetadata(puzzleString);
            for (var row = 0; row < rows.Length; row++)
            {
                var line = rows[row];
                for (var col = 0; col < Constants.Size; col++)
                {
                    if (int.TryParse($"{line[col]}", out var intValue))
                    {
                        puzzle.Cells[(row * 9) + col] = new Clue(col, row, intValue);
                    }
                }
            }
            return puzzle;
        }

        private static Metadata DeserializeMetadata(string puzzleString)
        {
            if (puzzleString == null) return null;

            var metadata = new Metadata();
            var lines = puzzleString
                .Split(Constants.NewLines, StringSplitOptions.None)
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
                    }
                }
                catch (Exception) { }
            }
            return metadata;
        }
    }
}
