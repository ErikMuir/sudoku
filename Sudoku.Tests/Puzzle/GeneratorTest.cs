using System.Linq;
using Sudoku.Generators;
using Xunit;

namespace Sudoku.Tests
{
    public class GeneratorTest
    {
        private readonly GeneratorPuzzle testObject;

        public GeneratorTest()
        {
            testObject = new GeneratorPuzzle(9);
        }

        [Fact]
        public void Constructor_Sets_Cells()
        {
            Assert.Equal(81, testObject.Cells.Length);
            Assert.Equal(9, testObject.Cells[0].Length);
        }

        [Fact]
        public void Constructor_Copies_Cells()
        {
            GeneratorPuzzle copy = new(testObject);
            for (int i = 0; i < testObject.Cells.Length; i++)
            {
                Assert.Equal(copy.Cells[i], testObject.Cells[i]);
            }
        }

        [Fact]
        public void Constructor_Parses_Cells()
        {
            // TODO
            Assert.True(true);
        }

        [Fact]
        public void Peers()
        {
            var peers = testObject.Peers(40);
            var expectedResult = new int[] { 4, 13, 22, 30, 31, 32, 36, 37, 38, 39, 41, 42, 43, 44, 48, 49, 50, 58, 67, 76 };
            Assert.Equal(expectedResult, peers);
        }

        [Fact]
        public void Solve()
        {
            GeneratorPuzzle solved = GeneratorPuzzle.Solve(testObject);
            Assert.True(solved.Cells.All(c => c.Length == 1));
            // SudokuPuzzle puzzle = new("..3.2.6..9..3.5..1..18.64....81.29..7.......8..67.82....26.95..8..2.3..9..5.1.3..");
            // int[] expectedResult = new[]
            // {
            //     4, 8, 3, 9, 2, 1, 6, 5, 7, 9, 6, 7, 3, 4, 5, 8, 2, 1, 2, 5, 1, 8, 7, 6, 4, 9, 3,
            //     5, 4, 8, 1, 3, 2, 9, 7, 6, 7, 2, 9, 5, 6, 4, 1, 3, 8, 1, 3, 6, 7, 9, 8, 2, 4, 5,
            //     3, 7, 2, 6, 8, 9, 5, 1, 4, 8, 1, 4, 2, 5, 3, 7, 6, 9, 6, 9, 5, 4, 1, 7, 3, 8, 2,
            // };
            // SudokuPuzzle actual = SudokuPuzzle.Solve(puzzle);
            // Assert.Equal(expectedResult, actual.Cells.Select(c => c[0]));
        }

        [Theory]
        [InlineData(1, 9)]
        [InlineData(12, 28)]
        [InlineData(8, 72)]
        [InlineData(76, 44)]
        [InlineData(64, 16)]
        public void Reflection_DiagonalDown(int cell, int expected)
        {
            GeneratorPuzzle puzzle = new(9);
            int[] actual = GeneratorPuzzle.GetDiagonalDownReflection(puzzle, cell);
            Assert.Equal(new[] { expected }, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(30)]
        [InlineData(40)]
        [InlineData(50)]
        [InlineData(60)]
        [InlineData(70)]
        [InlineData(80)]
        public void Reflection_DiagonalDown_Axis(int cell)
        {
            GeneratorPuzzle puzzle = new(9);
            int[] actual = GeneratorPuzzle.GetDiagonalDownReflection(puzzle, cell);
            Assert.Equal(new int[] { }, actual);
        }

        [Theory]
        [InlineData(7, 17)]
        [InlineData(14, 34)]
        [InlineData(0, 80)]
        [InlineData(44, 4)]
        [InlineData(70, 10)]
        [InlineData(60, 20)]
        public void Reflection_DiagonalUp(int cell, int expected)
        {
            GeneratorPuzzle puzzle = new(9);
            int[] actual = GeneratorPuzzle.GetDiagonalUpReflection(puzzle, cell);
            Assert.Equal(new[] { expected }, actual);
        }

        [Theory]
        [InlineData(8)]
        [InlineData(16)]
        [InlineData(24)]
        [InlineData(32)]
        [InlineData(40)]
        [InlineData(48)]
        [InlineData(56)]
        [InlineData(64)]
        [InlineData(72)]
        public void Reflection_DiagonalUp_Axis(int cell)
        {
            GeneratorPuzzle puzzle = new(9);
            int[] actual = GeneratorPuzzle.GetDiagonalUpReflection(puzzle, cell);
            Assert.Equal(new int[] { }, actual);
        }

        [Theory]
        [InlineData(0, 2, 2, 8)]
        [InlineData(2, 8, 8, 6)]
        [InlineData(8, 6, 6, 0)]
        [InlineData(6, 0, 0, 2)]
        [InlineData(1, 2, 2, 7)]
        [InlineData(6, 2, 2, 2)]
        public void RotateQuarterTurn(int sourceRow, int sourceCol, int targetRow, int targetCol)
        {
            GeneratorPuzzle solved = GeneratorPuzzle.Solve(testObject);
            GeneratorPuzzle rotated = GeneratorPuzzle.Rotate(solved);
            int sourceIndex = (sourceRow * 9) + sourceCol;
            int targetIndex = (targetRow * 9) + targetCol;
            Assert.Equal(rotated.Cells[targetIndex], solved.Cells[sourceIndex]);
        }
    }
}

/*
----------------------------------
|   |  0  1  2  3  4  5  6  7  8 |
|---+----------------------------|
| 0 |  0  1  2  3  4  5  6  7  8 |
| 1 |  9 10 11 12 13 14 15 16 17 |
| 2 | 18 19 20 21 22 23 24 25 26 |
| 3 | 27 28 29 30 31 32 33 34 35 |
| 4 | 36 37 38 39 40 41 42 43 44 |
| 5 | 45 46 47 48 49 50 51 52 53 |
| 6 | 54 55 56 57 58 59 60 61 62 |
| 7 | 63 64 65 66 67 68 69 70 71 |
| 8 | 72 73 74 75 76 77 78 79 80 |
----------------------------------
*/
