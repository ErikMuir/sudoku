using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sudoku.Logic
{
    public class Cell
    {
        public Cell(int row, int col, int? val = null)
        {
            this.Row = row;
            this.Col = col;
            this.Box = ((col / Puzzle.BoxSize) + ((row / Puzzle.BoxSize) * Puzzle.BoxSize));
            this.Index = (row * Puzzle.UnitSize) + col;
            this._value = val.HasValue ? this._validatedValue(val.Value) : null;
        }

        public Cell(Cell cell) : this(cell.Row, cell.Col, cell.Value)
        {
            if (cell is Clue) throw new SudokuException("A clue cell cannot be cloned as a non-clue cell");
            cell.Candidates.ToList().ForEach(x => this.AddCandidate(x));
        }

        public readonly int Row;
        public readonly int Col;
        public readonly int Box;
        public readonly int Index;

        public virtual bool IsClue => false;

        public virtual CellType Type => this.Value is not null ? CellType.Filled : CellType.Empty;

        protected int? _value;
        public virtual int? Value
        {
            get { return this._value; }
            set
            {
                if (value.HasValue)
                {
                    this._value = this._validatedValue(value.Value);
                    this.ClearCandidates();
                }
                else
                {
                    this._value = null;
                }
            }
        }

        protected SortedSet<int> _candidates = new();
        public ReadOnlyCollection<int> Candidates => this._candidates.ToList().AsReadOnly();
        public virtual void AddCandidate(int val) => this._candidates.Add(this._validatedValue(val));
        public virtual void RemoveCandidate(int val) => this._candidates.Remove(val);
        public virtual void FillCandidates()
        {
            for (int i = 0; i < Puzzle.UnitSize; i++)
                this.AddCandidate(i + 1);
        }
        public virtual void ClearCandidates() => this._candidates.Clear();
        public bool ContainsOnlyMatches(CandidateSet set) =>
            this._candidates.Except(set).Count() == 0 &&
            set.Except(this._candidates).Count() == 0;
        public bool ContainsAtLeastOneMatch(CandidateSet set) =>
            this._candidates.Intersect(set).Any();
        public List<int> GetNonMatchingCandidates(IEnumerable<int> set) =>
            this._candidates.Except(set).ToList();

        private int _validatedValue(int val)
        {
            if (!val.Between(1, Puzzle.UnitSize, true))
                throw new SudokuException($"Value must be between 1 and {Puzzle.UnitSize}, inclusive.");
            return val;
        }
    }
}
