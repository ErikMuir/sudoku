namespace Sudoku.Serialization;

public abstract class Serializer
{
    public abstract string FileExtension { get; }

    public abstract Puzzle Deserialize(string puzzleString);

    public abstract string Serialize(Puzzle puzzle);
}
