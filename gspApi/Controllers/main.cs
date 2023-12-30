namespace gspAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Models;
using Services;

[ApiController]
[Route("BusTable")]
public class MainController : ControllerBase
{
    readonly IBusTableRepository _busTableRepository;
    public MainController(IBusTableRepository busTableRepository)
    {
        _busTableRepository = busTableRepository ?? throw new ArgumentNullException(nameof(busTableRepository));
    }
    [HttpGet("/pings")]
    [EnableRateLimiting("fixedPings")]
    public async Task<ActionResult<IEnumerable<PingData>>> getFormattedPings(int? from,int? to)
    {

        if (from != null)
        {
            if (from.ToString()!.Length != 8) return BadRequest($"'{from}' is not a valid date format");
        }
        if (to != null)
        {
            if (to.ToString()!.Length != 8) return BadRequest($"'{to}' is not a valid date format");
        }
        var res =await _busTableRepository.getPingCacheFormattedData(from,to);
        if (!res.Any()) return NotFound();
        return Ok(res);
    }
    
    [HttpGet("/latest")]
    [EnableRateLimiting("fixedLatest")]
        public async Task<ActionResult<IEnumerable<LatestPingData>>> getLatestPings()
        {
            var res = await _busTableRepository.getLatestPings()!;
            if (!res.Any()) return NotFound();
            return Ok(res);
        }
}
