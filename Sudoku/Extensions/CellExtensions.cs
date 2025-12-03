namespace Sudoku.Extensions;

public static class CellExtensions
{
    private static bool IsUnit(this IEnumerable<Cell> unit) => unit.Count() == Puzzle.UnitSize;

    private static int ValueCount(this IEnumerable<Cell> unit) => unit.Count(x => x.Value.HasValue);

    private static int DistinctValueCount(this IEnumerable<Cell> unit)
        => unit
            .Where(x => x.Value.HasValue)
            .Select(x => x.Value!.Value)
            .Distinct()
            .Count();


    public static bool IsUnitSolved(this IEnumerable<Cell> unit)
        => (
            unit.IsUnit() &&
            unit.All(x => x.Value.HasValue) &&
            unit.DistinctValueCount() == Puzzle.UnitSize
        );

    public static bool IsUnitValid(this IEnumerable<Cell> unit)
        => (
            unit.IsUnit() &&
            unit.ValueCount() == unit.DistinctValueCount()
        );

    public static bool IsCandidateUnique(this IEnumerable<Cell> cells, int candidate)
        => cells.Where(x => x.Candidates.Contains(candidate)).Count() == 1;

    public static bool ContainsEveryCandidate(this IEnumerable<Cell> cells, CandidateSet set)
        => set.All(candidate => cells.Any(cell => cell.Candidates.Contains(candidate)));

    public static bool AllInSameCol(this IEnumerable<Cell> cells)
        => cells.Select(x => x.Col).Distinct().Count() == 1;

    public static bool AllInSameRow(this IEnumerable<Cell> cells)
        => cells.Select(x => x.Row).Distinct().Count() == 1;

    public static bool AllInSameBox(this IEnumerable<Cell> cells)
        => cells.Select(x => x.Box).Distinct().Count() == 1;

    public static IEnumerable<Cell> GetCandidateMatches(this IEnumerable<Cell> cells, int candidate)
        => cells.Where(x => x.Candidates.Contains(candidate));

    public static IEnumerable<Cell> ClueCells(this IEnumerable<Cell> cells)
        => cells.Where(x => x.Type == CellType.Clue);

    public static IEnumerable<Cell> NonClueCells(this IEnumerable<Cell> cells)
        => cells.Where(x => x.Type != CellType.Clue);

    public static IEnumerable<Cell> FilledCells(this IEnumerable<Cell> cells)
        => cells.Where(x => x.Type == CellType.Filled);

    public static IEnumerable<Cell> NonFilledCells(this IEnumerable<Cell> cells)
        => cells.Where(x => x.Type != CellType.Filled);

    public static IEnumerable<Cell> EmptyCells(this IEnumerable<Cell> cells)
        => cells.Where(x => x.Type == CellType.Empty);

    public static IEnumerable<Cell> NonEmptyCells(this IEnumerable<Cell> cells)
        => cells.Where(x => x.Type != CellType.Empty);
}
