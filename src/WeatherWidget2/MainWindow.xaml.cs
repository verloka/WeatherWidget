using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Verloka.HelperLib.Update;
using WeatherWidget2.Windows;

namespace WeatherWidget2
{
    public partial class MainWindow : Window
    {
        public Model.WidgetStorage wstorage;

        System.Timers.Timer timer;
        System.Windows.Forms.NotifyIcon notifyIcon;
        UpdateItem updateContent;
        const string UpdateUrl = "https://ogycode.github.io/WeatherWidget/update.json";
        int Major = 0, Minor = 0, Build = 0, Revision = 0;

        public MainWindow()
        {
            InitializeComponent();
            Model.ColorParser.Load();
        }

        public void Load(bool reload)
        {
            if (reload)
            {
                foreach (var item in wstorage.Widgets)
                    if (!item.Visible)
                        item.Destroy();
            }

            foreach (var item in wstorage.Widgets)
                if (item.Visible && !item.IsCreated)
                {
                    if (GetConnection())
                    {
                        item.CreateWindow();
                    }
                    else if (App.Settings.GetValue<bool>("ShowAlertInternetMsg"))
                        Dispatcher.Invoke(DispatcherPriority.Background,
                                          new Action(() => new Alert().ShowDialog(App.Lang["AlertTitle"], App.Lang["AlertNoInternet"])));
                }

            tbActiveWidgets.Text = $"{App.Lang["TabHomeActiveWidgets"]}{wstorage.Widgets.Count}";
            UpdateItemSource();
        }
        public void UpdateData()
        {
            if (GetConnection())
            {
                foreach (var item in wstorage.Widgets)
                    if (item.Visible)
                    {
                        item.UpdateData();
                        item.UpdateLook();
                    }
            }
            else if (App.Settings.GetValue<bool>("ShowAlertInternetMsg"))
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => new Alert().ShowDialog(App.Lang["AlertTitle"], App.Lang["AlertNoInternet"])));
        }
        public ImageSource GetIconFromRes(string name)
        {
            Uri oUri = new Uri($"pack://application:,,,/WeatherWidget2;component/Icons/{name}.png", UriKind.RelativeOrAbsolute);
            return BitmapFrame.Create(oUri);
        }
        public void UpdateItemSource()
        {
            int selected = lvWidgets.SelectedIndex;
            tbZeroWidgets.Visibility = wstorage.Widgets.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            lvWidgets.ItemsSource = null;
            lvWidgets.ItemsSource = wstorage.Widgets;
            lvWidgets.SelectedIndex = selected;
        }
        public double GetInMilisec(double minunte)
        {
            return 60000 * minunte;
        }
        public bool GetConnection()
        {
            bool result;
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("http://www.google.com"))
                    result = true;
            }
            catch { result = false; }

            Dispatcher.Invoke(DispatcherPriority.Background, new
             Action(() =>
             {
                 tbConnectionStatus.Text = result ?
                                           $"{App.Lang["TabHomeConnection"]}{App.Lang["TabHomeConnectionOK"]}" :
                                           $"{App.Lang["TabHomeConnection"]}{App.Lang["TabHomeConnectionNO"]}";
             }));

            return result;
        }

        void GetVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;
            Major = version.Major;
            Minor = version.Minor;
            Build = version.Build;
            Revision = version.Revision;
        }
        void SetTray()
        {
            if (notifyIcon != null)
                notifyIcon.Visible = false;

            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Text = "Weather Widget 2";
            notifyIcon.Icon = Properties.Resources.AppIcon;
            notifyIcon.DoubleClick += NotifyIconDoubleClick;
            notifyIcon.Visible = true;
            notifyIcon.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            notifyIcon.ContextMenuStrip.Items.Add(App.Lang["NotifyIconUpdate"]).Click += UpdateWidgetClick;
            notifyIcon.ContextMenuStrip.Items.Add(App.Lang["NotifyIconExit"]).Click += ExitAppClick;
        }
        void SetVersionData()
        {
            //info tab
            tbDevInfo.Text = $"{App.Lang["TabInfoDeveloped"]} Verloka";
            tbVersion.Text = $"{App.Lang["TabInfoVersion"]} {Major}.{Minor}.{Build}.{Revision}";
        }
        void SetLocale()
        {
            GetConnection();

            tbActiveWidgets.Text = $"{App.Lang["TabHomeActiveWidgets"]}{wstorage?.Widgets.Count}";
            btnUpdate.Text = App.Lang["TabHomeVersionBtnUpdate"];
            tbUpdateInfo.Text = App.Lang["TabHomeVersionOK"];
            tabHome.Header = App.Lang["GeneralTabHome"];
            tabWidgets.Header = App.Lang["GeneralTabWidgets"];
            tabOptions.Header = App.Lang["GeneralTabOptions"];
            tabInfo.Header = App.Lang["GeneralTabInformation"];
            tbWhatNew.Text = App.Lang["TabHomeWhatNew"];
            tbWidgets.Text = App.Lang["TabWIdgetsYourWidget"];
            tbZeroWidgets.Text = App.Lang["TabWIdgetsZeroWidgets"];
            cbStartup.Content = App.Lang["TabOptionsAutorun"];
            cbExit.Content = App.Lang["TabOptionsExit"];
            cbAlertInternet.Content = App.Lang["TabOptionsMsgInet"];
            tbLanguage.Text = App.Lang["TabOptionsLanguage"];
            tbTheme.Text = App.Lang["TabOptionsTheme"];
            cbiDark.Content = App.Lang["TabOptionsThemeDark"];
            cbiLight.Content = App.Lang["TabOptionsThemeLight"];

            SetVersionData();
            SetTray();
        }

        #region Window Events
        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            try { DragMove(); }
            catch { }
        }
        private void btnCloseClick()
        {
            if (App.Settings.GetValue<bool>("AppExit"))
            {
                Application.Current.Shutdown(0);
            }
            else
            {
                Hide();
            }
        }
        private void btnMinimazeClick()
        {
            WindowState = WindowState.Minimized;
        }
        #endregion

        private void mywindowLoaded(object sender, RoutedEventArgs e)
        {
            GetVersion();
            SetLocale();

            //widget storage
            wstorage = App.Settings.GetValue("widgets", new Model.WidgetStorage());
            wstorage.ListChangded += WstorageListChangded;

            //startup
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if ((string)key.GetValue("Weather Widget 2") == null)
                cbStartup.IsChecked = false;
            else
                cbStartup.IsChecked = true;
            cbStartup.Click += CbStartupClick;

            //wrong internet connection
            cbAlertInternet.IsChecked = App.Settings.GetValue("ShowAlertInternetMsg", false);
            cbAlertInternet.Click += CbAlertInternetClick;

            //lang
            cbLang.ItemsSource = App.Lang.AvailableLanguages;
            cbLang.SelectedItem = App.Lang.Current;
            cbLang.SelectionChanged += CbLangSelectionChanged;

            //theme
            cbTheme.SelectedIndex = App.Settings.GetValue<int>("Theme");
            cbTheme.SelectionChanged += CbThemeSelectionChanged;

            //app exit
            cbExit.IsChecked = App.Settings.GetValue("AppExit", false);
            cbExit.Click += CbExitClick;

            //set timer
            timer = new System.Timers.Timer(GetInMilisec(15d));
            timer.Elapsed += TimerElapsed;
            timer.Enabled = true;

            //app exit
            Application.Current.Exit += CurrentExit;

            //load widgets
            Load(false);
            //load weather data
            UpdateData();

            //check update
            using (UpdateClient updateClient = new UpdateClient(UpdateUrl))
            {
                updateClient.NewVersion += UpdateClientNewVersion;
                if (GetConnection())
                    updateClient.Check(new Verloka.HelperLib.Update.Version(Major, Minor, Build, Revision));
            }

            App.Lang.LanguageChanged += LangLanguageChanged;

            //new Alert().ShowDialog(App.Lang.AlertTitle, App.Lang.AlertNoInternet);
        }
        private void LangLanguageChanged(Verloka.HelperLib.Localization.Manager obj)
        {
            SetLocale();
        }
        private void ExitAppClick(object sender, EventArgs e)
        {
            Application.Current.Shutdown(0);
        }
        private void UpdateWidgetClick(object sender, EventArgs e)
        {
            UpdateData();
        }
        private void CbLangSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.Lang.SetCurrent((cbLang.SelectedItem as Verloka.HelperLib.Localization.Language).Code);
            App.Settings["LanguageCode"] = App.Lang.Current.Code;
        }
        private void UpdateClientNewVersion(UpdateItem obj)
        {
            tbUpdateInfo.Text = App.Lang["TabHomeVersionNO"];
            btnUpdate.Visibility = Visibility.Visible;
            updateContent = obj;
        }
        private void CbExitClick(object sender, RoutedEventArgs e)
        {
            App.Settings["AppExit"] = cbExit.IsChecked.Value;
        }
        private void NotifyIconDoubleClick(object sender, EventArgs e)
        {
            Show();
            if (WindowState == WindowState.Minimized)
                WindowState = WindowState.Normal;
        }
        private void CurrentExit(object sender, ExitEventArgs e)
        {
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
            notifyIcon = null;
            timer.Dispose();
            timer = null;
        }
        private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            UpdateData();
        }
        private void CbThemeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.Settings["Theme"] = cbTheme.SelectedIndex;
            App.UpdateTheme(cbTheme.SelectedIndex);
            this.UpdateDefaultStyle();
        }
        private void CbAlertInternetClick(object sender, RoutedEventArgs e)
        {
            App.Settings["ShowAlertInternetMsg"] = cbAlertInternet.IsChecked.Value;
        }
        private void CbStartupClick(object sender, RoutedEventArgs e)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (!cbStartup.IsChecked.Value)
                key.DeleteValue("Weather Widget 2", false);
            else
                key.SetValue("Weather Widget 2", $"\"{Assembly.GetExecutingAssembly().Location}\" -silent");
        }
        private void WstorageListChangded()
        {
            App.Settings["widgets"] = wstorage;
            tbActiveWidgets.Text = $"{App.Lang["TabHomeActiveWidgets"]}{wstorage.Widgets.Count}";
            UpdateItemSource();
        }
        private void bntAddWidgetClick()
        {
            if (GetConnection())
            {
                WidgetFactory wf = new WidgetFactory(this);
                wf.Show();
            }
            else
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => new Alert().ShowDialog(App.Lang["AlertTitle"], App.Lang["AlertNeedInternet"])));
        }
        private void lvWidgetsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lvWidgets.SelectedIndex == -1)
            {
                (bntRemoveWidget as UIElement).IsEnabled = false;
                bntVisibleWidget.Icon = GetIconFromRes("VisibleIcon");
                (bntVisibleWidget as UIElement).IsEnabled = false;
                (bntEditWidget as UIElement).IsEnabled = false;
                return;
            }
            (bntEditWidget as UIElement).IsEnabled = true;
            (bntRemoveWidget as UIElement).IsEnabled = true;
            bntVisibleWidget.Icon = (lvWidgets.SelectedItem as Model.Widget).Visible ? GetIconFromRes("VisibleIcon") : GetIconFromRes("HiddenIcon");
            (bntVisibleWidget as UIElement).IsEnabled = true;
        }
        private void bntVisibleWidgetClick()
        {
            if (lvWidgets.SelectedIndex == -1)
                return;

            wstorage.GetByUID((lvWidgets.SelectedItem as Model.Widget).guid).Visible = !wstorage.GetByUID((lvWidgets.SelectedItem as Model.Widget).guid).Visible;
            bntVisibleWidget.Icon = (lvWidgets.SelectedItem as Model.Widget).Visible ? GetIconFromRes("VisibleIcon") : GetIconFromRes("HiddenIcon");

            App.Settings["widgets"] = wstorage;
            Load(true);
        }
        private void bntRemoveWidgetClick()
        {
            if (lvWidgets.SelectedIndex == -1)
                return;

            wstorage.Remove((lvWidgets.SelectedItem as Model.Widget).guid);

            App.Settings["widgets"] = wstorage;
            Load(true);
            UpdateItemSource();
        }
        private void bntEditWidgetClick()
        {
            if (lvWidgets.SelectedIndex == -1)
                return;

            if (GetConnection())
            {
                WidgetFactory wf = new WidgetFactory(this, (lvWidgets.SelectedItem as Model.Widget));
                wf.Show();
            }
            else
                Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => new Alert().ShowDialog(App.Lang["AlertTitle"], App.Lang["AlertNeedInternet"])));
        }
        private void btnUpdateClick()
        {
            if (updateContent == null)
                return;

            notifyIcon.Visible = false;
            Hide();

            new UpdateWindow(updateContent).Show();
        }
        private void tbWhatNewClick(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://ogycode.github.io/WeatherWidget/new.html");
        }
    }
}
