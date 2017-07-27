using System;
using System.Collections.Generic;
using System.Windows;

namespace WeatherWidget2
{
    public partial class App : Application
    {
        public static Verloka.HelperLib.Settings.RegSettings Settings;
        public static Verloka.HelperLib.Localization.Manager Lang;
        public static List<string> Languages;

        public static void UpdateTheme(int num)
        {
            switch (num)
            {
                case 0:
                default:
                    ResourceDictionary dark = new ResourceDictionary();
                    dark.Source = new Uri("Theme\\Dark.xaml", UriKind.Relative);
                    Current.Resources.MergedDictionaries.Clear();
                    Current.Resources.MergedDictionaries.Add(dark);
                    break;
                case 1:
                    ResourceDictionary light = new ResourceDictionary();
                    light.Source = new Uri("Theme\\Light.xaml", UriKind.Relative);
                    Current.Resources.MergedDictionaries.Clear();
                    Current.Resources.MergedDictionaries.Add(light);
                    break;
                case 2:
                    ResourceDictionary purple = new ResourceDictionary();
                    purple.Source = new Uri("Theme\\Purple.xaml", UriKind.Relative);
                    Current.Resources.MergedDictionaries.Clear();
                    Current.Resources.MergedDictionaries.Add(purple);
                    break;
                case 3:
                    ResourceDictionary red = new ResourceDictionary();
                    red.Source = new Uri("Theme\\Red.xaml", UriKind.Relative);
                    Current.Resources.MergedDictionaries.Clear();
                    Current.Resources.MergedDictionaries.Add(red);
                    break;
                case 4:
                    ResourceDictionary indigo = new ResourceDictionary();
                    indigo.Source = new Uri("Theme\\Indigo.xaml", UriKind.Relative);
                    Current.Resources.MergedDictionaries.Clear();
                    Current.Resources.MergedDictionaries.Add(indigo);
                    break;
                case 5:
                    ResourceDictionary cyna = new ResourceDictionary();
                    cyna.Source = new Uri("Theme\\Cyna.xaml", UriKind.Relative);
                    Current.Resources.MergedDictionaries.Clear();
                    Current.Resources.MergedDictionaries.Add(cyna);
                    break;
                case 6:
                    ResourceDictionary amber = new ResourceDictionary();
                    amber.Source = new Uri("Theme\\Amber.xaml", UriKind.Relative);
                    Current.Resources.MergedDictionaries.Clear();
                    Current.Resources.MergedDictionaries.Add(amber);
                    break;
            }
        }

        void AppStartup(object sender, StartupEventArgs e)
        {
            bool silent = false;
            for (int i = 0; i != e.Args.Length; ++i)
                if (e.Args[i] == "-silent")
                    silent = true;


            //INIT block
            Settings = new Verloka.HelperLib.Settings.RegSettings("Weather Widget 2");
            UpdateTheme(Settings.GetValue("Theme", 0));
            Lang = new Verloka.HelperLib.Localization.Manager($@"{AppDomain.CurrentDomain.BaseDirectory}Lang\locales.ini");
            Lang.Load();
            Lang.SetCurrent(Settings.GetValue("LanguageCode", "en-us"));

            MainWindow mainWindow = new MainWindow();
            mainWindow.WindowState = silent ? WindowState.Minimized : WindowState.Normal;
            mainWindow.Show();
            if (silent)
                mainWindow.Hide();
        }
    }
}
