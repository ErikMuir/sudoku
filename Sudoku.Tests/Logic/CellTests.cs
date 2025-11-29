namespace Sudoku.Tests.Logic;

public class CellTests
{
    [Theory]
    [ClassData(typeof(ZeroToEightTestData))]
    public void Constructor_Sets_Row(int row)
    {
        Cell testObject = new(row, 0);
        Assert.Equal(row, testObject.Row);
    }

    [Theory]
    [ClassData(typeof(ZeroToEightTestData))]
    public void Constructor_Sets_Col(int col)
    {
        Cell testObject = new(0, col);
        Assert.Equal(col, testObject.Col);
    }

    [Theory]
    [ClassData(typeof(CellTestData))]
    public void Constructor_Sets_Box(int row, int col, int box, int idx)
    {
        Cell testObject = new(row, col);
        Assert.Equal(box, testObject.Box);
    }

    [Theory]
    [ClassData(typeof(CellTestData))]
    public void Constructor_Sets_Index(int row, int col, int box, int idx)
    {
        Cell testObject = new(row, col);
        Assert.Equal(idx, testObject.Index);
    }

    [Theory]
    [ClassData(typeof(OneToNineTestData))]
    public void Constructor_Sets_Value(int val)
    {
        Cell testObject = new(0, 0, val);
        Assert.Equal(val, testObject.Value);
    }

    [Fact]
    public void Constructor_DoesNotSet_Value()
    {
        Cell testObject = new(0, 0);
        Assert.Null(testObject.Value);
    }

    [Fact]
    public void Constructor_DoesNotSet_Candidates()
    {
        Cell testObject = new(0, 0);
        Assert.Empty(testObject.Candidates);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    public void Constructor_Throws_InvalidValue(int val)
    {
        Exception exception = Record.Exception(() => new Cell(0, 0, val));
        Assert.NotNull(exception);
        Assert.IsType<SudokuException>(exception);
    }

    [Fact]
    public void CopyConstructor_Returns_Copy()
    {
        Cell original = new(1, 2, 3);
        original.AddCandidate(4);
        Cell clone = new(original);
        Assert.NotNull(clone);
        Assert.NotSame(original, clone);
        Assert.Equal(original.Row, clone.Row);
        Assert.Equal(original.Col, clone.Col);
        Assert.Equal(original.Box, clone.Box);
        Assert.Equal(original.Index, clone.Index);
        Assert.Equal(original.Value, clone.Value);
        Assert.Equal(original.Candidates.Count, clone.Candidates.Count);
        for (int i = 0; i < original.Candidates.Count; i++)
        {
            Assert.Equal(original.Candidates[i], clone.Candidates[i]);
        }
    }

    [Fact]
    public void CopyConstructor_Throws()
    {
        Clue original = new(1, 2, 3);
        Exception exception = Record.Exception(() => new Cell(original));
        Assert.NotNull(exception);
        Assert.IsType<SudokuException>(exception);
    }

    [Fact]
    public void IsClue_Returns_False()
    {
        Cell testObject = new(0, 0);
        Assert.False(testObject.IsClue);
    }

    [Fact]
    public void Type_Returns_Filled()
    {
        Cell testObject = new(0, 0, 1);
        Assert.Equal(CellType.Filled, testObject.Type);
    }

    [Fact]
    public void Type_Returns_Empty()
    {
        Cell testObject = new(0, 0);
        Assert.Equal(CellType.Empty, testObject.Type);
    }

    [Theory]
    [ClassData(typeof(OneToNineTestData))]
    public void Value_Get(int val)
    {
        Cell testObject = new(0, 0);
        testObject.Value = val;
        Assert.Equal(val, testObject.Value);
    }

    [Fact]
    public void Value_Set_Null()
    {
        Cell testObject = new(0, 0, 1);
        testObject.Value = null;
        Assert.Null(testObject.Value);
    }

    [Theory]
    [ClassData(typeof(OneToNineTestData))]
    public void Value_Set_Value(int val)
    {
        Cell testObject = new(0, 0);
        testObject.Value = val;
        Assert.Equal(val, testObject.Value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    public void Value_Set_Throws(int val)
    {
        Cell testObject = new(0, 0);
        Exception exception = Record.Exception(() => testObject.Value = val);
        Assert.NotNull(exception);
        Assert.IsType<SudokuException>(exception);
    }

    [Theory]
    [ClassData(typeof(OneToNineTestData))]
    public void AddCandidate(int val)
    {
        Cell testObject = new(0, 0);
        testObject.AddCandidate(val);
        ReadOnlyCollection<int> actual = testObject.Candidates;
        Assert.Contains(actual, x => x == val);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10)]
    public void AddCandidate_Throws(int val)
    {
        Cell testObject = new(0, 0);
        Exception exception = Record.Exception(() => testObject.AddCandidate(val));
        Assert.NotNull(exception);
        Assert.IsType<SudokuException>(exception);
    }

    [Fact]
    public void FillCandidates()
    {
        Cell testObject = new(0, 0);
        testObject.FillCandidates();
        ReadOnlyCollection<int> actual = testObject.Candidates;
        Assert.Equal(9, actual.Count);
        Assert.Equal(9, actual.Distinct().Count());
    }

    [Fact]
    public void ClearCandidates()
    {
        Cell testObject = new(0, 0);
        testObject.ClearCandidates();
        Assert.Empty(testObject.Candidates);
    }

    [Theory]
    [ClassData(typeof(OneToNineTestData))]
    public void RemoveCandidate(int val)
    {
        Cell testObject = new(0, 0);
        testObject.FillCandidates();
        testObject.RemoveCandidate(val);
        ReadOnlyCollection<int> actual = testObject.Candidates;
        Assert.DoesNotContain(actual, x => x == val);
    }

    [Fact]
    public void ContainsOnlyMatches_Returns_True()
    {
        Cell testObject = new(0, 0);
        testObject.AddCandidate(1);
        testObject.AddCandidate(2);
        Assert.True(testObject.ContainsOnlyMatches(new DoubleSet(1, 2)));
    }

    [Fact]
    public void ContainsOnlyMatches_Returns_False_WithMissingCandidates()
    {
        Cell testObject = new(0, 0);
        testObject.AddCandidate(1);
        Assert.False(testObject.ContainsOnlyMatches(new DoubleSet(1, 2)));
    }

    [Fact]
    public void ContainsOnlyMatches_Returns_False_WithNonMatchingCandidates()
    {
        Cell testObject = new(0, 0);
        testObject.AddCandidate(1);
        testObject.AddCandidate(2);
        testObject.AddCandidate(3);
        Assert.False(testObject.ContainsOnlyMatches(new DoubleSet(1, 2)));
    }

    [Fact]
    public void ContainsAtLeastOneMatch_Returns_True()
    {
        Cell testObject = new(0, 0);
        testObject.AddCandidate(1);
        Assert.True(testObject.ContainsAtLeastOneMatch(new DoubleSet(1, 2)));
    }

    [Fact]
    public void ContainsAtLeastOneMatch_Returns_False()
    {
        Cell testObject = new(0, 0);
        Assert.False(testObject.ContainsAtLeastOneMatch(new DoubleSet(1, 2)));
    }

    [Fact]
    public void GetNonMatchingCandidates()
    {
        Cell testObject = new(0, 0);
        testObject.AddCandidate(1);
        testObject.AddCandidate(2);
        testObject.AddCandidate(3);
        List<int> actual = testObject.GetNonMatchingCandidates(new List<int> { 1, 2 });
        Assert.Single(actual);
        Assert.Equal(3, actual[0]);
    }

    [Fact]
    public void IsPeer_ReturnsTrue_WhenCellInSameRow()
    {
        Cell testObject = new(0, 0);
        Cell otherCell = new(0, 9);
        Assert.True(testObject.IsPeer(otherCell));
    }

    [Fact]
    public void IsPeer_ReturnsTrue_WhenCellInSameCol()
    {
        Cell testObject = new(0, 0);
        Cell otherCell = new(9, 0);
        Assert.True(testObject.IsPeer(otherCell));
    }

    [Fact]
    public void IsPeer_ReturnsTrue_WhenCellInSameBox()
    {
        Cell testObject = new(0, 0);
        Cell otherCell = new(1, 1);
        Assert.True(testObject.IsPeer(otherCell));
    }

    [Fact]
    public void IsPeer_ReturnsFalse_WhenCellIsSame()
    {
        Cell testObject = new(0, 0);
        Cell otherCell = new(0, 0);
        Assert.False(testObject.IsPeer(otherCell));
    }

    [Fact]
    public void IsPeer_ReturnsFalse_WhenCellHasNoCommonUnits()
    {
        Cell testObject = new(0, 0);
        Cell otherCell = new(9, 9);
        Assert.False(testObject.IsPeer(otherCell));
    }
}
