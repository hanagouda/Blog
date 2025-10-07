using BlogAPI.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;

namespace BlogIntegrationTests
{
    public class CustomWebAppFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(services =>
            {
                // Remove real DbContext.
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BlogDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                // Add SQLite InMemory (shared connection).
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                services.AddDbContext<BlogDbContext>(options =>
                {
                    options.UseSqlite(connection);
                });

                // Build provider and ensure DB created.
                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<BlogDbContext>();
                db.Database.EnsureCreated();
            });
        }
    }
}
