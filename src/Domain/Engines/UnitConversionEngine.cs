namespace Math.Domain.Engines;

/// <summary>
/// Pure computation engine for unit conversion.
/// All values are normalised to a base unit before converting to the target,
/// except temperature which requires offset arithmetic.
/// Change driver: supported units, base factors, temperature logic.
/// </summary>
public static class UnitConversionEngine
{
    // symbol → (category, factor relative to base unit)
    // Temperature entries use a dummy factor — handled by ConvertTemperature().
    private static readonly Dictionary<string, (string Category, decimal ToBase)> Units =
        new(StringComparer.OrdinalIgnoreCase)
        {
            // ── Length (base: metre) ──────────────────────────────────────────
            ["m"]    = ("length", 1m),
            ["km"]   = ("length", 1_000m),
            ["cm"]   = ("length", 0.01m),
            ["mm"]   = ("length", 0.001m),
            ["mi"]   = ("length", 1_609.344m),
            ["yd"]   = ("length", 0.9144m),
            ["ft"]   = ("length", 0.3048m),
            ["in"]   = ("length", 0.0254m),
            ["nm"]   = ("length", 1_852m),           // nautical mile

            // ── Weight (base: kilogram) ───────────────────────────────────────
            ["kg"]   = ("weight", 1m),
            ["g"]    = ("weight", 0.001m),
            ["mg"]   = ("weight", 0.000_001m),
            ["t"]    = ("weight", 1_000m),            // metric tonne
            ["lb"]   = ("weight", 0.453_592_37m),
            ["oz"]   = ("weight", 0.028_349_52m),
            ["st"]   = ("weight", 6.350_293_18m),     // stone

            // ── Temperature (handled separately, dummy factor) ────────────────
            ["c"]    = ("temperature", 0m),
            ["f"]    = ("temperature", 0m),
            ["k"]    = ("temperature", 0m),

            // ── Volume (base: litre) ──────────────────────────────────────────
            ["l"]    = ("volume", 1m),
            ["ml"]   = ("volume", 0.001m),
            ["cl"]   = ("volume", 0.01m),
            ["dl"]   = ("volume", 0.1m),
            ["m3"]   = ("volume", 1_000m),
            ["gal"]  = ("volume", 3.785_411_784m),    // US gallon
            ["qt"]   = ("volume", 0.946_352_946m),    // US quart
            ["pt"]   = ("volume", 0.473_176_473m),    // US pint
            ["fl_oz"]= ("volume", 0.029_573_529_56m), // US fluid ounce

            // ── Speed (base: m/s) ─────────────────────────────────────────────
            ["m_s"]  = ("speed", 1m),
            ["km_h"] = ("speed", 0.277_777_78m),
            ["mph"]  = ("speed", 0.44704m),
            ["kt"]   = ("speed", 0.514_444_44m),      // knot

            // ── Area (base: m²) ───────────────────────────────────────────────
            ["m2"]   = ("area", 1m),
            ["km2"]  = ("area", 1_000_000m),
            ["cm2"]  = ("area", 0.000_1m),
            ["mm2"]  = ("area", 0.000_001m),
            ["ft2"]  = ("area", 0.092_903_04m),
            ["in2"]  = ("area", 0.000_645_16m),
            ["ha"]   = ("area", 10_000m),
            ["acre"] = ("area", 4_046.856_422_4m),

            // ── Data (base: byte) ─────────────────────────────────────────────
            ["b"]    = ("data", 1m),
            ["kb"]   = ("data", 1_024m),
            ["mb"]   = ("data", 1_048_576m),
            ["gb"]   = ("data", 1_073_741_824m),
            ["tb"]   = ("data", 1_099_511_627_776m),
        };

    /// <summary>
    /// Converts <paramref name="value"/> from <paramref name="from"/> to <paramref name="to"/>.
    /// </summary>
    /// <exception cref="ArgumentException">Unknown unit symbol.</exception>
    /// <exception cref="InvalidOperationException">Units belong to different categories.</exception>
    public static (decimal OutputValue, string Category) Convert(string from, string to, decimal value)
    {
        if (!Units.TryGetValue(from, out var fromInfo))
            throw new ArgumentException($"Unknown unit: '{from}'.");

        if (!Units.TryGetValue(to, out var toInfo))
            throw new ArgumentException($"Unknown unit: '{to}'.");

        if (fromInfo.Category != toInfo.Category)
            throw new InvalidOperationException(
                $"Cannot convert '{from}' ({fromInfo.Category}) to '{to}' ({toInfo.Category}).");

        var output = fromInfo.Category == "temperature"
            ? ConvertTemperature(from.ToLowerInvariant(), to.ToLowerInvariant(), value)
            : value * fromInfo.ToBase / toInfo.ToBase;

        return (decimal.Round(output, 10), fromInfo.Category);
    }

    /// <summary>Returns all supported units grouped by category.</summary>
    public static IReadOnlyList<(string Category, IReadOnlyList<string> Units)> ListUnits() =>
        Units
            .GroupBy(kv => kv.Value.Category)
            .OrderBy(g => g.Key)
            .Select(g => (
                g.Key,
                (IReadOnlyList<string>)g.Select(kv => kv.Key.ToLowerInvariant()).OrderBy(s => s).ToList()))
            .ToList();

    private static decimal ConvertTemperature(string from, string to, decimal value)
    {
        double celsius = from switch
        {
            "c" => (double)value,
            "f" => ((double)value - 32.0) * 5.0 / 9.0,
            "k" => (double)value - 273.15,
            _   => throw new ArgumentException($"Unknown temperature unit: {from}")
        };

        double result = to switch
        {
            "c" => celsius,
            "f" => celsius * 9.0 / 5.0 + 32.0,
            "k" => celsius + 273.15,
            _   => throw new ArgumentException($"Unknown temperature unit: {to}")
        };

        return (decimal)result;
    }
}
