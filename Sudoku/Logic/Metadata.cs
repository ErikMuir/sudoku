namespace Sudoku.Logic;

public class Metadata
{
    public string? Author { get; set; }

    public string? Description { get; set; }

    public string? Comment { get; set; }

    public DateTime DatePublished { get; set; }

    public string? Source { get; set; }

    public Level Level { get; set; } = Level.Uninitialized;

    public Uri? SourceUrl { get; set; }

    public SymmetryType Symmetry { get; set; } = SymmetryType.Uninitialized;
}
