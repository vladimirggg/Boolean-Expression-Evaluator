namespace CourseProject.Hierarchy;

public class Negation(IBooleanExpression expr) : IUnaryOperation
{
    public IBooleanExpression Expression { get; set; } = expr;

    public bool Evaluate()
    {
        return !Expression.Evaluate();
    }
}