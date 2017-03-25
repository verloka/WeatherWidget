using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WeatherWidget2
{
    public partial class App : Application
    {
        public static Model.Localization Lang;
        public static Verloka.HelperLib.Settings.RegSettings Settings;

        void AppStartup(object sender, StartupEventArgs e)
        {
            bool silent = false;
            for (int i = 0; i != e.Args.Length; ++i)
                if (e.Args[i] == "-silent")
                    silent = true;


            //INIT block
            Lang = new Model.Localization();
            Settings = new Verloka.HelperLib.Settings.RegSettings("Weather widget 2");

            MainWindow mainWindow = new MainWindow();
            mainWindow.WindowState = silent ? WindowState.Minimized : WindowState.Normal;
            mainWindow.Show();
            if (silent)
                mainWindow.Hide();
        }
    }
}
