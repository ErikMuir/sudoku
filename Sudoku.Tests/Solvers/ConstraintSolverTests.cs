using System;
using System.Collections.Generic;
using Sudoku.Solution;
using Xunit;
using Xunit.Abstractions;

namespace Sudoku.Tests
{
    public class ConstraintSolverTests
    {
        private readonly ITestOutputHelper _logger;
        private readonly Puzzle _emptyPuzzle;

        public ConstraintSolverTests(ITestOutputHelper logger)
        {
            _logger = logger;
            _logger.WriteLine("");
            _emptyPuzzle = TestHelpers.GetEmptyPuzzle();
        }

        [Fact]
        public void Solve_EmptyPuzzle()
        {
            ConstraintSolver solver = new(_emptyPuzzle);
            solver.Solve();
            Assert.False(solver.Puzzle.IsSolved());
            Assert.True(solver.SolveDuration > TimeSpan.Zero);
            Assert.Empty(solver.Logs);
        }

        [Fact]
        public void Solve_SolvedPuzzle()
        {
            Puzzle puzzle = TestHelpers.GetSolvedPuzzle();
            ConstraintSolver solver = new(puzzle);
            solver.Solve();
            Assert.True(solver.Puzzle.IsSolved());
            Assert.Equal(TimeSpan.Zero, solver.SolveDuration);
            Assert.Empty(solver.Logs);
        }

        [Fact]
        public void Solve_EasyPuzzle()
        {
            Puzzle puzzle = TestHelpers.GetEasyPuzzle();
            ConstraintSolver solver = new(puzzle);
            solver.Solve();
            Assert.True(solver.Puzzle.IsSolved());
            Assert.True(solver.SolveDuration > TimeSpan.Zero);
            Assert.NotEmpty(solver.Logs);
        }

        [Fact]
        public void Solve_MediumPuzzle()
        {
            Puzzle puzzle = TestHelpers.GetMediumPuzzle();
            ConstraintSolver solver = new(puzzle);
            solver.Solve();
            Assert.True(solver.Puzzle.IsSolved());
            Assert.True(solver.SolveDuration > TimeSpan.Zero);
            Assert.NotEmpty(solver.Logs);
        }

        [Fact]
        public void Solve_DifficultPuzzle()
        {
            Puzzle puzzle = TestHelpers.GetDifficultPuzzle();
            ConstraintSolver solver = new(puzzle);
            solver.Solve();
            Assert.True(solver.Puzzle.IsSolved());
            Assert.True(solver.SolveDuration > TimeSpan.Zero);
            Assert.NotEmpty(solver.Logs);
        }

        [Fact]
        public void Solve_XWingPuzzle()
        {
            Puzzle puzzle = TestHelpers.GetXWingPuzzle();
            ConstraintSolver solver = new(puzzle);
            solver.Solve();
            Assert.True(solver.Puzzle.IsSolved());
            Assert.True(solver.SolveDuration > TimeSpan.Zero);
            Assert.NotEmpty(solver.Logs);
            Assert.Equal(42, solver.Logs.Count);
        }

        [Fact]
        public void NakedSingle_Returns_False()
        {
            ConstraintSolver solver = new(_emptyPuzzle);
            Assert.False(solver.NakedSingle());
            Assert.Empty(solver.Logs);
        }

        [Theory]
        [ClassData(typeof(OneToNineTestData))]
        public void NakedSingle_Returns_True(int val)
        {
            _emptyPuzzle.Cells[0].AddCandidate(val);
            ConstraintSolver solver = new(_emptyPuzzle);
            bool actual = solver.NakedSingle();
            Assert.True(actual);
            Assert.Equal(val, solver.Puzzle.Cells[0].Value);
            Assert.Equal(ConstraintType.NakedSingle, solver.Logs[0].Constraint);
        }

        [Fact]
        public void HiddenSingle_Returns_False()
        {
            ConstraintSolver solver = new(_emptyPuzzle);
            Assert.False(solver.HiddenSingle());
            Assert.Empty(solver.Logs);
        }

        [Theory]
        [ClassData(typeof(OneToNineTestData))]
        public void HiddenSingle_Returns_True(int val)
        {
            int otherVal = val == 9 ? 1 : val + 1;
            _emptyPuzzle.GetCell(0, 0).AddCandidate(val);
            _emptyPuzzle.GetCell(0, 0).AddCandidate(otherVal);
            _emptyPuzzle.GetCell(0, 1).AddCandidate(otherVal);
            _emptyPuzzle.GetCell(1, 0).AddCandidate(otherVal);
            ConstraintSolver solver = new(_emptyPuzzle);
            bool actual = solver.HiddenSingle();
            Assert.True(actual);
            Assert.Equal(val, solver.Puzzle.Cells[0].Value);
            Assert.Equal(ConstraintType.HiddenSingle, solver.Logs[0].Constraint);
        }

        [Fact]
        public void NakedSet_Returns_False()
        {
            ConstraintSolver solver = new(_emptyPuzzle);
            List<CandidateSet> sets = new() { new Double(1, 2) };
            Assert.False(solver.NakedSet(sets));
            Assert.Empty(solver.Logs);
        }

        [Fact]
        public void NakedSet_Double_Returns_True()
        {
            Puzzle puzzle = TestHelpers.GetDifficultPuzzle();
            puzzle.CalculateCandidates();
            ConstraintSolver solver = new(puzzle);
            List<CandidateSet> sets = new() { new Double(2, 4) };
            Assert.True(solver.NakedSet(sets));
            Assert.Equal(ConstraintType.NakedDouble, solver.Logs[0].Constraint);
        }

        [Fact]
        public void HiddenSet_Returns_False()
        {
            ConstraintSolver solver = new(_emptyPuzzle);
            List<CandidateSet> sets = new() { new Double(1, 2) };
            Assert.False(solver.HiddenSet(sets));
            Assert.Empty(solver.Logs);
        }

        [Fact]
        public void HiddenSet_Double_Returns_True()
        {
            Puzzle puzzle = TestHelpers.GetDifficultPuzzle();
            puzzle.CalculateCandidates();
            ConstraintSolver solver = new(puzzle);
            List<CandidateSet> sets = new() { new Double(1, 6) };
            Assert.True(solver.HiddenSet(sets));
            Assert.Equal(ConstraintType.HiddenDouble, solver.Logs[0].Constraint);
        }

        [Fact]
        public void PointingSet_Returns_False()
        {
            ConstraintSolver solver = new(_emptyPuzzle);
            Assert.False(solver.PointingSet());
            Assert.Empty(solver.Logs);
        }

        [Fact]
        public void PointingSet_Returns_True()
        {
            Puzzle puzzle = TestHelpers.GetDifficultPuzzle();
            puzzle.CalculateCandidates();
            ConstraintSolver solver = new(puzzle);
            Assert.True(solver.PointingSet());
            Assert.Equal(ConstraintType.PointingSet, solver.Logs[0].Constraint);
        }

        [Fact]
        public void BoxLineReduction_Returns_False()
        {
            ConstraintSolver solver = new(_emptyPuzzle);
            Assert.False(solver.BoxLineReduction());
            Assert.Empty(solver.Logs);
        }

        [Fact]
        public void BoxLineReduction_Returns_True()
        {
            Puzzle puzzle = TestHelpers.GetDifficultPuzzle();
            puzzle.CalculateCandidates();
            ConstraintSolver solver = new(puzzle);
            Assert.True(solver.BoxLineReduction());
            Assert.Equal(ConstraintType.BoxLineReduction, solver.Logs[0].Constraint);
        }

        [Fact]
        public void XWing_Returns_False()
        {
            ConstraintSolver solver = new(_emptyPuzzle);
            Assert.False(solver.XWing());
            Assert.Empty(solver.Logs);
        }

        [Fact]
        public void XWing_Returns_True()
        {
            Puzzle puzzle = TestHelpers.GetDifficultPuzzle();
            puzzle.CalculateCandidates();
            ConstraintSolver solver = new(puzzle);
            Assert.True(solver.XWing());
            Assert.Equal(ConstraintType.XWing, solver.Logs[0].Constraint);
        }

        [Fact]
        public void YWing_Returns_False()
        {
            ConstraintSolver solver = new(_emptyPuzzle);
            Assert.False(solver.YWing());
            Assert.Empty(solver.Logs);
        }

        [Fact]
        public void YWing_Returns_True()
        {
            Puzzle puzzle = TestHelpers.GetDifficultPuzzle();
            puzzle.CalculateCandidates();
            ConstraintSolver solver = new(puzzle);
            Assert.True(solver.YWing());
            Assert.Equal(ConstraintType.YWing, solver.Logs[0].Constraint);
        }
    }
}
