using AskSageControllerApi.Databases;
using Microsoft.EntityFrameworkCore;

namespace AskSageControllerApi.Test.Helpers
{
    public class MockDb : IDbContextFactory<AskSageDb>
    {
        public AskSageDb CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<AskSageDb>()
                .UseInMemoryDatabase($"InMemoryTestDb-{DateTime.Now.ToFileTimeUtc()}")
                .Options;

            return new AskSageDb(options);
        }
    }
}
