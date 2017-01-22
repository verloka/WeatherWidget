using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using static WeatherWidget.Win32;

namespace WeatherWidget
{
    public partial class Widget : Window
    {
        public bool IsEdit { get; set; }
        public bool IsShow { get; set; }

        WidgetPosition pos;
        MainWindow mainWindow;

        public string ImageURL
        {
            get { return GetValue(ImageURLProperty) as string; }
            set
            {
                SetValue(ImageURLProperty, value);
            }
        }
        public static readonly DependencyProperty ImageURLProperty = DependencyProperty.Register("ImageURL", typeof(string), typeof(Widget), null);

        public Widget(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            pos = mainWindow.settings.GetValue("WidgetPosittion", new WidgetPosition() { Left = 100, Top = 100 });
            DataContext = this;
        }

        public void SetWidgetTextColor()
        {
            SolidColorBrush scb = new SolidColorBrush(mainWindow.settings.GetValue<WeatherColor>("TextColor").GetColor());

            tbCondition.Foreground = scb;
            tbLocation.Foreground = scb;
            tbThemperature.Foreground = scb;
        }
        public void SetWidgetBackgroundColor()
        {
            SolidColorBrush scb = new SolidColorBrush(mainWindow.settings.GetValue<WeatherColor>("BackgroundColor").GetColor());
            widgetBorder.Background = scb;
        }
        public void SetWidgetBorderColor()
        {
            SolidColorBrush scb = new SolidColorBrush(mainWindow.settings.GetValue<WeatherColor>("BorderColor").GetColor());
            widgetBorder.BorderBrush = scb;
        }
        public void SetWidgetBorder()
        {
            widgetBorder.BorderThickness = mainWindow.settings.GetValue<WidgetBorder>("WidgetBorder").GetBorder();
        }
        public void Update(string t, string c, string l, string u)
        {
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                tbThemperature.Text = t;
                tbCondition.Text = c;
                tbLocation.Text = l;

                if(mainWindow.settings.GetValue<bool>("LoadIcon"))
                {
                    ImageURL = u;
                    imgIcon.Visibility = Visibility.Visible;
                }
                else
                {
                    ImageURL = "";
                    imgIcon.Visibility = Visibility.Collapsed;
                }

                tbThemperature.Visibility = mainWindow.settings.GetValue<bool>("ShowThemperatue") ? Visibility.Visible : Visibility.Collapsed;
                tbCondition.Visibility = mainWindow.settings.GetValue<bool>("ShowCondition") ? Visibility.Visible : Visibility.Collapsed;
                tbLocation.Visibility = mainWindow.settings.GetValue<bool>("ShowLocation") ? Visibility.Visible : Visibility.Collapsed;
            }));
        }
        public void EditMode(bool edit)
        {
            IsEdit = edit;

            gridHeader.Visibility = IsEdit ? Visibility.Visible : Visibility.Collapsed;
            windowBorder.BorderBrush = IsEdit ? (SolidColorBrush)FindResource("WidgetBorderColorActive") : (SolidColorBrush)FindResource("WidgetBorderColorInactive");
            rootGrid.Background = IsEdit ? (SolidColorBrush)FindResource("RootGridColorActive") : (SolidColorBrush)FindResource("RootGridColorInactive");
        }
        public void ShowWidget(bool show)
        {
            IsShow = show;

            if (IsShow)
                Show();
            else
                Hide();
        }

        private void gridHeaderMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
                mainWindow.settings["WidgetPosittion"] = new WidgetPosition() { Left = (int)Left, Top = (int)Top };
            }
            catch { }
        }
        private void widgetLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                WindowInteropHelper wndHelper = new WindowInteropHelper(this);

                int exStyle = (int)GetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE);

                exStyle |= (int)ExtendedWindowStyles.WS_EX_TOOLWINDOW;
                SetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);
            }
            catch { }

            gridHeader.Visibility = Visibility.Collapsed;
            Left = pos.Left;
            Top = pos.Top;
        }
    }
}
