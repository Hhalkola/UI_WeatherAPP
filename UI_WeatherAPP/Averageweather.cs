using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI_WeatherAPP
{
    public class Averageweather
    {
        public decimal AvgTemperature { get; set; }
        public decimal AvgPressure { get; set; }
        public decimal AvgHumidity { get; set; }

        public Averageweather(decimal temp, decimal pres, decimal hum)
        {
            this.AvgTemperature = temp;
            this.AvgPressure = pres;
            this.AvgHumidity = hum;
        }
        public Averageweather()
        {

        }
    }
}
