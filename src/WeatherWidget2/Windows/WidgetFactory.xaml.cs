using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
            cbCountrys.ItemsSource = countrys;
            cbCountrys.SelectedIndex = 0;
        }
        void LoadCitys(string city)
        {
            country = JsonConvert.DeserializeObject<Country>(File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}\\Data\\{city}"));
            cbCitys.ItemsSource = country.Сities;
            cbCitys.SelectedIndex = 0;
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
            widget.CityID = (cbCitys.SelectedItem as City).ID;
            widget.WidgetMeasure = 0;
            widget.CreateWindow();

            cbCitys.SelectionChanged += CbCitysSelectionChanged;
        }

        private void CbCitysSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCitys.SelectedIndex == -1)
                return;

            widget.CityID = (cbCitys.SelectedItem as City).ID;
            widget.UpdateData();
        }
        private void cbCountrysSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbCountrys.SelectedIndex == -1)
                return;

            LoadCitys(((sender as ComboBox).SelectedItem as Country).Path);
        }
        private void mywindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (delete)
                widget.Destroy();
        }
    }
}
