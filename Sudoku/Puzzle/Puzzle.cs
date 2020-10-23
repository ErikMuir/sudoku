using System.Collections.Generic;
using System.Linq;

namespace Sudoku
{
    public class Puzzle
    {
        public Puzzle()
        {
            Utils.Loop(col => Utils.Loop(row => Cells.Add(new Cell(col, row))));
        }

        public List<Cell> Cells { get; private set; } = new List<Cell>();
        public Metadata Metadata { get; set; } = new Metadata();

        public Cell GetCell(int col, int row) => this.Cells.Where(x => x.Col == col && x.Row == row).FirstOrDefault();
        public List<Cell> GetRow(int row) => this.Cells.Where(x => x.Row == row).OrderBy(x => x.Col).ToList();
        public List<Cell> GetCol(int col) => this.Cells.Where(x => x.Col == col).OrderBy(x => x.Row).ToList();
        public List<Cell> GetBox(int box) => this.Cells.Where(x => x.Box == box).OrderBy(x => x.Row).ThenBy(x => x.Col).ToList();
        public List<Cell> GetEmptyCells() => this.Cells.Where(x => !x.Value.HasValue).ToList();
        public Cell GetNextEmptyCell() => this.Cells.Where(x => !x.Value.HasValue).FirstOrDefault();
        public List<Cell> GetRelatives(Cell cell)
        {
            var relatives = new List<Cell>();
            var col = GetCol(cell.Col);
            var row = GetRow(cell.Row);
            var box = GetBox(cell.Box);
            for (var i = 0; i < Constants.Size; i++)
            {
                relatives.Add(box[i]);
                if (col[i].Box != cell.Box)
                    relatives.Add(col[i]);
                if (row[i].Box != cell.Box)
                    relatives.Add(row[i]);
            }
            return relatives;
        }
        public List<Cell> GetCommonRelatives(Cell cell1, Cell cell2)
            => GetRelatives(cell1).Intersect(GetRelatives(cell2)).ToList();

        public bool IsSolved()
        {
            return (
                Utils.LoopAnd(i => this.GetRow(i).IsUnitSolved()) &&
                Utils.LoopAnd(i => this.GetCol(i).IsUnitSolved()) &&
                Utils.LoopAnd(i => this.GetBox(i).IsUnitSolved())
            );
        }

        public bool IsValid()
        {
            return (
                Utils.LoopAnd(i => this.GetRow(i).IsUnitValid()) &&
                Utils.LoopAnd(i => this.GetCol(i).IsUnitValid()) &&
                Utils.LoopAnd(i => this.GetBox(i).IsUnitValid())
            );
        }

        public void CalculateCandidates()
        {
            // first fill all candidates of empty cells
            this.GetEmptyCells().ForEach(cell => cell.FillCandidates());

            // then reduce candidates by col/row/box constraints
            this.ReduceCandidates();
        }

        public void ReduceCandidates()
        {
            for (var iUnit = 0; iUnit < 9; iUnit++)
            {
                var col = this.GetCol(iUnit);
                var row = this.GetRow(iUnit);
                var box = this.GetBox(iUnit);

                for (var i = 0; i < 9; i++)
                {
                    var colValue = col[i].Value;
                    var rowValue = row[i].Value;
                    var boxValue = box[i].Value;

                    for (var j = 0; j < 9; j++)
                    {
                        if (colValue.HasValue && !col[j].IsClue)
                            col[j].RemoveCandidate(colValue.Value);

                        if (rowValue.HasValue && !row[j].IsClue)
                            row[j].RemoveCandidate(rowValue.Value);

                        if (boxValue.HasValue && !box[j].IsClue)
                            box[j].RemoveCandidate(boxValue.Value);
                    }
                }
            }
        }

        public Puzzle Clone()
        {
            var puzzleString = this.ToString();
            return Puzzle.Parse(puzzleString);
        }

        public static Puzzle Parse(string puzzleString)
        {
            if (puzzleString == null)
                throw new SudokuException("Cannot parse puzzle: value was null");

            var cells = puzzleString.Split(',');
            if (cells.Length != 81)
                throw new SudokuException("Cannot parse puzzle: invalid cell count");

            var puzzle = new Puzzle();
            Utils.Loop(81, i => puzzle.Cells[i] = Cell.Parse(cells[i]));

            if (!puzzle.AreCellCoordinatesValid())
                throw new SudokuException("Cannot parse puzzle: invalid cell coordinates");

            return puzzle;
        }

        public override string ToString()
        {
            var cellStrings = new List<string>();
            Utils.Loop(cellStrings, (i, list1) => Utils.Loop(9, cellStrings, (j, list) => list.Add(this.GetRow(i)[j].ToString())));
            return string.Join(",", cellStrings);
        }

        private bool AreCellCoordinatesValid()
        {
            return (
                Utils.LoopAnd(i => this.GetRow(i).Count == 9) &&
                Utils.LoopAnd(i => this.GetCol(i).Count == 9) &&
                Utils.LoopAnd(i => this.GetBox(i).Count == 9)
            );
        }
    }
}
