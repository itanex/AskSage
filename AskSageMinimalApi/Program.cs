using AskSageMinimalApi.Databases;
using AskSageMinimalApi.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AskSageDb>(options =>
{
    options.UseInMemoryDatabase("TodoList");
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var api = app.MapGroup("/api");

TodoEndpoints.MapEndpoints(api);

app.Run();
