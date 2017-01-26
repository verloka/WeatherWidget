using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WeatherWidgetLib;
using WeatherWidgetLib.Geoname;
using WeatherWidgetLib.Language;
using Verloka.HelperLib.Settings;

namespace WeatherWidget
{
    public partial class MainWindow : Window
    {
        public string Key
        {
            get
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\WeatherWidget");
                var value = key.GetValue("WeatherWidgetApixuKey");
                key.Close();
                return value == null ? string.Empty : value.ToString();
            }
            set
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WeatherWidget");
                key.SetValue("WeatherWidgetApixuKey", value);
                key.Close();
            }
        }
        public string Login
        {
            get
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WeatherWidget");
                var value = key.GetValue("WeatherWidgetApixuLogin");
                key.Close();
                return value == null ? string.Empty : value.ToString();
            }
            set
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\WeatherWidget");
                key.SetValue("WeatherWidgetApixuLogin", value);
                key.Close();
            }
        }
        
        public RegSettings settings { get; set; }
        public PCLocation location { get; set; }

        Weather weather;
        Widget widget;
        System.Windows.Forms.NotifyIcon notifyIcon;
        System.Timers.Timer timer;
        bool CanWork = false;

        public MainWindow()
        {
            InitializeComponent();

            settings = new RegSettings("WeatherWidget");
            location = new PCLocation();

            if (string.IsNullOrWhiteSpace(Key) && string.IsNullOrWhiteSpace(Login))
            {
                if (new Login(this).ShowDialog().Value)
                    Init();
            }
            else
                Init();
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
                var list = Web.GetConnection() ? await GetCitys.Get(contryCode, Login) : new GeonameCitys() { geonames = new List<GeonameCity>(), totalResultsCount = 0 };
                return list.geonames;
            });
        }
        void CreateIcon()
        {
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Icon = Properties.Resources.icon;
            notifyIcon.DoubleClick += NotifyIconDoubleClick;

            notifyIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            notifyIcon.ContextMenuStrip.Items.Add("Show window").Click += MainWindowNotifyIconOpenClick;
            notifyIcon.ContextMenuStrip.Items.Add("Update widget").Click += MainWindowUpdateWidgetClick; ;
            notifyIcon.ContextMenuStrip.Items.Add("-");
            notifyIcon.ContextMenuStrip.Items.Add("Full weather").Click += MainWindowFullWeatherClick;
            notifyIcon.ContextMenuStrip.Items.Add("About").Click += MainWindowInfoClick;
            notifyIcon.ContextMenuStrip.Items.Add("-");
            notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += MainWindowNotifyIconExitClick;

            notifyIcon.Visible = true;

        }
        void UpdateWidget()
        {
            Dispatcher.BeginInvoke(new Action(async delegate ()
            {
                if (Web.GetConnection())
                {
                    if (!CanWork)
                        return;
                    
                    weather.QParametr = settings.GetValue<bool>("AutomaticDetection") ? location.GetCoordinate() : (cbCity.SelectedItem as GeonameCity).name;

                    if (!await weather.LoadData())
                        return;

                    widget.Update(weather.GetThemperature(settings.GetValue<int>("Celsium") == 0 ? true : false),
                                  weather.GetCondition(weather.GetConditionCode(), settings.GetValue<string>("LanguageIso")),
                                  weather.GetLocation(),
                                  weather.GetConditionIconURL(weather.GetConditionCode()));
                }
                else
                {
                    if (settings.GetValue<bool>("ShowInetDis"))
                        MessageBox.Show("Nope internet! Update the widget can not be", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }));
        }
        double GetMilisec(int minunte)
        {
            return 60000 * minunte;
        }
        void Init()
        {
            Application.Current.Exit += CurrentExit;
            GetCitys.WrongUser += GetCitysWrongUser;

            weather = new Weather(Key, AppDomain.CurrentDomain.BaseDirectory);
            weather.ErrorLoadData += WeatherErrorLoadData;
            widget = new Widget(this);

            timer = new System.Timers.Timer(GetMilisec(10));
            timer.Elapsed += TimerElapsed;
            timer.Enabled = true;

            CanWork = true;
        }

        //comon events
        private async void windowLoaded(object sender, RoutedEventArgs e)
        {
            if (CanWork)
            {
                CreateIcon();

                var list = await LoadLanguages();
                list.Add(new LanguageObject() { LanguageIso = "en", LanguageName = "English" });
                cbLanguage.ItemsSource = list;
                var selectedLang = from lang in list where lang.LanguageIso == settings.GetValue("LanguageIso", "en") select lang;
                cbLanguage.SelectedItem = selectedLang.FirstOrDefault();
                cbLanguage.SelectionChanged += CbLanguageSelectionChanged;


                if(settings.GetValue("AutomaticDetection", false))
                {
                    cbAutomaticLocation.IsChecked = settings.GetValue<bool>("AutomaticDetection");
                    cbCuntry.IsEnabled = false;
                    cbCity.IsEnabled = false;
                }
                else
                {
                    cbCuntry.IsEnabled = true;
                    cbCuntry.ItemsSource = weather.geonames.geonames;
                    var selectedContry = from contry in weather.geonames.geonames where contry.geonameId == settings.GetValue("GeonameID", 6252001) select contry;
                    cbCuntry.SelectedItem = selectedContry.FirstOrDefault();
                    cbCuntry.SelectionChanged += CbCuntrySelectionChanged;

                    cbCity.IsEnabled = true;
                    var cityList = await LoadCity((cbCuntry.SelectedItem as Geoname).countryCode);
                    cbCity.ItemsSource = cityList;
                    var selectedCity = from city in cityList where city.geonameId == settings.GetValue("CityGeonameID", 5128581) select city;
                    cbCity.SelectedItem = selectedCity.FirstOrDefault();
                    cbCity.SelectionChanged += CbCitySelectionChanged;
                }
                cbAutomaticLocation.Click += CbAutomaticLocationClick;

                cbThemperature.SelectedIndex = settings.GetValue("Celsium", 0);
                cbThemperature.SelectionChanged += CbThemperatureSelectionChanged;

                cbIcon.IsChecked = settings.GetValue("LoadIcon", true);
                cbIcon.Click += CbIconClick;

                cbCondition.IsChecked = settings.GetValue("ShowCondition", true);
                cbCondition.Click += CbConditionClick;

                cbThemperatureShow.IsChecked = settings.GetValue("ShowThemperatue", true);
                cbThemperatureShow.Click += CbThemperatureShowClick;

                cbLocationShow.IsChecked = settings.GetValue("ShowLocation", true); 
                cbLocationShow.Click += CbLocationShowClick;

                cbShowInetMessage.IsChecked = settings.GetValue("ShowInetDis", false);
                cbShowInetMessage.Click += CbShowInetMessageClick;

                RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if ((string)key.GetValue("Weather Widget") == null)
                    cbStartUP.IsChecked = false;
                else
                    cbStartUP.IsChecked = true;
                cbStartUP.Click += CbStartUPClick;

                colorPicker.SelectedColor = settings.GetValue("TextColor", new WeatherColor(255,255,255,255)).GetColor(); 
                colorPicker.SelectedColorChanged += colorPickerSelectionColorChanged;

                colorPickerBackground.SelectedColor = settings.GetValue("BackgroundColor", new WeatherColor(0, 255, 255, 255)).GetColor();
                colorPickerBackground.SelectedColorChanged += ColorPickerBackgroundSelectedColorChanged;

                colorPickerBorder.SelectedColor = settings.GetValue("BorderColor", new WeatherColor(0, 255, 255, 255)).GetColor();
                colorPickerBorder.SelectedColorChanged += ColorPickerBorderSelectedColorChanged;

                var th = settings.GetValue("WidgetBorder", new WidgetBorder(1, 1, 1, 1)).GetBorder();
                tbBorderLeft.Text = th.Left.ToString();
                tbBorderRight.Text = th.Right.ToString();
                tbBorderTop.Text = th.Top.ToString();
                tbBorderBottom.Text = th.Bottom.ToString();

                tbBorderLeft.TextChanged += TbBorderTextChanged;
                tbBorderRight.TextChanged += TbBorderTextChanged;
                tbBorderTop.TextChanged += TbBorderTextChanged;
                tbBorderBottom.TextChanged += TbBorderTextChanged;

                widget.SetWidgetBorder();
                widget.SetWidgetTextColor();
                widget.SetWidgetBackgroundColor();
                widget.SetWidgetBorderColor();
                UpdateWidget();
                widget.ShowWidget(true);
            }
        }
        private void CurrentExit(object sender, ExitEventArgs e)
        {
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
            notifyIcon = null;
            timer.Dispose();
            timer = null;
        }
        private void windowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
        private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            UpdateWidget();
        }
        private void WeatherErrorLoadData(WeatherWidgetLib.Error.Error eror)
        {
            switch (eror.code)
            {
                case 2006:
                    MessageBox.Show($"Error code - {eror.code};\n{eror.message}\nRestart the application and start again", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Login = string.Empty;
                    Key = string.Empty;
                    CanWork = false;
                    break;
                case 2007:
                    MessageBox.Show($"Error code - {eror.code};\n{eror.message}\nKey and Login been reser\nRestart the application and start again", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                    Login = string.Empty;
                    Key = string.Empty;
                    CanWork = false;
                    break;
                default:
                    MessageBox.Show($"Error code - {eror.code};\n{eror.message}\n", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
            }
        }
        private void GetCitysWrongUser(string obj)
        {
            MessageBox.Show($"Wrong login: {obj}\nRestart the application and start again", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
            Login = string.Empty;
            Key = string.Empty;
            CanWork = false;
        }
        //options 
        private void CbConditionClick(object sender, RoutedEventArgs e)
        {
            settings["ShowCondition"] = cbCondition.IsChecked.Value;
        }
        private void CbIconClick(object sender, RoutedEventArgs e)
        {
            settings["LoadIcon"] = cbIcon.IsChecked.Value;
        }
        private void CbThemperatureSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            settings["Celsium"] = cbThemperature.SelectedIndex;
        }
        private void CbCitySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            settings["CityGeonameID"] = (cbCity.SelectedItem as GeonameCity).geonameId;
        }
        private async void CbCuntrySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                settings["GeonameID"] = (cbCuntry.SelectedItem as Geoname).geonameId;

                var cityList = await LoadCity((cbCuntry.SelectedItem as Geoname).countryCode);
                cbCity.ItemsSource = cityList;
                var selectedCity = from city in cityList where city.geonameId == settings.GetValue<int>("CityGeonameID") select city;
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
            settings["LanguageIso"] = (cbLanguage.SelectedItem as LanguageObject).LanguageIso;
        }
        private void btnShowClick(object sender, RoutedEventArgs e)
        {
            widget.ShowWidget(!widget.IsShow);
            btnShow.Content = widget.IsShow ? "Hide" : "Show";
        }
        private void btnApplyClick(object sender, RoutedEventArgs e)
        {
            UpdateWidget();
        }
        private void btnEditClick(object sender, RoutedEventArgs e)
        {
            widget.EditMode(!widget.IsEdit);
            btnEdit.Content = widget.IsEdit ? "Done" : "Edit";
        }
        private void colorPickerSelectionColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            settings["TextColor"] = new WeatherColor(e.NewValue.Value.A, e.NewValue.Value.R, e.NewValue.Value.G, e.NewValue.Value.B);
            widget.SetWidgetTextColor();
        }
        private void CbThemperatureShowClick(object sender, RoutedEventArgs e)
        {
            settings["ShowThemperatue"] = cbThemperatureShow.IsChecked.Value;
        }
        private void CbLocationShowClick(object sender, RoutedEventArgs e)
        {
            settings["ShowLocation"] = cbLocationShow.IsChecked.Value;
        }
        private void CbStartUPClick(object sender, RoutedEventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (!cbStartUP.IsChecked.Value)
                key.DeleteValue("Weather Widget", false);
            else
                key.SetValue("Weather Widget", $"\"{Assembly.GetExecutingAssembly().Location}\" -silent");
        }
        private void CbShowInetMessageClick(object sender, RoutedEventArgs e)
        {
            settings["ShowInetDis"] = cbShowInetMessage.IsChecked.Value;
        }
        private void ColorPickerBackgroundSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            settings["BackgroundColor"] = new WeatherColor(e.NewValue.Value.A, e.NewValue.Value.R, e.NewValue.Value.G, e.NewValue.Value.B);
            widget.SetWidgetBackgroundColor();
        }
        private void ColorPickerBorderSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            settings["BorderColor"] = new WeatherColor(e.NewValue.Value.A, e.NewValue.Value.R, e.NewValue.Value.G, e.NewValue.Value.B);
            widget.SetWidgetBorderColor();
        }
        private void TbBorderTextChanged(object sender, TextChangedEventArgs e)
        {
            int i = 0;
            int.TryParse((sender as TextBox).Text, out i);

            int tag = 0;
            int.TryParse((sender as TextBox).Tag.ToString(), out tag);

            var th = settings.GetValue<WidgetBorder>("WidgetBorder");

            switch (tag)
            {
                case 0://left
                    th.Left = i;
                    break;
                case 1://right
                    th.Right = i;
                    break;
                case 2://top
                    th.Top = i;
                    break;
                case 3://bottom
                    th.Bottom = i;
                    break;
                default:
                    break;
            }
            settings["WidgetBorder"] = th;
            widget.SetWidgetBorder();
        }
        private async void CbAutomaticLocationClick(object sender, RoutedEventArgs e)
        {
            settings["AutomaticDetection"] = cbAutomaticLocation.IsChecked.Value;

            if (settings.GetValue<bool>("AutomaticDetection"))
            {
                cbAutomaticLocation.IsChecked = settings.GetValue<bool>("AutomaticDetection");
                cbCuntry.IsEnabled = false;
                cbCity.IsEnabled = false;
            }
            else
            {
                cbCuntry.ItemsSource = weather.geonames.geonames;
                var selectedContry = from contry in weather.geonames.geonames where contry.geonameId == settings.GetValue("GeonameID", 6252001) select contry;
                cbCuntry.SelectedItem = selectedContry.FirstOrDefault();
                cbCuntry.SelectionChanged += CbCuntrySelectionChanged;

                var cityList = await LoadCity((cbCuntry.SelectedItem as Geoname).countryCode);
                cbCity.ItemsSource = cityList;
                var selectedCity = from city in cityList where city.geonameId == settings.GetValue("CityGeonameID", 5128581) select city;
                cbCity.SelectedItem = selectedCity.FirstOrDefault();
                cbCity.SelectionChanged += CbCitySelectionChanged;
            }
        }
        //tray contextmenu
        private void NotifyIconDoubleClick(object sender, EventArgs e)
        {
            Show();
        }
        private void MainWindowNotifyIconOpenClick(object sender, EventArgs e)
        {
            Show();
        }
        private void MainWindowNotifyIconExitClick(object sender, EventArgs e)
        {
            Application.Current.Shutdown(0);
        }
        private void MainWindowInfoClick(object sender, EventArgs e)
        {
            new Info().ShowDialog();
        }
        private void MainWindowFullWeatherClick(object sender, EventArgs e)
        {
            new FullWeather(weather).ShowDialog();
        }
        private void MainWindowUpdateWidgetClick(object sender, EventArgs e)
        {
            UpdateWidget();
        }
    }
}