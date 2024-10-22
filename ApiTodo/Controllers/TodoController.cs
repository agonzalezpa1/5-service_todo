
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("api/todo")]
public class TodoController : ControllerBase
{
    private readonly TodoContext _context;
    public TodoController(TodoContext context)
    {
    _context = context;
    }

    // GET: api/todo
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
    {
    // Get items
    var todos = _context.Todos;
    return await todos.ToListAsync();
    }

    // GET: api/todo/2
    [HttpGet("{id}")]
    public async Task<ActionResult<Todo>> GetItem(int id)
    {
        // Find a specific item
        // SingleAsync() throws an exception if no item is found (which is possible, depending on id)
        // SingleOrDefaultAsync() is a safer choice here
        var todo = await _context.Todos.SingleOrDefaultAsync(t => t.Id == id);

        if (todo == null)
            return NotFound();

        return todo;
    }

    // POST: api/item
    [HttpPost]
    public async Task<ActionResult<Todo>> PostItem(Todo todo)
    {
        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetItem), new { id = todo.Id }, todo);
    }

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

