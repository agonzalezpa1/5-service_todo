public class SeedData
{
    public static void Init()
    {
        using (var context = new TodoContext())
        {
            // Look for existing content
            if (context.Todos.Any())
            {
                return;   // DB already filled
            }

            // Add Todos
            context.Todos.AddRange(
                new Todo
                {
                    Completed = true,
                    Task = "oui"
                },
                new Todo
                {
                    Completed = false,
                    Task = "non"
                },
                new Todo
                {
                    Completed = false,
                    Task = "peut-être",
                    Deadline = DateTime.Now // date d'aujourd'hui
                }
            );

            // Commit changes into DB
            context.SaveChanges();
        }
    }
}