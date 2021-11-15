using System.Collections.Generic;
using System.Linq;

namespace Sudoku.Logic
{
    public class Puzzle
    {
        private static readonly Dictionary<int, int[]> _lazyPeers = new();

        public const int BoxSize = 3;
        public const int UnitSize = 9;
        public const int TotalCells = 81;
        public const int ReflectiveIndex = 4;

        public Puzzle()
        {
            for (int row = 0; row < UnitSize; row++)
                for (int col = 0; col < UnitSize; col++)
                    this.Cells[(row * UnitSize) + col] = new Cell(row, col);
        }

        public Puzzle(Puzzle puzzle)
        {
            this.Metadata = puzzle.Metadata;
            for (int i = 0; i < TotalCells; i++)
            {
                Cell cell = puzzle.Cells[i];
                this.Cells[i] = cell.IsClue
                    ? new Clue(cell as Clue)
                    : new Cell(cell);
            }
        }

        public Cell[] Cells { get; private set; } = new Cell[TotalCells];
        public Metadata Metadata { get; set; } = new();

        public Cell GetCell(int row, int col) => this.Cells[(row * UnitSize) + col];
        public Cell[] GetRow(int row) => this.Cells.Where(x => x.Row == row).OrderBy(x => x.Col).ToArray();
        public Cell[] GetCol(int col) => this.Cells.Where(x => x.Col == col).OrderBy(x => x.Row).ToArray();
        public Cell[] GetBox(int box) => this.Cells.Where(x => x.Box == box).OrderBy(x => x.Row).ThenBy(x => x.Col).ToArray();
        public Cell[] GetEmptyCells() => this.Cells.Where(x => x.Value is null).ToArray();
        public Cell GetNextEmptyCell() => this.Cells.Where(x => x.Value is null).FirstOrDefault();
        public Cell[] CommonPeers(Cell c1, Cell c2) => Peers(c1).Intersect(Peers(c2)).ToArray();
        public Cell[] Peers(Cell cell)
        {
            if (!_lazyPeers.ContainsKey(cell.Index))
            {
                int[] peers = this.Cells
                    .Where(c => cell.IsPeer(c))
                    .Select(c => c.Index)
                    .ToArray();
                _lazyPeers.Add(cell.Index, peers);
            }
            return _lazyPeers[cell.Index]
                .Select(c => this.Cells[c])
                .ToArray();
        }

        public bool IsSolved() =>
            UnitSize.LoopAnd(i => this.GetRow(i).IsUnitSolved())
            && UnitSize.LoopAnd(i => this.GetCol(i).IsUnitSolved())
            && UnitSize.LoopAnd(i => this.GetBox(i).IsUnitSolved());

        public bool IsValid() =>
            UnitSize.LoopAnd(i => this.GetRow(i).IsUnitValid())
            && UnitSize.LoopAnd(i => this.GetCol(i).IsUnitValid())
            && UnitSize.LoopAnd(i => this.GetBox(i).IsUnitValid());

        public void FillCandidates() =>
            this.GetEmptyCells()
                .ToList()
                .ForEach(cell => cell.FillCandidates());

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
    }
}
