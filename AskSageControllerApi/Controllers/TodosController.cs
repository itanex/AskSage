using AskSageControllerApi.Databases;
using AskSageControllerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AskSageControllerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly AskSageDb context;

        public TodosController(AskSageDb context)
        {
            this.context = context;
        }

        // GET: api/Todos
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            //var result =  Ok(await context.Todos.ToListAsync());
            return await context.Todos.ToListAsync();
        }

        // GET: api/Todos/5
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Todo>> GetTodo(int id)
        {
            var todo = await context.Todos.FindAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            return todo;
        }

        // POST: api/Todos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult<Todo>> PostTodo(Todo todo)
        {
            context.Todos.Add(todo);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, todo);
        }

        // PUT: api/Todos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> PutTodo(int id, Todo todo)
        {
            if (id != todo.Id)
            {
                return BadRequest();
            }

            context.Entry(todo).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Todos/5
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            //if (context.Todos == null)
            //{
            //    return NotFound();
            //}

            var todo = await context.Todos.FindAsync(id);

            if (todo == null)
            {
                return NotFound();
            }

            context.Todos.Remove(todo);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoExists(int id)
        {
            return (context.Todos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
