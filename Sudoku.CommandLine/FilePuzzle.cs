using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MuirDev.ConsoleTools;
using Sudoku.Serializers;

namespace Sudoku.CommandLine
{
    public static class FilePuzzle
    {
        private const string PuzzleDirectory = "./puzzles";
        private static readonly List<ISerializer> _serializers = new()
        {
            new PzlSerializer(),
            new SdkSerializer(),
            new SdmSerializer(),
            new SdxSerializer(),
        };
        private static readonly Dictionary<char, string> _menuOptions = new()
        {
            { '1', _serializers[0].FileExtension },
            { '2', _serializers[1].FileExtension },
            { '3', _serializers[2].FileExtension },
            { '4', _serializers[3].FileExtension },
            { '0', "Go back" },
        };
        private static readonly Menu _menu = new(_menuOptions, "Choose a file format:");
        private static readonly FluentConsole _console = new();

        public static void Save(Puzzle puzzle)
        {
            _console.LineFeed();
            char choice = _menu.Run();
            _console.LineFeed();

            if (choice == '0') throw new MenuExitException();

            try
            {
                string fileExtension = _menuOptions.GetValueOrDefault(choice);
                string path = $"{PuzzleDirectory}/{DateTime.Now:yyyyMMdd_HHmmss}.{fileExtension}";
                ISerializer serializer = _serializers.Single(x => x.FileExtension == fileExtension);
                string contents = serializer.Serialize(puzzle);
                File.WriteAllText(path, contents);
                _console.Success($"File successfully saved: {path}");
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
                string fileExtension;
                ISerializer serializer;
                string validationMessage;

                do
                {
                    fullPath = _console.Info("Enter the file with path: ").ReadLine();
                    fileExtension = fullPath.Split('.').Last().ToLower();
                    serializer = _serializers.FirstOrDefault(x => x.FileExtension == fileExtension);
                    validationMessage =
                        !File.Exists(fullPath) ? "File cannot be found!" :
                        serializer is null ? "File type not supported!" :
                        null;
                    if (validationMessage is not null)
                    {
                        Confirm confirm = new($"{validationMessage} Try again?", true);
                        bool tryAgain = confirm.Run(LogType.Warning);
                        if (!tryAgain) return null;
                    }
                } while (validationMessage is not null);

                string puzzleString = File.ReadAllText(fullPath);
                puzzle = serializer.Deserialize(puzzleString);
                PrintPuzzle.Run(puzzle);
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
