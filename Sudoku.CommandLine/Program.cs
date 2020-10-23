using System;
using MuirDev.ConsoleTools;

namespace Sudoku.CommandLine
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                new App().Run();            
            }
            catch (Exception e)
            {
                new FluentConsole().LineFeed().Failure($"Unhandled Error: {e}");
            }
        }
    }
}
