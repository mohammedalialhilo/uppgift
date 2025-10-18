using System.Text.Json.Serialization;
using WestcoastEducation.Domain.Interfaces;
using WestcoastEducation.Domain.ValueObjects;

namespace WestcoastEducation.Domain.Entities;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(ClassroomCourse), "classroom")]
[JsonDerivedType(typeof(OnlineCourse), "onDemand")]
public abstract class Course : IIdentifiable
{
    public string Id { get; init; } = Guid.NewGuid().ToString("N")[..8];
    public string CourseNumber { get; set; } = "";
    public string Title { get; set; } = "";
    public int LengthDays { get; set; }
    public Schedule Schedule { get; set; } = new(DateTime.UtcNow, DateTime.UtcNow, 0);

    public string? TeacherId { get; set; }     
    public string? CoordinatorId { get; set; }   

    public List<string> QnA { get; set; } = [];
    public List<int> Ratings { get; set; } = [];
    public double AverageRating => Ratings.Count == 0 ? 0 : Ratings.Average();

    public abstract string DeliveryType { get; }

    public override string ToString()
        => $"{Title} ({CourseNumber}) - {DeliveryType} - Längd: {LengthDays} dagar - Snittbetyg: {AverageRating:F1}";
}
