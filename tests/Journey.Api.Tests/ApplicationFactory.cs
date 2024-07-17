using Journey.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Time.Testing;
using System.Data.Common;

namespace Journey.Api.Tests;

public class ApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly DbConnection _connection = new SqliteConnection("Filename=:memory:");

    public FakeTimeProvider FakeTime { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _connection.Open();

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<JourneyDbContext>));
            services.RemoveAll(typeof(TimeProvider));

            services.AddDbContext<JourneyDbContext>(options =>
                options.UseSqlite(_connection));

            services.AddSingleton<TimeProvider>(FakeTime);
        });
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public new Task DisposeAsync()
    {
        _connection.Dispose();
        return Task.CompletedTask;
    }
}
