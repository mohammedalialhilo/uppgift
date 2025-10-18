namespace WestcoastEducation.Domain.ValueObjects;


public record Schedule(
    DateTime StartDate,
    DateTime EndDate,
    int LengthWeeks
);
