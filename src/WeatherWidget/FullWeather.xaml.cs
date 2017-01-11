using System.Windows;
using WeatherWidgetLib;

namespace WeatherWidget
{
    public partial class FullWeather : Window
    {
        Weather weatherInfo;

        public FullWeather()
        {
            InitializeComponent();
        }
        public FullWeather(Weather weather)
        {
            InitializeComponent();
            weatherInfo = weather;
        }

        private void windowLoaded(object sender, RoutedEventArgs e)
        {
            tbData.Text = weatherInfo.Current.current.last_updated;
            tbTemp.Text = $"{weatherInfo.Current.current.temp_c} °C | {weatherInfo.Current.current.temp_f} °F";
            tbFeelTemp.Text = $"{weatherInfo.Current.current.feelslike_c} °C | {weatherInfo.Current.current.feelslike_f} °F";
            tbCond.Text = weatherInfo.GetCondition(weatherInfo.GetConditionCode());
            tbWindSpeed.Text = $"{weatherInfo.Current.current.wind_kph} km/h | {weatherInfo.Current.current.wind_mph} m/h";
            tbWindDir.Text = $"{weatherInfo.Current.current.wind_degree}° | {weatherInfo.Current.current.wind_dir}";
            tbPres.Text = $"{weatherInfo.Current.current.pressure_mb} millibars | {weatherInfo.Current.current.pressure_in} inches";
            tbPrec.Text = $"{weatherInfo.Current.current.precip_mm} millimeters | {weatherInfo.Current.current.precip_in} inches";
            tbHum.Text = $"{weatherInfo.Current.current.humidity}%";
            tbCloud.Text = $"{weatherInfo.Current.current.cloud}%";
        }
    }
}
