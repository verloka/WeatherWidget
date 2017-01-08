using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using WeatherWidgetLib;
using WeatherWidgetLib.Geoname;
using WeatherWidgetLib.Language;

namespace WeatherWidget
{
    public partial class MainWindow : Window
    {
        Weather weather;
        Widget widget;

        public MainWindow()
        {
            InitializeComponent();

            weather = new Weather();
            widget = new Widget();
        }

        Task<List<LanguageObject>> LoadLanguages()
        {
            return Task.Run(() =>
            {
                List<LanguageObject> list = new List<LanguageObject>();

                foreach (var item in weather.conditions[0].languages)
                    list.Add(new LanguageObject() { LanguageIso = item.lang_iso, LanguageName = item.lang_name });
                return list;
            });
        }
        async Task<List<GeonameCity>> LoadCity(string contryCode)
        {
            return await Task.Run(async () =>
             {
                 var list = await GetCitys.Get(contryCode);
                 return list.geonames;
             });
        }

        private async void windowLoaded(object sender, RoutedEventArgs e)
        {
            var list = await LoadLanguages();
            cbLanguage.ItemsSource = list;
            var selectedLang = from lang in list where lang.LanguageIso == Properties.Settings.Default.LanguageIso select lang;
            cbLanguage.SelectedItem = selectedLang.FirstOrDefault();
            cbLanguage.SelectionChanged += CbLanguageSelectionChanged;

            cbCuntry.ItemsSource = weather.geonames.geonames;
            var selectedContry = from contry in weather.geonames.geonames where contry.geonameId == Properties.Settings.Default.GeonameID select contry;
            cbCuntry.SelectedItem = selectedContry.FirstOrDefault();
            cbCuntry.SelectionChanged += CbCuntrySelectionChanged;

            var cityList = await LoadCity((cbCuntry.SelectedItem as Geoname).countryCode);
            cbCity.ItemsSource = cityList;
            var selectedCity = from city in cityList where city.geonameId == Properties.Settings.Default.CityGeonameID select city;
            cbCity.SelectedItem = selectedCity.FirstOrDefault();
            cbCity.SelectionChanged += CbCitySelectionChanged;

            cbThemperature.SelectedIndex = Properties.Settings.Default.Celsium;
            cbThemperature.SelectionChanged += CbThemperatureSelectionChanged;

            cbUpdate.SelectedIndex = Properties.Settings.Default.Rate;
            cbUpdate.SelectionChanged += CbUpdateSelectionChanged;

            cbIcon.IsChecked = Properties.Settings.Default.LoadIcon;
            cbIcon.Click += CbIconClick;

            cbCondition.IsChecked = Properties.Settings.Default.ShowCondition;
            cbCondition.Click += CbConditionClick;

            weather.City = (cbCity.SelectedItem as GeonameCity).name;
            weather.LoadData();
            widget.Update(weather.GetThemperature(Properties.Settings.Default.Celsium == 0 ? true : false),
                          weather.GetCondition(weather.GetConditionCode(),
                          Properties.Settings.Default.LanguageIso),
                          weather.GetLocation(),
                          weather.GetConditionIcon(weather.GetConditionCode()));
        }

        private void CbConditionClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Properties.Settings.Default.LoadIcon = cbIcon.IsChecked.Value;
                Properties.Settings.Default.Save();
            }
            catch { }
        }
        private void CbIconClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Properties.Settings.Default.ShowCondition = cbCondition.IsChecked.Value;
                Properties.Settings.Default.Save();
            }
            catch { }
        }
        private void CbUpdateSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Properties.Settings.Default.Rate = cbUpdate.SelectedIndex;
                Properties.Settings.Default.Save();
            }
            catch { }
        }
        private void CbThemperatureSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Properties.Settings.Default.Celsium = cbThemperature.SelectedIndex;
                Properties.Settings.Default.Save();
            }
            catch { }
        }
        private void CbCitySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Properties.Settings.Default.CityGeonameID = (cbCity.SelectedItem as GeonameCity).geonameId;
                Properties.Settings.Default.Save();
            }
            catch { }
        }
        private async void CbCuntrySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Properties.Settings.Default.GeonameID = (cbCuntry.SelectedItem as Geoname).geonameId;
                Properties.Settings.Default.Save();

                var cityList = await LoadCity((cbCuntry.SelectedItem as Geoname).countryCode);
                cbCity.ItemsSource = cityList;
                var selectedCity = from city in cityList where city.geonameId == Properties.Settings.Default.CityGeonameID select city;
                cbCity.SelectedItem = selectedCity.First();
            }
            catch
            {
                if (cbCity.Items.Count > 0)
                    cbCity.SelectedIndex = 0;
            }
        }
        private void CbLanguageSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Properties.Settings.Default.LanguageIso = (cbLanguage.SelectedItem as LanguageObject).LanguageIso;
                Properties.Settings.Default.Save();
            }
            catch { }
        }
        private void btnShowClick(object sender, RoutedEventArgs e)
        {
            widget.ShowWidget(!widget.IsShow);
            btnShow.Content = widget.IsShow ? "Hide" : "Show";
        }
        private void btnApplyClick(object sender, RoutedEventArgs e)
        {
            weather.City = (cbCity.SelectedItem as GeonameCity).name;
            weather.LoadData();
            widget.Update(weather.GetThemperature(Properties.Settings.Default.Celsium == 0 ? true : false),
                          weather.GetCondition(weather.GetConditionCode(),
                          Properties.Settings.Default.LanguageIso),
                          weather.GetLocation(),
                          weather.GetConditionIcon(weather.GetConditionCode()));
        }
        private void btnEditClick(object sender, RoutedEventArgs e)
        {
            widget.EditMode(!widget.IsEdit);
            btnEdit.Content = widget.IsEdit ? "Done" : "Edit";
        }
    }
}
