using Microsoft.EntityFrameworkCore;

namespace ConformingEntities;

public static class Make
{
    public static Harmonizer The(ModelBuilder modelBuilder) => new(modelBuilder);

    public static Tuner The(ModelConfigurationBuilder conventionBuilder) => new(conventionBuilder);
}
