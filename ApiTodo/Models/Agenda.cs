public class Agenda
{
    public int Id {get; set;}
    public string? Name {get; set;}
    // Collection navigation containing dependents
    public ICollection<Todo> TodoList { get; } = new List<Todo>() {};
}