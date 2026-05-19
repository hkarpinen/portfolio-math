namespace Math.Domain.ValueObjects;

/// <summary>
/// Represents a unit symbol used in conversions (e.g. "kg", "lb", "c").
/// Normalised to lowercase on creation.
/// </summary>
public readonly record struct ConversionUnit(string Symbol)
{
    public static ConversionUnit Create(string symbol) => new(symbol.Trim().ToLowerInvariant());

    public override string ToString() => Symbol;
}
