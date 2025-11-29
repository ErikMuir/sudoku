namespace Sudoku.Console;

public static class AnalyzePuzzle
{
    private static readonly FluentConsole _console = new();

    public static void Run(Puzzle puzzle)
    {
        Analyzer analyzer = new Analyzer(puzzle);
        _statistics(analyzer);
    }

    private static void _statistics(Analyzer analyzer)
    {
        _console
            .LineFeed()
            .Info($"Level: {analyzer.Level}")
            .Info($"Solve Duration (ms): {analyzer.SolveDuration.Milliseconds}")
            .Info($"Solve Depth: {analyzer.SolveDepth}")
            .Info("Constraint Actions:");

        analyzer.Logs
            .Select(x => x.Constraint)
            .Distinct()
            .OrderBy(x => x)
            .ToList()
            .ForEach(constraint =>
            {
                int actionCount = analyzer.Logs
                    .Where(x => x.Constraint == constraint)
                    .Select(x => x.Actions.Count())
                    .Aggregate((result, item) => result + item);
                _console.Info($"  {constraint}: {actionCount}");
            });
    }
}
