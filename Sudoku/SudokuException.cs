using System;

namespace Sudoku
{
    public class SudokuException : Exception
    {
        public SudokuException() : base() { }
        public SudokuException(string message) : base(message) { }
        public SudokuException(string message, Exception innerException) : base(message, innerException) { }
    }
}
