namespace CourseProject.Hierarchy
{
    public class Negation(IBooleanExpression expr) : IBooleanExpression
    {
        private IBooleanExpression Expr { get; set; } = expr;

        public bool Evaluate()
        {
            return !Expr.Evaluate();
        }
    }
}