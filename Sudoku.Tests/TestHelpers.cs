using System.Text;
using Sudoku.Logic;
using Sudoku.Serialization;

namespace Sudoku.Tests
{
    public static class TestHelpers
    {
        private static readonly SdkSerializer _sdkSerializer = new();

        public static Puzzle GetEmptyPuzzle()
        {
            StringBuilder sb = new();
            for (int i = 0; i < 9; i++) sb.AppendLine(".........");
            string puzzleString = sb.ToString();
            return _sdkSerializer.Deserialize(puzzleString);
        }

        public static Puzzle GetSolvedPuzzle()
        {
            StringBuilder sb = new();
            sb.AppendLine("435269781");
            sb.AppendLine("682571493");
            sb.AppendLine("197834562");
            sb.AppendLine("826195347");
            sb.AppendLine("374682915");
            sb.AppendLine("951743628");
            sb.AppendLine("519326874");
            sb.AppendLine("248957136");
            sb.AppendLine("763418259");
            string puzzleString = sb.ToString();
            return _sdkSerializer.Deserialize(puzzleString);
        }

        public static Puzzle GetEasyPuzzle()
        {
            StringBuilder sb = new();
            sb.AppendLine("....23.5.");
            sb.AppendLine("6.3...42.");
            sb.AppendLine("...6..7..");
            sb.AppendLine("59.7.428.");
            sb.AppendLine("....6....");
            sb.AppendLine(".423.9.75");
            sb.AppendLine("..7..1...");
            sb.AppendLine(".54...3.8");
            sb.AppendLine(".3.54....");
            string puzzleString = sb.ToString();
            return _sdkSerializer.Deserialize(puzzleString);
        }

        public static Puzzle GetMediumPuzzle()
        {
            StringBuilder sb = new();
            sb.AppendLine(".5..9.1..");
            sb.AppendLine("4.1..89.5");
            sb.AppendLine("..953..7.");
            sb.AppendLine(".....3..2");
            sb.AppendLine(".6..8..1.");
            sb.AppendLine("9..1.....");
            sb.AppendLine(".4..562..");
            sb.AppendLine("1.87..5.6");
            sb.AppendLine("..6.1..4.");
            string puzzleString = sb.ToString();
            return _sdkSerializer.Deserialize(puzzleString);
        }

        public static Puzzle GetDifficultPuzzle()
        {
            StringBuilder sb = new();
            sb.AppendLine(".9...7.5.");
            sb.AppendLine(".5.8.....");
            sb.AppendLine("14..5.2..");
            sb.AppendLine("7.4.9....");
            sb.AppendLine("3817..596");
            sb.AppendLine("5.9.8.7.4");
            sb.AppendLine(".75.38.41");
            sb.AppendLine(".1...6.7.");
            sb.AppendLine(".3.17..8.");
            string puzzleString = sb.ToString();
            return _sdkSerializer.Deserialize(puzzleString);
        }

        public static Puzzle GetXWingPuzzle()
        {
            StringBuilder sb = new();
            sb.AppendLine("458...793");
            sb.AppendLine("6935..214");
            sb.AppendLine("7..493685");
            sb.AppendLine("..59....6");
            sb.AppendLine(".4..35.7.");
            sb.AppendLine("3....245.");
            sb.AppendLine(".6.1....7");
            sb.AppendLine("..4..9.6.");
            sb.AppendLine("1...5.84.");
            string puzzleString = sb.ToString();
            return _sdkSerializer.Deserialize(puzzleString);
        }
    }
}
