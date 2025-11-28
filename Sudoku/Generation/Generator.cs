using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Sudoku.Analysis;
using Sudoku.Extensions;
using Sudoku.Logic;

namespace Sudoku.Generation;

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

        public static Puzzle Generate(GenerationOptions options)
        {
            Puzzle puzzle = null;
            int puzzleIterations = 0;

            Stopwatch stopwatch = new();
            stopwatch.Start();

            while (puzzle is null)
            {
                if (++puzzleIterations % 25 == 0)
                    Console.WriteLine($"generation attempts: {puzzleIterations}");
                puzzle = _puzzleIteration(options);
            }

            stopwatch.Stop();
            Console.WriteLine($"Running time: {stopwatch.Elapsed:c}");

            return puzzle;
        }

        private static Puzzle _puzzleIteration(GenerationOptions options)
        {
            int maxClues = options?.MaxClues ?? 0;
            ISymmetry symmetry = options?.Symmetry ?? _supportedSymmetries[_rand.Next(_supportedSymmetries.Length)];
            Metadata metadata = new()
            {
                Source = "MuirDev.Sudoku",
                Symmetry = symmetry.Type,
            };
            Puzzle puzzle = new(metadata);
            puzzle.FillCandidates();
            puzzle.ReduceCandidates();
            while (true)
            {
                Puzzle workingPuzzle = _applySymmetry(puzzle, symmetry);
                if (workingPuzzle is null) return null;
                if (_isMaxClues(workingPuzzle, maxClues)) return null;
                List<Puzzle> solutions = Solver.MultiSolve(workingPuzzle, 2);
                if (!solutions.Any()) return null;
                if (solutions.Count() == 1) return _validatePuzzle(workingPuzzle, options);
                puzzle = workingPuzzle;
            }
        }

        private static Puzzle _applySymmetry(Puzzle puzzle, ISymmetry symmetry)
        {
            Puzzle workingPuzzle = new(puzzle);
            Cell randomEmptyCell = _getRandomEmptyCell(workingPuzzle);
            int[] reflections = symmetry.GetReflections(randomEmptyCell.Index);
            for (int i = 0; i < reflections.Length && workingPuzzle is not null; i++)
            {
                Cell cell = workingPuzzle.Cells[reflections[i]];
                int value = cell.Candidates[_rand.Next(cell.Candidates.Count)];
                workingPuzzle = _placeValue(workingPuzzle, cell.Index, value);
            }
            return workingPuzzle;
        }

        private static Cell _getRandomEmptyCell(Puzzle puzzle)
        {
            Cell[] emptyCells = puzzle.Cells.EmptyCells().ToArray();
            return emptyCells[_rand.Next(emptyCells.Length)];
        }

        private static Puzzle _placeValue(Puzzle input, int cellIndex, int value)
        {
            Puzzle puzzle = new(input);
            Cell cell = puzzle.Cells[cellIndex];
            if (!cell.Candidates.Contains(value)) return null;
            cell.Value = value;
            Cell[] emptyPeers = puzzle.Peers(cell)
                .Where(cell => cell.Type == CellType.Empty)
                .ToArray();
            foreach (Cell peer in emptyPeers)
            {
                int newCandidateCount = peer.Candidates.Except(new int[] { value }).Count();
                if (newCandidateCount == 0) return null;
                // TODO : is the next statement necessary? what does it do?
                if (newCandidateCount == 1 && peer.Candidates.Count > 1) return null;
                peer.RemoveCandidate(value);
            }
            return puzzle;
        }

        private static bool _isMaxClues(Puzzle puzzle, int maxClues)
            => maxClues > 0 && puzzle.Cells.FilledCells().Count() > maxClues;

        private static Puzzle _validatePuzzle(Puzzle input, GenerationOptions options)
        {
            // clone puzzle
            Puzzle puzzle = new(input);

            // convert filled cells into clues
            for (int i = 0; i < puzzle.Cells.Length; i++)
            {
                Cell cell = puzzle.Cells[i];
                if (cell.Value is null) continue;
                puzzle.Cells[i] = new Clue(cell.Row, cell.Col, (int)cell.Value);
            }

            // validate level
            Analyzer analyzer = new(puzzle);
            if (options.Level > Level.Uninitialized && options.Level != analyzer.Level)
                return null;

            // update metadata
            puzzle.Metadata.Level = analyzer.Level;
            puzzle.Metadata.DatePublished = DateTime.Now;

            return puzzle;
    }
}
