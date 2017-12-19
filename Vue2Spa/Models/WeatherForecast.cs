using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Vue2Spa.Models
{
    public class WeatherForecast
    {
        [Description("온도")]
        public string DateFormatted { get; set; }

        [Description("온도")]
        public int TemperatureC { get; set; }
        [Description("온도")]
        public string Summary { get; set; }

        [Description("온도")]
        public int TemperatureF
        {
            get
            {
                return 32 + (int)(TemperatureC / 0.5556);
            }
        }
    }
}
