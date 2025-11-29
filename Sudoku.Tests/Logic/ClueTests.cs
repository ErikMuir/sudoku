namespace Sudoku.Tests.Logic;

public class ClueTests
{
    private readonly Clue _testObject;

    public ClueTests()
    {
        _testObject = new Clue(0, 0, 1);
    }

    [Fact]
    public void CopyConstructor_Returns_Copy()
    {
        Clue clone = new(_testObject);
        Assert.NotNull(clone);
        Assert.NotSame(_testObject, clone);
        Assert.Equal(_testObject.Row, clone.Row);
        Assert.Equal(_testObject.Col, clone.Col);
        Assert.Equal(_testObject.Box, clone.Box);
        Assert.Equal(_testObject.Index, clone.Index);
        Assert.Equal(_testObject.Value, clone.Value);
        Assert.Empty(_testObject.Candidates);
    }

    [Fact]
    public void IsClue_Returns_True()
    {
        Assert.True(_testObject.IsClue);
    }

    [Fact]
    public void Type_Returns_Clue()
    {
        Assert.Equal(CellType.Clue, _testObject.Type);
    }

    [Fact]
    public void Value_Set_Throws()
    {
        Exception exception = Record.Exception(() => _testObject.Value = 1);
        Assert.NotNull(exception);
        Assert.IsType<SudokuException>(exception);
    }

    [Fact]
    public void GetCandidates_Returns_EmptyList()
    {
        ReadOnlyCollection<int> actual = _testObject.Candidates;
        Assert.Empty(actual);
    }

    [Fact]
    public void AddCandidate_Throws()
    {
        Exception exception = Record.Exception(() => _testObject.AddCandidate(1));
        Assert.NotNull(exception);
        Assert.IsType<SudokuException>(exception);
    }

    [Fact]
    public void RemoveCandidate_Throws()
    {
        Exception exception = Record.Exception(() => _testObject.RemoveCandidate(1));
        Assert.NotNull(exception);
        Assert.IsType<SudokuException>(exception);
    }

    [Fact]
    public void FillCandidates_Throws()
    {
        Exception exception = Record.Exception(() => _testObject.FillCandidates());
        Assert.NotNull(exception);
        Assert.IsType<SudokuException>(exception);
    }

    [Fact]
    public void ClearCandidates_Throws()
    {
        Exception exception = Record.Exception(() => _testObject.ClearCandidates());
        Assert.NotNull(exception);
        Assert.IsType<SudokuException>(exception);
    }
}
