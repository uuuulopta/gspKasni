namespace gspAPI.Controllers;

using BusTableAPI;
using gspApiGetter.BusTableAPI;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using StackExchange.Profiling;

[ApiController]
[Route("BusTable")]
public class testController : ControllerBase
{
    readonly IBusTableRepository _busTableRepository;
    readonly IBusTableGetter _busTableGetter;
    public testController(IBusTableRepository busTableRepository)
    {
        _busTableRepository = busTableRepository ?? throw new ArgumentNullException(nameof(busTableRepository));
        _busTableGetter = new BusTableGetter(_busTableRepository);
    }
    [HttpGet("{name}")]
    public async Task<ActionResult<ICollection<BusTableDto>>> getTest(string name)
    {
        var dtos = await _busTableGetter.getBusTableFromWebAndCache(name);
        if (dtos == null) return NotFound();
        return Ok(dtos);
    }
}
