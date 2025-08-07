namespace ConformingEntities.Notes;

public record DefaultDecimalPrecisionNote : Note
{
    public int Precision { get; init; } = 18;
    public int Scale { get; init; } = 18;
}
