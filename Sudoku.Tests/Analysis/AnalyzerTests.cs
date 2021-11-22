using System;
using Sudoku.Analysis;
using Sudoku.Logic;
using Xunit;

namespace Sudoku.Tests
{
    public class AnalyzerTests
    {
        [Fact]
        public void Analyze_Solves_Puzzle()
        {
            Puzzle puzzle = TestHelpers.GetEasyPuzzle();
            Analyzer analyzer = new(puzzle);
            Assert.False(analyzer.IsSolved);
            analyzer.Analyze();
            Assert.True(analyzer.IsSolved);
        }

        [Fact]
        public void Analyze_DoesNotMutateInput()
        {
            Puzzle puzzle = TestHelpers.GetEasyPuzzle();
            Analyzer analyzer = new(puzzle);
            analyzer.Analyze();
            Assert.True(analyzer.IsSolved);
            Assert.False(puzzle.IsSolved);
        }

        [Fact]
        public void Analyze_Sets_SolveDuration()
        {
            Puzzle puzzle = TestHelpers.GetEasyPuzzle();
            Analyzer analyzer = new(puzzle);
            analyzer.Analyze();
            Assert.True(analyzer.SolveDuration > TimeSpan.MinValue);
        }

        [Fact]
        public void Analyze_DoesNotSet_SolveDuration()
        {
            Puzzle puzzle = TestHelpers.GetSolvedPuzzle();
            Analyzer analyzer = new(puzzle);
            analyzer.Analyze();
            Assert.Equal(TimeSpan.FromMilliseconds(0), analyzer.SolveDuration);
        }

        [Fact]
        public void Analyze_Sets_SolveDepth()
        {
            Puzzle puzzle = TestHelpers.GetEasyPuzzle();
            Analyzer analyzer = new(puzzle);
            analyzer.Analyze();
            Assert.True(analyzer.SolveDepth > 0);
        }

        [Fact]
        public void Analyze_DoesNotSet_SolveDepth()
        {
            Puzzle puzzle = TestHelpers.GetSolvedPuzzle();
            Analyzer analyzer = new(puzzle);
            analyzer.Analyze();
            Assert.Equal(0, analyzer.SolveDepth);
        }

        [Fact]
        public void Analyze_Adds_Logs()
        {
            Puzzle puzzle = TestHelpers.GetEasyPuzzle();
            Analyzer analyzer = new(puzzle);
            analyzer.Analyze();
            Assert.NotEmpty(analyzer.Logs);
        }

        [Fact]
        public void Analyze_DoesNotAdd_Logs()
        {
            Puzzle puzzle = TestHelpers.GetSolvedPuzzle();
            Analyzer analyzer = new(puzzle);
            analyzer.Analyze();
            Assert.Empty(analyzer.Logs);
        }
    }
}
