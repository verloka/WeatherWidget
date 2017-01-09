using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WeatherWidget
{
    public partial class App : Application
    {
        void AppStartup(object sender, StartupEventArgs e)
        {
            bool silent = false;
            for (int i = 0; i != e.Args.Length; ++i)
                if (e.Args[i] == "-silent")
                    silent = true;

            MainWindow mainWindow = new MainWindow();
            mainWindow.WindowState = silent ? WindowState.Minimized : WindowState.Normal;
            mainWindow.Show();
            if (silent)
                mainWindow.Hide();
        }
    }
}
