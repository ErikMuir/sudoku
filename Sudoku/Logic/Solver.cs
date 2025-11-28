using System;
using System.Collections.Generic;
using System.Linq;
using Sudoku.Extensions;

namespace Sudoku.Logic;

    public static class Solver
    {
        private static int _iterationCount = 0;

        public static Puzzle Solve(Puzzle input)
        {
            _iterationCount = 0;
            input.FillCandidates();
            input.ReduceCandidates();
            return _doMultiSolve(input, null);
        }

        public static List<Puzzle> MultiSolve(Puzzle input, int maxSolutions = -1)
        {
            _iterationCount = 0;
            input.FillCandidates();
            input.ReduceCandidates();
            List<Puzzle> solutions = new();
            _doMultiSolve(input, p =>
            {
                _iterationCount = 0;
                solutions.Add(p);
                return solutions.Count() < maxSolutions || maxSolutions == -1;
            });
            return solutions;
        }

        private static Puzzle _doSolve(Puzzle input)
        {
            if (input.IsSolved) return input;
            if (++_iterationCount >= 1000) return null;
            Cell activeCell = _findWorkingCell(input);
            if (activeCell is null) return null;
            foreach (int guess in activeCell.Candidates)
            {
                Puzzle puzzle;
                if ((puzzle = _placeValue(input, activeCell.Index, guess)) is not null)
                    if ((puzzle = _doSolve(puzzle)) is not null)
                        return puzzle;
            }
            return null;
        }

        private static Puzzle _doMultiSolve(Puzzle input, Func<Puzzle, bool> solutionFunc = null)
        {
            if (input.IsSolved)
                return (solutionFunc is not null && solutionFunc(input)) ? null : input;
            if (++_iterationCount >= 1000) return null;
            Cell activeCell = _findWorkingCell(input);
            if (activeCell is null) return null;
            foreach (int guess in activeCell.Candidates)
            {
                Puzzle puzzle;
                if ((puzzle = _placeValue(input, activeCell.Index, guess)) is not null)
                    if ((puzzle = _doMultiSolve(puzzle, solutionFunc)) is not null)
                        return puzzle;
            }
            return null;
        }

        private static Puzzle _placeValue(Puzzle input, int cellIndex, int value)
        {
            Puzzle puzzle = new(input);
            Cell cell = puzzle.Cells[cellIndex];
            if (!cell.Candidates.Contains(value))
                return null;
            cell.Value = value;
            Dictionary<int, int> cellsToPlace = new();
            foreach (Cell peer in puzzle.Peers(cell))
            {
                if (peer.Value is not null)
                    continue;
                SortedSet<int> newCandidates = new(peer.Candidates.Except(new int[] { value }));
                if (!newCandidates.Any())
                    return null;
                if (newCandidates.Count == 1 && peer.Candidates.Count > 1)
                    cellsToPlace.Add(peer.Index, newCandidates.Single());
                peer.RemoveCandidate(value);
            }
            foreach (KeyValuePair<int, int> pair in cellsToPlace)
            {
                if ((puzzle = _placeValue(puzzle, pair.Key, pair.Value)) is null)
                    return null;
            }
            return puzzle;
        }

        private static Cell _findWorkingCell(Puzzle puzzle)
        {
            return puzzle.Cells
                .EmptyCells()
                .OrderBy(cell => cell.Candidates.Count)
                .FirstOrDefault();
    }
}
