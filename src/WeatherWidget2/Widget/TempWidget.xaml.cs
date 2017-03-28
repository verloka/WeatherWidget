using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using static WeatherWidget2.Win32;

namespace WeatherWidget2.Widget
{
    public partial class TempWidget : Window
    {
        public TempWidget()
        {
            InitializeComponent();
        }

        public void UpdateInfo(string temp, string condit, string location)
        {
            tbThemperature.Text = temp;
            tbCondition.Text = condit;
            tbLocation.Text = location;
        }
        public void SetIcon(string url)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(url, UriKind.Absolute);
            bitmap.EndInit();
            imgIcon.Source = bitmap;
        }
        public void Edit(bool edit)
        {
            gridHeader.Visibility = edit ? Visibility.Visible : Visibility.Collapsed;
        }

        private void gridHeaderMouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
                App.Settings["Temp_widgetPos"] = new Model.Point((int)Left, (int)Top);
            }
            catch { }
        }
        private void windowLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                WindowInteropHelper wndHelper = new WindowInteropHelper(this);

                int exStyle = (int)GetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE);

                exStyle |= (int)ExtendedWindowStyles.WS_EX_TOOLWINDOW;
                SetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);
            }
            catch { }

            Model.Point p = App.Settings.GetValue<Model.Point>("Temp_widgetPos");

            Left = p.X;
            Top = p.Y;
        }
    }
}
