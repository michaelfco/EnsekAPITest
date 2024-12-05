using CsvHelper;
using EnsekTest.API.Map;
using EnsekTest.Application.Interface;
using EnsekTest.Application.Services;
using EnsekTest.Domain.Entities;
using EnsekTest.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace EnsekTest.API.Controllers;

public class MeterReadingController : Controller
{
    //SOLID: Dependency Injection
    private readonly IMeterReadingService _meterReadingService;
    private readonly ICsvParserService _csvParser;
    private readonly IDatabaseSeederService _databaseSeeder;

    public MeterReadingController(IMeterReadingService service, ICsvParserService csvParser, IDatabaseSeederService databaseSeeder)
    {
        _meterReadingService = service;
        _csvParser = csvParser;
        _databaseSeeder = databaseSeeder;
    }
    public IActionResult Index()
    {
        return View();
    }

    //SOLID: Single Responsibility (handles file uploads and delegates processing to the service layer)
    [HttpPost("meter-reading-uploads")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Invalid file");

        //process file
        try
        {
            //DP: Delegation (delegates the file processing and reading the CSV to the service layer and CSV parser)
            using var stream = file.OpenReadStream();
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap<MeterReadingMap>();

            //DP: Strategy (can be changed by swapping implementations)
            var meterReadings = csv.GetRecords<MeterReading>().ToList();
           
            var result = await _meterReadingService.ProcessReadingsAsync(meterReadings);

            return Ok(result);
        }
        catch (Exception ex)
        {               
            return StatusCode(500, $"Internal server error while processing file. {ex}");
        }
    }


    //SOLID: Open/Closed Principle (This method can be extendable to support multiple types of file or other logic in the future)
    [HttpPost("seed-accounts")]
    public async Task<IActionResult> UploadAndSeedCsv(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        //validate csv extension
        if (!file.FileName.EndsWith(".csv"))
            return BadRequest("Only CSV files are allowed.");

        try
        {
            //DP: Delegation (file parsing is delegated to the _databaseSeeder service)
            using var stream = file.OpenReadStream();
            var acc = _databaseSeeder.Parse<Account>(stream);

            //seed db with the csv file
            using var scope = HttpContext.RequestServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<EnsekDbContext>();


            //add
            await context.Accounts.AddRangeAsync(acc);
            await context.SaveChangesAsync();

            return Ok("Accounts successfully uploaded and saved to the database.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error processing the CSV file: {ex.Message}");
        }
    }


}
