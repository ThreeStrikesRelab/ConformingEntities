using ConformingEntities.Notes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ConformingEntities;

public class DefaultDecimalPrecisionConvention : IPropertyAddedConvention
{
    private readonly SheetMusic sheet;

    public DefaultDecimalPrecisionConvention(SheetMusic sheet)
    {
        this.sheet = sheet;
    }

    public void ProcessPropertyAdded(IConventionPropertyBuilder propertyBuilder, IConventionContext<IConventionPropertyBuilder> context)
    {
        var property = propertyBuilder.Metadata;

        if (property.ClrType != typeof(decimal) && property.ClrType != typeof(decimal?)) return;
        if (property.GetPrecision() != null || property.GetScale() != null) return; // (18,2) is the default for EF 
        var ignoreNotes = sheet.Notes.OfType<IgnoreDefaultDecimalPrecisionNote>();
        foreach (var note in ignoreNotes)
        {
            if (note.AppliesTo(property))
            {
                return;
            }
        }
        var notes = sheet.Notes.OfType<DefaultDecimalPrecisionNote>();
        foreach (var note in notes)
        {
            if (note.AppliesTo(property))
            {
                property.SetPrecision(note.Precision);
                property.SetScale(note.Scale);
                return;
            }
        }
        property.SetPrecision(sheet.DefaultDecimalPrecision);
        property.SetScale(sheet.DefaultDecimalScale);
    }
}
