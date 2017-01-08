using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static WeatherWidget.Win32;

namespace WeatherWidget
{
    public partial class Widget : Window
    {
        public bool IsEdit { get; set; }
        public bool IsShow { get; set; }

        public Widget()
        {
            InitializeComponent();
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
        public void Update(string t, string c, string l, BitmapImage img)
        {
            tbThemperature.Text = t;
            tbCondition.Text = c;
            tbLocation.Text = l;
            imgIcon.Source = img;
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
