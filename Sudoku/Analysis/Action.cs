namespace Sudoku.Analysis;

public class Action(ActionType type, Cell cell, int value)
{
    ActionType ActionType { get; set; } = type;
    Cell Cell { get; set; } = cell;
    int Value { get; set; } = value;

    public static Action SetValue(Cell cell, int value) => new(ActionType.SetValue, cell, value);
    public static Action ClearValue(Cell cell) => new(ActionType.ClearValue, cell, 0);
    public static Action SetCandidate(Cell cell, int value) => new(ActionType.SetCandidate, cell, value);
    public static Action RemoveCandidate(Cell cell, int value) => new(ActionType.RemoveCandidate, cell, value);

    public override string ToString() => $"{ActionType} {Value} - [{Cell.Col}, {Cell.Row}]";
}
