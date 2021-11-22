using System;
using System.Linq;
using Sudoku.Logic;

namespace Sudoku.Extensions
{
    public static class Extensions
    {
        private static bool IsUnit(this Cell[] unit)
            => (unit is not null && unit.Length == Puzzle.UnitSize);

        private static int ValueCount(this Cell[] unit)
            => unit.Count(x => x.Value is not null);

        private static int DistinctValueCount(this Cell[] unit)
            => unit
                .Where(x => x.Value is not null)
                .Select(x => x.Value)
                .Distinct()
                .Count();


        public static bool IsUnitSolved(this Cell[] unit)
            => (
                unit.IsUnit() &&
                unit.All(x => x.Value is not null) &&
                unit.DistinctValueCount() == Puzzle.UnitSize
            );

        public static bool IsUnitValid(this Cell[] unit)
            => (
                unit.IsUnit() &&
                unit.ValueCount() == unit.DistinctValueCount()
            );

        public static bool IsCandidateUnique(this Cell[] cells, int candidate)
            => cells.Where(x => x.Candidates.Contains(candidate)).Count() == 1;

        public static bool ContainsEveryCandidate(this Cell[] cells, CandidateSet set)
            => set.All(candidate => cells.Any(cell => cell.Candidates.Contains(candidate)));

        public static bool AllInSameCol(this Cell[] cells)
            => cells.Select(x => x.Col).Distinct().Count() == 1;

        public static bool AllInSameRow(this Cell[] cells)
            => cells.Select(x => x.Row).Distinct().Count() == 1;

        public static bool AllInSameBox(this Cell[] cells)
            => cells.Select(x => x.Box).Distinct().Count() == 1;

        public static Cell[] GetCandidateMatches(this Cell[] cells, int candidate)
            => cells.Where(x => x.Candidates.Contains(candidate)).ToArray();

        public static bool LoopAnd(this int count, Func<int, bool> func)
        {
            for (int i = 0; i < count; i++)
                if (!func?.Invoke(i) ?? false)
                    return false;
            return true;
        }

        public static bool LoopOr(this int count, Func<int, bool> func)
        {
            for (int i = 0; i < count; i++)
                if (func?.Invoke(i) ?? false)
                    return true;
            return false;
        }

        public static int GetRowIndex(this int cellIndex)
            => cellIndex / Puzzle.UnitSize;

        public static int GetColIndex(this int cellIndex)
            => cellIndex % Puzzle.UnitSize;
    }
}
