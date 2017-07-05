using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WeatherWidget2.Model;
using WeatherWidget2ResourceLib;

namespace WeatherWidget2.Windows
{
    public partial class WidgetFactory : Window
    {
        MainWindow mw;
        List<Country> countrys;
        Country country;
        Widget widget;
        Widget copy = null;
        bool delete = true;
        bool EditMode = false;

        public WidgetFactory(MainWindow mw)
        {
            InitializeComponent();
            DataContext = App.Lang;
            this.mw = mw;
        }
        public WidgetFactory(MainWindow mw, Widget widget)
        {
            InitializeComponent();
            DataContext = App.Lang;
            this.mw = mw;
            this.widget = new Widget(widget);
            copy = widget;
        }

        void LoadCountrys()
        {
            countrys = JsonConvert.DeserializeObject<List<Country>>(Geonames.GetCoutrysJson());
        }
        void LoadCitys(string countryName)
        {
            country = JsonConvert.DeserializeObject<Country>(Geonames.GetCoutryJson(countryName));
        }
        void SetCurrentViewCode()
        {
            switch (cbWidgetViewCurrent.SelectedIndex)
            {
                default:
                case 0:
                    widget.ViewCode = 0;
                    break;
            }
        }
        void SetForecastViewCode()
        {
            switch (cbWidgetViewForecast.SelectedIndex)
            {
                default:
                case 0:
                    widget.ViewCode = 100;
                    break;
                case 1:
                    widget.ViewCode = 101;
                    break;
                case 2:
                    widget.ViewCode = 102;
                    break;
            }
        }
        void SetVisibleByType()
        {
            gridSizeIcon.Visibility = cbWidgetType.SelectedIndex == 0 ? Visibility.Visible : Visibility.Collapsed;
            cbWidgetViewCurrent.Visibility = cbWidgetType.SelectedIndex == 0 ? Visibility.Visible : Visibility.Collapsed;
            cbWidgetViewForecast.Visibility = cbWidgetType.SelectedIndex == 1 ? Visibility.Visible : Visibility.Collapsed;
        }

        #region Window Events
        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            try { DragMove(); }
            catch { }
        }
        private void btnCloseClick()
        {
            if (EditMode)
                copy.CreateWindow();

            Close();
        }
        private void btnMinimazeClick()
        {
            WindowState = WindowState.Minimized;
        }
        #endregion

        private void mywindowLoaded(object sender, RoutedEventArgs e)
        {
            cbTextColors.ItemsSource = ColorParser.ColorLibrary;

            if (copy == null)
            {
                widget = new Widget();
                cbTextColors.SelectedItem = ColorParser.ColorLibrary.Where(item => item.Name == "White").First();
            }
            else
            {
                copy.Destroy();
                EditMode = true;

                tbWidgetName.Text = widget.Name;
                cbTextColors.SelectedItem = ColorParser.ColorLibrary.Where(item => item.Name == widget.TextColor).First();
                cbMeasure.SelectedIndex = (int)widget.WidgetMeasure;
                cbSize.SelectedIndex = (int)widget.Size;
                cbIconTheme.SelectedIndex = (int)widget.Theme;
                cbWidgetType.SelectedIndex = widget.Type;

                SetVisibleByType();
            }

            btnAdd.Text = EditMode ? App.Lang.WidgetFactoryEditWidget : App.Lang.WidgetFactoryAddWidget;

            cbMeasure.SelectionChanged += CbMeasureSelectionChanged;
            cbSize.SelectionChanged += CbSizeSelectionChanged;
            cbIconTheme.SelectionChanged += CbIconThemeSelectionChanged;
            cbTextColors.SelectionChanged += CbTextColorsSelectionChanged;
            cbWidgetType.SelectionChanged += CbWidgetTypeSelectionChanged;
            cbWidgetViewCurrent.SelectionChanged += CbWidgetViewCurrentSelectionChanged;
            cbWidgetViewForecast.SelectionChanged += CbWidgetViewForecastSelectionChanged;

            LoadCountrys();

            widget.CreateWindow();
            widget.SetEditMode(true);
        }
        private void CbWidgetViewForecastSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetForecastViewCode();

            widget.Destroy();
            widget.CreateWindow();
            widget.SetEditMode(true);

            widget.UpdateLook();
            widget.UpdateData();
        }
        private void CbWidgetViewCurrentSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetCurrentViewCode();

            widget.Destroy();
            widget.CreateWindow();
            widget.SetEditMode(true);

            widget.UpdateLook();
            widget.UpdateData();
        }
        private void CbWidgetTypeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbWidgetType.SelectedIndex == -1)
                return;

            SetVisibleByType();

            if (cbWidgetType.SelectedIndex == 0)
                SetCurrentViewCode();
            else
                SetForecastViewCode();

            widget.Type = cbWidgetType.SelectedIndex;

            widget.Destroy();
            widget.CreateWindow();
            widget.SetEditMode(true);

            widget.UpdateLook();
            widget.UpdateData();
        }
        private void CbTextColorsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbTextColors.SelectedIndex == -1)
                return;

            widget.TextColor = (cbTextColors.SelectedItem as PropertyInfo).Name;
            widget.UpdateLook();
        }
        private void CbIconThemeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbIconTheme.SelectedIndex == -1)
                return;

            widget.Theme = (IconTheme)cbIconTheme.SelectedIndex;
            widget.UpdateLook();
            widget.UpdateData();
        }
        private void CbSizeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSize.SelectedIndex == -1)
                return;

            widget.Size = (IconSize)cbSize.SelectedIndex;
            widget.UpdateLook();
            widget.UpdateData();
        }
        private void CbMeasureSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbMeasure.SelectedIndex == -1)
                return;

            widget.WidgetMeasure = (Measure)cbMeasure.SelectedIndex;
            widget.UpdateData(updateMeasure: true);
            widget.UpdateLook();
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
            widget.UpdateData(updateCity: true);
        }
        private void btnAddClick()
        {
            delete = false;

            if (EditMode)
            {
                mw.wstorage.Remove(copy.guid);
                widget.NewGUID();
            }

            widget.SetEditMode(false);
            widget.CopyPosition();
            widget.Name = string.IsNullOrWhiteSpace(tbWidgetName.Text) ? "NaN" : tbWidgetName.Text;
            mw.wstorage.Add(widget);
            if (!widget.Visible)
                widget.Destroy();
            Close();
        }
    }
}
