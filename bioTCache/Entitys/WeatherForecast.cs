using System;

namespace bioTCache.Entitys
{
    public class WeatherForecast : IEntity
    {
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }
        public int Id { get; set; }
    }

}
