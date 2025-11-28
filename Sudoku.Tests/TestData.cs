using System.Collections;
using System.Collections.Generic;

namespace Sudoku.Tests;

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

    public class CellTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 0, 0, 0, 0 };
            yield return new object[] { 0, 1, 0, 1 };
            yield return new object[] { 0, 2, 0, 2 };
            yield return new object[] { 0, 3, 1, 3 };
            yield return new object[] { 0, 4, 1, 4 };
            yield return new object[] { 0, 5, 1, 5 };
            yield return new object[] { 0, 6, 2, 6 };
            yield return new object[] { 0, 7, 2, 7 };
            yield return new object[] { 0, 8, 2, 8 };

            yield return new object[] { 1, 0, 0, 9 };
            yield return new object[] { 1, 1, 0, 10 };
            yield return new object[] { 1, 2, 0, 11 };
            yield return new object[] { 1, 3, 1, 12 };
            yield return new object[] { 1, 4, 1, 13 };
            yield return new object[] { 1, 5, 1, 14 };
            yield return new object[] { 1, 6, 2, 15 };
            yield return new object[] { 1, 7, 2, 16 };
            yield return new object[] { 1, 8, 2, 17 };

            yield return new object[] { 2, 0, 0, 18 };
            yield return new object[] { 2, 1, 0, 19 };
            yield return new object[] { 2, 2, 0, 20 };
            yield return new object[] { 2, 3, 1, 21 };
            yield return new object[] { 2, 4, 1, 22 };
            yield return new object[] { 2, 5, 1, 23 };
            yield return new object[] { 2, 6, 2, 24 };
            yield return new object[] { 2, 7, 2, 25 };
            yield return new object[] { 2, 8, 2, 26 };

            yield return new object[] { 3, 0, 3, 27 };
            yield return new object[] { 3, 1, 3, 28 };
            yield return new object[] { 3, 2, 3, 29 };
            yield return new object[] { 3, 3, 4, 30 };
            yield return new object[] { 3, 4, 4, 31 };
            yield return new object[] { 3, 5, 4, 32 };
            yield return new object[] { 3, 6, 5, 33 };
            yield return new object[] { 3, 7, 5, 34 };
            yield return new object[] { 3, 8, 5, 35 };

            yield return new object[] { 4, 0, 3, 36 };
            yield return new object[] { 4, 1, 3, 37 };
            yield return new object[] { 4, 2, 3, 38 };
            yield return new object[] { 4, 3, 4, 39 };
            yield return new object[] { 4, 4, 4, 40 };
            yield return new object[] { 4, 5, 4, 41 };
            yield return new object[] { 4, 6, 5, 42 };
            yield return new object[] { 4, 7, 5, 43 };
            yield return new object[] { 4, 8, 5, 44 };

            yield return new object[] { 5, 0, 3, 45 };
            yield return new object[] { 5, 1, 3, 46 };
            yield return new object[] { 5, 2, 3, 47 };
            yield return new object[] { 5, 3, 4, 48 };
            yield return new object[] { 5, 4, 4, 49 };
            yield return new object[] { 5, 5, 4, 50 };
            yield return new object[] { 5, 6, 5, 51 };
            yield return new object[] { 5, 7, 5, 52 };
            yield return new object[] { 5, 8, 5, 53 };

            yield return new object[] { 6, 0, 6, 54 };
            yield return new object[] { 6, 1, 6, 55 };
            yield return new object[] { 6, 2, 6, 56 };
            yield return new object[] { 6, 3, 7, 57 };
            yield return new object[] { 6, 4, 7, 58 };
            yield return new object[] { 6, 5, 7, 59 };
            yield return new object[] { 6, 6, 8, 60 };
            yield return new object[] { 6, 7, 8, 61 };
            yield return new object[] { 6, 8, 8, 62 };

            yield return new object[] { 7, 0, 6, 63 };
            yield return new object[] { 7, 1, 6, 64 };
            yield return new object[] { 7, 2, 6, 65 };
            yield return new object[] { 7, 3, 7, 66 };
            yield return new object[] { 7, 4, 7, 67 };
            yield return new object[] { 7, 5, 7, 68 };
            yield return new object[] { 7, 6, 8, 69 };
            yield return new object[] { 7, 7, 8, 70 };
            yield return new object[] { 7, 8, 8, 71 };

            yield return new object[] { 8, 0, 6, 72 };
            yield return new object[] { 8, 1, 6, 73 };
            yield return new object[] { 8, 2, 6, 74 };
            yield return new object[] { 8, 3, 7, 75 };
            yield return new object[] { 8, 4, 7, 76 };
            yield return new object[] { 8, 5, 7, 77 };
            yield return new object[] { 8, 6, 8, 78 };
            yield return new object[] { 8, 7, 8, 79 };
            yield return new object[] { 8, 8, 8, 80 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
