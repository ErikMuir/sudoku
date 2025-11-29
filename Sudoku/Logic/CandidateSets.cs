namespace Sudoku.Logic;

public class CandidateSet : SortedSet<int>
{
    protected CandidateSet() { }
}

public class DoubleSet : CandidateSet
{
    public DoubleSet(int val1, int val2)
    {
        Add(val1);
        Add(val2);
    }
}

public class TripleSet : CandidateSet
{
    public TripleSet(int val1, int val2, int val3)
    {
        Add(val1);
        Add(val2);
        Add(val3);
    }
}

public class QuadrupleSet : CandidateSet
{
    public QuadrupleSet(int val1, int val2, int val3, int val4)
    {
        Add(val1);
        Add(val2);
        Add(val3);
        Add(val4);
    }
}

public static class CandidateSets
{
    private static readonly List<CandidateSet> _doubles = [];
    private static readonly List<CandidateSet> _triples = [];
    private static readonly List<CandidateSet> _quadruples = [];
    private static bool _initialized = false;

    public static List<CandidateSet> Doubles
    {
        get
        {
            if (!_initialized) Initialize();
            return _doubles;
        }
    }

    public static List<CandidateSet> Triples
    {
        get
        {
            if (!_initialized) Initialize();
            return _triples;
        }
    }

    public static List<CandidateSet> Quadruples
    {
        get
        {
            if (!_initialized) Initialize();
            return _quadruples;
        }
    }

    private static void Initialize()
    {
        for (var a = 0; a < Puzzle.UnitSize; a++)
        {
            for (var b = 1; b < Puzzle.UnitSize; b++)
            {
                if (b <= a) continue;
                _doubles.Add(new DoubleSet(a, b));
                for (var c = 2; c < Puzzle.UnitSize; c++)
                {
                    if (c <= a || c <= b) continue;
                    _triples.Add(new TripleSet(a, b, c));
                    for (var d = 3; d < Puzzle.UnitSize; d++)
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
