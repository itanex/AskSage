using AskSageControllerApi.Test.Helpers;
using AskSageMinimalApi.Endpoints;
using AskSageMinimalApi.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AskSageMinimalApi.Test
{
    public class TodosEndpointsTests
    {
        [Fact]
        public async void GetSpecificTodo_Ok()
        {
            // Arrange
            var expected = new Todo()
            {
                Id = 1,
                IsComplete = true,
                Name = "Test Task"
            };

            var dbContext = new MockDb().CreateDbContext();
            dbContext.Todos.Add(expected);

            // Act
            var actual = await TodoEndpoints.GetTodo(1, dbContext);

            // Assert
            actual.Should().BeOfType<Ok<Todo>>();
            actual.As<Ok<Todo>>().Value.Should().Be(expected);
        }

        [Fact]
        public async void GetSpecificTodo_NotFound()
        {
            // Arrange
            var dbContext = new MockDb().CreateDbContext();

            // Act
            var actual = await TodoEndpoints.GetTodo(1, dbContext);

            // Assert
            actual.Should().BeOfType<NotFound>();
        }

        [Fact]
        public async void GetAllTodos()
        {
            // Arrange
            var expected = new List<Todo>() {
                new Todo()
                {
                    Id = 1,
                    IsComplete = true,
                    Name = "Test Task 01"
                },
                new Todo()
                {
                    Id = 2,
                    IsComplete = false,
                    Name = "Test Task 02"
                }
            };

            var dbContext = new MockDb().CreateDbContext();
            dbContext.Todos.AddRange(expected);
            dbContext.SaveChanges();

            // Act
            var actual = await TodoEndpoints.GetAllTodos(dbContext);

            // Assert
            actual.Should().BeOfType<Ok<Todo[]>>();
            actual.As<Ok<Todo[]>>().Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async void CreateTodo()
        {
            // Arrange
            var expected = new Todo()
            {
                Id = 1234,
                IsComplete = true,
                Name = "Test Task"
            };

            var dbContext = new MockDb().CreateDbContext();

            // Act
            var actual = await TodoEndpoints.CreateTodo(expected, dbContext);

            // Assert
            actual.Should().BeOfType<Created<Todo>>()
                .Subject.Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async void UpdateTodo_NoId_BadRequest()
        {
            // Arrange
            var todo = new Todo()
            {
                IsComplete = true,
                Name = "Test Task"
            };

            var dbContext = new MockDb().CreateDbContext();

            // Act
            var actual = await TodoEndpoints.UpdateTodo(1, todo, dbContext);

            // Assert
            actual.Should().BeOfType<BadRequest>();
        }

        [Fact]
        public async void UpdateTodo_NotFound()
        {
            // Arrange
            var todo = new Todo()
            {
                Id = 1,
                IsComplete = true,
                Name = "Test Task"
            };

            var dbContext = new MockDb().CreateDbContext();

            // Act
            var actual = await TodoEndpoints.UpdateTodo(1, todo, dbContext);

            // Assert
            actual.Should().BeOfType<NotFound>();
        }

        [Fact]
        public async void UpdateTodo_Success()
        {
            // Arrange
            var todo = new Todo()
            {
                Id = 1,
                IsComplete = false,
                Name = "Test Task"
            };

            var dbContext = new MockDb().CreateDbContext();
            dbContext.Add(todo);

            // Act
            var actual = await TodoEndpoints.UpdateTodo(1, todo, dbContext);

            // Assert
            actual.Should().BeOfType<NoContent>();
        }

        [Fact]
        public async void DeleteTodo_NotFound()
        {
            // Arrange
            var dbContext = new MockDb().CreateDbContext();
            var todoId = 1;

            // Act
            var actual = await TodoEndpoints.DeleteTodo(todoId, dbContext);

            // Assert
            actual.Should().BeOfType<NotFound>();
        }

        [Fact]
        public async void DeleteTodo_Success()
        {
            // Arrange
            var todo = new Todo()
            {
                Id = 1,
                IsComplete = false,
                Name = "Test Task"
            };

            var dbContext = new MockDb().CreateDbContext();
            dbContext.Add(todo);
            dbContext.SaveChanges();

            // Act
            var actual = await TodoEndpoints.DeleteTodo(todo.Id, dbContext);

            // Assert
            actual.Should().BeOfType<NoContent>();
        }
    }
}