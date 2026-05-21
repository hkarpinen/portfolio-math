using Math.Domain.Engines;

namespace Tests.Conversion;

public sealed class UnitConversionEngineTests
{
    [Theory]
    [InlineData("km", "m",  1,    1000)]
    [InlineData("m",  "km", 1000, 1)]
    [InlineData("ft", "m",  1,    0.3048)]
    [InlineData("mi", "km", 1,    1.609344)]
    public void Length_ConvertsCorrectly(string from, string to, decimal input, decimal expected)
    {
        var (outputValue, category) = UnitConversionEngine.Convert(from, to, input);
        Assert.Equal(expected, outputValue, precision: 6);
        Assert.Equal("length", category);
    }

    [Theory]
    [InlineData("kg",  "lb", 1,   2.2046226218)]
    [InlineData("lb",  "kg", 1,   0.45359237)]
    [InlineData("g",   "kg", 1000, 1)]
    public void Weight_ConvertsCorrectly(string from, string to, decimal input, decimal expected)
    {
        var (outputValue, category) = UnitConversionEngine.Convert(from, to, input);
        Assert.Equal(expected, outputValue, precision: 6);
        Assert.Equal("weight", category);
    }

    [Theory]
    [InlineData("c", "f", 0,   32)]
    [InlineData("c", "f", 100, 212)]
    [InlineData("f", "c", 32,  0)]
    [InlineData("c", "k", 0,   273.15)]
    [InlineData("k", "c", 273.15, 0)]
    public void Temperature_ConvertsCorrectly(string from, string to, decimal input, decimal expected)
    {
        var (outputValue, category) = UnitConversionEngine.Convert(from, to, input);
        Assert.Equal((double)expected, (double)outputValue, precision: 4);
        Assert.Equal("temperature", category);
    }

    [Fact]
    public void SameUnit_ReturnsIdentity()
    {
        var (outputValue, _) = UnitConversionEngine.Convert("kg", "kg", 42m);
        Assert.Equal(42m, outputValue);
    }

    [Fact]
    public void UnknownFromUnit_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => UnitConversionEngine.Convert("parsec", "km", 1));
    }

    [Fact]
    public void UnknownToUnit_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => UnitConversionEngine.Convert("km", "parsec", 1));
    }

    [Fact]
    public void MismatchedCategories_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => UnitConversionEngine.Convert("kg", "km", 1));
    }

    [Fact]
    public void ListUnits_ReturnsAllCategories()
    {
        var categories = UnitConversionEngine.ListUnits().Select(c => c.Category).ToList();
        Assert.Contains("length",      categories);
        Assert.Contains("weight",      categories);
        Assert.Contains("temperature", categories);
        Assert.Contains("volume",      categories);
        Assert.Contains("speed",       categories);
        Assert.Contains("area",        categories);
        Assert.Contains("data",        categories);
    }

    [Fact]
    public void ListUnits_EachCategoryHasAtLeastTwoUnits()
    {
        foreach (var cat in UnitConversionEngine.ListUnits())
            Assert.True(cat.Units.Count >= 2, $"Category '{cat.Category}' has fewer than 2 units.");
    }
}
