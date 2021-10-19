using System;
using System.Text.RegularExpressions;

namespace Sudoku.Serializers
{
    public static class SerializationUtils
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

        public static bool SafeIsMatch(this Regex pattern, string value)
            => value is not null && pattern.IsMatch(value);
    }
}
