namespace WeatherWidget2.Model
{
    public class Point : Verloka.HelperLib.Settings.ISettingStruct
    {
        public int X { get; set; } = 100;
        public int Y { get; set; } = 100;

        public Point()
        {
        }
        public Point(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public Point(string str)
        {
            SetValue(str);
        }

        public string GetValue()
        {
            return $"{X}|{Y}";
        }
        public void SetValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            var str = value.Split('|');

            X = getInt(str[0]);
            Y = getInt(str[1]);
        }
        int getInt(string num)
        {
            int i;
            int.TryParse(num, out i);
            return i;
        }
    }
}
