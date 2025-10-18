using System.Text.Json;
using System.Text.Json.Serialization;

namespace WestcoastEducation.Domain.Persistence;

internal static class JsonDb
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };

    public static async Task<T?> LoadAsync<T>(string path)
    {
        if (!File.Exists(path)) return default;
        await using var fs = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync<T>(fs, Options);
    }

    public static async Task SaveAsync<T>(string path, T data)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        await using var fs = File.Create(path);
        await JsonSerializer.SerializeAsync(fs, data, Options);
    }
}
