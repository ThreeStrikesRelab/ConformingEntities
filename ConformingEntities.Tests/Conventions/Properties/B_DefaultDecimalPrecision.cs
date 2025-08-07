using ConformingEntities.Tests._tools.Helpers;
using ConformingEntities.Tests._tools.TestContexts;
using Microsoft.EntityFrameworkCore;
using QuickPulse.Explains;
using QuickPulse.Explains.Text;

namespace ConformingEntities.Tests.Conventions.Properties;

[DocFile]
public class B_DefaultDecimalPrecision
{
    public class Thing
    {
        public int Id { get; set; }
        public decimal Property { get; set; } = default!;
        public decimal? NullableProperty { get; set; } = default!;
    }

    public class ConformingDbContext : TestSqlServerContext<Thing> { }

    [Fact]
    [DocContent("When no precision and scale are specified, it defaults to (18,2).  ")]
    public void Default_is_18_2()
    {
        using var context = new ConformingDbContext();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Equal("    [Property] decimal(18,2) NOT NULL,", reader.SkipToLineContaining("Property"));
    }

    [Fact]
    [DocContent("This applies to both `decimal` and `decimal?` properties.  ")]
    public void Default_is_18_2_also_for_nullables()
    {
        using var context = new ConformingDbContext();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Equal("    [NullableProperty] decimal(18,2) NULL,", reader.SkipToLineContaining("NullableProperty"));
    }

    public class LessConformingDbContext : TestSqlServerContext<Thing>
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Thing>().Property(n => n.Property).HasPrecision(20, 3);
        }
    }

    [Fact]
    [DocContent("Whenever your regular EF Core Entity mapping specifies `HasPrecision`, this takes precedence.  ")]
    public void Default_not_set_when_defined_explicitely()
    {
        using var context = new LessConformingDbContext();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Equal("    [Property] decimal(20,3) NOT NULL,", reader.SkipToLineContaining("Property"));
        Assert.Equal("    [NullableProperty] decimal(18,2) NULL,", reader.SkipToLineContaining("NullableProperty"));
    }

    public class CustomConformingDbContext : TestSqlServerContext<Thing>
    {
        public CustomConformingDbContext()
            : base(SheetMusic.Compose() with { DefaultDecimalPrecision = 16, DefaultDecimalScale = 4 }) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

    [Fact]
    [DocContent("You can pass in a modified version of `SheetMusic` to influence the default, of course.  ")]
    [DocCode(
@"Make.The(configurationBuilder)
    .Conform(SheetMusic.Compose() with { DefaultDecimalPrecision = 16, DefaultDecimalScale = 4 });")]
    public void Default_override()
    {
        using var context = new CustomConformingDbContext();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Equal("    [Property] decimal(16,4) NOT NULL,", reader.SkipToLineContaining("Property"));
        Assert.Equal("    [NullableProperty] decimal(16,4) NULL,", reader.SkipToLineContaining("NullableProperty"));
    }

    private static readonly SheetMusic ignoreConformingSheet =
        (SheetMusic.Compose() with { DefaultDecimalPrecision = 16, DefaultDecimalScale = 4 })
                .Chart(Chord.IgnoreDecimalPrecisionFor("Property"));

    public class IgnoreConformingDbContext : TestSqlServerContext<Thing>
    {
        public IgnoreConformingDbContext() : base(ignoreConformingSheet) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

    [Fact]
    [DocContent("Sheet Music can also be used to opt out of default decimal precision completely.  ")]
    [DocCode(
@"
var score = 
    SheetMusic.Compose()
        .Chart(Chord.IgnoreDecimalPrecisionFor(""Property""));
Make.The(configurationBuilder).Conform(score);")]
    public void Default_opt_out()
    {
        using var context = new IgnoreConformingDbContext();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Equal("    [Property] decimal(18,2) NOT NULL,", reader.SkipToLineContaining("Property"));
        Assert.Equal("    [NullableProperty] decimal(16,4) NULL,", reader.SkipToLineContaining("NullableProperty"));
    }
}