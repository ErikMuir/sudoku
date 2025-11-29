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
        Cell cell = _testObject.GetCell(row, col);
        Assert.Equal(row, cell.Row);
        Assert.Equal(col, cell.Col);
    }

    [Theory]
    [ClassData(typeof(ZeroToEightTestData))]
    public void GetRow_Returns_RowCells(int row)
    {
        IEnumerable<Cell> rowCells = _testObject.GetRow(row);
        Assert.Equal(9, rowCells.Count());
        Assert.True(rowCells.All(x => x.Row == row));
    }

    [Theory]
    [ClassData(typeof(ZeroToEightTestData))]
    public void GetCol_Returns_ColCells(int col)
    {
        IEnumerable<Cell> colCells = _testObject.GetCol(col);
        Assert.Equal(9, colCells.Count());
        Assert.True(colCells.All(x => x.Col == col));
    }

    [Theory]
    [ClassData(typeof(ZeroToEightTestData))]
    public void GetBox_Returns_BoxCells(int box)
    {
        IEnumerable<Cell> boxCells = _testObject.GetBox(box);
        Assert.Equal(9, boxCells.Count());
        Assert.True(boxCells.All(x => x.Box == box));
    }

    [Fact]
    public void Peers_Returns_Peers()
    {
        Cell cell = new(0, 0);
        IEnumerable<Cell> actual = _testObject.Peers(cell);
        Assert.All(actual, x => Assert.True(x.Col == cell.Col || x.Row == cell.Row || x.Box == cell.Box));
    }

    [Fact]
    public void CommonPeers_Returns_CommonPeers()
    {
        Cell cell1 = new(0, 0);
        Cell cell2 = new(1, 1);
        IEnumerable<Cell> actual = _testObject.CommonPeers(cell1, cell2);
        Assert.All(actual, x => Assert.True(x.Box == cell1.Box));
    }

    [Fact]
    public void IsSolved_Returns_True()
    {
        Puzzle solved = TestHelpers.GetSolvedPuzzle();
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
        Cell[] row = _testObject.GetRow(0).ToArray();
        row[0].Value = 1;
        row[1].Value = 1;
        Assert.False(_testObject.IsValid);
    }

    [Fact]
    public void FillCandidates()
    {
        _testObject.FillCandidates();
        foreach (Cell cell in _testObject.Cells)
        {
            Assert.Equal(9, cell.Candidates.Count);
        }
    }

    [Theory]
    [ClassData(typeof(CellTestData))]
    public void ReduceCandidates_Clears_Row(int row, int col, int box, int idx)
    {
        int val = 9;
        Cell cellToUpdate = _testObject.GetCell(row, col);
        cellToUpdate.Value = val;
        _testObject.ReduceCandidates();
        IEnumerable<Cell> rowCells = _testObject.GetRow(cellToUpdate.Row);
        foreach (Cell cell in rowCells)
        {
            Assert.DoesNotContain(val, cell.Candidates);
        }
    }

    [Theory]
    [ClassData(typeof(CellTestData))]
    public void ReduceCandidates_Clears_Col(int row, int col, int box, int idx)
    {
        int val = 9;
        Cell cellToUpdate = _testObject.GetCell(row, col);
        cellToUpdate.Value = val;
        _testObject.ReduceCandidates();
        IEnumerable<Cell> colCells = _testObject.GetCol(cellToUpdate.Col);
        foreach (Cell cell in colCells)
        {
            Assert.DoesNotContain(val, cell.Candidates);
        }
    }

    [Theory]
    [ClassData(typeof(CellTestData))]
    public void ReduceCandidates_Clears_Box(int row, int col, int box, int idx)
    {
        int val = 9;
        Cell cellToUpdate = _testObject.GetCell(row, col);
        cellToUpdate.Value = val;
        _testObject.ReduceCandidates();
        IEnumerable<Cell> boxCells = _testObject.GetBox(cellToUpdate.Box);
        foreach (Cell cell in boxCells)
        {
            Assert.DoesNotContain(val, cell.Candidates);
        }
    }
}
