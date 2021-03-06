using System.Linq;
using Xunit;

namespace Sudoku.Tests
{
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
            Assert.Equal(81, _testObject.Cells.Count);
        }

        [Theory]
        [ClassData(typeof(ColRowTestData))]
        public void GetCell_Returns_Cell(int col, int row)
        {
            var cell = _testObject.GetCell(col, row);
            Assert.Equal(col, cell.Col);
            Assert.Equal(row, cell.Row);
        }

        [Theory]
        [ClassData(typeof(ZeroToEightTestData))]
        public void GetRow_Returns_RowCells(int row)
        {
            var rowCells = _testObject.GetRow(row);
            Assert.Equal(9, rowCells.Count);
            Assert.True(rowCells.All(x => x.Row == row));
        }

        [Theory]
        [ClassData(typeof(ZeroToEightTestData))]
        public void GetCol_Returns_ColCells(int col)
        {
            var colCells = _testObject.GetCol(col);
            Assert.Equal(9, colCells.Count);
            Assert.True(colCells.All(x => x.Col == col));
        }

        [Theory]
        [ClassData(typeof(ZeroToEightTestData))]
        public void GetBox_Returns_BoxCells(int box)
        {
            var boxCells = _testObject.GetBox(box);
            Assert.Equal(9, boxCells.Count);
            Assert.True(boxCells.All(x => x.Box == box));
        }

        [Fact]
        public void GetEmptyCells_Returns_EmptyCells()
        {
            var emptyCells = _testObject.GetEmptyCells();
            Assert.Equal(81, emptyCells.Count);
        }

        [Fact]
        public void GetNextEmptyCell_Returns_EmptyCell()
        {
            _testObject.Cells[0].Value = 9;
            var actual = _testObject.GetNextEmptyCell();
            Assert.Same(_testObject.Cells[1], actual);
        }

        [Fact]
        public void GetRelatives_Returns_Relatives()
        {
            var cell = new Cell(0, 0);
            var actual = _testObject.GetRelatives(cell);
            Assert.All(actual, x => Assert.True(x.Col == cell.Col || x.Row == cell.Row || x.Box == cell.Box));
        }

        [Fact]
        public void GetCommonRelatives_Returns_CommonRelatives()
        {
            var cell1 = new Cell(0, 0);
            var cell2 = new Cell(1, 1);
            var actual = _testObject.GetCommonRelatives(cell1, cell2);
            Assert.All(actual, x => Assert.True(x.Box == cell1.Box));
        }

        [Fact]
        public void GetEmptyCells_Returns_EmptyList()
        {
            var solved = TestHelpers.GetSolvedPuzzle();
            var emptyCells = solved.GetEmptyCells();
            Assert.Empty(emptyCells);
        }

        [Fact]
        public void IsSolved_Returns_True()
        {
            var solved = TestHelpers.GetSolvedPuzzle();
            Assert.True(solved.IsSolved());
        }

        [Fact]
        public void IsSolved_Returns_False()
        {
            Assert.False(_testObject.IsSolved());
        }

        [Fact]
        public void IsValid_Returns_True()
        {
            Assert.True(_testObject.IsValid());
            Assert.True(TestHelpers.GetSolvedPuzzle().IsValid());
        }

        [Fact]
        public void IsValid_Returns_False()
        {
            var row = _testObject.GetRow(0);
            row[0].Value = 1;
            row[1].Value = 1;
            Assert.False(_testObject.IsValid());
        }

        [Fact]
        public void CalculateCandidates_Sets_AllCandidatesInEmptyCells()
        {
            _testObject.CalculateCandidates();
            foreach (var cell in _testObject.Cells)
            {
                Assert.Equal(9, cell.Candidates.Count);
            }
        }

        [Theory]
        [ClassData(typeof(ColRowTestData))]
        public void CalculateCandidates_Clears_Row(int col, int row)
        {
            var val = 9;
            var cellToUpdate = _testObject.GetCell(col, row);
            cellToUpdate.Value = val;
            _testObject.CalculateCandidates();
            var rowCells = _testObject.GetRow(cellToUpdate.Row);
            foreach (var cell in rowCells)
            {
                Assert.DoesNotContain(val, cell.Candidates);
            }
        }

        [Theory]
        [ClassData(typeof(ColRowTestData))]
        public void CalculateCandidates_Clears_Col(int col, int row)
        {
            var val = 9;
            var cellToUpdate = _testObject.GetCell(col, row);
            cellToUpdate.Value = val;
            _testObject.CalculateCandidates();
            var colCells = _testObject.GetCol(cellToUpdate.Col);
            foreach (var cell in colCells)
            {
                Assert.DoesNotContain(val, cell.Candidates);
            }
        }

        [Theory]
        [ClassData(typeof(ColRowTestData))]
        public void CalculateCandidates_Clears_Box(int col, int row)
        {
            var val = 9;
            var cellToUpdate = _testObject.GetCell(col, row);
            cellToUpdate.Value = val;
            _testObject.CalculateCandidates();
            var boxCells = _testObject.GetBox(cellToUpdate.Box);
            foreach (var cell in boxCells)
            {
                Assert.DoesNotContain(val, cell.Candidates);
            }
        }

        [Theory]
        [ClassData(typeof(ColRowTestData))]
        public void ReduceCandidates_Clears_Row(int col, int row)
        {
            var val = 9;
            var cellToUpdate = _testObject.GetCell(col, row);
            cellToUpdate.Value = val;
            _testObject.ReduceCandidates();
            var rowCells = _testObject.GetRow(cellToUpdate.Row);
            foreach (var cell in rowCells)
            {
                Assert.DoesNotContain(val, cell.Candidates);
            }
        }

        [Theory]
        [ClassData(typeof(ColRowTestData))]
        public void ReduceCandidates_Clears_Col(int col, int row)
        {
            var val = 9;
            var cellToUpdate = _testObject.GetCell(col, row);
            cellToUpdate.Value = val;
            _testObject.ReduceCandidates();
            var colCells = _testObject.GetCol(cellToUpdate.Col);
            foreach (var cell in colCells)
            {
                Assert.DoesNotContain(val, cell.Candidates);
            }
        }

        [Theory]
        [ClassData(typeof(ColRowTestData))]
        public void ReduceCandidates_Clears_Box(int col, int row)
        {
            var val = 9;
            var cellToUpdate = _testObject.GetCell(col, row);
            cellToUpdate.Value = val;
            _testObject.ReduceCandidates();
            var boxCells = _testObject.GetBox(cellToUpdate.Box);
            foreach (var cell in boxCells)
            {
                Assert.DoesNotContain(val, cell.Candidates);
            }
        }

        [Fact]
        public void Clone_Returns_Copy()
        {
            var clone = _testObject.Clone();
            Assert.NotNull(clone);
            Assert.NotSame(_testObject, clone);
            Assert.Equal(_testObject.ToString(), clone.ToString());
        }

        [Fact]
        public void Parse_Returns_Puzzle()
        {
            var stringToParse = _testObject.ToString();
            var newPuzzle = Puzzle.Parse(stringToParse);
            Assert.NotNull(newPuzzle);
            // and just for fun...
            var newPuzzleString = newPuzzle.ToString();
            Assert.Equal(stringToParse, newPuzzleString);
        }

        [Theory]
        [ClassData(typeof(PuzzleParseThrowsTestData))]
        public void Parse_Throws_InvalidPuzzle(string puzzleString, string message)
        {
            var exception = Record.Exception(() => Puzzle.Parse(puzzleString));
            Assert.NotNull(exception);
            Assert.IsType<SudokuException>(exception);
            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public void ToString_Serializes_Puzzle()
        {
            var puzzleString = _testObject.ToString();
            var cells = puzzleString.Split(',');
            Assert.Equal(81, cells.Length);
            Assert.Equal(_testObject.Cells[0].ToString(), cells[0]);
        }
    }
}