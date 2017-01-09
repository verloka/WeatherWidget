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

        public string ImageURL
        {
            get { return GetValue(ImageURLProperty) as string; }
            set
            {
                SetValue(ImageURLProperty, value);
            }
        }
        public static readonly DependencyProperty ImageURLProperty = DependencyProperty.Register("ImageURL", typeof(string), typeof(Widget), null);

        public Widget()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void SetWidgetTextColor()
        {
            SolidColorBrush scb = new SolidColorBrush(new Color()
            {
                A = Properties.Settings.Default.TextColorA,
                R = Properties.Settings.Default.TextColorR,
                G = Properties.Settings.Default.TextColorG,
                B = Properties.Settings.Default.TextColorB
            });

            tbCondition.Foreground = scb;
            tbLocation.Foreground = scb;
            tbThemperature.Foreground = scb;
        }
        public void Update(string t, string c, string l, string u)
        {
            Dispatcher.BeginInvoke(new Action(delegate ()
            {
                tbThemperature.Text = t;
                tbCondition.Text = c;
                tbLocation.Text = l;

                if(Properties.Settings.Default.LoadIcon)
                {
                    ImageURL = u;
                    imgIcon.Visibility = Visibility.Visible;
                }
                else
                {
                    ImageURL = "";
                    imgIcon.Visibility = Visibility.Collapsed;
                }

                tbThemperature.Visibility = Properties.Settings.Default.ShowThemperatue ? Visibility.Visible : Visibility.Collapsed;
                tbCondition.Visibility = Properties.Settings.Default.ShowCondition ? Visibility.Visible : Visibility.Collapsed;
                tbLocation.Visibility = Properties.Settings.Default.ShowLocation ? Visibility.Visible : Visibility.Collapsed;
            }));
        }
        public void EditMode(bool edit)
        {
            IsEdit = edit;
            gridHeader.Visibility = IsEdit ? Visibility.Visible : Visibility.Collapsed;
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
                Properties.Settings.Default.WidgetLeft = Left;
                Properties.Settings.Default.WidgetTop = Top;
            }
            catch { }
            finally
            {
                Properties.Settings.Default.Save();
            }
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
            Left = Properties.Settings.Default.WidgetLeft;
            Top = Properties.Settings.Default.WidgetTop;
        }
    }
}
