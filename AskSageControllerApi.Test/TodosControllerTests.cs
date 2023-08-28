using AskSageControllerApi.Controllers;
using AskSageControllerApi.Models;
using AskSageControllerApi.Test.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace AskSageControllerApi.Test
{
    public class TodosControllerTests
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
            dbContext.SaveChanges();

            var subject = new TodosController(dbContext);

            // Act
            var actual = await subject.GetTodo(1);

            // Assert
            actual.Result.Should().BeNull();
            actual.Value.Should().BeSameAs(expected);
        }

        [Fact]
        public async void GetSpecificTodo_NotFound()
        {
            // Arrange
            var dbContext = new MockDb().CreateDbContext();

            var subject = new TodosController(dbContext);

            // Act
            var actual = await subject.GetTodo(1);

            // Assert
            actual.Result.Should()
                .BeOfType<NotFoundResult>();
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
            var subject = new TodosController(dbContext);

            // Act
            var actual = await subject.GetTodos();

            // Assert
            actual.Result.Should().BeNull();
            actual.Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async void PostTodo()
        {
            // Arrange
            var expected = new Todo()
            {
                Id = 1234,
                IsComplete = true,
                Name = "Test Task"
            };

            var dbContext = new MockDb().CreateDbContext();
            var subject = new TodosController(dbContext);

            // Act
            var actual = await subject.PostTodo(expected);

            // Assert
            actual.Result.Should().BeOfType<CreatedAtActionResult>()
                .Subject.Value.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async void PutTodo_NoId_BadRequest()
        {
            // Arrange
            var todo = new Todo()
            {
                IsComplete = true,
                Name = "Test Task"
            };

            var dbContext = new MockDb().CreateDbContext();
            var subject = new TodosController(dbContext);

            // Act
            var actual = await subject.PutTodo(1, todo);

            // Assert
            actual.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async void PutTodo_NotFound()
        {
            // Arrange
            var todo = new Todo()
            {
                Id = 1,
                IsComplete = true,
                Name = "Test Task"
            };

            var dbContext = new MockDb().CreateDbContext();
            var subject = new TodosController(dbContext);

            // Act
            var actual = await subject.PutTodo(1, todo);

            // Assert
            actual.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async void PutTodo_Success()
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
            var subject = new TodosController(dbContext);

            // Act
            var actual = await subject.PutTodo(1, todo);

            // Assert
            actual.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async void DeleteTodo_NotFound()
        {
            // Arrange
            var dbContext = new MockDb().CreateDbContext();
            var subject = new TodosController(dbContext);
            var todoId = 1;

            // Act
            var actual = await subject.DeleteTodo(todoId);

            // Assert
            actual.Should().BeOfType<NotFoundResult>();
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

            var subject = new TodosController(dbContext);

            // Act
            var actual = await subject.DeleteTodo(todo.Id);

            // Assert
            actual.Should().BeOfType<NoContentResult>();
        }
    }
}