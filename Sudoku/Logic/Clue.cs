namespace Sudoku.Logic;

public class Clue : Cell
{
    public Clue(int row, int col, int val) : base(row, col, val) { }

    public Clue(Clue clue) : base(clue.Row, clue.Col, clue.Value) { }

    public override bool IsClue => true;
    public override CellType Type => CellType.Clue;
    public override int? Value
    {
        get { return _value; }
        set { throw new SudokuException("Cannot change a clue"); }
    }
    public override void AddCandidate(int val) => throw new SudokuException("Cannot change a clue");
    public override void RemoveCandidate(int val) => throw new SudokuException("Cannot change a clue");
    public override void FillCandidates() => throw new SudokuException("Cannot change a clue");
    public override void ClearCandidates() => throw new SudokuException("Cannot change a clue");
}
