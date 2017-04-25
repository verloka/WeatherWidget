using System.Windows;
using System.Windows.Input;

namespace WeatherWidget2.Windows
{
    public partial class Alert : Window
    {
        public Alert()
        {
            InitializeComponent();
        }

        public bool? ShowDialog(string Title, string Message)
        {
            this.Title = Title;
            tbMessage.Text = Message;

            return ShowDialog();
        }

        #region Window Events
        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            try { DragMove(); }
            catch { }
        }
        private void btnCloseClick()
        {
            Close();
        }
        private void btnMinimazeClick()
        {
            WindowState = WindowState.Minimized;
        }
        #endregion
    }
}
