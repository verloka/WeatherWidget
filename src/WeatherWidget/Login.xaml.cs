using System.Windows;

namespace WeatherWidget
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLoginClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbKey.Text) && !string.IsNullOrWhiteSpace(tbLogin.Text))
            {
                Properties.Settings.Default.ApixuKey = tbKey.Text;
                Properties.Settings.Default.GeonamesLogin = tbLogin.Text;
                Properties.Settings.Default.Save();
                DialogResult = true;
            }
        }
    }
}
