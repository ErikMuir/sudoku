using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku
{
    public static class Extensions
    {
        public static bool Between(this int num, int lower, int upper, bool inclusive = false)
            => inclusive ? lower <= num && num <= upper : lower < num && num < upper;

        public static bool Between(this int? num, int lower, int upper, bool inclusive = false)
            => inclusive ? lower <= num && num <= upper : lower < num && num < upper;

        private static bool IsUnit(this List<Cell> unit)
            => (unit != null && unit.Count == 9);

        public static bool IsUnitSolved(this List<Cell> unit)
            => (unit.IsUnit() && unit.All(x => x.Value.HasValue) && unit.Select(x => x.Value).Distinct().Count() == 9);

        public static bool IsUnitValid(this List<Cell> unit)
            => (
                unit.IsUnit() &&
                unit.Where(x => x.Value.HasValue)
                    .Select(x => x.Value)
                    .Count() == unit.Where(x => x.Value.HasValue)
                                    .Select(x => x.Value)
                                    .Distinct()
                                    .Count()
            );

        public static bool IsCandidateUnique(this List<Cell> cells, int candidate)
            => cells.Where(x => x.Candidates.Contains(candidate)).Count() == 1;

        public static List<Cell> CloneCells(this List<Cell> cells)
            => cells.Select(x => x.Clone()).ToList();

        public static bool ContainsEveryCandidate(this List<Cell> cells, CandidateSet set)
            => set.All(candidate => cells.Any(cell => cell.Candidates.Contains(candidate)));

        public static bool AllInSameCol(this List<Cell> cells)
            => cells.Select(x => x.Col).Distinct().Count() == 1;

        public static bool AllInSameRow(this List<Cell> cells)
            => cells.Select(x => x.Row).Distinct().Count() == 1;

        public static bool AllInSameBox(this List<Cell> cells)
            => cells.Select(x => x.Box).Distinct().Count() == 1;

        public static List<Cell> GetCandidateMatches(this List<Cell> cells, int candidate)
            => cells.Where(x => x.Candidates.Contains(candidate)).ToList();

        public static string ToUtcString(this DateTime dt)
            => dt.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
    }
}
