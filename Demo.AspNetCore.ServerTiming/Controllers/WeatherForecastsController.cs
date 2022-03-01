using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Lib.AspNetCore.ServerTiming;

namespace Demo.AspNetCore.ServerTiming.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherForecastsController : Controller
    {
        public class WeatherForecast
        {
            public string DateFormatted { get; set; }

            public int TemperatureC { get; set; }

            public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

            public string Summary { get; set; }
        }

        private static readonly string[] SUMMARIES = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly Random _random = new();
        private readonly IServerTiming _serverTiming;

        public WeatherForecastsController(IServerTiming serverTiming)
        {
            _serverTiming = serverTiming;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            List<WeatherForecast> weatherForecasts = new();

            using (_serverTiming.TimeAction())
            {
                for (int daysFromToday = 1; daysFromToday <= 10; daysFromToday++)
                {
                    weatherForecasts.Add(await GetWeatherForecastAsync(daysFromToday));
                };
            }

            return weatherForecasts;
        }

        private async Task<WeatherForecast> GetWeatherForecastAsync(int daysFromToday)
        {
            await Task.Delay(100);

            return new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(daysFromToday).ToString("d"),
                TemperatureC = _random.Next(-20, 55),
                Summary = SUMMARIES[_random.Next(SUMMARIES.Length)]
            };
        }
    }
}
