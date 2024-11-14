using System;
using System.Collections;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading.Channels;
using CourseProject.HashTable;
using CourseProject.Hierarchy;
using static System.Boolean;

namespace CourseProject;

public static class Handler
{
    private static readonly ExpressionTable Table = new ExpressionTable();

    private static void Define(string input)
    {
        var mutable = new StringProperties(input);
        mutable.ToLower();
        var index = mutable.FindFirstOccurrence("(");
        if (index == -1)
            throw new ArgumentException("Invalid format. The expression must be formated as follows: _name(_parameters):\"_expression\".");

        var name = mutable.Substring(0, index);
        if (!AreAllParametersPresent(mutable))
            throw new ArgumentException("All parameters must be present in the expression.");

        if (!AreAllParametersDeclared(mutable))
            throw new ArgumentException("All parameters must be declared in the expression.");

        var expression = mutable.Substring(mutable.FindFirstOccurrence("\"") + 1, mutable.FindLastOccurrence("\"")).ToLower();
        Table.Add(name, expression);
        Console.WriteLine($"Expression {name} has been added to the table.");
    }
        
    private static bool AreAllParametersPresent(StringProperties input)
    {
        var parameters = input.Substring(input.FindFirstOccurrence("(") + 1, input.FindFirstOccurrence(")")).Split(", ");
        var expression = new StringProperties(input.Substring(input.FindFirstOccurrence("\"") + 1, input.FindLastOccurrence("\"")));

        foreach (var parameter in parameters)
        {
            if (!expression.Contains(parameter))
                return false;
        }
        return true;
    }

    private static bool AreAllParametersDeclared(StringProperties input)
    {
        var parameters = input.Substring(input.FindFirstOccurrence("(") + 1, input.FindFirstOccurrence(")")).Split(", ");
        var expression = input.Substring(input.FindFirstOccurrence("\"") + 1, input.FindLastOccurrence("\""));

        foreach (var item in expression)
        {
            if(char.IsLetter(item) && !parameters.Contains($"{item}"))
                return false;
        }
        return true;
    }

    private static bool Solve(string input)
    {
        // TODO fix the tokenizing of the parameters
        var mutable = new StringProperties(input);
        var index = mutable.FindFirstOccurrence("(");
        
        if (index == -1)
            throw new ArgumentException("Invalid format. The expression must be formated as follows: _name(_parameters):\"_expression\".");
        var name = mutable.Substring(0, index);
        
        var paramStr = mutable.Substring(index + 1, mutable.FindLastOccurrence(")")).Split(", ");
        var parameters = new List<bool>(paramStr.Length);
        
        for (var i = 0; i < paramStr.Length; i++)
        {
            TryParse(paramStr[i], out var result);
            parameters[i] = result;
        }

        var expression = Table[name];
        
        if (VariableCount(expression) != parameters.Count)
            throw new ArgumentException("Parameters are not matching the signature!");

        PopulateVariables(expression, parameters);
        return expression.Evaluate();
    }

    private static int VariableCount(IBooleanExpression expr)
    {
        while (true)
        {
            switch (expr)
            {
                case Variable:
                    return 1;
                case IBinaryOperation binaryOperation:
                    return VariableCount(binaryOperation.Left) + VariableCount(binaryOperation.Right);
                case IUnaryOperation unaryOperation:
                    return VariableCount(unaryOperation.Expression);
            }
            return 0;
        }
    }

    private static void PopulateVariables(IBooleanExpression expr, List<bool> parameters)
    {
        switch(expr)
        {
            case Variable variable:
                variable.Value = parameters[0];
                parameters.RemoveAt(0);
                return;
            case IBinaryOperation binaryOperation:
                PopulateVariables(binaryOperation.Left, parameters);
                PopulateVariables(binaryOperation.Right, parameters);
                return;
            case IUnaryOperation unaryOperation:
                PopulateVariables(unaryOperation.Expression, parameters);
                return;
        }
    }

    private static string Join(string[] arr, string separator)
    {
        var result = "";
        for (var i = 0; i < arr.Length; i++)
        {
            result += arr[i];
            if (i != arr.Length - 1)
                result += separator;
        }
        return result;
    }
    
    private static string Join(List<bool> arr, string separator)
    {
        var result = "";
        for (var i = 0; i < arr.Count; i++)
        {
            result += arr[i] ? "1" : "0";
            if (i != arr.Count - 1)
                result += separator;
        }
        return result;
    }
    
    private static void All(string input)
    {
        //TODO Fix the binary operation because I get index out of range exception
        var expression = Table[input];
        var paramCount = VariableCount(expression);
        var variables = new string[paramCount];
        
        for (var i = 0; i < paramCount; i++)
        {
            variables[i] = $"{(char)('a' + i)}"; 
        }

        Console.WriteLine(Join(variables, " , ") + $" : {input}");
        for (var mask = 0; mask < Math.Pow(2, paramCount); mask++)
        {
            var parameters = new List<bool>(paramCount);
            for (var j = 0; j < paramCount; j++)
            {
                parameters.Add((mask & (1 << j)) != 0);
            }
            var paramStr = Join(parameters, " , ");
            PopulateVariables(expression, parameters);
            Console.WriteLine(paramStr + $" : {expression.Evaluate()}");
        }
    }

    private static void Find(string input)
    {
        // TODO Find the expression that matches the result of the table
    }

    private static void HandleRequest(string input)
    {
        var mutableString = new StringProperties(input);
        var action = new StringProperties(mutableString.Split(' ')[0]);
        switch (action.ToUpper())
        {
            case "DEFINE":
                Define(mutableString.Substring(action.Length() + 1, mutableString.Length()));
                break;
            case "SOLVE":
                Solve(mutableString.Substring(action.Length() + 1, mutableString.Length()));
                break;
            case "ALL":
                All(mutableString.Substring(action.Length() + 1, mutableString.Length()));
                break;
            case "FIND":
                Find(mutableString.Substring(action.Length() + 1, mutableString.Length())); // Not sure
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }

    }

    public static void Main()
    {
        Console.WriteLine("Welcome to the Boolean Expression Evaluator!\n Please enter an option or type: \"exit\" to stop the program:");
        var input = Console.ReadLine()!;
        while (input != "exit")
        {
            HandleRequest(input);
            input = Console.ReadLine()!;
        }
    }
}