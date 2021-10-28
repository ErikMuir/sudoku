using System.Linq;
using Sudoku.Generators;
using Xunit;

namespace Sudoku.Tests
{
    public class GeneratorTest
    {
        private readonly GeneratorPuzzle _testObject;

        public GeneratorTest()
        {
            _testObject = new GeneratorPuzzle(9);
        }

        [Fact]
        public void Constructor_Sets_Cells()
        {
            Assert.Equal(81, _testObject.Cells.Length);
            Assert.Equal(9, _testObject.Cells[0].Length);
        }

        [Fact]
        public void Constructor_Copies_Cells()
        {
            GeneratorPuzzle copy = new(_testObject);
            for (int i = 0; i < _testObject.Cells.Length; i++)
            {
                Assert.Equal(copy.Cells[i], _testObject.Cells[i]);
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
            var peers = _testObject.Peers(40);
            var expectedResult = new int[] { 4, 13, 22, 30, 31, 32, 36, 37, 38, 39, 41, 42, 43, 44, 48, 49, 50, 58, 67, 76 };
            Assert.Equal(expectedResult, peers);
        }

        [Fact]
        public void Solve()
        {
            GeneratorPuzzle solved = GeneratorPuzzle.Solve(_testObject);
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
        public void Reflection_Diagonal_Down(int cell, int expected)
        {
            int[] actual = GeneratorPuzzle.GetDiagonalDownReflection(_testObject, cell);
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
        public void Reflection_Diagonal_Down_Axis(int cell)
        {
            int[] actual = GeneratorPuzzle.GetDiagonalDownReflection(_testObject, cell);
            Assert.Equal(new int[] { }, actual);
        }

        [Theory]
        [InlineData(7, 17)]
        [InlineData(14, 34)]
        [InlineData(0, 80)]
        [InlineData(44, 4)]
        [InlineData(70, 10)]
        [InlineData(60, 20)]
        public void Reflection_Diagonal_Up(int cell, int expected)
        {
            int[] actual = GeneratorPuzzle.GetDiagonalUpReflection(_testObject, cell);
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
        public void Reflection_Diagonal_Up_Axis(int cell)
        {
            int[] actual = GeneratorPuzzle.GetDiagonalUpReflection(_testObject, cell);
            Assert.Equal(new int[] { }, actual);
        }

        [Theory]
        [InlineData(1, 79)]
        [InlineData(10, 70)]
        [InlineData(53, 27)]
        [InlineData(30, 50)]
        [InlineData(76, 4)]
        [InlineData(46, 34)]
        public void Reflection_Rotational_TwoFold(int cell, int expected)
        {
            int[] actual = GeneratorPuzzle.GetRotationalTwoFoldReflection(_testObject, cell);
            Assert.Equal(new[] { expected }, actual);
        }

        [Fact]
        public void Reflection_Rotational_TwoFold_Axis()
        {
            int[] actual = GeneratorPuzzle.GetRotationalTwoFoldReflection(_testObject, 40);
            Assert.Equal(new int[] { }, actual);
        }

        [Theory]
        [InlineData(1, 17, 79, 63)]
        [InlineData(10, 16, 70, 64)]
        [InlineData(53, 75, 27, 5)]
        [InlineData(30, 32, 50, 48)]
        [InlineData(76, 36, 4, 44)]
        [InlineData(46, 12, 34, 68)]
        public void Reflection_Rotational_FourFold(int cell, int expected1, int expected2, int expected3)
        {
            int[] actual = GeneratorPuzzle.GetRotationalFourFoldReflection(_testObject, cell);
            Assert.Equal(new[] { expected1, expected2, expected3 }, actual);
        }

        [Fact]
        public void Reflection_Rotational_FourFold_Axis()
        {
            int[] actual = GeneratorPuzzle.GetRotationalFourFoldReflection(_testObject, 40);
            Assert.Equal(new int[] { }, actual);
        }

        [Theory]
        [InlineData(2, 26)]
        [InlineData(26, 78)]
        [InlineData(78, 54)]
        [InlineData(54, 2)]
        [InlineData(11, 25)]
        [InlineData(56, 20)]
        public void RotateCell(int sourceIndex, int expected)
        {
            int actual = GeneratorPuzzle.RotateCell(_testObject, sourceIndex);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(2, 26)]
        [InlineData(26, 78)]
        [InlineData(78, 54)]
        [InlineData(54, 2)]
        [InlineData(11, 25)]
        [InlineData(56, 20)]
        public void RotatePuzzle(int sourceIndex, int targetIndex)
        {
            GeneratorPuzzle solved = GeneratorPuzzle.Solve(_testObject);
            GeneratorPuzzle rotated = GeneratorPuzzle.RotatePuzzle(solved);
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
