namespace CourseProject.Hierarchy;

public interface IBinaryOperation: IBooleanExpression
{
    public IBooleanExpression Left { get;  set; }
    public IBooleanExpression Right { get;  set; }
}