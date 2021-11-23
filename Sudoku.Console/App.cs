using System;
using MuirDev.ConsoleTools;
using Sudoku.Exceptions;
using Sudoku.Logic;

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
                        ? MainMenu.Run()
                        : PuzzleMenu.Run(puzzle);
                    _console
                        .Write("Press any key to continue... ")
                        .WaitForKeyPress()
                        .LineFeed();
                }
                catch (MenuExitException)
                {
                    // gulp!
                }
                catch (ProgramExitException)
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
