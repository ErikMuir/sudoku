using System;
using System.Diagnostics;

namespace Sudoku.Solution
{
    public interface ISolver
    {
        Puzzle Puzzle { get; }
        Stopwatch Timer { get; }
        TimeSpan SolveDuration { get; }
        void Solve();
        string Statistics();
    }
}
