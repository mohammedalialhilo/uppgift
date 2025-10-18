namespace WestcoastEducation.Domain.Entities;

public class ProgramCoordinator : Teacher
{
    public DateTime EmploymentDate { get; set; } = DateTime.Now;
}
