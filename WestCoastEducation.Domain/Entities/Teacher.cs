namespace WestcoastEducation.Domain.Entities;

public class Teacher : Person
{
    public string ExpertiseArea { get; set; } = "";
    public List<string> ResponsibleCourseIds { get; set; } = []; 
    public override string ToString() => $"Lärare: {FullName} - Område: {ExpertiseArea}";
}
