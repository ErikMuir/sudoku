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
            StringBuilder sb = new();
            sb.AppendLine($"Is Solved: {Puzzle.IsSolved()}");
            sb.AppendLine($"Solve Duration (ms): {SolveDuration.Milliseconds}");
            sb.AppendLine($"Solve Depth: {SolveDepth}");
            return sb.ToString();
        }

        private bool _doSolve()
        {
            SolveDepth++;

            Cell nextEmptyCell = Puzzle.GetNextEmptyCell();
            if (nextEmptyCell is null) return Puzzle.IsSolved();

            Puzzle.CalculateCandidates();
            foreach (int candidate in nextEmptyCell.Candidates)
            {
                nextEmptyCell.Value = candidate;
                if (_doSolve()) return true;
            }

            if (Puzzle.GetNextEmptyCell() is not null) nextEmptyCell.Value = null;

            return false;
        }
    }
}
