using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WeatherWidget2ResourceLib;
using static WeatherWidget2.Win32;

namespace WeatherWidget2.Windows
{
    public partial class WidgetCurrent : Window
    {
        public Icons icons;

        public WidgetCurrent(IconSize iconSize, IconTheme iconTheme)
        {
            InitializeComponent();

            icons = new Icons(iconSize, iconTheme);
        }

        public void UpdateInfo(string temp, string condi, string loc)
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new
             Action(() =>
             {
                 tbThemperature.Text = temp;
                 tbCondition.Text = condi;
                 tbLocation.Text = loc;
             }));
        }
        public void UpdateLook(string icon)
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new
             Action(() =>
             {
                 BitmapImage bitmap = new BitmapImage();
                 bitmap.BeginInit();
                 bitmap.UriSource = icons.GetIcon(icon);
                 bitmap.EndInit();
                 imgIcon.Source = bitmap;

                 switch (icons.GetSize())
                 {
                     case 64:
                     default:
                         imgIcon.Width = 64;
                         imgIcon.Height = 64;

                         tbThemperature.FontSize = 36;
                         tbCondition.FontSize = 24;
                         tbLocation.FontSize = 16;
                         break;
                     case 32:
                         imgIcon.Width = 32;
                         imgIcon.Height = 32;

                         tbThemperature.FontSize = 30;
                         tbCondition.FontSize = 20;
                         tbLocation.FontSize = 14;
                         break;
                     case 128:
                         imgIcon.Width = 128;
                         imgIcon.Height = 128;

                         tbThemperature.FontSize = 56;
                         tbCondition.FontSize = 36;
                         tbLocation.FontSize = 24;
                         break;
                 }
             }));
        }
        public void UpdateTextColor(string name)
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new
             Action(() =>
             {
                 SolidColorBrush scb = new SolidColorBrush(Model.ColorParser.FromName(name));
                 tbThemperature.Foreground = scb;
                 tbCondition.Foreground = scb;
                 tbLocation.Foreground = scb;
             }));
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
        }
    }
}
