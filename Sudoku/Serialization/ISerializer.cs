namespace Sudoku.Serialization
{
    public interface ISerializer
    {
        string FileExtension { get; }
        Puzzle Deserialize(string input);
        string Serialize(Puzzle puzzle);
    }
}
