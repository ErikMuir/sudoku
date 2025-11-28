using Sudoku.Logic;

namespace Sudoku.Generation;

public class GenerationOptions
{
    public Level Level { get; set; }
    public ISymmetry Symmetry { get; set; }
    public int MaxClues { get; set; }
}
