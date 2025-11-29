namespace Sudoku.Serialization;

public interface ISerializer
{
    string FileExtension { get; }

    Puzzle Deserialize(string puzzleString);

    string Serialize(Puzzle puzzle);
}
