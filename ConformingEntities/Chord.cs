using ConformingEntities.Notes;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ConformingEntities;

public static class Chord
{
    public static TNote For<TNote>(string propertyName) where TNote : Note, new() =>
        new() { AppliesTo = p => p.Name == propertyName };

    public static TNote ForType<T, TNote>() where TNote : Note, new() =>
        new() { AppliesTo = p => p.DeclaringType?.ClrType == typeof(T) };

    public static TNote For<TNote>(Func<IConventionProperty, bool> match) where TNote : Note, new() =>
        new() { AppliesTo = match };

    public static IgnoreDefaultStringLengthNote IgnoreLengthFor(string propertyName) =>
        For<IgnoreDefaultStringLengthNote>(propertyName);

    public static DefaultDecimalPrecisionNote DecimalPrecisionFor(string propertyName, int precision, int scale) =>
        For<DefaultDecimalPrecisionNote>(propertyName) with { Precision = precision, Scale = scale };

    public static IgnoreDefaultDecimalPrecisionNote IgnoreDecimalPrecisionFor(string propertyName) =>
        For<IgnoreDefaultDecimalPrecisionNote>(propertyName);
}
