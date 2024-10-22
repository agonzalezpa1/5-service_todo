public class Todo
{
    public int Id {get; set;}
    public string? Task {get; set;}
    public bool Completed {get; set;}
    public DateTime? Deadline {get; set;}
    public int? AgendaID { get; set; } // Required foreign key property
    public Agenda Agenda { get; set; } = null!; // Required reference navigation to principal
}