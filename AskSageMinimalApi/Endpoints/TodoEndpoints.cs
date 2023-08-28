using AskSageMinimalApi.Databases;
using AskSageMinimalApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace AskSageMinimalApi.Endpoints
{
    public static class TodoEndpoints
    {
        public static void MapEndpoints(RouteGroupBuilder api)
        {
            var todos = api.MapGroup("/todos");

            todos.MapGet("/", GetAllTodos)
                .Produces((int)HttpStatusCode.OK)
                .WithOpenApi();
            todos.MapGet("/{id}", GetTodo)
                .Produces((int)HttpStatusCode.OK)
                .Produces((int)HttpStatusCode.NotFound)
                .WithOpenApi();
            todos.MapPost("/", CreateTodo)
                .Produces((int)HttpStatusCode.Created)
                .WithOpenApi();
            todos.MapPut("/{id}", UpdateTodo)
                .Produces((int)HttpStatusCode.NoContent)
                .Produces((int)HttpStatusCode.NotFound)
                .WithOpenApi();
            todos.MapDelete("/{id}", DeleteTodo)
                .Produces((int)HttpStatusCode.NoContent)
                .Produces((int)HttpStatusCode.NotFound)
                .WithOpenApi();
        }

        public static async Task<IResult> GetAllTodos(AskSageDb db)
        {
            return TypedResults
                .Ok(await db.Todos.ToArrayAsync());
        }

        public static async Task<IResult> GetTodo(int id, AskSageDb db)
        {
            return await db.Todos.FindAsync(id) is Todo todo
                    ? TypedResults.Ok(todo)
                    : TypedResults.NotFound();
        }

        public static async Task<IResult> CreateTodo(Todo todo, AskSageDb db)
        {
            db.Todos.Add(todo);
            await db.SaveChangesAsync();

            return TypedResults.Created($"/todoitems/{todo.Id}", todo);
        }

        public static async Task<IResult> UpdateTodo(int id, Todo inputTodo, AskSageDb db)
        {
            if (id != inputTodo.Id)
            {
                return TypedResults.BadRequest();
            }

            var todo = await db.Todos.FindAsync(id);

            if (todo is null)
            {
                return TypedResults.NotFound();
            }

            todo.Name = inputTodo.Name;
            todo.IsComplete = inputTodo.IsComplete;

            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        public static async Task<IResult> DeleteTodo(int id, AskSageDb db)
        {
            if (await db.Todos.FindAsync(id) is Todo todo)
            {
                db.Todos.Remove(todo);
                await db.SaveChangesAsync();

                //return TypedResults.Ok(todo);
                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }
    }
}
