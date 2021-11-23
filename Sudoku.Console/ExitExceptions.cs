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

    public class ProgramExitException : SudokuException
    {
        public ProgramExitException() : base() { }
        public ProgramExitException(string message) : base(message) { }
        public ProgramExitException(string message, Exception innerException) : base(message, innerException) { }
    }
}
