using System;
using Sudoku.Solvers;
using Xunit;

namespace Sudoku.Tests
{
    public class BacktrackingSolverTests
    {
        [Fact]
        public void Solve_EmptyPuzzle()
        {
            var puzzle = TestHelpers.GetEmptyPuzzle();
            var solver = new BacktrackingSolver(puzzle);
            solver.Solve();
            Assert.True(solver.SolveDepth > 0);
            Assert.True(solver.SolveDuration > TimeSpan.Zero);
            Assert.True(solver.Puzzle.IsSolved());
        }

        [Fact]
        public void Solve_SolvedPuzzle()
        {
            var puzzle = TestHelpers.GetSolvedPuzzle();
            var solver = new BacktrackingSolver(puzzle);
            solver.Solve();
            Assert.Equal(0, solver.SolveDepth);
            Assert.Equal(TimeSpan.Zero, solver.SolveDuration);
            Assert.True(solver.Puzzle.IsSolved());
        }

        [Fact]
        public void Solve_EasyPuzzle()
        {
            var puzzle = TestHelpers.GetEasyPuzzle();
            var solver = new BacktrackingSolver(puzzle);
            solver.Solve();
            Assert.True(solver.SolveDepth > 0);
            Assert.True(solver.SolveDuration > TimeSpan.Zero);
            Assert.True(solver.Puzzle.IsSolved());
        }

        [Fact]
        public void Solve_MediumPuzzle()
        {
            var puzzle = TestHelpers.GetMediumPuzzle();
            var solver = new BacktrackingSolver(puzzle);
            solver.Solve();
            Assert.True(solver.SolveDepth > 0);
            Assert.True(solver.SolveDuration > TimeSpan.Zero);
            Assert.True(solver.Puzzle.IsSolved());
        }

        [Fact]
        public void Solve_DifficultPuzzle()
        {
            var puzzle = TestHelpers.GetDifficultPuzzle();
            var solver = new BacktrackingSolver(puzzle);
            solver.Solve();
            Assert.True(solver.SolveDepth > 0);
            Assert.True(solver.SolveDuration > TimeSpan.Zero);
            Assert.True(solver.Puzzle.IsSolved());
        }
    }
}