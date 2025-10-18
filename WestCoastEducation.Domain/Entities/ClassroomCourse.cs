namespace WestcoastEducation.Domain.Entities;

public class ClassroomCourse : Course
{
    public string Location { get; set; } = "";
    public bool IsRemoteTeacherLed { get; set; } 
    public override string DeliveryType => IsRemoteTeacherLed ? "Distans (lärarledd)" : "Klassrum";
}
