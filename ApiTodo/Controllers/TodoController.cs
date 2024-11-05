
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations; // Pour annotations sur Swagger UI

[ApiController]
[Route("api/todo")]
public class TodoController : ControllerBase
{
    private readonly TodoContext _context;
    public TodoController(TodoContext context)
    {
    _context = context;
    }

    // Annotations sur Swagger UI
    [SwaggerOperation
        (
            Summary = "Fetchs all the todos created"
        )
    ]
    [SwaggerResponse(StatusCodes.Status200OK, "Todo(s) found", typeof(Todo))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Todo list not found")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Todo list retrieval error")]

    // GET: api/todo
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
    {
        // Get items
        var todos = _context.Todos;

        return await todos.ToListAsync();
    }

    // Annotations sur Swagger UI
    [SwaggerOperation
        (
            Summary = "Get a todo by id",
            Description = "Returns a specific todo targeted by its identifier"
        )
    ]
    [SwaggerResponse(StatusCodes.Status200OK, "Todo found", typeof(Todo))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Todo not found")]

    // GET: api/todo/2
    [HttpGet("{id}")]
    public async Task<ActionResult<Todo>> GetItem([SwaggerParameter("The unique identifier of the item", Required = true)] int id)
    {
        // Find a specific item
        // SingleAsync() throws an exception if no item is found (which is possible, depending on id)
        // SingleOrDefaultAsync() is a safer choice here
        var todo = await _context.Todos.SingleOrDefaultAsync(t => t.Id == id);

        if (todo == null)
            return NotFound();

        return todo;
    }

    // Annotations sur Swagger UI
    [SwaggerOperation
        (
            Summary = "Creates a todo"
        )
    ]
    [SwaggerResponse(StatusCodes.Status200OK, "Todo created", typeof(Todo))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, "Todo creation error")]

    // POST: api/item
    [HttpPost]
    public async Task<ActionResult<Todo>> PostItem(Todo todo)
    {
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetItem), new { id = todo.Id }, todo);
    }

    // Annotations sur Swagger UI
    [SwaggerOperation
        (
            Summary = "Update a todo by id",
            Description = "Modifies a specific todo targeted by its identifier"
        )
    ]
    [SwaggerResponse(StatusCodes.Status200OK, "Todo updated", typeof(Todo))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Todo not found")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Untreatable request")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Empty todo list")]

    // PUT: api/item/2 => mise à jour d'un Todo
    [HttpPut("{id}")]
    public async Task<IActionResult> PutItem(int id, Todo todo)
    {
        if (id != todo.Id)
        {
            return BadRequest();
        }

        _context.Entry(todo).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Todos.Any(t => t.Id == id))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    // Annotations sur Swagger UI
    [SwaggerOperation
        (
            Summary = "Delete a todo by id",
            Description = "Deletes a specific todo targeted by its identifier"
        )
    ]
    [SwaggerResponse(StatusCodes.Status200OK, "Todo deleted", typeof(Todo))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Todo not found")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Empty todo list")]
    
    // DELETE: api/item/2
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        var todo = await _context.Todos.FindAsync(id);

        if (todo == null)
        {
            return NotFound();
        }

        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}