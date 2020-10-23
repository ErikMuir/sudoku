using System.Collections;
using System.Collections.Generic;

namespace Sudoku.Tests
{
    public class CellParseTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "0000" };
            yield return new object[] { "1100" };
            yield return new object[] { "2200" };
            yield return new object[] { "3300" };
            yield return new object[] { "4400" };
            yield return new object[] { "5500" };
            yield return new object[] { "6600" };
            yield return new object[] { "7700" };
            yield return new object[] { "8800" };
            yield return new object[] { "0010" };
            yield return new object[] { "0011" };
            yield return new object[] { "00001" };
            yield return new object[] { "00002" };
            yield return new object[] { "00003" };
            yield return new object[] { "00004" };
            yield return new object[] { "00005" };
            yield return new object[] { "00006" };
            yield return new object[] { "00007" };
            yield return new object[] { "00008" };
            yield return new object[] { "00009" };
            yield return new object[] { "000019" };
            yield return new object[] { "0000123456789" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class CellParseThrowsTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "9000" };
            yield return new object[] { "0900" };
            yield return new object[] { "0001" };
            yield return new object[] { "00111" };
            yield return new object[] { "00112" };
            yield return new object[] { "00113" };
            yield return new object[] { "00114" };
            yield return new object[] { "00115" };
            yield return new object[] { "00116" };
            yield return new object[] { "00117" };
            yield return new object[] { "00118" };
            yield return new object[] { "00119" };
            yield return new object[] { "00101" };
            yield return new object[] { "00102" };
            yield return new object[] { "00103" };
            yield return new object[] { "00104" };
            yield return new object[] { "00105" };
            yield return new object[] { "00106" };
            yield return new object[] { "00107" };
            yield return new object[] { "00108" };
            yield return new object[] { "00109" };
            yield return new object[] { "000091" };
            yield return new object[] { "0000x" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class ColRowBoxTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 0, 0, 0 };
            yield return new object[] { 1, 0, 0 };
            yield return new object[] { 2, 0, 0 };
            yield return new object[] { 3, 0, 1 };
            yield return new object[] { 4, 0, 1 };
            yield return new object[] { 5, 0, 1 };
            yield return new object[] { 6, 0, 2 };
            yield return new object[] { 7, 0, 2 };
            yield return new object[] { 8, 0, 2 };

            yield return new object[] { 0, 1, 0 };
            yield return new object[] { 1, 1, 0 };
            yield return new object[] { 2, 1, 0 };
            yield return new object[] { 3, 1, 1 };
            yield return new object[] { 4, 1, 1 };
            yield return new object[] { 5, 1, 1 };
            yield return new object[] { 6, 1, 2 };
            yield return new object[] { 7, 1, 2 };
            yield return new object[] { 8, 1, 2 };

            yield return new object[] { 0, 2, 0 };
            yield return new object[] { 1, 2, 0 };
            yield return new object[] { 2, 2, 0 };
            yield return new object[] { 3, 2, 1 };
            yield return new object[] { 4, 2, 1 };
            yield return new object[] { 5, 2, 1 };
            yield return new object[] { 6, 2, 2 };
            yield return new object[] { 7, 2, 2 };
            yield return new object[] { 8, 2, 2 };

            yield return new object[] { 0, 3, 3 };
            yield return new object[] { 1, 3, 3 };
            yield return new object[] { 2, 3, 3 };
            yield return new object[] { 3, 3, 4 };
            yield return new object[] { 4, 3, 4 };
            yield return new object[] { 5, 3, 4 };
            yield return new object[] { 6, 3, 5 };
            yield return new object[] { 7, 3, 5 };
            yield return new object[] { 8, 3, 5 };

            yield return new object[] { 0, 4, 3 };
            yield return new object[] { 1, 4, 3 };
            yield return new object[] { 2, 4, 3 };
            yield return new object[] { 3, 4, 4 };
            yield return new object[] { 4, 4, 4 };
            yield return new object[] { 5, 4, 4 };
            yield return new object[] { 6, 4, 5 };
            yield return new object[] { 7, 4, 5 };
            yield return new object[] { 8, 4, 5 };

            yield return new object[] { 0, 5, 3 };
            yield return new object[] { 1, 5, 3 };
            yield return new object[] { 2, 5, 3 };
            yield return new object[] { 3, 5, 4 };
            yield return new object[] { 4, 5, 4 };
            yield return new object[] { 5, 5, 4 };
            yield return new object[] { 6, 5, 5 };
            yield return new object[] { 7, 5, 5 };
            yield return new object[] { 8, 5, 5 };

            yield return new object[] { 0, 6, 6 };
            yield return new object[] { 1, 6, 6 };
            yield return new object[] { 2, 6, 6 };
            yield return new object[] { 3, 6, 7 };
            yield return new object[] { 4, 6, 7 };
            yield return new object[] { 5, 6, 7 };
            yield return new object[] { 6, 6, 8 };
            yield return new object[] { 7, 6, 8 };
            yield return new object[] { 8, 6, 8 };

            yield return new object[] { 0, 7, 6 };
            yield return new object[] { 1, 7, 6 };
            yield return new object[] { 2, 7, 6 };
            yield return new object[] { 3, 7, 7 };
            yield return new object[] { 4, 7, 7 };
            yield return new object[] { 5, 7, 7 };
            yield return new object[] { 6, 7, 8 };
            yield return new object[] { 7, 7, 8 };
            yield return new object[] { 8, 7, 8 };

            yield return new object[] { 0, 8, 6 };
            yield return new object[] { 1, 8, 6 };
            yield return new object[] { 2, 8, 6 };
            yield return new object[] { 3, 8, 7 };
            yield return new object[] { 4, 8, 7 };
            yield return new object[] { 5, 8, 7 };
            yield return new object[] { 6, 8, 8 };
            yield return new object[] { 7, 8, 8 };
            yield return new object[] { 8, 8, 8 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class ColRowTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 0, 0 };
            yield return new object[] { 1, 0 };
            yield return new object[] { 2, 0 };
            yield return new object[] { 3, 0 };
            yield return new object[] { 4, 0 };
            yield return new object[] { 5, 0 };
            yield return new object[] { 6, 0 };
            yield return new object[] { 7, 0 };
            yield return new object[] { 8, 0 };

            yield return new object[] { 0, 1 };
            yield return new object[] { 1, 1 };
            yield return new object[] { 2, 1 };
            yield return new object[] { 3, 1 };
            yield return new object[] { 4, 1 };
            yield return new object[] { 5, 1 };
            yield return new object[] { 6, 1 };
            yield return new object[] { 7, 1 };
            yield return new object[] { 8, 1 };

            yield return new object[] { 0, 2 };
            yield return new object[] { 1, 2 };
            yield return new object[] { 2, 2 };
            yield return new object[] { 3, 2 };
            yield return new object[] { 4, 2 };
            yield return new object[] { 5, 2 };
            yield return new object[] { 6, 2 };
            yield return new object[] { 7, 2 };
            yield return new object[] { 8, 2 };

            yield return new object[] { 0, 3 };
            yield return new object[] { 1, 3 };
            yield return new object[] { 2, 3 };
            yield return new object[] { 3, 3 };
            yield return new object[] { 4, 3 };
            yield return new object[] { 5, 3 };
            yield return new object[] { 6, 3 };
            yield return new object[] { 7, 3 };
            yield return new object[] { 8, 3 };

            yield return new object[] { 0, 4 };
            yield return new object[] { 1, 4 };
            yield return new object[] { 2, 4 };
            yield return new object[] { 3, 4 };
            yield return new object[] { 4, 4 };
            yield return new object[] { 5, 4 };
            yield return new object[] { 6, 4 };
            yield return new object[] { 7, 4 };
            yield return new object[] { 8, 4 };

            yield return new object[] { 0, 5 };
            yield return new object[] { 1, 5 };
            yield return new object[] { 2, 5 };
            yield return new object[] { 3, 5 };
            yield return new object[] { 4, 5 };
            yield return new object[] { 5, 5 };
            yield return new object[] { 6, 5 };
            yield return new object[] { 7, 5 };
            yield return new object[] { 8, 5 };

            yield return new object[] { 0, 6 };
            yield return new object[] { 1, 6 };
            yield return new object[] { 2, 6 };
            yield return new object[] { 3, 6 };
            yield return new object[] { 4, 6 };
            yield return new object[] { 5, 6 };
            yield return new object[] { 6, 6 };
            yield return new object[] { 7, 6 };
            yield return new object[] { 8, 6 };

            yield return new object[] { 0, 7 };
            yield return new object[] { 1, 7 };
            yield return new object[] { 2, 7 };
            yield return new object[] { 3, 7 };
            yield return new object[] { 4, 7 };
            yield return new object[] { 5, 7 };
            yield return new object[] { 6, 7 };
            yield return new object[] { 7, 7 };
            yield return new object[] { 8, 7 };

            yield return new object[] { 0, 8 };
            yield return new object[] { 1, 8 };
            yield return new object[] { 2, 8 };
            yield return new object[] { 3, 8 };
            yield return new object[] { 4, 8 };
            yield return new object[] { 5, 8 };
            yield return new object[] { 6, 8 };
            yield return new object[] { 7, 8 };
            yield return new object[] { 8, 8 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class OneToNineTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 1 };
            yield return new object[] { 2 };
            yield return new object[] { 3 };
            yield return new object[] { 4 };
            yield return new object[] { 5 };
            yield return new object[] { 6 };
            yield return new object[] { 7 };
            yield return new object[] { 8 };
            yield return new object[] { 9 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class PuzzleParseThrowsTestData : IEnumerable<object[]>
    {
        private readonly string _notEnoughCells = "0000,1000,2000,3000,4000,5000,6000,7000,8000,0100,1100,2100,3100,4100,5100,6100,7100,8100,0200,1200,2200,3200,4200,5200,6200,7200,8200,0300,1300,2300,3300,4300,5300,6300,7300,8300,0400,1400,2400,3400,4400,5400,6400,7400,8400,0500,1500,2500,3500,4500,5500,6500,7500,8500,0600,1600,2600,3600,4600,5600,6600,7600,8600,0700,1700,2700,3700,4700,5700,6700,7700,8700,0800,1800,2800,3800,4800,5800,6800,7800";
        private readonly string _tooManyCells = "0000,1000,2000,3000,4000,5000,6000,7000,8000,0100,1100,2100,3100,4100,5100,6100,7100,8100,0200,1200,2200,3200,4200,5200,6200,7200,8200,0300,1300,2300,3300,4300,5300,6300,7300,8300,0400,1400,2400,3400,4400,5400,6400,7400,8400,0500,1500,2500,3500,4500,5500,6500,7500,8500,0600,1600,2600,3600,4600,5600,6600,7600,8600,0700,1700,2700,3700,4700,5700,6700,7700,8700,0800,1800,2800,3800,4800,5800,6800,7800,8800,8800";
        private readonly string _invalidCoordinates = "0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000";

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { null as string, "Cannot parse puzzle: value was null" };
            yield return new object[] { _notEnoughCells, "Cannot parse puzzle: invalid cell count" };
            yield return new object[] { _tooManyCells, "Cannot parse puzzle: invalid cell count" };
            yield return new object[] { _invalidCoordinates, "Cannot parse puzzle: invalid cell coordinates" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class ZeroToEightTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 0 };
            yield return new object[] { 1 };
            yield return new object[] { 2 };
            yield return new object[] { 3 };
            yield return new object[] { 4 };
            yield return new object[] { 5 };
            yield return new object[] { 6 };
            yield return new object[] { 7 };
            yield return new object[] { 8 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
