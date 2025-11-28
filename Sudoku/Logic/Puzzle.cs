using System.Collections.Generic;
using System.Linq;
using Sudoku.Extensions;

namespace Sudoku.Logic;

    public class Puzzle
    {
        private static readonly Dictionary<int, int[]> _lazyPeers = new();

        public const int BoxSize = 3;
        public const int UnitSize = 9;
        public const int TotalCells = 81;
        public const int ReflectiveIndex = 4;

        public Puzzle()
        {
            UnitSize.Loop(row =>
                UnitSize.Loop(col =>
                    this.Cells[(row * UnitSize) + col] = new Cell(row, col)));
        }

        public Puzzle(Metadata metadata) : this()
        {
            this.Metadata = metadata;
        }

        public Puzzle(Puzzle puzzle)
        {
            this.Metadata = puzzle.Metadata;
            TotalCells.Loop(i =>
            {
                Cell cell = puzzle.Cells[i];
                this.Cells[i] = cell.IsClue
                    ? new Clue(cell as Clue)
                    : new Cell(cell);
            });
        }

        public Cell[] Cells { get; private set; } = new Cell[TotalCells];
        public Metadata Metadata { get; set; } = new();

        public Cell GetCell(int row, int col) => this.Cells[(row * UnitSize) + col];
        public IEnumerable<Cell> GetRow(int row) => this.Cells.Where(x => x.Row == row).OrderBy(x => x.Col);
        public IEnumerable<Cell> GetCol(int col) => this.Cells.Where(x => x.Col == col).OrderBy(x => x.Row);
        public IEnumerable<Cell> GetBox(int box) => this.Cells.Where(x => x.Box == box).OrderBy(x => x.Row).ThenBy(x => x.Col);
        public IEnumerable<Cell> CommonPeers(Cell c1, Cell c2) => Peers(c1).Intersect(Peers(c2));
        public IEnumerable<Cell> Peers(Cell cell)
        {
            if (!_lazyPeers.ContainsKey(cell.Index))
            {
                int[] peers = this.Cells
                    .Where(c => cell.IsPeer(c))
                    .Select(c => c.Index)
                    .ToArray();
                _lazyPeers.Add(cell.Index, peers);
            }
            return _lazyPeers[cell.Index].Select(c => this.Cells[c]);
        }

        public bool IsSolved =>
            UnitSize.LoopAnd(i => this.GetRow(i).IsUnitSolved())
            && UnitSize.LoopAnd(i => this.GetCol(i).IsUnitSolved())
            && UnitSize.LoopAnd(i => this.GetBox(i).IsUnitSolved());

        public bool IsValid =>
            UnitSize.LoopAnd(i => this.GetRow(i).IsUnitValid())
            && UnitSize.LoopAnd(i => this.GetCol(i).IsUnitValid())
            && UnitSize.LoopAnd(i => this.GetBox(i).IsUnitValid());

        public void ResetFilledCells() =>
            this.Cells
                .FilledCells()
                .ToList()
                .ForEach(cell => cell.Value = null);

        public void FillCandidates() =>
            this.Cells
                .EmptyCells()
                .ToList()
                .ForEach(cell => cell.FillCandidates());

        public void ReduceCandidates()
        {
            foreach (Cell cell in this.Cells.NonEmptyCells())
                foreach (Cell peer in this.Peers(cell).NonClueCells())
                    peer.RemoveCandidate((int)cell.Value);
        }

        // public void ReduceCandidates()
        // {
        //     UnitSize.Loop(iUnit =>
        //     {
        //         Cell[] row = this.GetRow(iUnit);
        //         Cell[] col = this.GetCol(iUnit);
        //         Cell[] box = this.GetBox(iUnit);

        //         UnitSize.Loop(iCell =>
        //         {
        //             int? rowCellValue = row[iCell].Value;
        //             int? colCellValue = col[iCell].Value;
        //             int? boxCellValue = box[iCell].Value;

        //             UnitSize.Loop(iPeer =>
        //             {
        //                 if (iPeer == iCell) return;

        //                 Cell rowPeer = row[iPeer];
        //                 Cell colPeer = col[iPeer];
        //                 Cell boxPeer = box[iPeer];

        //                 if (rowCellValue is not null && !rowPeer.IsClue)
        //                     rowPeer.RemoveCandidate(rowCellValue.Value);

        //                 if (colCellValue is not null && !colPeer.IsClue)
        //                     colPeer.RemoveCandidate(colCellValue.Value);

        //                 if (boxCellValue is not null && !boxPeer.IsClue)
        //                     boxPeer.RemoveCandidate(boxCellValue.Value);
        //             });
        //         });
        //     });
        // }
}
