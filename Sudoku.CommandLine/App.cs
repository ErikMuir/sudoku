using System.Collections.Generic;
using MuirDev.ConsoleTools;
using Sudoku.Serialization;

namespace Sudoku.CommandLine
{
    public class App
    {
        private static readonly FluentConsole _console = new FluentConsole();
        private static readonly Dictionary<char, string> _noPuzzleMenuOptions = new Dictionary<char, string>
        {
            { '1', "Input" },
            { '2', "Load" },
            { '0', "Quit" },
        };
        private static readonly Dictionary<char, string> _puzzleMenuOptions = new Dictionary<char, string>
        {
            { '1', "Save" },
            { '2', "Solve" },
            { '3', "Print" },
            { '4', "Clear" },
            { '0', "Quit" },
        };
        private static readonly Menu _noPuzzleMenu = new Menu(_noPuzzleMenuOptions, "No Puzzle Menu");
        private static readonly Menu _puzzleMenu = new Menu(_puzzleMenuOptions, "Puzzle Menu");
        
        public Puzzle Puzzle { get; private set; }

        public void Run()
        {
            while (true)
            {
                try
                {
                    ShowMenu();
                }
                catch (MenuExitException)
                {
                    return;
                }
                catch (SudokuException e)
                {
                    _console.Failure(e.Message);
                }
            }
        }

        private void ShowMenu()
        {
            if (Puzzle == null) ShowNoPuzzleMenu();
            else ShowPuzzleMenu();
        }

        private void ShowNoPuzzleMenu()
        {
            switch (_noPuzzleMenu.Run())
            {
                case '1': Input(); break;
                case '2': Load(); break;
                case '0': throw new MenuExitException();
                default: throw new SudokuException("Invalid option");
            }
        }

        private void ShowPuzzleMenu()
        {
            switch (_puzzleMenu.Run())
            {
                case '1': Save(); break;
                case '2': Solve(); break;
                case '3': Print(); break;
                case '4': Clear(); break;
                case '0': throw new MenuExitException();
                default: throw new SudokuException("Invalid option");
            }
        }

        private void Input()
        {
            Puzzle = InputPuzzle.Run();
            _console
                .Success("Puzzle is now in memory.")
                .Write("Press any key to continue... ")
                .WaitForKeyPress()
                .LineFeed();
        }

        private void Load()
        {
            Puzzle = FilePuzzle.Load();
            _console
                .Write("Press any key to continue... ")
                .WaitForKeyPress()
                .LineFeed();
        }

        private void Save()
        {
            _console.Info(Puzzle.ToSdkString());
            FilePuzzle.Save(Puzzle);
            _console
                .Write("Press any key to continue... ")
                .WaitForKeyPress()
                .LineFeed();
        }

        private void Solve()
        {
            try
            {
                SolvePuzzle.Run(Puzzle);
                _console
                    .Write("Press any key to continue... ")
                    .WaitForKeyPress()
                    .LineFeed();
            }
            catch (MenuExitException) { }
        }

        private void Print()
        {
            try
            {
                PrintPuzzle.Run(Puzzle);
                _console
                    .LineFeed()
                    .Write("Press any key to continue... ")
                    .WaitForKeyPress()
                    .LineFeed();
            }
            catch (MenuExitException) { }
        }

        private void Clear()
        {
            Puzzle = null;
            _console
                .Warning("Puzzle cleared from memory!")
                .Write("Press any key to continue... ")
                .WaitForKeyPress()
                .LineFeed();
        }
    }
}
