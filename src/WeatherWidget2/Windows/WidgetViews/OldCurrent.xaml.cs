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

        public int Type => 0;

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
        public void UpdateLook(List<object> param)
        {
            icons.UpdateData((IconSize)param[0], (IconTheme)param[1]);

            Dispatcher.Invoke(DispatcherPriority.Background, new
             Action(() =>
             {
                 SolidColorBrush scb = new SolidColorBrush(Model.ColorParser.FromName(param[2].ToString()));
                 tbThemperature.Foreground = scb;
                 tbCondition.Foreground = scb;
                 tbLocation.Foreground = scb;
             }));
        }
        public void UpdateInfo(List<object> param)
        {
            Dispatcher.Invoke(DispatcherPriority.Background, new
             Action(() =>
             {
                 BitmapImage bitmap = new BitmapImage();
                 bitmap.BeginInit();
                 bitmap.UriSource = icons.GetIcon(param[0].ToString());
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

                 tbThemperature.Text = param[1].ToString();
                 tbCondition.Text = param[2].ToString();
                 tbLocation.Text = param[3].ToString();
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
