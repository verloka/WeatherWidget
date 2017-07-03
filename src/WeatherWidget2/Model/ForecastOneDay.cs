using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherWidget2.Model
{
    public class ForecastOneDay
    {
        public int Day { get; set; }
        public List<string> Labels { get; set; }
        public List<int> Values { get; set; }

        public ForecastOneDay()
        {
            Labels = new List<string>();
            Values = new List<int>();
            Day = -1;
        }

        public override bool Equals(object obj)
        {
            return Day == ((ForecastOneDay)obj).Day;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode() + Day;
        }
        public override string ToString()
        {
            return $"Day: {Day}";
        }
    }
}
