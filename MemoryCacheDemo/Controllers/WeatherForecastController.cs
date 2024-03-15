using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace MemoryCacheDemo.Controllers;
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IMemoryCache _cacheService;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IMemoryCache cacheService)
    {
        _logger = logger;
        _cacheService = cacheService;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public List<WeatherForecast> Get()
    {
        //Do not hardcode
        if (!_cacheService.TryGetValue("MyCacheKey", out List<WeatherForecast> weathersFromCache))
        {
            var weathers = Enumerable.Range(1, 300).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            }).ToList();

            _cacheService.Set<List<WeatherForecast>>("MyCacheKey", weathers);

            return weathers;
        }

        return weathersFromCache;
    }
}
