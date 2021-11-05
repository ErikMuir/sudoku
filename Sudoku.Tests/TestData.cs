using System.Collections;
using System.Collections.Generic;

namespace Sudoku.Tests
{
    public class RowColBoxTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 0, 0, 0 };
            yield return new object[] { 0, 1, 0 };
            yield return new object[] { 0, 2, 0 };
            yield return new object[] { 0, 3, 1 };
            yield return new object[] { 0, 4, 1 };
            yield return new object[] { 0, 5, 1 };
            yield return new object[] { 0, 6, 2 };
            yield return new object[] { 0, 7, 2 };
            yield return new object[] { 0, 8, 2 };

            yield return new object[] { 1, 0, 0 };
            yield return new object[] { 1, 1, 0 };
            yield return new object[] { 1, 2, 0 };
            yield return new object[] { 1, 3, 1 };
            yield return new object[] { 1, 4, 1 };
            yield return new object[] { 1, 5, 1 };
            yield return new object[] { 1, 6, 2 };
            yield return new object[] { 1, 7, 2 };
            yield return new object[] { 1, 8, 2 };

            yield return new object[] { 2, 0, 0 };
            yield return new object[] { 2, 1, 0 };
            yield return new object[] { 2, 2, 0 };
            yield return new object[] { 2, 3, 1 };
            yield return new object[] { 2, 4, 1 };
            yield return new object[] { 2, 5, 1 };
            yield return new object[] { 2, 6, 2 };
            yield return new object[] { 2, 7, 2 };
            yield return new object[] { 2, 8, 2 };

            yield return new object[] { 3, 0, 3 };
            yield return new object[] { 3, 1, 3 };
            yield return new object[] { 3, 2, 3 };
            yield return new object[] { 3, 3, 4 };
            yield return new object[] { 3, 4, 4 };
            yield return new object[] { 3, 5, 4 };
            yield return new object[] { 3, 6, 5 };
            yield return new object[] { 3, 7, 5 };
            yield return new object[] { 3, 8, 5 };

            yield return new object[] { 4, 0, 3 };
            yield return new object[] { 4, 1, 3 };
            yield return new object[] { 4, 2, 3 };
            yield return new object[] { 4, 3, 4 };
            yield return new object[] { 4, 4, 4 };
            yield return new object[] { 4, 5, 4 };
            yield return new object[] { 4, 6, 5 };
            yield return new object[] { 4, 7, 5 };
            yield return new object[] { 4, 8, 5 };

            yield return new object[] { 5, 0, 3 };
            yield return new object[] { 5, 1, 3 };
            yield return new object[] { 5, 2, 3 };
            yield return new object[] { 5, 3, 4 };
            yield return new object[] { 5, 4, 4 };
            yield return new object[] { 5, 5, 4 };
            yield return new object[] { 5, 6, 5 };
            yield return new object[] { 5, 7, 5 };
            yield return new object[] { 5, 8, 5 };

            yield return new object[] { 6, 0, 6 };
            yield return new object[] { 6, 1, 6 };
            yield return new object[] { 6, 2, 6 };
            yield return new object[] { 6, 3, 7 };
            yield return new object[] { 6, 4, 7 };
            yield return new object[] { 6, 5, 7 };
            yield return new object[] { 6, 6, 8 };
            yield return new object[] { 6, 7, 8 };
            yield return new object[] { 6, 8, 8 };

            yield return new object[] { 7, 0, 6 };
            yield return new object[] { 7, 1, 6 };
            yield return new object[] { 7, 2, 6 };
            yield return new object[] { 7, 3, 7 };
            yield return new object[] { 7, 4, 7 };
            yield return new object[] { 7, 5, 7 };
            yield return new object[] { 7, 6, 8 };
            yield return new object[] { 7, 7, 8 };
            yield return new object[] { 7, 8, 8 };

            yield return new object[] { 8, 0, 6 };
            yield return new object[] { 8, 1, 6 };
            yield return new object[] { 8, 2, 6 };
            yield return new object[] { 8, 3, 7 };
            yield return new object[] { 8, 4, 7 };
            yield return new object[] { 8, 5, 7 };
            yield return new object[] { 8, 6, 8 };
            yield return new object[] { 8, 7, 8 };
            yield return new object[] { 8, 8, 8 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public class RowColTestData : IEnumerable<object[]>
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
