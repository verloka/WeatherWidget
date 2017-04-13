using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
            {
                foreach (var item in wstorage.Widgets)
                    if (!item.Visible)
                        item.Destroy();
            }

            foreach (var item in wstorage.Widgets)
                if (item.Visible && !item.IsCreated)
                    item.CreateWindow();

            UpdateItemSource();
        }
        public void UpdateData()
        {
            foreach (var item in wstorage.Widgets)
                if (item.Visible)
                    item.UpdateData();
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
            UpdateItemSource();
        }
        private void bntAddWidgetClick()
        {
            Windows.WidgetFactory wf = new Windows.WidgetFactory(this);
            wf.Show();
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

            settings["widgets"] = wstorage;
            Load(true);
        }
        private void bntRemoveWidgetClick()
        {
            if (lvWidgets.SelectedIndex == -1)
                return;

            wstorage.Remove((lvWidgets.SelectedItem as Model.Widget).guid);

            settings["widgets"] = wstorage;
            Load(true);
            UpdateItemSource();
        }
    }
}
