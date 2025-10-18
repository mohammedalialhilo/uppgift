namespace WestcoastEducation.Domain.Entities;

public class Student : Person
{
    public List<string> EnrolledCourseIds { get; set; } = [];
    public override string ToString() => $"Student: {FullName} - Kurser: {EnrolledCourseIds.Count}";
}
