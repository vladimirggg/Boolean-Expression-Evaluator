using CourseProject.Hierarchy;

namespace CourseProject.HashTable;

public class ExpressionTable
{
    private readonly MyDictionary<string, IBooleanExpression> _expressions = new MyDictionary<string, IBooleanExpression>();
    // private MyDictionary<string, bool> _exprValues = new MyDictionary<string, bool>();

    public IBooleanExpression this[string key] => _expressions[key];

    private static bool IsOperand(char token)
    {
        return token is >= 'a' and <= 'z';
    }

    private static bool IsOperator(char token)
    {
        return token is '&' or '|' or '!';
    }

    private static int GetPrecedence(char v)
    {
        return v switch
        {
            '!' => 3,
            '&' => 2,
            '|' => 1,
            _ => 0
        };
    }

    private static List<char> ConvertToRpn(string expression)
    {
        var output = new List<char>();
        var operators = new Stack<char>();

        foreach (var token in expression) {
            if (token == ' ') 
            {
                continue;
            }
            if (IsOperand(token)) 
            {
                output.Add(token);
            } 
            else if (IsOperator(token)) {
                while (operators.Count > 0 && IsOperator(operators.Peek()) && GetPrecedence(operators.Peek()) >= GetPrecedence(token)) {
                    output.Add(operators.Pop());
                }
                operators.Push(token);
            } 
            else if (token == '(') 
            {
                operators.Push(token);
            } 
            else if (token == ')') 
            {
                while (operators.Peek() != '(') {
                    output.Add(operators.Pop());
                }
                operators.Pop(); // Remove the "(" from the stack
            }
        }

        while (operators.Count > 0) {
            output.Add(operators.Pop());
        }

        return output;
    }

    private static IBooleanExpression ConstructNode(string expression)
    {
        var rpn = ConvertToRpn(expression);
        var stack = new Stack<IBooleanExpression>();

        foreach (var token in rpn) 
        {
            if (IsOperand(token)) 
            {
                stack.Push(new Variable(null));
            } 
            else if (IsOperator(token)) 
            {
                var right = stack.Pop();
                if (token == '!')
                {
                    stack.Push(new Negation(right));
                }
                else
                {
                    var left = stack.Pop();
                    switch (token)
                    {
                        case '&':
                            stack.Push(new Conjunction(left,right));
                            break;
                        case '|':
                            stack.Push(new Disjunction(left,right));
                            break;
                    }
                }
            }
        }
        return stack.Pop();
    }

    public void Add(string exprName, string expr)
    {
        _expressions[exprName] = ConstructNode(expr);
    }
}