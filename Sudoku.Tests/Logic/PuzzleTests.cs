namespace Sudoku.Tests.Logic;

public class PuzzleTests
{
    private readonly Puzzle _testObject;

    public PuzzleTests()
    {
        _testObject = new Puzzle();
    }

    [Fact]
    public void Constructor_Sets_Cells()
    {
        Assert.Equal(81, _testObject.Cells.Length);
    }

    [Theory]
    [ClassData(typeof(CellTestData))]
    public void GetCell_Returns_Cell(int row, int col, int box, int idx)
    {
        var cell = _testObject.GetCell(row, col);
        Assert.Equal(row, cell.Row);
        Assert.Equal(col, cell.Col);
    }

    [Theory]
    [ClassData(typeof(ZeroToEightTestData))]
    public void GetRow_Returns_RowCells(int row)
    {
        var rowCells = _testObject.GetRow(row);
        Assert.Equal(9, rowCells.Count());
        Assert.True(rowCells.All(x => x.Row == row));
    }

    [Theory]
    [ClassData(typeof(ZeroToEightTestData))]
    public void GetCol_Returns_ColCells(int col)
    {
        var colCells = _testObject.GetCol(col);
        Assert.Equal(9, colCells.Count());
        Assert.True(colCells.All(x => x.Col == col));
    }

    [Theory]
    [ClassData(typeof(ZeroToEightTestData))]
    public void GetBox_Returns_BoxCells(int box)
    {
        var boxCells = _testObject.GetBox(box);
        Assert.Equal(9, boxCells.Count());
        Assert.True(boxCells.All(x => x.Box == box));
    }

    [Fact]
    public void Peers_Returns_Peers()
    {
        var cell = new Cell(0, 0);
        var actual = _testObject.Peers(cell);
        Assert.All(actual, x => Assert.True(x.Col == cell.Col || x.Row == cell.Row || x.Box == cell.Box));
    }

    [Fact]
    public void CommonPeers_Returns_CommonPeers()
    {
        var cell1 = new Cell(0, 0);
        var cell2 = new Cell(1, 1);
        var actual = _testObject.CommonPeers(cell1, cell2);
        Assert.All(actual, x => Assert.True(x.Box == cell1.Box));
    }

    [Fact]
    public void IsSolved_Returns_True()
    {
        var solved = TestHelpers.GetSolvedPuzzle();
        Assert.True(solved.IsSolved);
    }

    [Fact]
    public void IsSolved_Returns_False()
    {
        Assert.False(_testObject.IsSolved);
    }

    [Fact]
    public void IsValid_Returns_True()
    {
        Assert.True(_testObject.IsValid);
        Assert.True(TestHelpers.GetSolvedPuzzle().IsValid);
    }

    [Fact]
    public void IsValid_Returns_False()
    {
        var row = _testObject.GetRow(0).ToArray();
        row[0].Value = 1;
        row[1].Value = 1;
        Assert.False(_testObject.IsValid);
    }

    [Fact]
    public void FillCandidates()
    {
        _testObject.FillCandidates();
        foreach (var cell in _testObject.Cells)
        {
            Assert.Equal(9, cell.Candidates.Count);
        }
    }

    [Theory]
    [ClassData(typeof(CellTestData))]
    public void ReduceCandidates_Clears_Row(int row, int col, int box, int idx)
    {
        var val = 9;
        var cellToUpdate = _testObject.GetCell(row, col);
        cellToUpdate.Value = val;
        _testObject.ReduceCandidates();
        var rowCells = _testObject.GetRow(cellToUpdate.Row);
        foreach (var cell in rowCells)
        {
            Assert.DoesNotContain(val, cell.Candidates);
        }
    }

    [Theory]
    [ClassData(typeof(CellTestData))]
    public void ReduceCandidates_Clears_Col(int row, int col, int box, int idx)
    {
        var val = 9;
        var cellToUpdate = _testObject.GetCell(row, col);
        cellToUpdate.Value = val;
        _testObject.ReduceCandidates();
        var colCells = _testObject.GetCol(cellToUpdate.Col);
        foreach (var cell in colCells)
        {
            Assert.DoesNotContain(val, cell.Candidates);
        }
    }

    [Theory]
    [ClassData(typeof(CellTestData))]
    public void ReduceCandidates_Clears_Box(int row, int col, int box, int idx)
    {
        var val = 9;
        var cellToUpdate = _testObject.GetCell(row, col);
        cellToUpdate.Value = val;
        _testObject.ReduceCandidates();
        var boxCells = _testObject.GetBox(cellToUpdate.Box);
        foreach (var cell in boxCells)
        {
            Assert.DoesNotContain(val, cell.Candidates);
        }
    }
}
