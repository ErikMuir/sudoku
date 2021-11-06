using System.Collections.Generic;
using System.Linq;
using Sudoku.Generation;
using Sudoku.Serialization;

namespace Sudoku
{
    public class Puzzle
    {
        public const int BoxSize = 3;
        public const int UnitSize = BoxSize * BoxSize;
        public const int TotalCells = UnitSize * UnitSize;

        public Puzzle()
        {
            Utils.Loop(row => Utils.Loop(col => Cells[(row * UnitSize) + col] = new Cell(row, col)));
        }

        public Puzzle(GeneratorPuzzle puzzle)
        {
            Cells = puzzle.Cells
                .Select((candidates, index) =>
                {
                    int row = index / UnitSize;
                    int col = index % UnitSize;
                    if (candidates.Length == 1)
                        return new Clue(row, col, candidates[0]);
                    Cell cell = new Cell(row, col);
                    foreach (int cand in candidates)
                        cell.AddCandidate(cand);
                    return cell;
                })
                .ToArray();
        }

        public Cell[] Cells { get; private set; } = new Cell[TotalCells];
        public Metadata Metadata { get; set; } = new();

        public Cell GetCell(int row, int col) => this.Cells[(row * UnitSize) + col];
        public Cell[] GetRow(int row) => this.Cells.Where(x => x.Row == row).OrderBy(x => x.Col).ToArray();
        public Cell[] GetCol(int col) => this.Cells.Where(x => x.Col == col).OrderBy(x => x.Row).ToArray();
        public Cell[] GetBox(int box) => this.Cells.Where(x => x.Box == box).OrderBy(x => x.Row).ThenBy(x => x.Col).ToArray();
        public Cell[] GetEmptyCells() => this.Cells.Where(x => x.Value is null).ToArray();
        public Cell GetNextEmptyCell() => this.Cells.Where(x => x.Value is null).FirstOrDefault();

        private static readonly Dictionary<int, int[]> _savedPeers = new();
        public int[] Peers(int cellIndex)
        {
            if (!_savedPeers.ContainsKey(cellIndex))
                _savedPeers.Add(cellIndex, Enumerable.Range(0, 81).Where(c => IsPeer(cellIndex, c)).ToArray());
            return _savedPeers[cellIndex];
        }
        public Cell[] Peers(Cell cell) => Peers((cell.Row * UnitSize) + cell.Col).Select(i => Cells[i]).ToArray();
        public Cell[] CommonPeers(Cell c1, Cell c2) => Peers(c1).Intersect(Peers(c2)).ToArray();
        private bool IsPeer(int c1, int c2) => c1 != c2 && (IsSameRow(c1, c2) || IsSameColumn(c1, c2) || IsSameBox(c1, c2));
        private bool IsSameRow(int c1, int c2) => c1 / UnitSize == c2 / UnitSize;
        private bool IsSameColumn(int c1, int c2) => c1 % UnitSize == c2 % UnitSize;
        private bool IsSameBox(int c1, int c2) => c1 / UnitSize / BoxSize == c2 / UnitSize / BoxSize && c1 % UnitSize / BoxSize == c2 % UnitSize / BoxSize;

        public bool IsSolved() =>
            Utils.LoopAnd(i => this.GetRow(i).IsUnitSolved())
            && Utils.LoopAnd(i => this.GetCol(i).IsUnitSolved())
            && Utils.LoopAnd(i => this.GetBox(i).IsUnitSolved());

        public bool IsValid() =>
            Utils.LoopAnd(i => this.GetRow(i).IsUnitValid())
            && Utils.LoopAnd(i => this.GetCol(i).IsUnitValid())
            && Utils.LoopAnd(i => this.GetBox(i).IsUnitValid());

        public void CalculateCandidates()
        {
            // first fill all candidates of empty cells
            this.GetEmptyCells().ToList().ForEach(cell => cell.FillCandidates());

            // then reduce candidates by col/row/box constraints
            this.ReduceCandidates();
        }

        public void ReduceCandidates()
        {
            for (int iUnit = 0; iUnit < UnitSize; iUnit++)
            {
                Cell[] row = this.GetRow(iUnit);
                Cell[] col = this.GetCol(iUnit);
                Cell[] box = this.GetBox(iUnit);

                for (int iCell = 0; iCell < UnitSize; iCell++)
                {
                    int? rowCellValue = row[iCell].Value;
                    int? colCellValue = col[iCell].Value;
                    int? boxCellValue = box[iCell].Value;

                    for (int iPeer = 0; iPeer < UnitSize; iPeer++)
                    {
                        if (iPeer == iCell) continue;

                        Cell rowPeer = row[iPeer];
                        Cell colPeer = col[iPeer];
                        Cell boxPeer = box[iPeer];

                        if (rowCellValue is not null && !rowPeer.IsClue)
                            rowPeer.RemoveCandidate(rowCellValue.Value);

                        if (colCellValue is not null && !colPeer.IsClue)
                            colPeer.RemoveCandidate(colCellValue.Value);

                        if (boxCellValue is not null && !boxPeer.IsClue)
                            boxPeer.RemoveCandidate(boxCellValue.Value);
                    }
                }
            }
        }

        public Puzzle Clone()
        {
            PzlSerializer pzl = new();
            string puzzleString = pzl.Serialize(this);
            return pzl.Deserialize(puzzleString);
        }
    }
}
