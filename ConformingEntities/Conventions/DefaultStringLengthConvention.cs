using ConformingEntities.Notes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ConformingEntities;

public class DefaultStringLengthConvention : IPropertyAddedConvention
{
    private readonly SheetMusic sheet;

    public DefaultStringLengthConvention(SheetMusic sheet)
    {
        this.sheet = sheet;
    }

    public void ProcessPropertyAdded(IConventionPropertyBuilder propertyBuilder, IConventionContext<IConventionPropertyBuilder> context)
    {
        var property = propertyBuilder.Metadata;

        if (property.ClrType != typeof(string)) return;
        if (property.GetMaxLength() is not null) return;
        var notes = sheet.Notes.OfType<IgnoreDefaultStringLengthNote>();
        foreach (var note in notes)
        {
            if (note.AppliesTo(property))
                return;
        }

        propertyBuilder.HasMaxLength(sheet.DefaultStringLength);
    }
}
