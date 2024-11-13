namespace CourseProject.Hierarchy;

public class Variable(bool value) : IBooleanExpression
{
    private bool Value { get; set; } = value;

    public bool Evaluate()
    {
        return Value;
    }
}