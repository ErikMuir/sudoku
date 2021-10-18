using System;

namespace Sudoku.Serialization
{
    public static class Utils
    {
        public static string RemoveNewLines(this string val)
            => val.Replace('\n', ' ').Replace('\r', ' ');

        public static string SerializeMetadataEntry(this string value, string token)
            => $"{MetadataTokens.Prefix}{token}{value.RemoveNewLines()}";

        public static string SerializeMetadataEntry(this DateTime value, string token)
            => $"{MetadataTokens.Prefix}{token}{value.ToUtcString()}";

        public static string SerializeMetadataEntry(this Uri value, string token)
            => $"{MetadataTokens.Prefix}{token}{value.ToString()}";

        public static string SerializeMetadataEntry(this Level value, string token)
            => $"{MetadataTokens.Prefix}{token}{value}";
    }
}
