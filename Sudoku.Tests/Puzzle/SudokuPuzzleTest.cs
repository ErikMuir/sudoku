using System.Linq;
using Xunit;

namespace Sudoku.Tests
{
    public class SudokuPuzzleTest
    {
        [Fact]
        public void Constructor_Sets_Cells()
        {
            SudokuPuzzle puzzle = new(9);
            Assert.Equal(81, puzzle.Cells.Length);
            Assert.Equal(9, puzzle.Cells[0].Length);

            var peers = puzzle.Peers(40);
            var expectedResult = new int[] { 4, 13, 22, 30, 31, 32, 36, 37, 38, 39, 41, 42, 43, 44, 48, 49, 50, 58, 67, 76 };
            Assert.Equal(expectedResult, peers);
        }

        [Fact]
        public static void SimpleTest()
        {
            SudokuPuzzle puzzle = new("..3.2.6..9..3.5..1..18.64....81.29..7.......8..67.82....26.95..8..2.3..9..5.1.3..");
            int[] expectedResult = new[]
            {
                4, 8, 3, 9, 2, 1, 6, 5, 7, 9, 6, 7, 3, 4, 5, 8, 2, 1, 2, 5, 1, 8, 7, 6, 4, 9, 3,
                5, 4, 8, 1, 3, 2, 9, 7, 6, 7, 2, 9, 5, 6, 4, 1, 3, 8, 1, 3, 6, 7, 9, 8, 2, 4, 5,
                3, 7, 2, 6, 8, 9, 5, 1, 4, 8, 1, 4, 2, 5, 3, 7, 6, 9, 6, 9, 5, 4, 1, 7, 3, 8, 2,
            };
            SudokuPuzzle actual = SudokuPuzzle.Solve(puzzle);
            Assert.Equal(expectedResult, actual.Cells.Select(c => c[0]));
        }

        [Fact]
        public static void RandomGridTest()
        {
            SudokuPuzzle puzzle = SudokuPuzzle.RandomGrid(9);
            Assert.True(puzzle.Cells.All(c => c.Length == 1));
            SudokuPuzzle puzzle2 = new(puzzle.Cells.Select(c => c.Single()).ToArray());
        }
    }
}
