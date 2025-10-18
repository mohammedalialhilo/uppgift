namespace WestcoastEducation.Domain.Utils;

public static class PathService
{
    public static string DomainDataPath()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null && dir.Name != "WestcoastEducation.Domain")
            dir = dir.Parent;
        // Om vi kör från App: gå upp tills solution-roten och in i Domain/Data
        dir ??= new DirectoryInfo(AppContext.BaseDirectory)
            .Parent?.Parent?.Parent?.Parent; // fallback
        var candidate = Path.Combine(dir!.FullName, "WestcoastEducation.Domain", "Data");
        if (Directory.Exists(candidate)) return candidate;

        // Alternativ: relativ från App/bin/Debug/net8.0
        var alt = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WestcoastEducation.Domain", "Data"));
        return alt;
    }

    public static string File(string filename) => Path.Combine(DomainDataPath(), filename);
}
