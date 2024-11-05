public class SeedData
{
    public static void Init()
    {
        using (var context = new TodoContext())
        {
            if (context.Todos.Any())
            {
                return;   // DB already filled
            }

            Todo todo1 = new Todo
            {
                Task = "oui",
                State = Todo.States.Completed
            };
            Todo todo2 = new Todo
            {
                Task = "non"
            };
            Todo todo3 = new Todo
            {
                Task = "peut-être",
                State = Todo.States.On_going,
                Deadline = DateTime.Now // date d'aujourd'hui
            };

            // Add Todos
            context.Todos.AddRange(
                todo1,
                todo2,
                todo3
            );

            if (context.Agendas.Any())
            {
                return;   // DB already filled
            }

            // Add Todos
            context.Agendas.AddRange(
                new Agenda
                {
                    Name = "Chores",
                    TodoList = { todo1, todo2}
                },
                new Agenda
                {
                    Name = "Holidays",
                }
            );

            // Commit changes into DB
            context.SaveChanges();
        }
    }
}