using System.Linq.Expressions;
using WestcoastEducation.Domain.Interfaces;

namespace WestcoastEducation.Domain.Persistence;

public class JsonRepository<T> : IRepository<T> where T : class
{
    private readonly string _path;
    public JsonRepository(string path) => _path = path;

    private async Task<List<T>> LoadAllAsync()
        => await JsonDb.LoadAsync<List<T>>(_path) ?? [];

    private async Task SaveAllAsync(List<T> items)
        => await JsonDb.SaveAsync(_path, items);

    public async Task<List<T>> GetAllAsync() => await LoadAllAsync();

    public async Task<T?> GetAsync(string id) 
    {
        var all = await LoadAllAsync();
        var prop = typeof(T).GetProperty("Id");
        return all.FirstOrDefault(e => (string)(prop!.GetValue(e)!) == id);
    }

    public async Task AddAsync(T entity)
    {
        var all = await LoadAllAsync();
        all.Add(entity);
        await SaveAllAsync(all);
    }

    public async Task UpdateAsync(T entity)
    {
        var all = await LoadAllAsync();
        var prop = typeof(T).GetProperty("Id");
        var id = (string)(prop!.GetValue(entity) ?? "");
        var idx = all.FindIndex(e => (string)(prop.GetValue(e)!) == id);
        if (idx >= 0) all[idx] = entity;
        await SaveAllAsync(all);
    }

    public async Task DeleteAsync(string id)
    {
        var all = await LoadAllAsync();
        var prop = typeof(T).GetProperty("Id");
        all.RemoveAll(e => (string)(prop!.GetValue(e)!) == id);
        await SaveAllAsync(all);
    }

    public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        var all = await LoadAllAsync();
        return all.AsQueryable().Where(predicate).ToList();
    }
}
