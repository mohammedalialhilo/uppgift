using WestcoastEducation.Domain.Entities;
using WestcoastEducation.Domain.Interfaces;
using WestcoastEducation.Domain.Utils;

namespace WestcoastEducation.Domain.Services;

public class CourseService
{
    private readonly IRepository<Course> _courses;
    private readonly IRepository<Teacher> _teachers;

    public CourseService()
    {
        _courses  = new Persistence.JsonRepository<Course>(PathService.File("courses.json"));
        _teachers = new Persistence.JsonRepository<Teacher>(PathService.File("teachers.json"));
    }

    public Task<List<Course>> ListAsync() => _courses.GetAllAsync();

    public async Task<Course> AddAsync(Course c, string? teacherId = null) 
    {
        if (!string.IsNullOrWhiteSpace(teacherId))
        {
            var teacher = await _teachers.GetAsync(teacherId) ?? throw new InvalidOperationException("Lärare saknas");
            c.TeacherId = teacher.Id;
            if (!teacher.ResponsibleCourseIds.Contains(c.Id))
                teacher.ResponsibleCourseIds.Add(c.Id);
            await _teachers.UpdateAsync(teacher);
        }
        await _courses.AddAsync(c);
        return c;
    }

    public async Task AssignTeacherAsync(string courseId, string teacherId) 
    {
        var course  = await _courses.GetAsync(courseId) ?? throw new InvalidOperationException("Kurs saknas");
        var teacher = await _teachers.GetAsync(teacherId) ?? throw new InvalidOperationException("Lärare saknas");

        course.TeacherId = teacher.Id;
        if (!teacher.ResponsibleCourseIds.Contains(course.Id))
            teacher.ResponsibleCourseIds.Add(course.Id);

        await _courses.UpdateAsync(course);
        await _teachers.UpdateAsync(teacher);
    }

    public async Task<bool> AutoCancelIfLowEnrollmentAsync(string courseId, int minParticipants, int weeksBeforeStart)
    {
        var course = await _courses.GetAsync(courseId) ?? throw new InvalidOperationException("Kurs saknas");
        var studentsRepo = new Persistence.JsonRepository<Student>(PathService.File("students.json"));
        var allStudents = await studentsRepo.GetAllAsync();

        var enrolledCount = allStudents.Count(s => s.EnrolledCourseIds.Contains(courseId));
        var weeksUntilStart = (course.Schedule.StartDate - DateTime.UtcNow).TotalDays / 7.0;
        var thresholdWindow = weeksUntilStart <= weeksBeforeStart + 0.01;

        if (thresholdWindow && enrolledCount < minParticipants)
        {
            course.QnA.Add($"SYSTEM: Kursen bör ställas in ({enrolledCount}/{minParticipants}) {weeksBeforeStart} veckor före start.");
            await _courses.UpdateAsync(course);
            return true;
        }
        return false;
    }
}
