using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xunit;

namespace Sudoku.Tests
{
    public class CellTests
    {
        [Theory]
        [ClassData(typeof(ZeroToEightTestData))]
        public void Constructor_Sets_Col(int col)
        {
            Cell testObject = new(col, 0);
            Assert.Equal(col, testObject.Col);
        }

        [Theory]
        [ClassData(typeof(ZeroToEightTestData))]
        public void Constructor_Sets_Row(int row)
        {
            Cell testObject = new(0, row);
            Assert.Equal(row, testObject.Row);
        }

        [Theory]
        [ClassData(typeof(ColRowBoxTestData))]
        public void Constructor_Sets_Box(int col, int row, int expectedBox)
        {
            Cell testObject = new(col, row);
            Assert.Equal(expectedBox, testObject.Box);
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
        [InlineData(-1)]
        [InlineData(9)]
        public void Constructor_Throws_InvalidCol(int col)
        {
            Exception exception = Record.Exception(() => new Cell(col, 0));
            Assert.NotNull(exception);
            Assert.IsType<SudokuException>(exception);
            Assert.Equal("Cell columns must be between 0 and 8, inclusive.", exception.Message);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(9)]
        public void Constructor_Throws_InvalidRow(int row)
        {
            Exception exception = Record.Exception(() => new Cell(0, row));
            Assert.NotNull(exception);
            Assert.IsType<SudokuException>(exception);
            Assert.Equal("Cell rows must be between 0 and 8, inclusive.", exception.Message);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        public void Constructor_Throws_InvalidValue(int val)
        {
            Exception exception = Record.Exception(() => new Cell(0, 0, val));
            Assert.NotNull(exception);
            Assert.IsType<SudokuException>(exception);
            Assert.Equal("Cell values must be between 1 and 9, inclusive.", exception.Message);
        }

        [Fact]
        public void Constructor_Throws_MultipleErrors()
        {
            string expected = "Cell columns must be between 0 and 8, inclusive. Cell rows must be between 0 and 8, inclusive. Cell values must be between 1 and 9, inclusive.";
            Exception exception = Record.Exception(() => new Cell(9, 9, 0));
            Assert.NotNull(exception);
            Assert.IsType<SudokuException>(exception);
            Assert.Equal(expected, exception.Message);
        }

        [Fact]
        public void IsClue_Returns_False()
        {
            Cell testObject = new(0, 0);
            Assert.False(testObject.IsClue);
        }

        [Theory]
        [ClassData(typeof(OneToNineTestData))]
        public void Value_Returns(int val)
        {
            Cell testObject = new(0, 0);
            testObject.Value = val;
            Assert.Equal(val, testObject.Value);
        }

        [Fact]
        public void Value_Set_ToNull()
        {
            Cell testObject = new(0, 0, 1);
            testObject.Value = null;
            Assert.Null(testObject.Value);
        }

        [Theory]
        [ClassData(typeof(OneToNineTestData))]
        public void Value_Set_ToValue(int val)
        {
            Cell testObject = new(0, 0);
            testObject.Value = val;
            Assert.Equal(val, testObject.Value);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        public void Value_Set_Throws_InvalidValue(int val)
        {
            Cell testObject = new(0, 0);
            Exception exception = Record.Exception(() => testObject.Value = val);
            Assert.NotNull(exception);
            Assert.IsType<SudokuException>(exception);
            Assert.Equal("Value must be between 1 and 9, inclusive.", exception.Message);
        }

        [Theory]
        [ClassData(typeof(OneToNineTestData))]
        public void AddCandidate_Adds_Candidate(int val)
        {
            Cell testObject = new(0, 0);
            testObject.AddCandidate(val);
            ReadOnlyCollection<int> actual = testObject.Candidates;
            Assert.Contains(actual, x => x == val);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        public void AddCandidate_Throws_InvalidValue(int val)
        {
            Cell testObject = new(0, 0);
            Exception exception = Record.Exception(() => testObject.AddCandidate(val));
            Assert.NotNull(exception);
            Assert.IsType<SudokuException>(exception);
            Assert.Equal("Value must be between 1 and 9, inclusive.", exception.Message);
        }

        [Fact]
        public void FillCandidates_Adds_AllCandidates()
        {
            Cell testObject = new(0, 0);
            testObject.FillCandidates();
            ReadOnlyCollection<int> actual = testObject.Candidates;
            Assert.Equal(9, actual.Count);
            Assert.Equal(9, actual.Distinct().Count());
        }

        [Fact]
        public void ClearCandidates_Removes_AllCandidates()
        {
            Cell testObject = new(0, 0);
            testObject.ClearCandidates();
            Assert.Empty(testObject.Candidates);
        }

        [Theory]
        [ClassData(typeof(OneToNineTestData))]
        public void RemoveCandidate_Removes_Candidate(int val)
        {
            Cell testObject = new(0, 0);
            testObject.FillCandidates();
            testObject.RemoveCandidate(val);
            ReadOnlyCollection<int> actual = testObject.Candidates;
            Assert.DoesNotContain(actual, x => x == val);
        }

        [Fact]
        public void ContainsOnlyMatches_Returns_False_WithMissingCandidates()
        {
            Cell testObject = new(0, 0);
            testObject.AddCandidate(1);
            Assert.False(testObject.ContainsOnlyMatches(new Double(1, 2)));
        }

        [Fact]
        public void ContainsOnlyMatches_Returns_False_WithNonMatchingCandidates()
        {
            Cell testObject = new(0, 0);
            testObject.AddCandidate(1);
            testObject.AddCandidate(2);
            testObject.AddCandidate(3);
            Assert.False(testObject.ContainsOnlyMatches(new Double(1, 2)));
        }

        [Fact]
        public void ContainsOnlyMatches_Returns_True()
        {
            Cell testObject = new(0, 0);
            testObject.AddCandidate(1);
            testObject.AddCandidate(2);
            Assert.True(testObject.ContainsOnlyMatches(new Double(1, 2)));
        }

        [Fact]
        public void ContainsAtLeastOneMatch_Returns_False()
        {
            Cell testObject = new(0, 0);
            Assert.False(testObject.ContainsAtLeastOneMatch(new Double(1, 2)));
        }

        [Fact]
        public void ContainsAtLeastOneMatch_Returns_True()
        {
            Cell testObject = new(0, 0);
            testObject.AddCandidate(1);
            Assert.True(testObject.ContainsAtLeastOneMatch(new Double(1, 2)));
        }

        [Fact]
        public void GetNonMatchingCandidates_Returns_List()
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
        public void Clone_Returns_Copy()
        {
            Cell original = new(1, 2, 3);
            original.AddCandidate(4);
            Cell clone = original.Clone();
            Assert.NotNull(clone);
            Assert.NotSame(original, clone);
            Assert.Equal(original.ToString(), clone.ToString());
        }
    }
}
