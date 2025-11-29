namespace Sudoku.Tests.Logic;

public class SolverTests
{
    [Fact]
    public void Solve_Returns_SolvedPuzzle()
    {
        var puzzle = TestHelpers.GetEasyPuzzle();
        var actual = Solver.Solve(puzzle);
        Assert.True(actual.IsSolved);
    }

    [Fact]
    public void Solve_DoesNotMutateInput()
    {
        var puzzle = TestHelpers.GetEasyPuzzle();
        var clone = new Puzzle(puzzle);
        var actual = Solver.Solve(clone);
        Assert.NotSame(clone, actual);
        for (var i = 0; i < 81; i++)
        {
            Assert.Equal(puzzle.Cells[i].Value, clone.Cells[i].Value);
        }
    }

    [Fact]
    public void Solve_Returns_Null()
    {
        var puzzle = TestHelpers.GetUnsolvablePuzzle();
        var actual = Solver.Solve(puzzle);
        Assert.Null(actual);
    }

    [Fact]
    public void MultiSolve_Returns_NoSolutions()
    {
        var puzzle = TestHelpers.GetUnsolvablePuzzle();
        var solutions = Solver.MultiSolve(puzzle);
        Assert.Empty(solutions);
    }

    [Fact]
    public void MultiSolve_Returns_AllSolutions()
    {
        var puzzle = TestHelpers.GetPuzzleWithExactlyTwoSolutions();
        var solutions = Solver.MultiSolve(puzzle);
        Assert.Equal(2, solutions.Count);
    }

    [Fact]
    public void MultiSolve_Returns_MaxSolutions()
    {
        var puzzle = TestHelpers.GetEmptyPuzzle();
        var solutions = Solver.MultiSolve(puzzle, 3);
        Assert.Equal(3, solutions.Count);
    }
}
