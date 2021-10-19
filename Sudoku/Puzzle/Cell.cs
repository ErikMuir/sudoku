using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sudoku
{
    public class Cell
    {
        private static Regex _cluePattern = new("^[0-8][0-8][1-9]1$");
        private static Regex _filledPattern = new("^[0-8][0-8][1-9]0$");
        private static Regex _emptyPattern = new("^[0-8][0-8]001?2?3?4?5?6?7?8?9?$");

        public Cell(int col, int row, int? val = null)
        {
            this.Validate(col, row, val);
            this.Col = col;
            this.Row = row;
            this.Box = ((col / 3) + ((row / 3) * 3));
            this._value = val;
        }

        public readonly int Col;
        public readonly int Row;
        public readonly int Box;

        public virtual bool IsClue => false;

        protected int? _value;
        public virtual int? Value
        {
            get { return this._value; }
            set
            {
                if (value.HasValue)
                {
                    this._value = this.ValidatedValue(value.Value);
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
        public virtual void AddCandidate(int val) => this._candidates.Add(this.ValidatedValue(val));
        public virtual void RemoveCandidate(int val) => this._candidates.Remove(val);
        public virtual void FillCandidates() => Utils.Loop(i => this.AddCandidate(i + 1));
        public virtual void ClearCandidates() => this._candidates.Clear();
        public virtual bool ContainsOnlyMatches(CandidateSet set) =>
            this._candidates.Except(set).Count() == 0 &&
            set.Except(this._candidates).Count() == 0;
        public virtual bool ContainsAtLeastOneMatch(CandidateSet set) =>
            this._candidates.Intersect(set).Any();
        public virtual List<int> GetNonMatchingCandidates(IEnumerable<int> set) =>
            this._candidates.Except(set).ToList();

        public virtual Cell Clone()
        {
            Cell clone = new(this.Col, this.Row, this.Value);
            this._candidates.ToList().ForEach(x => clone.AddCandidate(x));
            return clone;
        }

        public static Cell Parse(string cellString)
        {
            CellType cellType = Cell.GetCellType(cellString);
            if (cellType == CellType.Invalid)
            {
                throw new SudokuException("Invalid cell");
            }

            int col = (int)char.GetNumericValue(cellString[0]);
            int row = (int)char.GetNumericValue(cellString[1]);
            int val = (int)char.GetNumericValue(cellString[2]);

            switch (cellType)
            {
                case CellType.Clue:
                    return new Clue(col, row, val);
                case CellType.Filled:
                    Cell filledCell = new(col, row);
                    filledCell.Value = val;
                    return filledCell;
                case CellType.Empty:
                    Cell emptyCell = new(col, row);
                    cellString.Skip(4).ToList().ForEach(x => emptyCell.AddCandidate(int.Parse($"{x}")));
                    return emptyCell;
                default:
                    throw new NotImplementedException("Unsupported cell type");
            }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new();
            stringBuilder.Append(this.Col);
            stringBuilder.Append(this.Row);
            stringBuilder.Append(this.Value ?? 0);
            stringBuilder.Append(this.IsClue ? 1 : 0);
            this._candidates.ToList().ForEach(x => stringBuilder.Append(x));
            return stringBuilder.ToString();
        }

        private static CellType GetCellType(string cell)
        {
            if (Cell.IsClueString(cell))
                return CellType.Clue;
            if (Cell.IsFilledString(cell))
                return CellType.Filled;
            if (Cell.IsEmptyString(cell))
                return CellType.Empty;
            return CellType.Invalid;
        }

        private static bool IsClueString(string cell) => cell is not null && _cluePattern.IsMatch(cell);
        private static bool IsFilledString(string cell) => cell is not null && _filledPattern.IsMatch(cell);
        private static bool IsEmptyString(string cell) => cell is not null && _emptyPattern.IsMatch(cell);

        private void Validate(int col, int row, int? val)
        {
            List<string> errors = new();

            if (!col.Between(0, 8, true))
                errors.Add("Cell columns must be between 0 and 8, inclusive.");

            if (!row.Between(0, 8, true))
                errors.Add("Cell rows must be between 0 and 8, inclusive.");

            if (val is not null && !val.Between(1, 9, true))
                errors.Add("Cell values must be between 1 and 9, inclusive.");

            if (errors.Any())
                throw new SudokuException(string.Join(" ", errors));
        }

        private int ValidatedValue(int val)
        {
            if (val.Between(1, 9, true)) return val;
            throw new SudokuException("Value must be between 1 and 9, inclusive.");
        }
    }
}
