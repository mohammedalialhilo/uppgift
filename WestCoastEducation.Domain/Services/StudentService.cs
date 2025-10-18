using WestcoastEducation.Domain.Entities;
using WestcoastEducation.Domain.Interfaces;
using WestcoastEducation.Domain.Utils;

namespace WestcoastEducation.Domain.Services;

public class StudentService
{
    private readonly IRepository<Student> _students;

    public StudentService()
    {
        _students = new Persistence.JsonRepository<Student>(PathService.File("students.json"));
    }

    public Task<List<Student>> ListAsync() => _students.GetAllAsync();

    public async Task<Student> RegisterAsync(Student s)
    {
        await _students.AddAsync(s);
        return s;
    }

    public async Task EnrollAsync(string studentId, string courseId)
    {
        var s = await _students.GetAsync(studentId) ?? throw new InvalidOperationException("Student saknas");
        if (!s.EnrolledCourseIds.Contains(courseId))
        {
            s.EnrolledCourseIds.Add(courseId);
            await _students.UpdateAsync(s);
        }
    }
}
