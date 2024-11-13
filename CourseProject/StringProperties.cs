namespace CourseProject
{
    public class StringProperties(string str)
    {
        private string Str {get; set;} = str;
        // Function to split a string by a delimiter
        public string[] Split(char delimiter)
        {
            var parts = new string[CharCount(' ') + 1];
            var index = 0;

            //Iterate through the string and split it by the delimiter
            foreach (var t in Str)
            {
                if(t == delimiter)
                {
                    index++;
                    continue;
                }
                parts[index] += t;
            }

            return parts;
        }

        private int CharCount(char character)
        {
            var count = 0;
            foreach (var c in Str)
            {
                if (c == character)
                    count++;
            }

            return count;
        }

        // Function to get a substring from a string
        public string Substring(int startIndex, int length)
        {
            var result = "";
            for(var i = startIndex; i < length || i < Str.Length; i++)
            {
                result += Str[i];
            }
            
            return result;
        }

        public int Length()
        {
            return Str.Length;
        }   

        // Function to check if a string contains a substring
        public bool Contains(string substr)
        {
            for (var i = 0; i < Str.Length; i++)
            {
                if (Str[i] != substr[0]) continue;
                var found = true;
                for (var j = 1; j < substr.Length; j++)
                {
                    if (Str[i + j] == substr[j]) continue;
                    found = false;
                    break;
                }

                if (found)
                {
                    return true;
                }
            }

            return false;
        }

        public int FindFirstOccurrence(string substr)
        {
            for (var i = 0; i < Str.Length; i++)
            {
                if (Str[i] != substr[0]) continue;
                var found = true;
                for (var j = 1; j < substr.Length; j++)
                {
                    if (Str[i + j] == substr[j]) continue;
                    found = false;
                    break;
                }

                if (found)
                {
                    return i;
                }
            }

            return -1;
        }

        // Function to convert a string to uppercase
        public string ToUpper()
        {
            var result = Str.ToCharArray();
            for (var i = 0; i < Str.Length; i++)
            {
                if (Str[i] >= 'a' && Str[i] <= 'z')
                {
                    result[i] = (char)(Str[i] - 32);
                }
            }

            return new string(result);
        }

        // Function to convert a string to lowercase
        public string ToLower()
        {
            var result = Str.ToCharArray();
            for (var i = 0; i < Str.Length; i++)
            {
                if (Str[i] >= 'A' && Str[i] <= 'A')
                {
                    result[i] = (char)(Str[i] + 32);
                }
            }

            return new string(result);
        }

        public void Trim()
        {
            var start = 0;
            var end = Str.Length - 1;
            while (Str[start] == ' ')
            {
                start++;
            }

            while (Str[end] == ' ')
            {
                end--;
            }

            Str = Substring(start, end - start + 1);
        }

        public override string ToString()
        {
            return Str;
        }

        internal int FindLastOccurrence(string v)
        {
            for (var i = Str.Length - v.Length; i >= 0; i--)
            {
                if (Str[i] != v[0]) continue;
                var found = true;
                for (var j = 1; j < v.Length; j++)
                {
                    if (Str[i + j] == v[j]) continue;
                    found = false;
                    break;
                }

                if (found) return i;
            }
            return -1;
        }
    }
}