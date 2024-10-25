namespace CourseProject.HashTable
{
    public class Node(char data, Node? left = null, Node? right = null)
    {
        public char Data { get; set; } = data;
        public Node? Left { get; set; } = left;
        public Node? Right { get; set; } = right;

    }
}