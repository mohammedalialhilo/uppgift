namespace WestcoastEducation.Domain.Entities;

public class OnlineCourse : Course
{
    public bool IsSubscriptionEligible { get; set; } = true;
    public List<string> PreviewChapters { get; set; } = new();
    public override string DeliveryType => "On-Demand";
}
