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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Verloka.HelperLib.Settings;

namespace WeatherWidget2
{
    public partial class MainWindow : Window
    {
        public RegSettings settings;
        public Model.WidgetStorage wstorage;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = App.Lang;
            settings = new RegSettings("Weather Widget 2");
        }

        public void Load(bool reload)
        {
            if (reload)
                foreach (var item in wstorage.Widgets)
                    item.Destroy();

            foreach (var item in wstorage.Widgets)
                if (item.Visible)
                    item.CreateWindow();

            lvWidgets.ItemsSource = wstorage.Widgets;
            tbZeroWidgets.Visibility = wstorage.Widgets.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
        }
        public void UpdateData()
        {
            foreach (var item in wstorage.Widgets)
                if(item.Visible)
                    item.UpdateData();
        }

        #region Window Events
        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            try { DragMove(); }
            catch { }
        }
        private void btnCloseClick()
        {
            //TODO
            Application.Current.Shutdown(0);
        }
        private void btnMinimazeClick()
        {
            WindowState = WindowState.Minimized;
        }
        #endregion

        private void mywindowLoaded(object sender, RoutedEventArgs e)
        {
            wstorage = settings.GetValue("widgets", new Model.WidgetStorage());
            wstorage.ListChangded += WstorageListChangded;

            Load(false);
            UpdateData();
        }
        private void WstorageListChangded()
        {
            settings["widgets"] = wstorage;
        }
        private void bntAddWidgetClick()
        {
            Windows.WidgetFactory wf = new Windows.WidgetFactory(this);
            wf.Show();
        }
    }
}
