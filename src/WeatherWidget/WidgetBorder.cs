using System.Windows;

namespace WeatherWidget
{
    public class WidgetBorder: Verloka.HelperLib.Settings.ISettingStruct
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }

        public WidgetBorder()
        {
            Left = 1;
            Top = 1;
            Right = 1;
            Bottom = 1;
        }
        public WidgetBorder(int Left, int Top, int Right, int Bottom)
        {
            this.Left = Left;
            this.Top = Top;
            this.Right = Right;
            this.Bottom = Bottom;
        }
        public WidgetBorder(string argb)
        {
            SetValue(argb);
        }

        public Thickness GetBorder()
        {
            return new Thickness(Left, Top, Right, Bottom);
        }
        public string GetValue()
        {
            return $"{Left}|{Top}|{Right}|{Bottom}";
        }
        public void SetValue(string value)
        {
            var str = value.Split('|');

            Left = getInt(str[0]);
            Top = getInt(str[1]);
            Right = getInt(str[2]);
            Bottom = getInt(str[3]);
        }
        int getInt(string num)
        {
            int i;
            int.TryParse(num, out i);
            return i;
        }
    }
}
