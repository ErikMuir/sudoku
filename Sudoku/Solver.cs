using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku
{
    public static class Solver
    {
        public static int FindWorkingCell(Puzzle puzzle)
        {
            int minCandidates = puzzle.Cells.Where(cands => cands.Length >= 2).Min(cands => cands.Length);
            return Array.FindIndex(puzzle.Cells, c => c.Length == minCandidates);
        }

        public static Puzzle Solve(Puzzle input, Func<Puzzle, bool> solutionFunc = null)
        {
            if (input.Cells.All(cell => cell.Length == 1))
                return (solutionFunc != null && solutionFunc(input)) ? null : input;

            int activeCell = FindWorkingCell(input);
            foreach (int guess in input.Cells[activeCell])
            {
                Puzzle puzzle;
                if ((puzzle = Puzzle.PlaceValue(input, activeCell, guess)) is not null)
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
    }
}
