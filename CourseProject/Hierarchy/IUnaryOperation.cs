namespace CourseProject.Hierarchy;

public interface IUnaryOperation : IBooleanExpression
{
    public IBooleanExpression Expression { get; protected internal set; }
}