using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using WeatherWidget2.Model;

namespace WeatherWidget2.Windows
{
    public partial class WidgetFactory : Window
    {
        List<Country> countrys;
        Country country;

        public WidgetFactory()
        {
            InitializeComponent();
            DataContext = App.Lang;
        }

        void LoadCountrys()
        {
            countrys = JsonConvert.DeserializeObject<List<Country>>(File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}\\Data\\Countrys.json"));
            cbCountrys.ItemsSource = countrys;
        }
        void LoadCitys(string city)
        {
            country = JsonConvert.DeserializeObject<Country>(File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}\\Data\\{city}"));
            cbCitys.ItemsSource = country.Сities;
        }

        #region Window Events
        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            try { DragMove(); }
            catch { }
        }
        private void btnCloseClick()
        {
            Close();
        }
        private void btnMinimazeClick()
        {
            WindowState = WindowState.Minimized;
        }
        #endregion

        private void mywindowLoaded(object sender, RoutedEventArgs e)
        {
            btnAdd.Text = App.Lang.WidgetFactoryAddWidget;

            LoadCountrys();
        }
        private void cbCountrysSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadCitys(((sender as ComboBox).SelectedItem as Country).Path);
        }
    }
}
