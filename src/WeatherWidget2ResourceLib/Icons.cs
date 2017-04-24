using System;

namespace WeatherWidget2ResourceLib
{
    public class Icons
    {
        string size;
        string theme;

        public Icons(IconSize size, IconTheme theme)
        {
            UpdateData(size, theme);
        }

        public void UpdateData(IconSize size, IconTheme theme)
        {
            //icon size
            switch (size)
            {
                case IconSize.Big:
                    this.size = "128";
                    break;
                default:
                case IconSize.Medium:
                    this.size = "64";
                    break;
                case IconSize.Small:
                    this.size = "32";
                    break;
            }

            //icon theme
            switch (theme)
            {
                default:
                case IconTheme.Standart:
                    this.theme = "Standart";
                    break;
                case IconTheme.Thin:
                    this.theme = "Thin";
                    break;
                case IconTheme.Square:
                    this.theme = "Square";
                    break;
            }
        }
        public Uri GetIcon(string name)
        {
            return new Uri($"{AppDomain.CurrentDomain.BaseDirectory}\\Icons\\{theme}\\{size}\\{name}.png", UriKind.RelativeOrAbsolute);
        }
        public int GetSize()
        {
            switch (size)
            {
                case "128":
                    return 128;
                case "32":
                    return 32;
                case "64":
                default:
                    return 64;
            }
        }
    }
}
