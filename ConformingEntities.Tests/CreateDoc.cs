using QuickPulse.Explains;

namespace ConformingEntities.Tests;

[DocFile]
[DocFileHeader("Conforming Entities")]
[DocContent(
@"> Making persistence behave. Nicely.

This library provides a lightweight, extensible convention system for Entity Framework Core.  
It allows you to apply default configuration behaviors globally across your domain model without repeating
the same fluent API calls across every mapping.")]
[DocCode(
@"public class AppDbContext : DbContext
{
    public DbSet<MyEntity> Entities => Set<MyEntity>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        Make.The(configurationBuilder).Conform();
    }
}
")]
public class CreateDoc
{
    [Fact]
    [DocContent(
@"You define a single configuration object and use it to tweak your `ModelConfigurationBuilder`.
The system applies conventions such as default string lengths automatically, for instance,
while allowing you to override or opt out per property as needed.

**Note:** This is very much an early experiment, with lots of conformal ideas brewing.")]
    public void Now()
    {
        Explain.This<CreateDoc>("README.md");
    }
}
