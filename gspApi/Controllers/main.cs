namespace gspAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<IEnumerable<PingData>>> getFormattedPings(int? from,int? to)
    {
        var res =await _busTableRepository.getPingCacheFormattedData(from,to);
        if (!res.Any()) return NotFound();
        return Ok(res);
    }
    [HttpGet("/latest")]
        public async Task<ActionResult<IEnumerable<LatestPingData>>> getLatestPings()
        {
            var res = await _busTableRepository.getLatestPings()!;
            if (!res.Any()) return NotFound();
            return Ok(res);
        }
}
