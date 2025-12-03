namespace Sudoku.Serialization;

public static class SerializationUtils
{
    public static string[] NewLines => ["\r\n", "\r", "\n"];

    public static string RemoveNewLines(this string val)
        => val.Replace('\n', ' ').Replace('\r', ' ');

    public static string SerializeMetadataEntry(this string value, string token)
        => $"{MetadataTokens.Prefix}{token}{value.RemoveNewLines()}";

    public static string SerializeMetadataEntry(this DateTime value, string token)
        => $"{MetadataTokens.Prefix}{token}{value.ToUniversalTime():yyyy-MM-ddTHH:mm:ssZ}";

    public static string SerializeMetadataEntry(this Uri value, string token)
        => $"{MetadataTokens.Prefix}{token}{value}";

    public static string SerializeMetadataEntry(this Level value)
        => $"{MetadataTokens.Prefix}{MetadataTokens.Level}{value}";

    public static string SerializeMetadataEntry(this SymmetryType value)
        => $"{MetadataTokens.Prefix}{MetadataTokens.Symmetry}{value}";

    public static bool SafeIsMatch(this Regex pattern, string? value)
        => value != null && pattern.IsMatch(value);
}
