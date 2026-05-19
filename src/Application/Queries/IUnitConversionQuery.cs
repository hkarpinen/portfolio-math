using Math.Application.Dtos;

namespace Math.Application.Queries;

/// <summary>
/// Application-layer query interface. Returns DTOs suitable for serialisation.
/// Implementations map from domain models produced by <see cref="Math.Domain.Services.IUnitConverter"/>.
/// </summary>
public interface IUnitConversionQuery
{
    /// <exception cref="ArgumentException">Thrown when either unit symbol is unknown.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the two units belong to different categories.</exception>
    ConversionResultDto Convert(string from, string to, decimal value);

    IReadOnlyList<UnitCategoryDto> ListUnits();
}
