using System.Collections.Generic;
using System.Text;
using Sudoku.Serializers;

namespace Sudoku.Tests
{
    public static class TestHelpers
    {
        public static Puzzle GetEmptyPuzzle()
        {
            string row0 = "00,00,00,00,00,00,00,00,00";
            string row1 = "00,00,00,00,00,00,00,00,00";
            string row2 = "00,00,00,00,00,00,00,00,00";
            string row3 = "00,00,00,00,00,00,00,00,00";
            string row4 = "00,00,00,00,00,00,00,00,00";
            string row5 = "00,00,00,00,00,00,00,00,00";
            string row6 = "00,00,00,00,00,00,00,00,00";
            string row7 = "00,00,00,00,00,00,00,00,00";
            string row8 = "00,00,00,00,00,00,00,00,00";
            string puzzleString = $"{row0},{row1},{row2},{row3},{row4},{row5},{row6},{row7},{row8}";
            return PzlSerializer.Deserialize(puzzleString);
        }

        public static Puzzle GetSolvedPuzzle()
        {
            string row0 = "40,30,50,20,60,90,70,80,10";
            string row1 = "60,80,20,50,70,10,40,90,30";
            string row2 = "10,90,70,80,30,40,50,60,20";
            string row3 = "80,20,60,10,90,50,30,40,70";
            string row4 = "30,70,40,60,80,20,90,10,50";
            string row5 = "90,50,10,70,40,30,60,20,80";
            string row6 = "50,10,90,30,20,60,80,70,40";
            string row7 = "20,40,80,90,50,70,10,30,60";
            string row8 = "70,60,30,40,10,80,20,50,90";
            string puzzleString = $"{row0},{row1},{row2},{row3},{row4},{row5},{row6},{row7},{row8}";
            return PzlSerializer.Deserialize(puzzleString);
        }

        public static Puzzle GetEasyPuzzle()
        {
            string row0 = "00,00,00,00,21,31,00,51,00";
            string row1 = "61,00,31,00,00,00,41,21,00";
            string row2 = "00,00,00,61,00,00,71,00,00";
            string row3 = "51,91,00,71,00,41,21,81,00";
            string row4 = "00,00,00,00,61,00,00,00,00";
            string row5 = "00,41,21,31,00,91,00,71,51";
            string row6 = "00,00,71,00,00,11,00,00,00";
            string row7 = "00,51,41,00,00,00,31,00,81";
            string row8 = "00,31,00,51,41,00,00,00,00";
            string puzzleString = $"{row0},{row1},{row2},{row3},{row4},{row5},{row6},{row7},{row8}";
            return PzlSerializer.Deserialize(puzzleString);
        }

        public static Puzzle GetMediumPuzzle()
        {
            string row0 = "00,51,00,00,91,00,11,00,00";
            string row1 = "41,00,11,00,00,81,91,00,51";
            string row2 = "00,00,91,51,31,00,00,71,00";
            string row3 = "00,00,00,00,00,31,00,00,21";
            string row4 = "00,61,00,00,81,00,00,11,00";
            string row5 = "91,00,00,11,00,00,00,00,00";
            string row6 = "00,41,00,00,51,61,21,00,00";
            string row7 = "11,00,81,71,00,00,51,00,61";
            string row8 = "00,00,61,00,11,00,00,41,00";
            string puzzleString = $"{row0},{row1},{row2},{row3},{row4},{row5},{row6},{row7},{row8}";
            return PzlSerializer.Deserialize(puzzleString);
        }

        public static Puzzle GetDifficultPuzzle()
        {
            string row0 = "00,91,00,00,00,71,00,51,00";
            string row1 = "00,51,00,81,00,00,00,00,00";
            string row2 = "11,41,00,00,51,00,21,00,00";
            string row3 = "71,00,41,00,91,00,00,00,00";
            string row4 = "31,81,11,71,00,00,51,91,61";
            string row5 = "51,00,91,00,81,00,71,00,41";
            string row6 = "00,71,51,00,31,81,00,41,11";
            string row7 = "00,11,00,00,00,61,00,71,00";
            string row8 = "00,31,00,11,71,00,00,81,00";
            string puzzleString = $"{row0},{row1},{row2},{row3},{row4},{row5},{row6},{row7},{row8}";
            return PzlSerializer.Deserialize(puzzleString);
        }

        public static Puzzle GetXWingPuzzle()
        {
            string row0 = "41,51,81,00,00,00,71,91,31";
            string row1 = "61,91,31,51,00,00,21,11,41";
            string row2 = "71,00,00,41,91,31,61,81,51";
            string row3 = "00,00,51,91,00,00,00,00,61";
            string row4 = "00,41,00,00,31,51,00,71,00";
            string row5 = "31,00,00,00,00,21,41,51,00";
            string row6 = "00,61,00,11,00,00,00,00,71";
            string row7 = "00,00,41,00,00,91,00,61,00";
            string row8 = "11,00,00,00,51,00,81,41,00";
            string puzzleString = $"{row0},{row1},{row2},{row3},{row4},{row5},{row6},{row7},{row8}";
            return PzlSerializer.Deserialize(puzzleString);
        }
    }
}
