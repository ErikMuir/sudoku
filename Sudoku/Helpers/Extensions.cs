using System;
using System.Linq;
using Sudoku.Logic;

namespace Sudoku
{
    public static class Extensions
    {
        public static bool Between(this int num, int lower, int upper, bool inclusive = false)
            => inclusive ? lower <= num && num <= upper : lower < num && num < upper;

        public static bool Between(this int? num, int lower, int upper, bool inclusive = false)
            => inclusive ? lower <= num && num <= upper : lower < num && num < upper;

        private static bool IsUnit(this Cell[] unit)
            => (unit is not null && unit.Length == 9);

        public static bool IsUnitSolved(this Cell[] unit)
            => (unit.IsUnit() && unit.All(x => x.Value.HasValue) && unit.Select(x => x.Value).Distinct().Count() == 9);

        public static bool IsUnitValid(this Cell[] unit)
            => (
                unit.IsUnit() &&
                unit.Where(x => x.Value.HasValue)
                    .Select(x => x.Value)
                    .Count() == unit.Where(x => x.Value.HasValue)
                                    .Select(x => x.Value)
                                    .Distinct()
                                    .Count()
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

        public static string ToUtcString(this DateTime dt)
            => dt.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

        public static bool LoopAnd(this int count, Func<int, bool> func)
        {
            for (int i = 0; i < count; i++)
            {
                if (!func?.Invoke(i) ?? false) return false;
            }
            return true;
        }

        public static bool LoopOr(this int count, Func<int, bool> func)
        {
            for (int i = 0; i < count; i++)
            {
                if (func?.Invoke(i) ?? false) return true;
            }
            return false;
        }

        public static int GetRowIndex(this int cellIndex)
            => cellIndex / Puzzle.UnitSize;

        public static int GetColIndex(this int cellIndex)
            => cellIndex % Puzzle.UnitSize;
    }
}
