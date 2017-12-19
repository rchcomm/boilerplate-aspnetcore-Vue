using Microsoft.AspNetCore.Hosting;
using Swashbuckle.AspNetCore.Examples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vue2Spa.Models
{
    public class WeatherForecastExample : IExamplesProvider
    {
        private readonly IHostingEnvironment env;

        public WeatherForecastExample(IHostingEnvironment env)
        {
            this.env = env;
        }

        public object GetExamples()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = rng.Next(10).ToString()
            });
        }
    }
}
