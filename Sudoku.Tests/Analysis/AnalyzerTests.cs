namespace Sudoku.Tests.Analysis;

public class AnalyzerTests
{
    private Puzzle _puzzle;

    public AnalyzerTests()
    {
        _puzzle = TestHelpers.GetEasyPuzzle();
    }

    [Fact]
    public void Analyze_DoesNotMutateInput()
    {
        Puzzle clone = new(_puzzle);
        Analyzer analyzer = new(_puzzle);
        for (int i = 0; i < Puzzle.TotalCells; i++)
        {
            Cell expected = clone.Cells[i];
            Cell actual = _puzzle.Cells[i];
            Assert.Equal(expected.Type, actual.Type);
            Assert.Equal(expected.Value, actual.Value);
        }
    }

    [Fact]
    public void Analyze_Sets_SolveDuration()
    {
        Analyzer analyzer = new(_puzzle);
        Assert.True(analyzer.SolveDuration > TimeSpan.MinValue);
    }

    [Fact]
    public void Analyze_Sets_SolveDepth()
    {
        Analyzer analyzer = new(_puzzle);
        Assert.True(analyzer.SolveDepth > 0);
    }

    [Fact]
    public void Analyze_Adds_Logs()
    {
        Analyzer analyzer = new(_puzzle);
        Assert.NotEmpty(analyzer.Logs);
    }

    [Fact]
    public void Analyze_Sets_Level()
    {
        Analyzer analyzer = new(_puzzle);
        Assert.NotEqual(Level.Uninitialized, analyzer.Level);
    }
}
