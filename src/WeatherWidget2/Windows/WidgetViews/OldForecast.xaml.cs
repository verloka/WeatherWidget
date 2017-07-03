using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WeatherWidget2.Model;
using static WeatherWidget2.Win32;

namespace WeatherWidget2.Windows.WidgetViews
{
    public partial class OldForecast : Window, IWidgetView
    {
        ChartValues<int> Values;
        List<ForecastOneDay> Days;
        public string[] Labels { get; set; }

        public OldForecast()
        {
            InitializeComponent();
            DataContext = this;
        }

        public int Type => 1;

        public void DestroyView()
        {
            Close();
        }

        #region IWidgetView
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
            Values = new ChartValues<int>();
            
            Days = (List<ForecastOneDay>)param["Days"];

            Labels = Days[0].Labels.ToArray();

            foreach (var item in Days[0].Values)
                Values.Add(item);

            chartLine.Values = Values;
            chartXAxis.Labels = Labels;
        }
        public void UpdateLook(Dictionary<string, object> param)
        {
            chartLine.DataLabels = true;
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
