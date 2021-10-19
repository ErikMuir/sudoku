using System;
using System.IO;
using MuirDev.ConsoleTools;

namespace Sudoku.CommandLine
{
    public static class FilePuzzle
    {
        private const string PuzzleDirectory = "./puzzles";
        private const string PuzzleExtension = "sdk";
        private static readonly FluentConsole _console = new();

        public static void Save(Puzzle puzzle)
        {
            try
            {
                string fileName;
                string fullPath;
                bool isFileNameValid;
                Directory.CreateDirectory(PuzzleDirectory);
                do
                {
                    fileName = _console.Write("Save as: ").ReadLine();
                    fullPath = $"{PuzzleDirectory}/{fileName}.{PuzzleExtension}";
                    isFileNameValid = File.Exists(fullPath)
                        ? new Confirm("File exists! Overwrite?", true).Run(LogType.Warning)
                        : true;
                } while (!isFileNameValid);
                File.WriteAllText(fullPath, puzzle.ToString());
                _console.Success($"File successfully saved: {fullPath}");
            }
            catch (Exception e)
            {
                _console.Failure($"Failed to save the puzzle! {e.Message}");
            }
        }

        public static Puzzle Load()
        {
            Puzzle puzzle = null;
            try
            {
                string fullPath;
                bool fileExists;
                do
                {
                    fullPath = _console.Info("Enter the file with path: ").ReadLine();
                    fileExists = File.Exists(fullPath);
                    if (!fileExists)
                    {
                        Confirm confirm = new("File cannot be found! Try again?", true);
                        bool tryAgain = confirm.Run(LogType.Warning);
                        if (!tryAgain) return null;
                    }
                } while (!fileExists);
                string puzzleString = File.ReadAllText(fullPath);
                puzzle = Puzzle.Parse(puzzleString);
                _console.Success("Successfully loaded puzzle from file!");
            }
            catch (SudokuException e)
            {
                _console.Failure(e.Message);
            }
            catch (Exception e)
            {
                _console.Failure($"Failed to load the puzzle! {e.Message}");
            }

            return puzzle;
        }
    }
}
