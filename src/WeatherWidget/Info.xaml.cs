using System.Windows;
using System.Windows.Controls;

namespace WeatherWidget
{
    public partial class Info : Window
    {
        public Info()
        {
            InitializeComponent();
        }

        private void tbVersionLoaded(object sender, RoutedEventArgs e)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var version = assembly.GetName().Version;

            (sender as TextBlock).Text = $"{version.Major}.{version.Minor}.{version.MajorRevision}.{version.MinorRevision}";
        }
        private void AppPageClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://ogycode.github.io/WeatherWidget");
        }
        private void AuthorPageClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://verloka.github.io/");
        }
    }
}
