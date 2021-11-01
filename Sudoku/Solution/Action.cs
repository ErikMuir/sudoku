namespace Sudoku.Solution
{
    public class Action
    {
        public Action(ActionType type, int cellIndex, int value)
        {
            ActionType = type;
            CellIndex = cellIndex;
            Value = value;
        }

        ActionType ActionType { get; set; }
        int CellIndex { get; set; }
        int Value { get; set; }

        public static Action SetValue(int cellIndex, int value) => new(ActionType.SetValue, cellIndex, value);
        public static Action ClearValue(int cellIndex) => new(ActionType.ClearValue, cellIndex, 0);
        public static Action SetCandidate(int cellIndex, int value) => new(ActionType.SetCandidate, cellIndex, value);
        public static Action RemoveCandidate(int cellIndex, int value) => new(ActionType.RemoveCandidate, cellIndex, value);

        public override string ToString() => $"{ActionType} {Value} - [{CellIndex}]";
    }
}
