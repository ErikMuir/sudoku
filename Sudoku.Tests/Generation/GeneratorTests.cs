// using System.Collections.Generic;
// using System.Linq;
// using Sudoku.Generation;
// using Sudoku.Logic;
// using Xunit;

// namespace Sudoku.Tests
// {
//     public class GeneratorTests
//     {
//         private static readonly SymmetryType[] _supportedSymmetries = new[]
//         {
//             Horizontal.Symmetry.Type,
//             Vertical.Symmetry.Type,
//             DiagonalUp.Symmetry.Type,
//             DiagonalDown.Symmetry.Type,
//             RotationalTwoFold.Symmetry.Type,
//             RotationalFourFold.Symmetry.Type,
//         };

//         private void _puzzleAssertions(Puzzle puzzle, ISymmetry symmetry = null)
//         {
//             Assert.NotNull(puzzle);
//             IEnumerable<Cell> clues = puzzle.Cells.Where(cell => cell.Type == CellType.Clue);
//             IEnumerable<Cell> filled = puzzle.Cells.Where(cell => cell.Type == CellType.Filled);
//             IEnumerable<Cell> empties = puzzle.Cells.Where(cell => cell.Type == CellType.Empty);
//             Assert.NotEmpty(clues);
//             Assert.NotEmpty(empties);
//             Assert.Empty(filled);
//             if (symmetry is not null)
//                 Assert.Equal(symmetry.Type, puzzle.Metadata.Symmetry);
//             else
//                 Assert.Contains(puzzle.Metadata.Symmetry, _supportedSymmetries);
//         }

//         [Fact]
//         public void Generate()
//         {
//             GenerationOptions config = new();
//             Puzzle puzzle = Generator.Generate();
//             _puzzleAssertions(puzzle, Asymmetric.Symmetry);
//         }

//         [Fact]
//         public void Generate_AsymmetricSymmetry()
//         {
//             ISymmetry symmetry = Asymmetric.Symmetry;
//             Puzzle puzzle = Generator.Generate(symmetry);
//             _puzzleAssertions(puzzle, symmetry);
//         }

//         [Fact]
//         public void Generate_HorizontalSymmetry()
//         {
//             ISymmetry symmetry = Horizontal.Symmetry;
//             Puzzle puzzle = Generator.Generate(symmetry);
//             _puzzleAssertions(puzzle, symmetry);
//         }

//         [Fact]
//         public void Generate_VerticalSymmetry()
//         {
//             ISymmetry symmetry = Vertical.Symmetry;
//             Puzzle puzzle = Generator.Generate(symmetry);
//             _puzzleAssertions(puzzle, symmetry);
//         }

//         [Fact]
//         public void Generate_DiagonalUpSymmetry()
//         {
//             ISymmetry symmetry = DiagonalUp.Symmetry;
//             Puzzle puzzle = Generator.Generate(symmetry);
//             _puzzleAssertions(puzzle, symmetry);
//         }

//         [Fact]
//         public void Generate_DiagonalDownSymmetry()
//         {
//             ISymmetry symmetry = DiagonalDown.Symmetry;
//             Puzzle puzzle = Generator.Generate(symmetry);
//             _puzzleAssertions(puzzle, symmetry);
//         }

//         [Fact]
//         public void Generate_RotationalTwoFoldSymmetry()
//         {
//             ISymmetry symmetry = RotationalTwoFold.Symmetry;
//             Puzzle puzzle = Generator.Generate(symmetry);
//             _puzzleAssertions(puzzle, symmetry);
//         }

//         [Fact]
//         public void Generate_RotationalFourFoldSymmetry()
//         {
//             ISymmetry symmetry = RotationalFourFold.Symmetry;
//             Puzzle puzzle = Generator.Generate(symmetry);
//             _puzzleAssertions(puzzle, symmetry);
//         }

//         [Fact]
//         public void GenerateRandomSymmetry()
//         {
//             Puzzle puzzle = Generator.GenerateRandomSymmetry();
//             _puzzleAssertions(puzzle);
//         }
//     }
// }
