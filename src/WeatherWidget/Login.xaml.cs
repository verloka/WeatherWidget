using System.Windows;

namespace WeatherWidget
{
    public partial class Login : Window
    {
        MainWindow mainWindow;

        public Login(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void btnLoginClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbKey.Text) && !string.IsNullOrWhiteSpace(tbLogin.Text))
            {
                mainWindow.Key = tbKey.Text;
                mainWindow.Login = tbLogin.Text;
                DialogResult = true;
            }
        }
        private void tBLoginClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://ogycode.github.io/WeatherWidget/login.html");
        }
        private void tbKey1Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("https://ogycode.github.io/WeatherWidget/key.html");
        }
    }
}
