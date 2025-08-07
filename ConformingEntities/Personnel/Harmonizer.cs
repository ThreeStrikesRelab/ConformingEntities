using Microsoft.EntityFrameworkCore;

namespace ConformingEntities;

public class Harmonizer
{
    private readonly ModelBuilder modelBuilder;

    public Harmonizer(ModelBuilder modelBuilder)
    {
        this.modelBuilder = modelBuilder;
    }

    public void Conform()
    {

    }
}
