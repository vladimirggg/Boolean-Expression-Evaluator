using System;
using System.Collections;
using System.Net.NetworkInformation;
using CourseProject.HashTable;

namespace CourseProject
{
    public static class Handler
    {
        private static readonly ExpressionTable Table = [];

        private static void Define(string input)
        {
            var mutable = new StringProperties(input);
            var index = mutable.FindFirstOccurrence("(");
            if (index == -1)
                throw new ArgumentException("Invalid format. The expression must be formated as follows: _name(_parameters):\"_expression\".");

            var name = mutable.Substring(0, index);
            if (!AreAllParametersPresent(mutable))
                throw new ArgumentException("All parameters must be present in the expression.");

            else if (!AreAllParametersDeclared(mutable))
                throw new ArgumentException("All parameters must be declared in the expression.");

            var expression = mutable.Substring(mutable.FindLastOccurrence("\"") + 1, mutable.FindLastOccurrence("\"")).ToLower();
            Table.Add(name, expression);
        }
        
        private static bool AreAllParametersPresent(StringProperties input)
        {
            var parameters = input.Substring(input.FindFirstOccurrence("(") + 1, input.FindFirstOccurrence(")")).Split(',');
            var expression = input.Substring(input.FindFirstOccurrence("\"") + 1, input.FindLastOccurrence("\"")).Split(' ');

            foreach (var parameter in parameters)
            {
                if (!expression.Contains(parameter))
                    return false;
            }
            return true;
        }

        private static bool AreAllParametersDeclared(StringProperties input)
        {
            var parameters = input.Substring(input.FindFirstOccurrence("(") + 1, input.FindFirstOccurrence(")")).Split(',');
            var expression = input.Substring(input.FindLastOccurrence("\"") + 1, input.Length()).Split(' ');

            foreach (var item in expression)
            {
                if(item.All(char.IsLetter) && !parameters.Contains(item))
                    return false;
            }
            return true;
        }

        private static void Solve(string input)
        {
            // TODO Solve an expression
        }

        private static void All(string input)
        {
            // TODO Print table representing all possible values of the 
            // expression with all the posible values of the variables
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
            }
        }
    }
}