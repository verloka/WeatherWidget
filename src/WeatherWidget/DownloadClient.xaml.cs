using System.Diagnostics;
using System.Windows;
using Verloka.HelperLib.Update;

namespace WeatherWidget
{
    public partial class DownloadClient : Window
    {
        bool Update = true;
        string path = System.IO.Path.GetTempPath();

        public DownloadClient(UpdateItem item)
        {
            InitializeComponent();

            Verloka.HelperLib.Update.DownloadClient dc = new Verloka.HelperLib.Update.DownloadClient(item.Files, path);
            dc.DownloadProgress += Dc_DownloadProgress;
            dc.DownloadCompleted += Dc_DownloadCompleted;

            dc.Start();
        }

        private void Dc_DownloadCompleted()
        {
            Update = false;
            btnUpdate.IsEnabled = true;
        }
        private void Dc_DownloadProgress(string name, int perc, double speed)
        {
            tbInfo.Text = $"{name} / {speed.ToString("0")} kB";
            pbProgress.Value = perc;
        }
        private void windowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Update)
            {
                e.Cancel = true;
            }
        }
        private void btnUpdateClick(object sender, RoutedEventArgs e)
        {
            Process.Start($"{path}WeatherWidgetSetup.msi.exe");
            Application.Current.Shutdown(0);
        }
    }
}
