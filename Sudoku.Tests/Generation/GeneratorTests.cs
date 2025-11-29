// namespace Sudoku.Tests.Generation;

// public class GeneratorTests
// {
//     private static readonly SymmetryType[] _supportedSymmetries =
//     [
//         Horizontal.Symmetry.Type,
//         Vertical.Symmetry.Type,
//         DiagonalUp.Symmetry.Type,
//         DiagonalDown.Symmetry.Type,
//         RotationalTwoFold.Symmetry.Type,
//         RotationalFourFold.Symmetry.Type,
//     ];

//     private static void PuzzleAssertions(Puzzle puzzle, ISymmetry symmetry = null)
//     {
//         Assert.NotNull(puzzle);
//         var clues = puzzle.Cells.Where(cell => cell.Type == CellType.Clue);
//         var filled = puzzle.Cells.Where(cell => cell.Type == CellType.Filled);
//         var empties = puzzle.Cells.Where(cell => cell.Type == CellType.Empty);
//         Assert.NotEmpty(clues);
//         Assert.NotEmpty(empties);
//         Assert.Empty(filled);
//         if (symmetry is not null)
//             Assert.Equal(symmetry.Type, puzzle.Metadata.Symmetry);
//         else
//             Assert.Contains(puzzle.Metadata.Symmetry, _supportedSymmetries);
//     }

//     [Fact]
//     public void Generate_AsymmetricSymmetry()
//     {
//         var symmetry = Asymmetric.Symmetry;
//         var options = new GenerationOptions { Symmetry = symmetry };
//         var puzzle = Generator.Generate(options);
//         PuzzleAssertions(puzzle, symmetry);
//     }

//     [Fact]
//     public void Generate_HorizontalSymmetry()
//     {
//         var symmetry = Horizontal.Symmetry;
//         var options = new GenerationOptions { Symmetry = symmetry };
//         var puzzle = Generator.Generate(options);
//         PuzzleAssertions(puzzle, symmetry);
//     }

//     [Fact]
//     public void Generate_VerticalSymmetry()
//     {
//         var symmetry = Vertical.Symmetry;
//         var options = new GenerationOptions { Symmetry = symmetry };
//         var puzzle = Generator.Generate(options);
//         PuzzleAssertions(puzzle, symmetry);
//     }

//     [Fact]
//     public void Generate_DiagonalUpSymmetry()
//     {
//         var symmetry = DiagonalUp.Symmetry;
//         var options = new GenerationOptions { Symmetry = symmetry };
//         var puzzle = Generator.Generate(options);
//         PuzzleAssertions(puzzle, symmetry);
//     }

//     [Fact]
//     public void Generate_DiagonalDownSymmetry()
//     {
//         var symmetry = DiagonalDown.Symmetry;
//         var options = new GenerationOptions { Symmetry = symmetry };
//         var puzzle = Generator.Generate(options);
//         PuzzleAssertions(puzzle, symmetry);
//     }

//     [Fact]
//     public void Generate_RotationalTwoFoldSymmetry()
//     {
//         var symmetry = RotationalTwoFold.Symmetry;
//         var options = new GenerationOptions { Symmetry = symmetry };
//         var puzzle = Generator.Generate(options);
//         PuzzleAssertions(puzzle, symmetry);
//     }

//     [Fact]
//     public void Generate_RotationalFourFoldSymmetry()
//     {
//         var symmetry = RotationalFourFold.Symmetry;
//         var options = new GenerationOptions { Symmetry = symmetry };
//         var puzzle = Generator.Generate(options);
//         PuzzleAssertions(puzzle, symmetry);
//     }
// }
