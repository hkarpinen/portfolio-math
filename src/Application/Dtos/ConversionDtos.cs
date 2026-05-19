namespace Math.Application.Dtos;

public sealed record ConversionResultDto(
    string From,
    string To,
    decimal InputValue,
    decimal OutputValue,
    string Category);

public sealed record UnitCategoryDto(
    string Category,
    IReadOnlyList<string> Units);
