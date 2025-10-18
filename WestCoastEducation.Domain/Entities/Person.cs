using WestcoastEducation.Domain.Interfaces;
using WestcoastEducation.Domain.ValueObjects;

namespace WestcoastEducation.Domain.Entities;

public abstract class Person : IIdentifiable, IContactable
{
    public string Id { get; init; } = Guid.NewGuid().ToString("N")[..8];
    public string FirstName { get; set; } = "";
    public string LastName  { get; set; } = "";
    public string Phone     { get; set; } = "";
    public string Email     { get; set; } = "";
    public string PersonalNumber { get; set; } = "";
    public Address Address { get; set; } = new("", "", "");

    public string FullName => $"{FirstName} {LastName}";
    public override string ToString() => $"{FullName} ({Email}, {Phone})";
}
