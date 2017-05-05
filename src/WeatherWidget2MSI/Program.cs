using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using WixSharp;

namespace WeatherWidget2MSI
{
    class Program
    {
        static void Main(string[] args)
        {
            const string Company = "Verloka";
            const string Owner = "Verloka Vadim";
            const string Product = "Weather Widget 2";
            const string RootPathThisMoment = @"C:\Projects\Windows\WeatherWidget\src\WeatherWidget2\bin\Debug\";
            const string ExeIconPathThisMoment = @"C:\Projects\Windows\WeatherWidget\src\WeatherWidget2\bin\Debug\";
            const string ExeNameThisMoment = "WeatherWidget2.exe";

            ProductInfo pi = new ProductInfo();
            pi.Manufacturer = Company;
            pi.Name = Product;
            pi.Contact = Owner;
            pi.Id = Guid.NewGuid().ToString();
            pi.UrlInfoAbout = "https://ogycode.github.io/WeatherWidget/";
            pi.UrlUpdateInfo = "https://ogycode.github.io/WeatherWidget/";
            pi.ProductIcon = ExeIconPathThisMoment;

            Feature Conmplete = new Feature("Conmplete", "All files from project for Wiget can work", true);

            Project prj = new Project();

            prj.Name = "Weather Widget 2";
            prj.UI = WUI.WixUI_InstallDir;
            prj.ControlPanelInfo = pi;
            prj.GUID = Guid.NewGuid();
            prj.Version = new Version(2, 0, 0, 0);

            Dir installDir = new Dir("%ProgramFiles%");
            Dir root = new Dir(Company);
            Dir rootProduct = getDir(RootPathThisMoment, Conmplete, "*.dll|*.exe|*.ico|*.png|*.json");
            rootProduct.Name = Product;
            root.Dirs = new Dir[] { rootProduct };
            installDir.Dirs = new Dir[] { root };

            Dir shortcuts = new Dir(@"%Desktop%",
                        new ExeFileShortcut(Product, $"[INSTALLDIR]{ExeNameThisMoment}", ""));

            prj.Dirs = new Dir[] { installDir, shortcuts };

            Compiler.BuildMsi(prj);
        }

        static Dir getDir(string path, Feature f, string ex = "*.*")
        {
            DirectoryInfo di = new DirectoryInfo(path);
            Dir dir = new Dir(di.Name);

            List<WixSharp.File> files = new List<WixSharp.File>();
            foreach (var item in getFiles(di.FullName, ex, SearchOption.TopDirectoryOnly))
            {
                files.Add(new WixSharp.File(f, item));
            }
            dir.Files = files.ToArray();

            List<Dir> dirs = new List<Dir>();
            foreach (var item in di.GetDirectories())
            {
                dirs.Add(getDir(item.FullName, f, ex));
            }
            dir.Dirs = dirs.ToArray();

            return dir;
        }
        static string[] getFiles(string SourceFolder, string Filter, SearchOption searchOption)
        {
            ArrayList alFiles = new ArrayList();
            string[] MultipleFilters = Filter.Split('|');
            foreach (string FileFilter in MultipleFilters)
                alFiles.AddRange(Directory.GetFiles(SourceFolder, FileFilter, searchOption));
            return (string[])alFiles.ToArray(typeof(string));
        }
    }
}
