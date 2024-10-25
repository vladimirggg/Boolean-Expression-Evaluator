using System;

namespace CourseProject
{
    public class StringProperties(string str)
    {
        private string Str {get; set;} = str;
        // Function to split a string by a delimiter
        public string[] Split(char delimiter)
        {
            string[] parts = new string[CharCount(' ') + 1];
            int index = 0;

            //Iterate through the string and split it by the delimiter
            for (int i = 0; i < Str.Length; i++)
            {
                if(Str[i] == delimiter)
                {
                    index++;
                    continue;
                }
                parts[index] += Str[i];
            }

            return parts;
        }

        public int CharCount(char character)
        {
            int count = 0;
            foreach (char c in Str)
            {
                if (c == character)
                    count++;
            }

            return count;
        }

        // Function to get a substring from a string
        public string Substring(int startIndex, int length)
        {
            string result = "";
            for(int i = startIndex; i < length || i < Str.Length; i++)
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
            for (int i = 0; i < Str.Length; i++)
            {
                if (Str[i] == substr[0])
                {
                    bool found = true;
                    for (int j = 1; j < substr.Length; j++)
                    {
                        if (Str[i + j] != substr[j])
                        {
                            found = false;
                            break;
                        }
                    }

                    if (found)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public int FindFirstOccurrence(string substr)
        {
            for (int i = 0; i < Str.Length; i++)
            {
                if (Str[i] == substr[0])
                {
                    bool found = true;
                    for (int j = 1; j < substr.Length; j++)
                    {
                        if (Str[i + j] != substr[j])
                        {
                            found = false;
                            break;
                        }
                    }

                    if (found)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        // Function to convert a string to uppercase
        public string ToUpper()
        {
            char[] result = Str.ToCharArray();
            for (int i = 0; i < Str.Length; i++)
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
            char[] result = Str.ToCharArray();
            for (int i = 0; i < Str.Length; i++)
            {
                if (Str[i] >= 'A' && Str[i] <= 'A')
                {
                    result[i] = (char)(Str[i] + 32);
                }
            }

            return new string(result);
        }

        public override string ToString()
        {
            return Str;
        }
    }
}