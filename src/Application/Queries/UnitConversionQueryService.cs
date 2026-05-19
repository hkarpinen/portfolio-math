using Math.Application.Dtos;
using Math.Domain.Engines;

namespace Math.Application.Queries;

/// <summary>
/// Application service that delegates to the static domain <see cref="UnitConversionEngine"/>
/// and maps results to DTOs for downstream consumers.
/// </summary>
internal sealed class UnitConversionQueryService : IUnitConversionQuery
{
    public ConversionResultDto Convert(string from, string to, decimal value)
    {
        var (outputValue, category) = UnitConversionEngine.Convert(from, to, value);
        return new ConversionResultDto(
            From: from.ToLowerInvariant(),
            To: to.ToLowerInvariant(),
            InputValue: value,
            OutputValue: outputValue,
            Category: category);
    }

    public IReadOnlyList<UnitCategoryDto> ListUnits() =>
        UnitConversionEngine.ListUnits()
            .Select(c => new UnitCategoryDto(c.Category, c.Units))
            .ToList();
}
