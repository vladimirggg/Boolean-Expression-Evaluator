namespace CourseProject.Hierarchy;

public class Disjunction(IBooleanExpression left, IBooleanExpression right) : IBooleanExpression
{
    private IBooleanExpression Left { get; set; } = left;
    private IBooleanExpression Right { get; set; } = right;

    public bool Evaluate()
    {
        return Left.Evaluate() || Right.Evaluate();
    }
}