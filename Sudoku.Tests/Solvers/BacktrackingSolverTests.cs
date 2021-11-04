using System;
using Sudoku.Solution;
using Xunit;

namespace Sudoku.Tests
{
    public class BacktrackingSolverTests
    {
        [Fact]
        public void Solve_EmptyPuzzle()
        {
            Puzzle puzzle = TestHelpers.GetEmptyPuzzle();
            BacktrackingSolver solver = new(puzzle);
            solver.Solve();
            Assert.True(solver.SolveDepth > 0);
            Assert.True(solver.SolveDuration > TimeSpan.Zero);
            Assert.True(solver.Puzzle.IsSolved());
        }

        [Fact]
        public void Solve_SolvedPuzzle()
        {
            Puzzle puzzle = TestHelpers.GetSolvedPuzzle();
            BacktrackingSolver solver = new(puzzle);
            solver.Solve();
            Assert.Equal(0, solver.SolveDepth);
            Assert.Equal(TimeSpan.Zero, solver.SolveDuration);
            Assert.True(solver.Puzzle.IsSolved());
        }

        [Fact]
        public void Solve_EasyPuzzle()
        {
            Puzzle puzzle = TestHelpers.GetEasyPuzzle();
            BacktrackingSolver solver = new(puzzle);
            solver.Solve();
            Assert.True(solver.SolveDepth > 0);
            Assert.True(solver.SolveDuration > TimeSpan.Zero);
            Assert.True(solver.Puzzle.IsSolved());
        }

        [Fact]
        public void Solve_MediumPuzzle()
        {
            Puzzle puzzle = TestHelpers.GetMediumPuzzle();
            BacktrackingSolver solver = new(puzzle);
            solver.Solve();
            Assert.True(solver.SolveDepth > 0);
            Assert.True(solver.SolveDuration > TimeSpan.Zero);
            Assert.True(solver.Puzzle.IsSolved());
        }

        [Fact]
        public void Solve_DifficultPuzzle()
        {
            Puzzle puzzle = TestHelpers.GetDifficultPuzzle();
            BacktrackingSolver solver = new(puzzle);
            solver.Solve();
            Assert.True(solver.SolveDepth > 0);
            Assert.True(solver.SolveDuration > TimeSpan.Zero);
            Assert.True(solver.Puzzle.IsSolved());
        }
    }
}
