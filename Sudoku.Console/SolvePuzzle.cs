using System.Collections.Generic;
using MuirDev.ConsoleTools;
using Sudoku.Solution;

namespace Sudoku.Console
{
    public static class SolvePuzzle
    {
        private static readonly FluentConsole _console = new();
        private static readonly Dictionary<char, string> _menuOptions = new()
        {
            { '1', "Backtracking Solver" },
            { '2', "Constraint Solver" },
            { '0', "Go back" },
        };
        private static readonly Menu _menu = new(_menuOptions, "Choose a solver:");

        public static void Run(Puzzle puzzle)
        {
            _console.LineFeed();
            ISolver solver;
            switch (_menu.Run())
            {
                case '1': solver = new BacktrackingSolver(puzzle); break;
                case '2': solver = new ConstraintSolver(puzzle); break;
                case '0': throw new MenuExitException();
                default: throw new SudokuException("Invalid option");
            }
            solver.Solve();
            _console.LineFeed().Write(solver.Statistics());
            if (solver.Puzzle.IsSolved())
                _console.Success("Puzzle was successfully solved!");
            else
                _console.Failure("Failed to solve puzzle!");
        }
    }
}
