namespace CourseProject.Hierarchy;

public class Variable(bool? value) : IBooleanExpression
{
    public bool? Value { get; set; } = value;

    public bool Evaluate()
    {
        if (Value is null) throw new ArgumentException("The variable has not been set!");
        return (bool)Value;
    }
}