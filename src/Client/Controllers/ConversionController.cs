using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Math.Application.Queries;

namespace Client.Controllers;

/// <summary>
/// Unit conversion endpoints. Marked [AllowAnonymous] — pure math, no external
/// API key involved, and useful without a login (e.g. embedded widgets, mobile).
/// </summary>
[ApiController]
[Route("api/math/convert")]
[AllowAnonymous]
public sealed class ConversionController(IUnitConversionQuery conversionQuery) : ControllerBase
{
    [HttpGet]
    public IActionResult Convert([FromQuery] ConvertRequest request)
    {
        try
        {
            var result = conversionQuery.Convert(request.From, request.To, request.Value);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("units")]
    public IActionResult ListUnits()
    {
        return Ok(conversionQuery.ListUnits());
    }
}

public sealed record ConvertRequest(string From, string To, decimal Value);
