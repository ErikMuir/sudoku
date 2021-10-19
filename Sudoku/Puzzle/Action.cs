namespace Sudoku
{
    public class Action
    {
        public Action(ActionType type, Cell cell, int value)
        {
            ActionType = type;
            Cell = cell;
            Value = value;
        }

        ActionType ActionType { get; set; }
        Cell Cell { get; set; }
        int Value { get; set; }

        public static Action SetValue(Cell cell, int value) => new(ActionType.SetValue, cell, value);
        public static Action ClearValue(Cell cell) => new(ActionType.ClearValue, cell, 0);
        public static Action SetCandidate(Cell cell, int value) => new(ActionType.SetCandidate, cell, value);
        public static Action RemoveCandidate(Cell cell, int value) => new(ActionType.RemoveCandidate, cell, value);

        public override string ToString() => $"{ActionType} {Value} - [{Cell.Col}, {Cell.Row}]";
    }
}
