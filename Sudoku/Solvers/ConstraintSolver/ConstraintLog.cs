using System.Collections.Generic;

namespace Sudoku.Solvers
{
    public class ConstraintLog
    {
        public ConstraintLog(ConstraintType type)
        {
            Constraint = type;
        }

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
                    Actions.Add(new Action(ActionType.SetValue, cell, value));
                    break;
                default:
                    Actions.Add(new Action(ActionType.RemoveCandidate, cell, value));
                    break;
            }
        }

        public ConstraintType Constraint { get; set; }
        public List<Action> Actions { get; set; } = new List<Action>();

        public override string ToString() => $"{Constraint}\n  {string.Join("\n  ", Actions)}";
    }
}
