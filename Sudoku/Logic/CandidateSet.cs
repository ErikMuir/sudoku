using System.Collections.Generic;

namespace Sudoku.Logic
{
    public class CandidateSet : SortedSet<int>
    {
        protected CandidateSet() { }
    }

    public class SingleSet : CandidateSet
    {
        public SingleSet(int val)
        {
            this.Add(val);
        }
    }

    public class DoubleSet : CandidateSet
    {
        public DoubleSet (int val1, int val2)
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
}
