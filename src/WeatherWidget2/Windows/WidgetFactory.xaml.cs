using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WeatherWidget2.Model;

namespace WeatherWidget2.Windows
{
    public partial class WidgetFactory : Window
    {
        List<Country> countrys;
        Country country;
        Model.Widget widget;
        bool delete = true;

        public WidgetFactory()
        {
            InitializeComponent();
            DataContext = App.Lang;
        }

        void LoadCountrys()
        {
            countrys = JsonConvert.DeserializeObject<List<Country>>(File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}\\Data\\Countrys.json"));
        }
        void LoadCitys(string countryName)
        {
            country = JsonConvert.DeserializeObject<Country>(File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}\\Data\\{countryName}"));
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

            widget = new Model.Widget();
            widget.CityID = 2172797;
            widget.WidgetMeasure = 0;
            widget.CreateWindow();
        }
        private void mywindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (delete)
                widget.Destroy();
        }
        private void tbCountrysTextChanged(object sender, TextChangedEventArgs e)
        {
            IEnumerable<Country> itemsResult = countrys.Where(item => item.Name.StartsWith(tbCountrys.Text, StringComparison.CurrentCultureIgnoreCase));
            lvSearchedCountrys.ItemsSource = tbCountrys.Text != "" ? itemsResult : null;
        }
        private void lvSearchedCountrysSeletionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvSearchedCountrys.SelectedIndex == -1)
                return;

            LoadCitys((lvSearchedCountrys.SelectedItem as Country).Path);
            gridCity.Visibility = Visibility.Visible;
        }
        private void tbCitysTextChanged(object sender, TextChangedEventArgs e)
        {
            IEnumerable<City> itemsResult = country.Сities.Where(item => item.Name.StartsWith(tbCitys.Text, StringComparison.CurrentCultureIgnoreCase));
            lvSearchedCitys.ItemsSource = tbCitys.Text != "" ? itemsResult : null;
        }
        private void lvSearchedCitysSeletionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvSearchedCitys.SelectedIndex == -1)
                return;

            widget.CityID = (lvSearchedCitys.SelectedItem as City).ID;
            widget.UpdateData();
        }
    }
}
