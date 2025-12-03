namespace Sudoku.Generation;

public class GenerationOptions
{
    public Level Level { get; set; }

    public Symmetry? Symmetry { get; set; }

    public int MaxClues { get; set; }
}
