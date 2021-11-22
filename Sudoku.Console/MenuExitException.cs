using System;
using Sudoku.Exceptions;

namespace Sudoku.Console
{
    public class MenuExitException : SudokuException
    {
        public MenuExitException() : base() { }
        public MenuExitException(string message) : base(message) { }
        public MenuExitException(string message, Exception innerException) : base(message, innerException) { }
    }
}
