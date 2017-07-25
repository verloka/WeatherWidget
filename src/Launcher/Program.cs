using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Launcher
{
    class Program
    {
        public const string APP_NAME = "WeatherWidget2.exe";

        static void Main(string[] args)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;

            string arg = "";
            foreach (var item in args)
                arg += $"{item} ";

            string[] dirs = Directory.GetDirectories(path);

            List<Version> vers = new List<Version>();
            foreach (var item in dirs)
            {
                string oldName = new DirectoryInfo(item).Name;
                string str = new Regex(@"(?<Major>\d*)\.(?<Minor>\d*)\.(?<Build>\d*)\.(?<Revision>\d*)").Match(item).Value;
                if (oldName == str && !string.IsNullOrWhiteSpace(str))
                    vers.Add(new Version(str));
            }

            if (vers.Count == 1)
                Process.Start($@"{path}\{vers[0].ToString()}\{APP_NAME}", arg);
            else if (vers.Count > 1)
            {
                Version v = vers.Max();

                foreach (var item in vers)
                    if (item != v)
                        Directory.Delete($@"{path}\{item.ToString()}", true);

                Process.Start($@"{path}\{v.ToString()}\{APP_NAME}", arg);
            }

            Environment.Exit(0);
        }
    }
}
