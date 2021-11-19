using System.Collections.Generic;

namespace Sudoku.Logic
{
    public class CandidateSet : SortedSet<int>
    {
        protected CandidateSet() { }
    }

    public class DoubleSet : CandidateSet
    {
        public DoubleSet(int val1, int val2)
        {
            this.Add(val1);
            this.Add(val2);
        }
    }

    public class TripleSet : CandidateSet
    {
        public TripleSet(int val1, int val2, int val3)
        {
            this.Add(val1);
            this.Add(val2);
            this.Add(val3);
        }
    }

    public class QuadrupleSet : CandidateSet
    {
        public QuadrupleSet(int val1, int val2, int val3, int val4)
        {
            this.Add(val1);
            this.Add(val2);
            this.Add(val3);
            this.Add(val4);
        }
    }

    public static class CandidateSets
    {
        private static bool _initialized = false;
        private static List<CandidateSet> _doubles = new();
        private static List<CandidateSet> _triples = new();
        private static List<CandidateSet> _quadruples = new();

        public static List<CandidateSet> Doubles
        {
            get
            {
                if (!_initialized) _initialize();
                return _doubles;
            }
        }

        public static List<CandidateSet> Triples
        {
            get
            {
                if (!_initialized) _initialize();
                return _triples;
            }
        }

        public static List<CandidateSet> Quadruples
        {
            get
            {
                if (!_initialized) _initialize();
                return _quadruples;
            }
        }

        private static void _initialize()
        {
            for (int a = 0; a < Puzzle.UnitSize; a++)
            {
                for (int b = 1; b < Puzzle.UnitSize; b++)
                {
                    if (b <= a) continue;
                    _doubles.Add(new DoubleSet(a, b));
                    for (int c = 2; c < Puzzle.UnitSize; c++)
                    {
                        if (c <= a || c <= b) continue;
                        _triples.Add(new TripleSet(a, b, c));
                        for (int d = 3; d < Puzzle.UnitSize; d++)
                        {
                            if (d <= a || d <= b || d <= c) continue;
                            _quadruples.Add(new QuadrupleSet(a, b, c, d));
                        }
                    }
                }
            }
            _initialized = true;
        }
    }
}
