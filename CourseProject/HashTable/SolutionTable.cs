namespace CourseProject.HashTable;

public class SolutionTable
{
    private readonly MyDictionary<string, MyDictionary<List<bool>, bool>> _solutions = new MyDictionary<string, MyDictionary<List<bool>, bool>>();
    
    public bool Add(string name, List<bool> parameters, bool solution)
    {
        if (!_solutions.ContainsKey(name))
            _solutions.Add(name, new MyDictionary<List<bool>, bool>());
        if (!_solutions[name].ContainsKey(parameters))
            _solutions[name].Add(parameters, solution);
        else 
            return false; // The solution is already present in the table.
        
        return true; //Solution added successfully.
    }
    
    public bool Find(string name, List<bool> parameters)
    {
        if(IsPresent(name, parameters))
            return _solutions[name][parameters];
        throw new Exception("The solution is not present in the table.");
    }
    
    public bool IsPresent(string name, List<bool> parameters)
    {
        return _solutions.ContainsKey(name) && _solutions[name].ContainsKey(parameters);
    }
}