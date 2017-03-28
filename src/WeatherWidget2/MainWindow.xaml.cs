using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeatherWidget2
{
    public partial class MainWindow : Window
    {
        Weather weather;
        Widget.TempWidget tempWidget;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = App.Lang;
        }

        void LoadTempWidget()
        {
            weather = new Weather();
            weather.LoadCurrent();

            tempWidget = new Widget.TempWidget();
            tempWidget.Show();
            tempWidget.UpdateInfo(weather.Current.Main.Temperature.ToString(), weather.Current.WeatherList[0].Description, weather.Current.Name);
            tempWidget.SetIcon($"http://openweathermap.org/img/w/{weather.Current.WeatherList[0].Icon}.png");
        }

        #region Window Events
        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            try { DragMove(); }
            catch { }
        }
        private void btnCloseClick()
        {
            //TODO
            Application.Current.Shutdown(0);
        }
        private void btnMinimazeClick()
        {
            WindowState = WindowState.Minimized;
        }
        #endregion

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Windows.WidgetFactory wf = new Windows.WidgetFactory();
            wf.Show();
            
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            LoadTempWidget();
        }
    }
}
