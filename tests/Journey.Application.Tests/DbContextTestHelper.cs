using Journey.Infrastructure;
using Journey.TestHelpers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Journey.Application.Tests;

public class DbContextTestHelper : IDisposable
{
    private readonly DbConnection _connection;
    private readonly DbContextOptions<JourneyDbContext> _contextOptions;

    public DbContextTestHelper()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<JourneyDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new JourneyDbContext(_contextOptions);

        context.Database.EnsureCreated();
        context.AddRange(TestDataHelper.Trips);
        context.SaveChanges();
    }

    public JourneyDbContext CreateContext()
    {
        return new(_contextOptions);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        _connection.Dispose();
    }
}
