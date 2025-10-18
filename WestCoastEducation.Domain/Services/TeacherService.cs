using WestcoastEducation.Domain.Entities;
using WestcoastEducation.Domain.Interfaces;
using WestcoastEducation.Domain.Utils;

namespace WestcoastEducation.Domain.Services;

public class TeacherService
{
    private readonly IRepository<Teacher> _teachers;

    public TeacherService()
    {
        _teachers = new Persistence.JsonRepository<Teacher>(PathService.File("teachers.json"));
    }

    public Task<List<Teacher>> ListAsync() => _teachers.GetAllAsync();

    public Task<List<Teacher>> FindByAreaAsync(string area)
        => _teachers.FindAsync(t => t.ExpertiseArea.ToLower().Contains(area.ToLower()));

    public async Task<Teacher> AddAsync(Teacher t)
    {
        await _teachers.AddAsync(t);
        return t;
    }
}
