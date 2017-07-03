using System.Collections.Generic;

namespace WeatherWidget2.Model
{
    public class ForecastOneDay
    {
        public int Day { get; set; }
        public List<string> Labels { get; set; }
        public List<string> Condi { get; set; }
        public List<int> Values { get; set; }
        public List<string> Icons { get; set; }
        public List<double> Press { get; set; }
        public List<double> Humi { get; set; }
        public Dictionary<int, string> Wind { get; set; }

        public ForecastOneDay()
        {
            Labels = new List<string>();
            Values = new List<int>();
            Icons = new List<string>();
            Press = new List<double>();
            Humi = new List<double>();
            Wind = new Dictionary<int, string>();
            Condi = new List<string>();
            Day = -1;
        }

        public string GetDayIcon()
        {
            return Icons[Icons.Count / 2];
        }
        public string GetCurrentIcon()
        {
            return Icons[0];
        }
        public string GetDayCondition()
        {
            return Condi[Condi.Count / 2];
        }
        public string GetCurrentCondition()
        {
            return Condi[0];
        }
        public double GetDayPressure()
        {
            return Press[Press.Count / 2];
        }
        public double GetCurrentPressure()
        {
            return Press[0];
        }
        public double GetDayHumidity()
        {
            return Humi[Humi.Count / 2];
        }
        public double GetCurrentHumidity()
        {
            return Humi[0];
        }
        public int GetDayValue()
        {
            return Values[Values.Count / 2];
        }
        public int GetCurrentValue()
        {
            return Values[0];
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
