namespace WeatherWidget
{
    public class WidgetPosition : Verloka.HelperLib.Settings.ISettingStruct
    {
        public int Top { get; set; } = 100;
        public int Left { get; set; } = 100;

        public WidgetPosition()
        {
        }
        public WidgetPosition(string str)
        {
            SetValue(str);
        }

        public string GetValue()
        {
            return $"{Top}|{Left}";
        }
        public void SetValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            var str = value.Split('|');

            Top = getInt(str[0]);
            Left = getInt(str[1]);
        }
        int getInt(string num)
        {
            int i;
            int.TryParse(num, out i);
            return i;
        }
    }
}
