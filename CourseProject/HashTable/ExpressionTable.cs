using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace CourseProject.HashTable;

class ExpressionTable : IDictionary<string, Node>
{
    private readonly IEqualityComparer<string> _equalityComparer;
    private readonly List<KeyValuePair<string, Node>>[] _items;
    private const int DefaultCapacity = 16;

    public ExpressionTable() : this(DefaultCapacity, EqualityComparer<string>.Default) { }

    public ExpressionTable(int capacity, IEqualityComparer<string> equalityComparer)
    {
        _equalityComparer = equalityComparer ?? throw new ArgumentNullException(nameof(equalityComparer));
        _items = new List<KeyValuePair<string, Node>>[capacity];
        for (var i = 0; i < capacity; i++)
        {
            _items[i] = [];
        }
        Keys = new List<string>();
        Values = new List<Node>();
    }

    public Node this[string key]
    {
        get
        {
            var hash = _equalityComparer.GetHashCode(key);
            hash = Math.Abs(hash);
            hash %= _items.Length;
            var list = _items[hash];

            if (list is null || list.Count == 0)
                throw new KeyNotFoundException();

            foreach (var item in list)
            {
                if (_equalityComparer.Equals(item.Key, key))
                    return item.Value;
            }

            throw new KeyNotFoundException();
        }
        set
        {
            var hash = _equalityComparer.GetHashCode(key);
            hash = Math.Abs(hash);
            hash %= _items.Length;
            var list = _items[hash];

            for (var i = 0; i < list.Count; i++)
            {
                if (!_equalityComparer.Equals(list[i].Key, key)) continue;
                list[i] = new KeyValuePair<string, Node>(key, value);
                return;
            }

            list.Add(new KeyValuePair<string, Node>(key, value));
            ((List<string>)Keys).Add(key);
            ((List<Node>)Values).Add(value);
            Count++;
        }
    }

    public ICollection<string> Keys { get; }

    public ICollection<Node> Values { get; }

    public int Count { get; private set; }

    public bool IsReadOnly => false;

    public void Add(string key, string expression)
    {
        this[key] = ConstructNode(expression);
    }

    public void Add(string key, Node value)
    {
        this[key] = value;
    }

    public void Add(KeyValuePair<string, Node> item)
    {
        this[item.Key] = item.Value;
    }

    public static bool IsOperand(char token)
    {
        return token is >= 'a' and <= 'z';
    }

    public static bool IsOperator(char token)
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

    private static List<char> ConvertToRPN(string expression)
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

    private static Node ConstructNode(string expression)
    {
        var rpn = ConvertToRPN(expression);
        var stack = new Stack<Node>();

        foreach (var token in rpn) 
        {
            if (IsOperand(token)) 
            {
                stack.Push(new Node(token));
            } 
            else if (IsOperator(token)) 
            {
                var right = stack.Pop();
                var left = stack.Pop();
                stack.Push(new Node(token, left, right));
            }
        }
        return stack.Pop();
    }

    public void Clear()
    {
        foreach (var list in _items)
        {
            list.Clear();
        }
        ((List<string>)Keys).Clear();
        ((List<Node>)Values).Clear();
        Count = 0;
    }

    public bool Contains(KeyValuePair<string, Node> item)
    {
        return TryGetValue(item.Key, out var value) && EqualityComparer<Node>.Default.Equals(value, item.Value);
    }

    public bool ContainsKey(string key)
    {
        return TryGetValue(key, out _);
    }

    public void CopyTo(KeyValuePair<string, Node>[] array, int arrayIndex)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        if (arrayIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        if (array.Length - arrayIndex < Count)
            throw new ArgumentException("The array is too small.");

        foreach (var list in _items)
        {
            foreach (var item in list)
            {
                array[arrayIndex++] = item;
            }
        }
    }

    public IEnumerator<KeyValuePair<string, Node>> GetEnumerator()
    {
        foreach (var list in _items)
        {
            foreach (var item in list)
            {
                yield return item;
            }
        }
    }

    public bool Remove(string key)
    {
        var hash = _equalityComparer.GetHashCode(key);
        hash = Math.Abs(hash);
        hash %= _items.Length;
        var list = _items[hash];

        for (var i = 0; i < list.Count; i++)
        {
            if (!_equalityComparer.Equals(list[i].Key, key)) continue;
            list.RemoveAt(i);
            ((List<string>)Keys).Remove(key);
            ((List<Node>)Values).Remove(list[i].Value);
            Count--;
            return true;
        }

        return false;
    }

    public bool Remove(KeyValuePair<string, Node> item)
    {
        return Remove(item.Key);
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out Node value)
    {
        var hash = _equalityComparer.GetHashCode(key);
        hash = Math.Abs(hash);
        hash %= _items.Length;
        var list = _items[hash];

        foreach (var item in list)
        {
            if (!_equalityComparer.Equals(item.Key, key)) continue;
            value = item.Value;
            return true;
        }

        value = default;
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

}