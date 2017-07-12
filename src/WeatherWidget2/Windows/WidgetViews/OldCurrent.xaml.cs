using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WeatherWidget2.Model;
using WeatherWidget2ResourceLib;
using static WeatherWidget2.Win32;

namespace WeatherWidget2.Windows.WidgetViews
{
    public partial class OldCurrent : Window, IWidgetView
    {
        public Icons icons;

        public OldCurrent()
        {
            InitializeComponent();
            icons = new Icons();
        }
        public OldCurrent(IconSize s, IconTheme t)
        {
            InitializeComponent();
            icons = new Icons(s, t);
        }

        #region IWidgetView
        public int Type => 0;

        public void DestroyView()
        {
            Close();
        }
        public void Edit(bool mode)
        {
            gridHeader.Visibility = mode ? Visibility.Visible : Visibility.Collapsed;
        }
        public int GetLeft()
        {
            return (int)Left;
        }
        public int GetTop()
        {
            return (int)Top;
        }
        public void SetLeft(int left)
        {
            Left = left;
        }
        public void SetTop(int top)
        {
            Top = top;
        }
        public void ShowWidget()
        {
            Show();
        }
        public void UpdateInfo(Dictionary<string, object> param)
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new
             Action(() =>
             {
                 tbThemperature.Text = param["Themperature"].ToString();
                 tbCondition.Text = param["WeatherParam"].ToString();
                 tbLocation.Text = param["Location"].ToString();
             }));
        }
        public void UpdateLook(Dictionary<string, object> param)
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new
             Action(() =>
             {
                 icons.UpdateData((IconSize)param["Size"], (IconTheme)param["Theme"]);

                 BitmapImage bitmap = new BitmapImage();
                 bitmap.BeginInit();
                 bitmap.UriSource = icons.GetIcon(param["Icon"].ToString());
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

                 SolidColorBrush scb = new SolidColorBrush(ColorParser.FromName(param["TextColor"].ToString()));
                 tbThemperature.Foreground = scb;
                 tbCondition.Foreground = scb;
                 tbLocation.Foreground = scb;
             }));
        }
        #endregion

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
