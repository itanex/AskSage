using Microsoft.EntityFrameworkCore;
using AskSageMinimalApi.Models;

namespace AskSageMinimalApi.Databases
{
    public class AskSageDb : DbContext
    {
        public AskSageDb(DbContextOptions<AskSageDb> options) : base(options) { }

        public DbSet<Todo> Todos => Set<Todo>();
    }
}
