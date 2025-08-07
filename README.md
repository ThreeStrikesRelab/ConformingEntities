# Conforming Entities
> Making persistence behave. Nicely.

This library provides a lightweight, extensible convention system for Entity Framework Core.  
It allows you to apply default configuration behaviors globally across your domain model without repeating
the same fluent API calls across every mapping.
```csharp
public class AppDbContext : DbContext
{
    public DbSet<MyEntity> Entities => Set<MyEntity>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        Make.The(configurationBuilder).Conform();
    }
}
```
You define a single configuration object and use it to tweak your `ModelConfigurationBuilder`.
The system applies conventions such as default string lengths automatically, for instance,
while allowing you to override or opt out per property as needed.

**Note:** This is very much an early experiment, with lots of conformal ideas brewing.
## Conventions

### Default String Length
When no string length is specified, it defaults to 255.  
This applies to both `string` and `string?` properties.  
Whenever your regular EF Core Entity mapping specifies `HasMaxLength`, this takes precedence.  
You can pass in a modified version of `SheetMusic` to influence the default, of course.  
```csharp
Make.The(configurationBuilder)
    .Conform(SheetMusic.Compose() with { DefaultStringLength = 42 });
```
Sheet Music can also be used to opt out of default string length completely.  
```csharp
var score = 
    SheetMusic.Compose()
        .Chart(Chord.IgnoreLengthFor("Property"));
Make.The(configurationBuilder).Conform(score);
```
### Default Decimal Precision
When no precision and scale are specified, it defaults to (18,2).  
This applies to both `decimal` and `decimal?` properties.  
Whenever your regular EF Core Entity mapping specifies `HasPrecision`, this takes precedence.  
You can pass in a modified version of `SheetMusic` to influence the default, of course.  
```csharp
Make.The(configurationBuilder)
    .Conform(SheetMusic.Compose() with { DefaultDecimalPrecision = 16, DefaultDecimalScale = 4 });
```
Sheet Music can also be used to opt out of default decimal precision completely.  
```csharp
var score = 
    SheetMusic.Compose()
        .Chart(Chord.IgnoreDecimalPrecisionFor("Property"));
Make.The(configurationBuilder).Conform(score);
```
