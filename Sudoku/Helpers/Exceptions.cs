using System;

namespace Sudoku
{
    public class SudokuException : Exception
    {
        public SudokuException() : base() { }
        public SudokuException(string message) : base(message) { }
        public SudokuException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class MenuExitException : SudokuException
    {
        public MenuExitException() : base() { }
        public MenuExitException(string message) : base(message) { }
        public MenuExitException(string message, Exception innerException) : base(message, innerException) { }
    }
}