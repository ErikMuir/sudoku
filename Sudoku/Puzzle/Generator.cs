using System;
using System.Collections.Generic;
using System.Linq;
using Sudoku.Symmetries;

namespace Sudoku
{
    public static class Generator
    {
        private static readonly Random _rand = new();

        public static Puzzle Generate()
        {
            Puzzle puzzle = new();
            puzzle.CalculateCandidates();

            while (true)
            {
                Cell cell = GetRandomEmptyCell(puzzle);
                int randomIndex = _rand.Next(cell.Candidates.Count);
                int candidateValue = cell.Candidates[randomIndex];
                Puzzle workingPuzzle = PlaceValue(puzzle, cell.Index, candidateValue);
                if (workingPuzzle is null) continue;
                List<Puzzle> solutions = Solver.MultiSolve(workingPuzzle, 2);
                switch (solutions.Count)
                {
                    case 0: continue;
                    case 1: return workingPuzzle;
                    default: puzzle = workingPuzzle; break;
                }
            }
        }

        public static Puzzle Generate(SymmetryType symmetryType)
        {
            if (symmetryType == SymmetryType.None)
                return Generate();

            Symmetry symmetry = GetSymmetry(symmetryType);

            while (true)
            {
                Puzzle puzzle = new();
                puzzle.CalculateCandidates();
                int puzzleIterations = 0;

                while (puzzleIterations < 100)
                {
                    puzzleIterations++;
                    Puzzle workingPuzzle = new(puzzle);
                    Cell randomEmptyCell = GetRandomEmptyCell(puzzle);
                    Cell[] reflections = symmetry.GetReflections(puzzle, randomEmptyCell);
                    for (int i = 0; i < reflections.Length && workingPuzzle is not null; i++)
                    {
                        Cell cell = reflections[i];
                        int candidateValue = cell.Candidates[_rand.Next(cell.Candidates.Count)];
                        workingPuzzle = PlaceValue(workingPuzzle, cell.Index, candidateValue);
                    }

                    if (workingPuzzle is null) continue;

                    List<Puzzle> solutions = Solver.MultiSolve(workingPuzzle, 2);
                    switch (solutions.Count)
                    {
                        case 0: continue;
                        case 1: return workingPuzzle;
                        default: puzzle = workingPuzzle; break;
                    }
                }
            }
        }

        private static Cell GetRandomEmptyCell(Puzzle puzzle)
        {
            Cell[] emptyCells = puzzle.GetEmptyCells();
            return emptyCells[_rand.Next(emptyCells.Length)];
        }

        private static Puzzle PlaceValue(Puzzle input, int cellIndex, int value)
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

        private static readonly SymmetryType[] _supportedSymmetryTypes = new[]
        {
            SymmetryType.Horizontal,
            SymmetryType.Vertical,
            SymmetryType.DiagonalUp,
            SymmetryType.DiagonalDown,
            SymmetryType.RotationalTwoFold,
            SymmetryType.RotationalFourFold,
        };

        private static Symmetry GetSymmetry(SymmetryType type)
        {
            if (type == SymmetryType.Random)
                type = _supportedSymmetryTypes[_rand.Next(_supportedSymmetryTypes.Length)];
            return type switch
            {
                SymmetryType.None => new NoSymmetry(),
                SymmetryType.Horizontal => new HorizontalSymmetry(),
                SymmetryType.Vertical => new VerticalSymmetry(),
                SymmetryType.DiagonalUp => new DiagonalUpSymmetry(),
                SymmetryType.DiagonalDown => new DiagonalDownSymmetry(),
                SymmetryType.RotationalTwoFold => new RotationalTwoFoldSymmetry(),
                SymmetryType.RotationalFourFold => new RotationalFourFoldSymmetry(),
                _ => throw new SudokuException("Unsupported SymmetryType"),
            };
        }
    }
}
