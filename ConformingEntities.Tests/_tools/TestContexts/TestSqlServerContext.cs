using Microsoft.EntityFrameworkCore;

namespace ConformingEntities.Tests._tools.TestContexts;

public class TestSqlServerContext<T> : DbContext where T : class
{
    private readonly SheetMusic sheetMusic;

    public DbSet<T> Items => Set<T>();

    public TestSqlServerContext(SheetMusic sheetMusic = null!)
    {
        this.sheetMusic = sheetMusic;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("DoesNotMatter"); // Required by EF, never actually used

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        Make.The(configurationBuilder).Conform(sheetMusic);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Make.The(modelBuilder).Conform();
    }
}

public class TestSqlServerContext<T1, T2> : DbContext where T1 : class where T2 : class
{
    public DbSet<T1> Items1 => Set<T1>();
    public DbSet<T2> Items2 => Set<T2>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("DoesNotMatter"); // Required by EF, never actually used
}