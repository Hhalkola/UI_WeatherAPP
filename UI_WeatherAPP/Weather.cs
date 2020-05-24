using System;

namespace UI_WeatherAPP
{
    public class Weather
    {
        public decimal Temperature { get; set; }
        public decimal Pressure { get; set; }
        public decimal Humidity { get; set; }
        public DateTime Date { get; set; }

        public Weather(decimal temp, decimal pres, decimal hum, DateTime date)
        {
            this.Temperature = temp;
            this.Pressure = pres;
            this.Humidity = hum;
            this.Date = date;
        }
        public Weather()
        {

        }
    }
}
