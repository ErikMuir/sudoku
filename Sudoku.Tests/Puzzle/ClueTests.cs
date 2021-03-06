using Xunit;

namespace Sudoku.Tests
{
    public class ClueTests
    {
        private readonly Clue _testObject;
        private readonly string _exceptionMessage = "Cannot change a clue";

        public ClueTests()
        {
            _testObject = new Clue(0, 0, 1);
        }

        [Fact]
        public void IsClue_Returns_True()
        {
            Assert.True(_testObject.IsClue);
        }

        [Fact]
        public void Value_Set_Throws()
        {
            var exception = Record.Exception(() => _testObject.Value = 1);
            Assert.NotNull(exception);
            Assert.IsType<SudokuException>(exception);
            Assert.Equal(_exceptionMessage, exception.Message);
        }

        [Fact]
        public void GetCandidates_Returns_EmptyList()
        {
            var actual = _testObject.Candidates;
            Assert.Empty(actual);
        }

        [Fact]
        public void AddCandidate_Throws()
        {
            var exception = Record.Exception(() => _testObject.AddCandidate(1));
            Assert.NotNull(exception);
            Assert.IsType<SudokuException>(exception);
            Assert.Equal(_exceptionMessage, exception.Message);
        }

        [Fact]
        public void RemoveCandidate_Throws()
        {
            var exception = Record.Exception(() => _testObject.RemoveCandidate(1));
            Assert.NotNull(exception);
            Assert.IsType<SudokuException>(exception);
            Assert.Equal(_exceptionMessage, exception.Message);
        }

        [Fact]
        public void FillCandidates_Throws()
        {
            var exception = Record.Exception(() => _testObject.FillCandidates());
            Assert.NotNull(exception);
            Assert.IsType<SudokuException>(exception);
            Assert.Equal(_exceptionMessage, exception.Message);
        }

        [Fact]
        public void ClearCandidates_Throws()
        {
            var exception = Record.Exception(() => _testObject.ClearCandidates());
            Assert.NotNull(exception);
            Assert.IsType<SudokuException>(exception);
            Assert.Equal(_exceptionMessage, exception.Message);
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
        public void ToString_Serializes_Clue()
        {
            var actual = _testObject.ToString().Substring(3, 1);
            Assert.Equal("1", actual);
        }
    }
}