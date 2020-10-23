using System;
using System.Diagnostics;
using System.Text;

namespace Sudoku.Solvers
{
    public class BacktrackingSolver : ISolver
    {
        public BacktrackingSolver(Puzzle puzzle)
        {
            Puzzle = puzzle.Clone();
            Timer = new Stopwatch();
        }

        public Puzzle Puzzle { get; }
        public Stopwatch Timer { get; }
        public TimeSpan SolveDuration => Timer.Elapsed;
        public int SolveDepth { get; set; } = 0;

        public void Solve()
        {
            if (Puzzle.IsSolved()) return;
            Timer.Start();
            _doSolve();
            Timer.Stop();
        }

        public string Statistics()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Is Solved: {Puzzle.IsSolved()}");
            sb.AppendLine($"Solve Duration (ms): {SolveDuration.Milliseconds}");
            sb.AppendLine($"Solve Depth: {SolveDepth}");
            return sb.ToString();
        }

        private bool _doSolve()
        {
            SolveDepth++;

            var nextEmptyCell = Puzzle.GetNextEmptyCell();
            if (nextEmptyCell == null) return Puzzle.IsSolved();

            Puzzle.CalculateCandidates();
            foreach (var candidate in nextEmptyCell.Candidates)
            {
                nextEmptyCell.Value = candidate;
                if (_doSolve()) return true;
            }

            if (Puzzle.GetNextEmptyCell() != null) nextEmptyCell.Value = null;

            return false;
        }
    }
}