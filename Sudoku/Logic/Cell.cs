namespace Sudoku.Logic;

public class Cell
{
    public Cell(int row, int col, int? val = null)
    {
        Row = row;
        Col = col;
        Box = ((col / Puzzle.BoxSize) + ((row / Puzzle.BoxSize) * Puzzle.BoxSize));
        Index = (row * Puzzle.UnitSize) + col;
        _value = val.HasValue ? ValidatedValue(val.Value) : null;
    }

    public Cell(Cell cell) : this(cell.Row, cell.Col, cell.Value)
    {
        if (cell is Clue) throw new SudokuException("A clue cell cannot be cloned as a non-clue cell");
        cell.Candidates.ToList().ForEach(x => AddCandidate(x));
    }

    public readonly int Row;
    public readonly int Col;
    public readonly int Box;
    public readonly int Index;

    public virtual bool IsClue => false;

    public virtual CellType Type => Value is not null ? CellType.Filled : CellType.Empty;

    protected int? _value;
    public virtual int? Value
    {
        get { return _value; }
        set
        {
            if (value.HasValue)
            {
                _value = ValidatedValue(value.Value);
                ClearCandidates();
            }
            else
            {
                _value = null;
            }
        }
    }

    protected SortedSet<int> _candidates = [];
    public ReadOnlyCollection<int> Candidates => _candidates.ToList().AsReadOnly();
    public virtual void AddCandidate(int val) => _candidates.Add(ValidatedValue(val));
    public virtual void RemoveCandidate(int val) => _candidates.Remove(val);
    public virtual void FillCandidates()
    {
        for (var i = 0; i < Puzzle.UnitSize; i++)
            AddCandidate(i + 1);
    }
    public virtual void ClearCandidates() => _candidates.Clear();
    public bool ContainsOnlyMatches(CandidateSet set) =>
        !_candidates.Except(set).Any() &&
        !set.Except(_candidates).Any();
    public bool ContainsAtLeastOneMatch(CandidateSet set) =>
        _candidates.Intersect(set).Any();
    public List<int> GetNonMatchingCandidates(IEnumerable<int> set) =>
        [.. _candidates.Except(set)];
    public bool IsPeer(Cell cell) => cell.Index != Index
        && (SameRow(cell) || SameCol(cell) || SameBox(cell));

    private bool SameRow(Cell cell) => cell.Row == Row;
    private bool SameCol(Cell cell) => cell.Col == Col;
    private bool SameBox(Cell cell) => cell.Box == Box;
    private static int ValidatedValue(int val)
    {
        if (val < 1 || val > Puzzle.UnitSize)
            throw new SudokuException($"Value must be between 1 and {Puzzle.UnitSize}, inclusive.");
        return val;
    }
}
