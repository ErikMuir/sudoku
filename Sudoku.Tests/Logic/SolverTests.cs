using System.Collections.Generic;
using Sudoku.Logic;
using Xunit;

namespace Sudoku.Tests
{
    public class SolverTests
    {
        [Fact]
        public void Solve_Returns_SolvedPuzzle()
        {
            Puzzle puzzle = TestHelpers.GetEasyPuzzle();
            Puzzle actual = Solver.Solve(puzzle);
            Assert.True(actual.IsSolved());
        }

        [Fact]
        public void Solve_DoesNotMutateInput()
        {
            Puzzle puzzle = TestHelpers.GetEasyPuzzle();
            Puzzle clone = new(puzzle);
            Puzzle actual = Solver.Solve(clone);
            Assert.NotSame(clone, actual);
            for (int i = 0; i < 81; i++)
            {
                Assert.Equal(puzzle.Cells[i].Value, clone.Cells[i].Value);
            }
        }

        [Fact]
        public void Solve_Returns_Null()
        {
            Puzzle puzzle = TestHelpers.GetUnsolvablePuzzle();
            Puzzle actual = Solver.Solve(puzzle);
            Assert.Null(actual);
        }

        [Fact]
        public void MultiSolve_Returns_NoSolutions()
        {
            Puzzle puzzle = TestHelpers.GetUnsolvablePuzzle();
            List<Puzzle> solutions = Solver.MultiSolve(puzzle);
            Assert.Empty(solutions);
        }

        [Fact]
        public void MultiSolve_Returns_AllSolutions()
        {
            Puzzle puzzle = TestHelpers.GetPuzzleWithExactlyTwoSolutions();
            List<Puzzle> solutions = Solver.MultiSolve(puzzle);
            Assert.Equal(2, solutions.Count);
        }

        [Fact]
        public void MultiSolve_Returns_MaxSolutions()
        {
            Puzzle puzzle = TestHelpers.GetEmptyPuzzle();
            List<Puzzle> solutions = Solver.MultiSolve(puzzle, 3);
            Assert.Equal(3, solutions.Count);
        }
    }
}
