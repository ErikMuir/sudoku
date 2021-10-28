// using System.Collections.Generic;
// using System.Linq;
// using Sudoku.Generators;
// using Sudoku.Serializers;

// namespace Sudoku
// {
//     public class Puzzle
//     {
//         public Puzzle()
//         {
//             Utils.Loop(row => Utils.Loop(col => Cells[(row * Constants.UnitSize) + col] = new Cell(col, row)));
//         }

//         public Puzzle(GeneratorPuzzle puzzle)
//         {
//             Cells = puzzle.Cells
//                 .Select((candidates, index) =>
//                 {
//                     int row = index / Constants.UnitSize;
//                     int col = index % Constants.UnitSize;
//                     if (candidates.Length == 1)
//                         return new Clue(col, row, candidates[0]);
//                     Cell cell = new Cell(col, row);
//                     foreach (int cand in candidates)
//                         cell.AddCandidate(cand);
//                     return cell;
//                 })
//                 .ToArray();
//         }

//         public Cell[] Cells { get; private set; } = new Cell[Constants.TotalCells];
//         public Metadata Metadata { get; set; } = new();

//         public Cell GetCell(int col, int row) => this.Cells[(row * Constants.UnitSize) + col];
//         public List<Cell> GetRow(int row) => this.Cells.Where(x => x.Row == row).OrderBy(x => x.Col).ToList();
//         public List<Cell> GetCol(int col) => this.Cells.Where(x => x.Col == col).OrderBy(x => x.Row).ToList();
//         public List<Cell> GetBox(int box) => this.Cells.Where(x => x.Box == box).OrderBy(x => x.Row).ThenBy(x => x.Col).ToList();
//         public List<Cell> GetEmptyCells() => this.Cells.Where(x => x.Value is null).ToList();
//         public Cell GetNextEmptyCell() => this.Cells.Where(x => x.Value is null).FirstOrDefault();
//         public List<Cell> GetRelatives(Cell cell)
//         {
//             List<Cell> relatives = new();
//             List<Cell> col = GetCol(cell.Col);
//             List<Cell> row = GetRow(cell.Row);
//             List<Cell> box = GetBox(cell.Box);
//             for (int i = 0; i < Constants.UnitSize; i++)
//             {
//                 relatives.Add(box[i]);
//                 if (col[i].Box != cell.Box)
//                     relatives.Add(col[i]);
//                 if (row[i].Box != cell.Box)
//                     relatives.Add(row[i]);
//             }
//             return relatives;
//         }
//         public List<Cell> GetCommonRelatives(Cell cell1, Cell cell2)
//             => GetRelatives(cell1).Intersect(GetRelatives(cell2)).ToList();

//         public bool IsSolved() =>
//             Utils.LoopAnd(i => this.GetRow(i).IsUnitSolved())
//             && Utils.LoopAnd(i => this.GetCol(i).IsUnitSolved())
//             && Utils.LoopAnd(i => this.GetBox(i).IsUnitSolved());

//         public bool IsValid() =>
//             Utils.LoopAnd(i => this.GetRow(i).IsUnitValid())
//             && Utils.LoopAnd(i => this.GetCol(i).IsUnitValid())
//             && Utils.LoopAnd(i => this.GetBox(i).IsUnitValid());

//         public void CalculateCandidates()
//         {
//             // first fill all candidates of empty cells
//             this.GetEmptyCells().ForEach(cell => cell.FillCandidates());

//             // then reduce candidates by col/row/box constraints
//             this.ReduceCandidates();
//         }

//         public void ReduceCandidates()
//         {
//             for (int iUnit = 0; iUnit < Constants.UnitSize; iUnit++)
//             {
//                 List<Cell> col = this.GetCol(iUnit);
//                 List<Cell> row = this.GetRow(iUnit);
//                 List<Cell> box = this.GetBox(iUnit);

//                 for (int i = 0; i < Constants.UnitSize; i++)
//                 {
//                     int? colValue = col[i].Value;
//                     int? rowValue = row[i].Value;
//                     int? boxValue = box[i].Value;

//                     for (int j = 0; j < Constants.UnitSize; j++)
//                     {
//                         if (colValue.HasValue && !col[j].IsClue)
//                             col[j].RemoveCandidate(colValue.Value);

//                         if (rowValue.HasValue && !row[j].IsClue)
//                             row[j].RemoveCandidate(rowValue.Value);

//                         if (boxValue.HasValue && !box[j].IsClue)
//                             box[j].RemoveCandidate(boxValue.Value);
//                     }
//                 }
//             }
//         }

//         public Puzzle Clone()
//         {
//             PzlSerializer pzl = new();
//             string puzzleString = pzl.Serialize(this);
//             return pzl.Deserialize(puzzleString);
//         }
//     }
// }
