using System;
using MuirDev.ConsoleTools;

namespace Sudoku.Console
{
    public static class App
    {
        private static readonly FluentConsole _console = new();

        public static void Run()
        {
            Puzzle puzzle = null;
            while (true)
            {
                try
                {
                    puzzle = puzzle is null
                        ? HomeMenu.Run()
                        : PuzzleMenu.Run(puzzle);
                    _console
                        .Write("Press any key to continue... ")
                        .WaitForKeyPress()
                        .LineFeed();
                }
                catch (MenuExitException)
                {
                    return;
                }
                catch (SudokuException e)
                {
                    _console.Failure(e.Message);
                }
                catch (Exception e)
                {
                    _console.LineFeed().Failure($"Unhandled Error: {e}");
                    return;
                }
            }
        }
    }
}
