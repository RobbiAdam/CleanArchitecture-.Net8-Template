using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Contract.Errors;
using Template.Contract.Exceptions;

namespace Template.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("internal-error")]
        public IActionResult InternalError()
        {
            throw new Exception("Internal Error Happened");
        }

        [HttpGet]
        [Route("notfound-error")]
        public IActionResult GetNotFoundWeather()
        {
            throw new NotFoundException("Weather not found");
        }

        [HttpGet]
        [Route("validation-error")]
        public IActionResult GetValidationError()
        {
            var validationErrors = new List<ValidationError>
            {
                new() { PropertyName = "City", Message = "City is required" },
                new() { PropertyName = "Date", Message = "Date is invalid" }
            };

            throw new CustomValidationError(validationErrors);
        }
    }

    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }


}
