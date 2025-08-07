using QuickPulse.Explains.Text;

namespace ConformingEntities.Tests._tools.Helpers;

public static class LinesReaderExtensions
{
    public static string SkipToLineContaining(this LinesReader reader, string fragment)
    {
        while (true)
        {
            var line = reader.NextLine();
            if (line.Contains(fragment, StringComparison.OrdinalIgnoreCase))
                return line;
        }
    }
}