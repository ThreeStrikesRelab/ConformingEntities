using ConformingEntities.Tests._tools.Helpers;
using ConformingEntities.Tests._tools.TestContexts;
using Microsoft.EntityFrameworkCore;
using QuickPulse.Explains;
using QuickPulse.Explains.Text;

namespace ConformingEntities.Tests.Conventions.Properties;

[DocFile]
public class A_DefaultStringLength
{
    public class Thing
    {
        public int Id { get; set; }
        public string Property { get; set; } = default!;
        public string? NullableProperty { get; set; } = default!;
    }

    public class ConformingDbContext : TestSqlServerContext<Thing> { }

    [Fact]
    [DocContent("When no string length is specified, it defaults to 255.  ")]
    public void Default_is_255()
    {
        using var context = new ConformingDbContext();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Equal("    [Property] nvarchar(255) NOT NULL,", reader.SkipToLineContaining("Property"));
    }

    [Fact]
    [DocContent("This applies to both `string` and `string?` properties.  ")]
    public void Default_is_255_also_for_nullables()
    {
        using var context = new ConformingDbContext();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Equal("    [NullableProperty] nvarchar(255) NULL,", reader.SkipToLineContaining("NullableProperty"));
    }

    public class LessConformingDbContext : TestSqlServerContext<Thing>
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Thing>().Property(n => n.Property).HasMaxLength(100);
        }
    }

    [Fact]
    [DocContent("Whenever your regular EF Core Entity mapping specifies `HasMaxLength`, this takes precedence.  ")]
    public void Default_not_set_when_defined_explicitely()
    {
        using var context = new LessConformingDbContext();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Equal("    [Property] nvarchar(100) NOT NULL,", reader.SkipToLineContaining("Property"));
        Assert.Equal("    [NullableProperty] nvarchar(255) NULL,", reader.SkipToLineContaining("NullableProperty"));
    }

    public class CustomConformingDbContext : TestSqlServerContext<Thing>
    {
        public CustomConformingDbContext() : base(SheetMusic.Compose() with { DefaultStringLength = 42 }) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

    [Fact]
    [DocContent("You can pass in a modified version of `SheetMusic` to influence the default, of course.  ")]
    [DocCode(
@"Make.The(configurationBuilder)
    .Conform(SheetMusic.Compose() with { DefaultStringLength = 42 });")]
    public void Default_override()
    {
        using var context = new CustomConformingDbContext();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Equal("    [Property] nvarchar(42) NOT NULL,", reader.SkipToLineContaining("Property"));
        Assert.Equal("    [NullableProperty] nvarchar(42) NULL,", reader.SkipToLineContaining("NullableProperty"));
    }

    public class IgnoreConformingDbContext : TestSqlServerContext<Thing>
    {
        public IgnoreConformingDbContext() : base(
            SheetMusic.Compose()
                .Chart(Chord.IgnoreLengthFor("Property")))
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

    [Fact]
    [DocContent("Sheet Music can also be used to opt out of default string length completely.  ")]
    [DocCode(
@"var score = 
    SheetMusic.Compose()
        .Chart(Chord.IgnoreLengthFor(""Property""));
Make.The(configurationBuilder).Conform(score);")]
    public void Default_opt_out()
    {
        using var context = new IgnoreConformingDbContext();
        var sql = context.Database.GenerateCreateScript();
        var reader = LinesReader.FromText(sql);
        Assert.Equal("    [Property] nvarchar(max) NOT NULL,", reader.SkipToLineContaining("Property"));
        Assert.Equal("    [NullableProperty] nvarchar(255) NULL,", reader.SkipToLineContaining("NullableProperty"));
    }
}