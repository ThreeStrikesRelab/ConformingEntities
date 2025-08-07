using ConformingEntities.Notes;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ConformingEntities;

public record SheetMusic
{
    public int DefaultStringLength { get; init; } = 255;

    public int DefaultDecimalPrecision { get; init; } = 18;
    public int DefaultDecimalScale { get; init; } = 2;

    public IReadOnlyList<Note> Notes { get; init; } = [];

    public static SheetMusic Compose() => new();

    public IPropertyAddedConvention ToStringLengthConvention() =>
        new DefaultStringLengthConvention(this);

    public IPropertyAddedConvention ToDefaultDecimalPrecisionConvention() =>
        new DefaultDecimalPrecisionConvention(this);

    public SheetMusic Chart(params Note[] notes) =>
        this with { Notes = [.. Notes, .. notes] };
}