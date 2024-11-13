namespace CourseProject.Hierarchy;

public class Disjunction(IBooleanExpression left, IBooleanExpression right) : IBinaryOperation
{

    public IBooleanExpression Left { get; set; } = left;
    public IBooleanExpression Right { get; set; } = right;
    
    public bool Evaluate()
    {
        return Left.Evaluate() || Right.Evaluate();
    }
}