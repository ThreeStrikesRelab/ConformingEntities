using Microsoft.EntityFrameworkCore;

namespace ConformingEntities;

public class Tuner
{
    private readonly ModelConfigurationBuilder conventionBuilder;

    public Tuner(ModelConfigurationBuilder conventionBuilder)
    {
        this.conventionBuilder = conventionBuilder;
    }

    public void Conform(SheetMusic maybeSheetMusic = null!)
    {
        var sheetMusic = maybeSheetMusic ?? new SheetMusic();
        conventionBuilder.Conventions.Add(_ => sheetMusic.ToStringLengthConvention());
        conventionBuilder.Conventions.Add(_ => sheetMusic.ToDefaultDecimalPrecisionConvention());
    }
}