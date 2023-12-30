namespace gspAPI.Controllers;

using DbContexts;
using gspApiGetter.BusTableAPI;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

[ApiController]
[Route("BusTable")]
public class mainController : ControllerBase
{
    readonly IBusTableRepository _busTableRepository;
    readonly IBusTableGetter _busTableGetter;
    readonly ILogger<mainController> _logger;
    private readonly MysqlContext _context;
    public mainController(IBusTableRepository busTableRepository, ILogger<mainController> logger, IBusTableGetter busTableGetter, MysqlContext context)
    {
        _busTableRepository = busTableRepository ?? throw new ArgumentNullException(nameof(busTableRepository));
        _logger = logger;
        _busTableGetter = busTableGetter;
        _context = context;
    }
    [HttpGet("/pings")]
    public async Task<ActionResult<IEnumerable<PingData>>> getFormattedPings(int? from,int? to)
    {
        var res =await _busTableRepository.getPingCacheFormattedData(from,to);
        if (res == null) return NotFound();
        return Ok(res);
    }
    [HttpGet("/latest")]
        public async Task<ActionResult<IEnumerable<LatestPingData>>> getLatestPings()
        {
            var res = await _busTableRepository.getLatestPings();
            if (res == null) return NotFound();
            return Ok(res);
        }
}
