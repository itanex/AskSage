using Microsoft.EntityFrameworkCore;
using AskSageControllerApi.Models;

namespace AskSageControllerApi.Databases
{
    public class AskSageDb : DbContext
    {
        public AskSageDb(DbContextOptions<AskSageDb> options) : base(options) { }

        public DbSet<Todo> Todos => Set<Todo>();

    }
}
