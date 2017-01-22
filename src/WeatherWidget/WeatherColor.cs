using System.Windows.Media;

namespace WeatherWidget
{
    public class WeatherColor: Verloka.HelperLib.Settings.ISettingStruct
    {
        public int A { get; set; }
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }

        public WeatherColor()
        {
            A = 255;
            R = 0;
            G = 0;
            B = 0;
        }
        public WeatherColor(int A, int R, int G, int B)
        {
            this.A = A;
            this.R = R;
            this.G = G;
            this.B = B;
        }
        public WeatherColor(string argb)
        {
            SetValue(argb);
        }

        public Color GetColor()
        {
            return new Color() { A = (byte)A, R = (byte)R, G = (byte)G, B = (byte)B };
        }
        public string GetValue()
        {
            return $"{A}|{R}|{G}|{B}";
        }
        public void SetValue(string value)
        {
            var str = value.Split('|');

            A = getInt(str[0]);
            R = getInt(str[1]);
            G = getInt(str[2]);
            B = getInt(str[3]);
        }
        int getInt(string num)
        {
            int i;
            int.TryParse(num, out i);
            return i;
        }
    }
}
