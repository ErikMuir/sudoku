using System;
using System.Collections.Generic;
using System.Linq;
using Sudoku.Logic;

namespace Sudoku.Generation
{
    public static class Generator
    {
        private static readonly Random _rand = new();
        private static readonly ISymmetry[] _supportedSymmetries = new[]
        {
            Horizontal.Symmetry,
            Vertical.Symmetry,
            DiagonalUp.Symmetry,
            DiagonalDown.Symmetry,
            RotationalTwoFold.Symmetry,
            RotationalFourFold.Symmetry,
        };

        public static Puzzle Generate() => _generate(Asymmetric.Symmetry);
        public static Puzzle Generate(ISymmetry symmetry) => _generate(symmetry);
        public static Puzzle GenerateRandomSymmetry() => _generate(_supportedSymmetries[_rand.Next(_supportedSymmetries.Length)]);

        private static Puzzle _generate(ISymmetry symmetry)
        {
            while (true)
            {
                Puzzle puzzle = new(symmetry);
                puzzle.FillCandidates();
                puzzle.ReduceCandidates();
                int puzzleIterations = 0;

                while (puzzleIterations < 100)
                {
                    puzzleIterations++;
                    Puzzle workingPuzzle = new(puzzle);
                    Cell randomEmptyCell = _getRandomEmptyCell(puzzle);
                    int[] reflections = symmetry.GetReflections(randomEmptyCell.Index);
                    for (int i = 0; i < reflections.Length && workingPuzzle is not null; i++)
                    {
                        Cell cell = puzzle.Cells[reflections[i]];
                        int candidateValue = cell.Candidates[_rand.Next(cell.Candidates.Count)];
                        workingPuzzle = _placeValue(workingPuzzle, cell.Index, candidateValue);
                    }

                    if (workingPuzzle is null) continue;

                    List<Puzzle> solutions = Solver.MultiSolve(workingPuzzle, 2);
                    switch (solutions.Count)
                    {
                        case 0: continue;
                        case 1: return _finalizePuzzle(workingPuzzle);
                        default: puzzle = workingPuzzle; break;
                    }
                }
            }
        }

        private static Cell _getRandomEmptyCell(Puzzle puzzle)
        {
            Cell[] emptyCells = puzzle.GetEmptyCells();
            return emptyCells[_rand.Next(emptyCells.Length)];
        }

        private static Puzzle _placeValue(Puzzle input, int cellIndex, int value)
        {
            Puzzle puzzle = new(input);
            Cell cell = puzzle.Cells[cellIndex];
            if (!cell.Candidates.Contains(value))
                return null;
            cell.Value = value;
            foreach (Cell peer in puzzle.Peers(cell))
            {
                if (peer.Value is not null)
                    continue;
                int newCandidateCount = peer.Candidates.Except(new int[] { value }).Count();
                if (newCandidateCount == 0)
                    return null;
                if (newCandidateCount == 1 && peer.Candidates.Count > 1)
                    return null;
                peer.RemoveCandidate(value);
            }
            return puzzle;
        }

        private static Puzzle _finalizePuzzle(Puzzle puzzle)
        {
            for (int i = 0; i < puzzle.Cells.Length; i++)
            {
                Cell cell = puzzle.Cells[i];
                if (cell.Value is null) continue;
                puzzle.Cells[i] = new Clue(cell.Row, cell.Col, (int)cell.Value);
            }
            return puzzle;
        }
    }
}
