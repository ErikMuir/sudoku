using Sudoku.Logic;

namespace Sudoku.Serialization
{
    public interface ISerializer
    {
        string FileExtension { get; }
        string Serialize(Puzzle puzzle);
        Puzzle Deserialize(string puzzleString);
    }
}
