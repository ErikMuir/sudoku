using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Sudoku.Tests
{
    public class CellTests
    {
        #region // Constructor

        [Theory]
        [ClassData(typeof(ZeroToEightTestData))]
        public void Constructor_Sets_Col(int col)
        {
            var testObject = new Cell(col, 0);
            Assert.Equal(col, testObject.Col);
        }

        [Theory]
        [ClassData(typeof(ZeroToEightTestData))]
        public void Constructor_Sets_Row(int row)
        {
            var testObject = new Cell(0, row);
            Assert.Equal(row, testObject.Row);
        }

        [Theory]
        [ClassData(typeof(ColRowBoxTestData))]
        public void Constructor_Sets_Box(int col, int row, int expectedBox)
        {
            var testObject = new Cell(col, row);
            Assert.Equal(expectedBox, testObject.Box);
        }

        [Theory]
        [ClassData(typeof(OneToNineTestData))]
        public void Constructor_Sets_Value(int val)
        {
            var testObject = new Cell(0, 0, val);
            Assert.Equal(val, testObject.Value);
        }

        [Fact]
        public void Constructor_DoesNotSet_Value()
        {
            var testObject = new Cell(0, 0);
            Assert.Null(testObject.Value);
        }

        [Fact]
        public void Constructor_DoesNotSet_Candidates()
        {
            var testObject = new Cell(0, 0);
            Assert.Empty(testObject.Candidates);
        }

        #endregion // Constructor

        #region // Constructor Exceptions

        [Theory]
        [InlineData(-1)]
        [InlineData(9)]
        public void Constructor_Throws_InvalidCol(int col)
        {
            var exception = Record.Exception(() => new Cell(col, 0));
            Assert.NotNull(exception);
            Assert.IsType<SudokuException>(exception);
            Assert.Equal("Cell columns must be between 0 and 8, inclusive.", exception.Message);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(9)]
        public void Constructor_Throws_InvalidRow(int row)
        {
            var exception = Record.Exception(() => new Cell(0, row));
            Assert.NotNull(exception);
            Assert.IsType<SudokuException>(exception);
            Assert.Equal("Cell rows must be between 0 and 8, inclusive.", exception.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        public void Constructor_Throws_InvalidValue(int val)
        {
            var exception = Record.Exception(() => new Cell(0, 0, val));
            Assert.NotNull(exception);
            Assert.IsType<SudokuException>(exception);
            Assert.Equal("Cell values must be between 1 and 9, inclusive.", exception.Message);
        }

        [Fact]
        public void Constructor_Throws_MultipleErrors()
        {
            var expected = "Cell columns must be between 0 and 8, inclusive. Cell rows must be between 0 and 8, inclusive. Cell values must be between 1 and 9, inclusive.";
            var exception = Record.Exception(() => new Cell(9, 9, 0));
            Assert.NotNull(exception);
            Assert.IsType<SudokuException>(exception);
            Assert.Equal(expected, exception.Message);
        }

        #endregion // Constructor Exceptions

        #region // Members

        [Fact]
        public void IsClue_Returns_False()
        {
            var testObject = new Cell(0, 0);
            Assert.False(testObject.IsClue);
        }

        [Theory]
        [ClassData(typeof(OneToNineTestData))]
        public void Value_Returns(int val)
        {
            var testObject = new Cell(0, 0);
            testObject.Value = val;
            Assert.Equal(val, testObject.Value);
        }

        #endregion // Members

        #region // Methods

        [Fact]
        public void Value_Set_ToNull()
        {
            var testObject = new Cell(0, 0, 1);
            testObject.Value = null;
            Assert.Null(testObject.Value);
        }

        [Theory]
        [ClassData(typeof(OneToNineTestData))]
        public void Value_Set_ToValue(int val)
        {
            var testObject = new Cell(0, 0);
            testObject.Value = val;
            Assert.Equal(val, testObject.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        public void Value_Set_Throws_InvalidValue(int val)
        {
            var testObject = new Cell(0, 0);
            var exception = Record.Exception(() => testObject.Value = val);
            Assert.NotNull(exception);
            Assert.IsType<SudokuException>(exception);
            Assert.Equal("Value must be between 1 and 9, inclusive.", exception.Message);
        }

        [Theory]
        [ClassData(typeof(OneToNineTestData))]
        public void AddCandidate_Adds_Candidate(int val)
        {
            var testObject = new Cell(0, 0);
            testObject.AddCandidate(val);
            var actual = testObject.Candidates;
            Assert.Contains(actual, x => x == val);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        public void AddCandidate_Throws_InvalidValue(int val)
        {
            var testObject = new Cell(0, 0);
            var exception = Record.Exception(() => testObject.AddCandidate(val));
            Assert.NotNull(exception);
            Assert.IsType<SudokuException>(exception);
            Assert.Equal("Value must be between 1 and 9, inclusive.", exception.Message);
        }

        [Fact]
        public void FillCandidates_Adds_AllCandidates()
        {
            var testObject = new Cell(0, 0);
            testObject.FillCandidates();
            var actual = testObject.Candidates;
            Assert.Equal(9, actual.Count);
            Assert.Equal(9, actual.Distinct().Count());
        }

        [Fact]
        public void ClearCandidates_Removes_AllCandidates()
        {
            var testObject = new Cell(0, 0);
            testObject.ClearCandidates();
            Assert.Empty(testObject.Candidates);
        }

        [Theory]
        [ClassData(typeof(OneToNineTestData))]
        public void RemoveCandidate_Removes_Candidate(int val)
        {
            var testObject = new Cell(0, 0);
            testObject.FillCandidates();
            testObject.RemoveCandidate(val);
            var actual = testObject.Candidates;
            Assert.DoesNotContain(actual, x => x == val);
        }

        [Fact]
        public void ContainsOnlyMatches_Returns_False_WithMissingCandidates()
        {
            var testObject = new Cell(0, 0);
            testObject.AddCandidate(1);
            Assert.False(testObject.ContainsOnlyMatches(new Double(1, 2)));
        }

        [Fact]
        public void ContainsOnlyMatches_Returns_False_WithNonMatchingCandidates()
        {
            var testObject = new Cell(0, 0);
            testObject.AddCandidate(1);
            testObject.AddCandidate(2);
            testObject.AddCandidate(3);
            Assert.False(testObject.ContainsOnlyMatches(new Double(1, 2)));
        }

        [Fact]
        public void ContainsOnlyMatches_Returns_True()
        {
            var testObject = new Cell(0, 0);
            testObject.AddCandidate(1);
            testObject.AddCandidate(2);
            Assert.True(testObject.ContainsOnlyMatches(new Double(1, 2)));
        }

        [Fact]
        public void ContainsAtLeastOneMatch_Returns_False()
        {
            var testObject = new Cell(0, 0);
            Assert.False(testObject.ContainsAtLeastOneMatch(new Double(1, 2)));
        }

        [Fact]
        public void ContainsAtLeastOneMatch_Returns_True()
        {
            var testObject = new Cell(0, 0);
            testObject.AddCandidate(1);
            Assert.True(testObject.ContainsAtLeastOneMatch(new Double(1, 2)));
        }

        [Fact]
        public void GetNonMatchingCandidates_Returns_List()
        {
            var testObject = new Cell(0, 0);
            testObject.AddCandidate(1);
            testObject.AddCandidate(2);
            testObject.AddCandidate(3);
            var actual = testObject.GetNonMatchingCandidates(new List<int> { 1, 2 });
            Assert.Single(actual);
            Assert.Equal(3, actual[0]);
        }

        [Fact]
        public void Clone_Returns_Copy()
        {
            var original = new Cell(1, 2, 3);
            original.AddCandidate(4);
            var clone = original.Clone();
            Assert.NotNull(clone);
            Assert.NotSame(original, clone);
            Assert.Equal(original.ToString(), clone.ToString());
        }

        [Theory]
        [ClassData(typeof(CellParseTestData))]
        public void Parse_Returns_Cell(string val)
        {
            var actual = Cell.Parse(val);
            Assert.NotNull(actual);
            // and just for fun...
            var newVal = actual.ToString();
            Assert.Equal(val, newVal);
        }

        [Theory]
        [ClassData(typeof(CellParseThrowsTestData))]
        public void Parse_Throws_InvalidCell(string val)
        {
            var exception = Record.Exception(() => Cell.Parse(val));
            Assert.NotNull(exception);
            Assert.IsType<SudokuException>(exception);
            Assert.Equal("Invalid cell", exception.Message);
        }

        [Theory]
        [ClassData(typeof(ZeroToEightTestData))]
        public void ToString_Serializes_Col(int col)
        {
            var testObject = new Cell(col, 0);
            var actual = testObject.ToString().Substring(0, 1);
            Assert.Equal($"{col}", actual);
        }

        [Theory]
        [ClassData(typeof(ZeroToEightTestData))]
        public void ToString_Serializes_Row(int row)
        {
            var testObject = new Cell(0, row);
            var actual = testObject.ToString().Substring(1, 1);
            Assert.Equal($"{row}", actual);
        }

        [Theory]
        [ClassData(typeof(OneToNineTestData))]
        public void ToString_Serializes_Value(int val)
        {
            var testObject = new Cell(0, 0, val);
            var actual = testObject.ToString().Substring(2, 1);
            Assert.Equal($"{val}", actual);
        }

        [Fact]
        public void ToString_Serializes_NoValue()
        {
            var testObject = new Cell(0, 0, null);
            var actual = testObject.ToString().Substring(2, 1);
            Assert.Equal("0", actual);
        }

        [Fact]
        public void ToString_Serializes_NoClue()
        {
            var testObject = new Cell(0, 0, 9);
            var actual = testObject.ToString().Substring(3, 1);
            Assert.Equal("0", actual);
        }

        [Theory]
        [ClassData(typeof(OneToNineTestData))]
        public void ToString_Serializes_Candidates(int val)
        {
            var testObject = new Cell(0, 0);
            testObject.AddCandidate(val);
            var actual = testObject.ToString();
            Assert.Equal($"{val}", actual.Substring(4));
        }

        #endregion // Methods
    }
}
