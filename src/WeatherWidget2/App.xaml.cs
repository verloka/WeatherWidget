using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WeatherWidget2
{
    public partial class App : Application
    {
        public static Model.Localization Lang;
        public static Verloka.HelperLib.Settings.RegSettings Settings;
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
            }
        }
        public static void UpdateLang(string lang)
        {
            Lang = new Model.Localization();
            DirectoryInfo di = new DirectoryInfo($"{AppDomain.CurrentDomain.BaseDirectory}\\Lang\\");
            var files = di.GetFiles();
            Languages = new List<string>(files.Length);
            foreach (var item in files)
            {
                string name = item.Name.Replace(item.Extension, "");
                Languages.Add(name);
                if (name == lang)
                    Lang = JsonConvert.DeserializeObject<Model.Localization>(File.ReadAllText(item.FullName));
            }
            if (Lang == null)
                Lang = new Model.Localization();
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
            UpdateLang(Settings.GetValue("Language", "English"));

            MainWindow mainWindow = new MainWindow();
            mainWindow.WindowState = silent ? WindowState.Minimized : WindowState.Normal;
            mainWindow.Show();
            if (silent)
                mainWindow.Hide();
        }
    }
}
