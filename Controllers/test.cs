namespace gspAPI.Controllers;

using BusTableAPI;
using DbContexts;
using Entities;
using gspApiGetter.BusTableAPI;
using Mappings;
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
    readonly ILogger<testController> _logger;
    private readonly MysqlContext _context;
    public testController(IBusTableRepository busTableRepository, ILogger<testController> logger, IBusTableGetter busTableGetter, MysqlContext context)
    {
        _busTableRepository = busTableRepository ?? throw new ArgumentNullException(nameof(busTableRepository));
        _logger = logger;
        _busTableGetter = busTableGetter;
        _context = context;
    }
    [HttpGet("/BusTable/{name}")]
    public async Task<ActionResult<ICollection<BusTableDto>>> getTest(string name)
    {
        // var dtos = await _busTableGetter.getBusTableFromWebAndCache(name);
       // var dtos = BusTableMapping.toDto(await _busTableRepository.getBusTablesByTime(8, 59, 1));
       var busTables = await _busTableRepository.getBusTablesByName(name);
       if (!busTables.Any()) return NotFound();
       var dtos = Mappings.BusTableMapping.toDto(busTables);
        
        return Ok(dtos);
    }

    [HttpGet("/updateTable/{name}")]
    public async Task<ActionResult> updateTable(string name)
    {
        _busTableRepository.deleteBusTablesByName(name);
        await _busTableRepository.saveChangesAsync();
        await _busTableGetter.getBusTableFromWebAndCache(name);
        return Ok();
    }


    [HttpPost("/updateTables")]
    public async Task getUpdates()
    {
        await _busTableGetter.updateAllTables();
    }
}
