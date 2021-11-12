using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Sudoku.Logic;

namespace Sudoku.Serialization
{
    public class SdkSerializer : ISerializer
    {
        private static readonly Regex _sdkLinePattern = new(@"^[1-9\.]{9}$");

        public string FileExtension => "sdk";

        public string Serialize(Puzzle puzzle)
        {
            StringBuilder sb = new();
            string serializedMetadata = Serialize(puzzle.Metadata);
            sb.Append(serializedMetadata);
            for (int row = 0; row < Puzzle.UnitSize; row++)
            {
                for (int col = 0; col < Puzzle.UnitSize; col++)
                {
                    sb.Append(Serialize(puzzle.GetCell(row, col)));
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private string Serialize(Metadata metadata)
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

        private string Serialize(Cell cell) => cell.Value is not null ? $"{cell.Value}" : ".";

        public Puzzle Deserialize(string puzzleString)
        {
            if (puzzleString is null) return null;

            string[] rows = puzzleString
                .Trim()
                .Split(SerializationUtils.NewLines, StringSplitOptions.None)
                .Where(x => x.Substring(0, 1) != MetadataTokens.Prefix)
                .ToArray();

            if (rows.Length != Puzzle.UnitSize || rows.Any(row => !_sdkLinePattern.IsMatch(row)))
                throw new SudokuException("Invalid sdk file format");

            Puzzle puzzle = new();
            puzzle.Metadata = DeserializeMetadata(puzzleString);
            for (int row = 0; row < Puzzle.UnitSize; row++)
            {
                string line = rows[row];
                for (int col = 0; col < Puzzle.UnitSize; col++)
                {
                    int.TryParse($"{line[col]}", out int val);
                    if (val > 0) puzzle.Cells[(row * Puzzle.UnitSize) + col] = new Clue(row, col, val);
                }
            }
            return puzzle;
        }

        private Metadata DeserializeMetadata(string puzzleString)
        {
            if (puzzleString is null) return null;

            Metadata metadata = new();
            IEnumerable<string> lines = puzzleString
                .Split(SerializationUtils.NewLines, StringSplitOptions.None)
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
