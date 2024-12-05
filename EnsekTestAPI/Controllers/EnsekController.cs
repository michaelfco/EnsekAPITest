using CsvHelper;
using EnsekTest.Application.Interface;
using EnsekTest.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EnsekTest.API.Controllers;

public class EnsekController : Controller
{
    //SOLID: Dependency Injection
    private readonly IMeterReadingService _meterReadingService;

    public EnsekController(IMeterReadingService meterReadingService)
    {               
        _meterReadingService = meterReadingService;
    }
    public IActionResult Index()
    {
        return View();
    }


    //SOLID: Single Responsability (delegate data retrieval and return the result)
    [HttpGet("all-meter-readings")]
    public async Task<IActionResult> GetAllMeterReadings([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var meterReadings = await _meterReadingService.GetAllMeterReadingsAsync();

        if (meterReadings == null || !meterReadings.Any())
        {
            return NotFound("No meter readings found.");
        }

        //DP: Pagination 
        var paginated = meterReadings.Skip((page - 1) * pageSize).Take(pageSize);
        return Ok(paginated);
    }
}
