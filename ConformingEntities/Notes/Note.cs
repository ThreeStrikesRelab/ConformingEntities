using Microsoft.EntityFrameworkCore.Metadata;

namespace ConformingEntities.Notes;

public record Note
{
    public Func<IConventionProperty, bool> AppliesTo { get; init; } = _ => false;
}
