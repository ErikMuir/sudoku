using System.Text;

namespace Sudoku.Serialization
{
    public static class Sdk
    {
        public static string ToSdkString(this Cell cell)
            => cell.IsClue ? $"{cell.Value}" : ".";

        public static string ToSdkString(this Cell cell, char placeholder)
        {
            if (char.IsDigit(placeholder) && placeholder != '0')
                throw new SudokuException("Invalid placeholder");
            return cell.IsClue ? $"{cell.Value}" : $"{placeholder}";
        }

        public static string ToSdkString(this Puzzle puzzle)
        {
            var sb = new StringBuilder();
            var serializedMetadata = puzzle.Metadata.Serialize();
            sb.Append(serializedMetadata);
            for (var row = 0; row < Constants.Size; row++)
            {
                for (var col = 0; col < Constants.Size; col++)
                {
                    var serializedCell = puzzle.GetCell(col, row).ToSdkString();
                    sb.Append(serializedCell);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public static string Serialize(this Metadata metadata)
        {
            if (metadata == null) return null;

            var sb = new StringBuilder();
            if (metadata?.Author != null) sb.AppendLine($"{MetadataTokens.Author}{metadata.Author.RemoveNewLines()}");
            if (metadata?.Description != null) sb.AppendLine($"{MetadataTokens.Description}{metadata.Description.RemoveNewLines()}");
            if (metadata?.Comment != null) sb.AppendLine($"{MetadataTokens.Comment}{metadata.Comment.RemoveNewLines()}");
            if (metadata?.DatePublished != null) sb.AppendLine($"{MetadataTokens.DatePublished}{metadata.DatePublished.ToUtcString()}");
            if (metadata?.Source != null) sb.AppendLine($"{MetadataTokens.Source}{metadata.Source.RemoveNewLines()}");
            if (metadata?.Level != Level.Uninitialized) sb.AppendLine($"{MetadataTokens.Level}{metadata.Level}");
            if (metadata?.SourceUrl != null) sb.AppendLine($"{MetadataTokens.SourceUrl}{metadata.SourceUrl.ToString()}");
            return sb.ToString();
        }

        public static Cell ParseSdkCell(this string cellString)
        {
            
            return null;
        }
    }
}
