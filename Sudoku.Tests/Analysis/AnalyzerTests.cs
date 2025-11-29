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
        var clone = new Puzzle(_puzzle);
        var _ = new Analyzer(_puzzle);
        for (var i = 0; i < Puzzle.TotalCells; i++)
        {
            var expected = clone.Cells[i];
            var actual = _puzzle.Cells[i];
            Assert.Equal(expected.Type, actual.Type);
            Assert.Equal(expected.Value, actual.Value);
        }
    }

    [Fact]
    public void Analyze_Sets_SolveDuration()
    {
        var analyzer = new Analyzer(_puzzle);
        Assert.True(analyzer.SolveDuration > TimeSpan.MinValue);
    }

    [Fact]
    public void Analyze_Sets_SolveDepth()
    {
        var analyzer = new Analyzer(_puzzle);
        Assert.True(analyzer.SolveDepth > 0);
    }

    [Fact]
    public void Analyze_Adds_Logs()
    {
        var analyzer = new Analyzer(_puzzle);
        Assert.NotEmpty(analyzer.Logs);
    }

    [Fact]
    public void Analyze_Sets_Level()
    {
        var analyzer = new Analyzer(_puzzle);
        Assert.NotEqual(Level.Uninitialized, analyzer.Level);
    }
}
