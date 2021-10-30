using System;

namespace Sudoku.CommandLine
{
    public class MenuExitException : SudokuException
    {
        public MenuExitException() : base() { }
        public MenuExitException(string message) : base(message) { }
        public MenuExitException(string message, Exception innerException) : base(message, innerException) { }
    }
}