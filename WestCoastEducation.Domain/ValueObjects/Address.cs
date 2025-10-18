namespace WestcoastEducation.Domain.ValueObjects;

public record Address(
    string Street,
    string PostalCode,
    string City
);
