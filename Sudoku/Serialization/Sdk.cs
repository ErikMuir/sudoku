using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sudoku.Serialization
{
    public static class Sdk
    {
        public static string Serialize(Puzzle puzzle)
        {
            StringBuilder sb = new();
            string serializedMetadata = Serialize(puzzle.Metadata);
            sb.Append(serializedMetadata);
            for (int row = 0; row < Constants.Size; row++)
            {
                for (int col = 0; col < Constants.Size; col++)
                {
                    string serializedCell = Serialize(puzzle.GetCell(col, row));
                    sb.Append(serializedCell);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private static string Serialize(Metadata metadata)
        {
            if (metadata is null) return null;

            StringBuilder sb = new();
            if (metadata.Author is not null) sb.AppendLine(metadata.Author.SerializeMetadataEntry(MetadataTokens.Author));
            if (metadata.Description is not null) sb.AppendLine(metadata.Description.SerializeMetadataEntry(MetadataTokens.Description));
            if (metadata.Comment is not null) sb.AppendLine(metadata.Comment.SerializeMetadataEntry(MetadataTokens.Comment));
            if (metadata.DatePublished != default(DateTime)) sb.AppendLine(metadata.DatePublished.SerializeMetadataEntry(MetadataTokens.DatePublished));
            if (metadata.Source is not null) sb.AppendLine(metadata.Source.SerializeMetadataEntry(MetadataTokens.Source));
            if (metadata.Level != Level.Uninitialized) sb.AppendLine(metadata.Level.SerializeMetadataEntry(MetadataTokens.Level));
            if (metadata.SourceUrl is not null) sb.AppendLine(metadata.SourceUrl.SerializeMetadataEntry(MetadataTokens.SourceUrl));
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
            if (puzzleString is null) return null;

            string[] rows = puzzleString
                .Split(Constants.NewLines, StringSplitOptions.None)
                .Where(x => x.Length == Constants.Size)
                .Where(x => x.Substring(0, 1) != MetadataTokens.Prefix)
                .ToArray();

            if (rows.Length != Constants.Size)
                throw new SudokuException("Invalid sdk file format");

            Puzzle puzzle = new();
            puzzle.Metadata = DeserializeMetadata(puzzleString);
            for (int row = 0; row < rows.Length; row++)
            {
                string line = rows[row];
                for (int col = 0; col < Constants.Size; col++)
                {
                    if (int.TryParse($"{line[col]}", out int intValue))
                    {
                        puzzle.Cells[(row * 9) + col] = new Clue(col, row, intValue);
                    }
                }
            }
            return puzzle;
        }

        private static Metadata DeserializeMetadata(string puzzleString)
        {
            if (puzzleString is null) return null;

            Metadata metadata = new();
            IEnumerable<string> lines = puzzleString
                .Split(Constants.NewLines, StringSplitOptions.None)
                .Where(x => x.Length >= 2)
                .Where(x => x.Substring(0, 1) == MetadataTokens.Prefix);

            foreach (string line in lines)
            {
                string token = line.Substring(1, 1);
                string value = line.Substring(2);
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
