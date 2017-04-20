using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

namespace WeatherWidget2.Model
{
    public static class ColorParser
    {
        public static List<PropertyInfo> ColorLibrary;

        public static void Load()
        {
            ColorLibrary = new List<PropertyInfo>();
            var colors = typeof(Colors).GetTypeInfo().DeclaredProperties;
            foreach (var item in colors)
                ColorLibrary.Add(item);
        }

        public static Color FromName(string name) => (Color)ColorLibrary.Where(color => color.Name == name).First().GetValue(null);
        public static Color FromString(string str) => new Color()
        {
            A = (byte)int.Parse(str.Split(',')[0]),
            R = (byte)int.Parse(str.Split(',')[1]),
            G = (byte)int.Parse(str.Split(',')[2]),
            B = (byte)int.Parse(str.Split(',')[3])
        };
        public static string FromColor(Color color) => $"{color.A},{color.R},{color.G},{color.B}";
    }
}
