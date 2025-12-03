namespace Sudoku.Analysis;

public class ConstraintLog(ConstraintType type)
{
    public ConstraintType Constraint { get; set; } = type;

    public List<Action> Actions { get; set; } = [];

    public ConstraintLog(ConstraintType type, List<Action> actions) : this(type)
    {
        Actions = actions;
    }

    public ConstraintLog(ConstraintType type, Cell cell, int value) : this(type)
    {
        switch (type)
        {
            case ConstraintType.NakedSingle:
            case ConstraintType.HiddenSingle:
                Actions.Add(Action.SetValue(cell, value));
                break;
            default:
                Actions.Add(Action.RemoveCandidate(cell, value));
                break;
        }
    }

    public override string ToString() => $"{Constraint}\n  {string.Join("\n  ", Actions)}";
}
