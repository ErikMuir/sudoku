using System;
using System.Collections.Generic;
using MuirDev.ConsoleTools;

namespace Sudoku.Console
{
    public class App
    {
        private static readonly FluentConsole _console = new();
        private static readonly Dictionary<char, string> _homeMenuOptions = new()
        {
            { '1', "Input" },
            { '2', "Load" },
            { '3', "Generate" },
            // { '4', "** debug **" },
            { '0', "Quit" },
        };
        private static readonly Dictionary<char, string> _puzzleMenuOptions = new()
        {
            { '1', "Save" },
            { '2', "Analyze" },
            { '3', "Solve" },
            { '4', "Print" },
            { '5', "Clear" },
            { '0', "Quit" },
        };
        private static readonly Menu _homeMenu = new(_homeMenuOptions, "Home Menu");
        private static readonly Menu _puzzleMenu = new(_puzzleMenuOptions, "Puzzle Menu");

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
                catch (Exception e)
                {
                    _console.LineFeed().Failure($"Unhandled Error: {e}");
                    return;
                }
            }
        }

        private void ShowMenu()
        {
            if (Puzzle is null) ShowHomeMenu();
            else ShowPuzzleMenu();
            _console
                .Write("Press any key to continue... ")
                .WaitForKeyPress()
                .LineFeed();
        }

        private void ShowHomeMenu()
        {
            switch (_homeMenu.Run())
            {
                case '1': Input(); break;
                case '2': Load(); break;
                case '3': Generate(); break;
                case '4': Debug(); break;
                case '0': throw new MenuExitException();
                default: throw new SudokuException("Invalid option");
            }
        }

        private void ShowPuzzleMenu()
        {
            switch (_puzzleMenu.Run())
            {
                case '1': Save(); break;
                case '2': Analyze(); break;
                case '3': Solve(); break;
                case '4': Print(); break;
                case '5': Clear(); break;
                case '0': throw new MenuExitException();
                default: throw new SudokuException("Invalid option");
            }
        }

        private void Input() => Puzzle = InputPuzzle.Run();

        private void Load() => Puzzle = FilePuzzle.Load();

        private void Generate() => Puzzle = GeneratePuzzle.Run();

        private void Save() => FilePuzzle.Save(Puzzle);

        private void Analyze() => AnalyzePuzzle.Run(Puzzle);

        private void Solve() => Puzzle = SolvePuzzle.Run(Puzzle);

        private void Print() => PrintPuzzle.Run(Puzzle);

        private void Clear() => Puzzle = null;

        private void Debug()
        {
            _console.Warning("Not implemented!");
        }
    }
}
