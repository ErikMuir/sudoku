namespace Sudoku.Solvers
{
    public enum ConstraintType
    {
        NakedSingle,
        HiddenSingle,
        NakedDouble,
        HiddenDouble,
        NakedTriple,
        HiddenTriple,
        NakedQuadruple,
        HiddenQuadruple,
        PointingSet,
        BoxLineReduction,
        XWing,
        YWing,
    }
}
