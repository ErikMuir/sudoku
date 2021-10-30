// using System.Collections.Generic;
// using System.Collections.ObjectModel;
// using System.Linq;

// namespace Sudoku
// {
//     public class Cell
//     {
//         public Cell(int col, int row, int? val = null)
//         {
//             this.Validate(col, row, val);
//             this.Col = col;
//             this.Row = row;
//             this.Box = ((col / 3) + ((row / 3) * 3));
//             this._value = val;
//         }

//         public readonly int Col;
//         public readonly int Row;
//         public readonly int Box;

//         public virtual bool IsClue => false;

//         protected int? _value;
//         public virtual int? Value
//         {
//             get { return this._value; }
//             set
//             {
//                 if (value.HasValue)
//                 {
//                     this._value = this.ValidatedValue(value.Value);
//                     this.ClearCandidates();
//                 }
//                 else
//                 {
//                     this._value = null;
//                 }
//             }
//         }
        
//         protected SortedSet<int> _candidates = new();
//         public ReadOnlyCollection<int> Candidates => this._candidates.ToList().AsReadOnly();
//         public virtual void AddCandidate(int val) => this._candidates.Add(this.ValidatedValue(val));
//         public virtual void RemoveCandidate(int val) => this._candidates.Remove(val);
//         public virtual void FillCandidates() => Utils.Loop(i => this.AddCandidate(i + 1));
//         public virtual void ClearCandidates() => this._candidates.Clear();
//         public virtual bool ContainsOnlyMatches(CandidateSet set) =>
//             this._candidates.Except(set).Count() == 0 &&
//             set.Except(this._candidates).Count() == 0;
//         public virtual bool ContainsAtLeastOneMatch(CandidateSet set) =>
//             this._candidates.Intersect(set).Any();
//         public virtual List<int> GetNonMatchingCandidates(IEnumerable<int> set) =>
//             this._candidates.Except(set).ToList();

//         public virtual Cell Clone()
//         {
//             Cell clone = new(this.Col, this.Row, this.Value);
//             this._candidates.ToList().ForEach(x => clone.AddCandidate(x));
//             return clone;
//         }

//         private void Validate(int col, int row, int? val)
//         {
//             List<string> errors = new();

//             if (!col.Between(0, 8, true))
//                 errors.Add("Cell columns must be between 0 and 8, inclusive.");

//             if (!row.Between(0, 8, true))
//                 errors.Add("Cell rows must be between 0 and 8, inclusive.");

//             if (val is not null && !val.Between(1, 9, true))
//                 errors.Add("Cell values must be between 1 and 9, inclusive.");

//             if (errors.Any())
//                 throw new SudokuException(string.Join(" ", errors));
//         }

//         private int ValidatedValue(int val)
//         {
//             if (val.Between(1, 9, true)) return val;
//             throw new SudokuException("Value must be between 1 and 9, inclusive.");
//         }
//     }
// }