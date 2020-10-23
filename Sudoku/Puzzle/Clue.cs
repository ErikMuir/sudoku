namespace Sudoku
{
    public class Clue : Cell
    {
        public Clue(int col, int row, int val) : base(col, row, val) { }

        public override bool IsClue => true;
        public override int? Value
        {
            get { return this._value; }
            set { throw new SudokuException("Cannot change a clue"); }
        }
        public override void AddCandidate(int val) => throw new SudokuException("Cannot change a clue");
        public override void RemoveCandidate(int val) => throw new SudokuException("Cannot change a clue");
        public override void FillCandidates() => throw new SudokuException("Cannot change a clue");
        public override void ClearCandidates() => throw new SudokuException("Cannot change a clue");
        public override Cell Clone() => new Clue(this.Col, this.Row, this.Value.Value);
    }
}