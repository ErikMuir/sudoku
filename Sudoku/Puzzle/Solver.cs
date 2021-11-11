using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku
{
    public static class Solver
    {
        public static bool BacktrackingSolve(Puzzle puzzle)
        {
            Cell nextEmptyCell = puzzle.GetNextEmptyCell();
            if (nextEmptyCell is null) return puzzle.IsSolved();

            puzzle.CalculateCandidates();
            foreach (int candidate in nextEmptyCell.Candidates)
            {
                nextEmptyCell.Value = candidate;
                if (BacktrackingSolve(puzzle)) return true;
            }

            if (puzzle.GetNextEmptyCell() is not null) nextEmptyCell.Value = null;

            return false;
        }

        public static Puzzle BacktrackingSolve2(Puzzle input)
        {
            if (input.IsSolved()) return input;

            Puzzle puzzle = new(input);
            puzzle.CalculateCandidates();

            Cell nextEmptyCell = puzzle.GetNextEmptyCell();
            if (nextEmptyCell is null) return null;

            foreach (int guess in nextEmptyCell.Candidates)
            {
                nextEmptyCell.Value = guess;
                if ((puzzle = BacktrackingSolve2(puzzle)) is not null) return puzzle;
            }

            return null;
        }

        public static Puzzle Solve(Puzzle input, Func<Puzzle, bool> solutionFunc = null)
        {
            if (input.IsSolved())
                return (solutionFunc is not null && solutionFunc(input)) ? null : input;

            input.CalculateCandidates();
            Cell activeCell = FindWorkingCell(input);
            foreach (int guess in activeCell.Candidates)
            {
                Puzzle puzzle;
                if ((puzzle = PlaceValue(input, activeCell.Index, guess)) is not null)
                    if ((puzzle = Solve(puzzle, solutionFunc)) is not null)
                        return puzzle;
            }
            return null;
        }

        public static List<Puzzle> MultiSolve(Puzzle input, int maxSolutions = -1)
        {
            List<Puzzle> solutions = new();
            Solve(input, p =>
            {
                solutions.Add(p);
                return solutions.Count() < maxSolutions || maxSolutions == -1;
            });
            return solutions;
        }

        private static Puzzle PlaceValue(Puzzle input, int cellIndex, int value)
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
                if ((puzzle = PlaceValue(puzzle, pair.Key, pair.Value)) is null)
                    return null;
            }
            return puzzle;
        }

        private static Cell FindWorkingCell(Puzzle puzzle)
        {
            return puzzle
                .GetEmptyCells()
                .OrderBy(cell => cell.Candidates.Count)
                .FirstOrDefault();
        }
    }
}
