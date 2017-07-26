using Microsoft.Deployment.WindowsInstaller;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using WixSharp;

namespace setup
{
    class Program
    {
        public const string Company = "Verloka";                        //Company name
        public const string Owner = "Verloka Vadim";                    //Publisher name
        public const string Product = "Weather Widget 2";               //Application name
        public const string ExeNameThisMoment = "Launcher.exe";         //EXE name, what launch app
        public static Version version = new Version(2, 3, 0, 2);        //Veriosn of app

        public const string RootPathThisMoment = @"C:\Users\verlo\Desktop\app\";
        public const string ExeIconPathThisMoment = @"C:\Projects\App\WeatherWidget\src\WeatherWidget2\Icons\AppIcon.ico";
        public const string LicencePathThisMoment = @"C:\Projects\App\WeatherWidget\src\WeatherWidget2MSI\Lic.rtf";

        static void Main(string[] args)
        {
            ProductInfo pi = new ProductInfo()
            {
                Manufacturer = Company,
                Name = Product,
                Contact = Owner,
                Id = Guid.NewGuid().ToString(),
                UrlInfoAbout = "https://ogycode.github.io/WeatherWidget/",
                UrlUpdateInfo = "https://ogycode.github.io/WeatherWidget/",
                ProductIcon = ExeIconPathThisMoment
            };
            Feature Conmplete = new Feature("Conmplete", "All files from project for Wiget can work", true);

            Project prj = new Project()
            {
                Name = Product,
                UI = WUI.WixUI_ProgressOnly,
                ControlPanelInfo = pi,
                GUID = Guid.NewGuid(),
                Version = version,
                LicenceFile = LicencePathThisMoment
            };



            Dir installDir = new Dir(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            Dir root = new Dir(Company);
            Dir rootProduct = GetDir(RootPathThisMoment, Conmplete, "*.dll|*.exe|*.ico|*.png|*.json|*.ini");
            rootProduct.Name = Product;
            root.Dirs = new Dir[] { rootProduct };
            installDir.Dirs = new Dir[] { root };

            Dir shortcuts = new Dir(@"%Desktop%", new ExeFileShortcut(Product, $"[INSTALLDIR]{ExeNameThisMoment}", ""));

            prj.Dirs = new Dir[] { installDir, shortcuts };

            prj.MajorUpgradeStrategy = MajorUpgradeStrategy.Default;
            prj.MajorUpgradeStrategy.RemoveExistingProductAfter = Step.InstallInitialize;
            prj.MajorUpgradeStrategy.PreventDowngradingVersions.OnlyDetect = false;

            prj.Actions = new WixSharp.Action[] { new ManagedAction(CustonActions.RunApp, Return.ignore, When.After, Step.PreviousActionOrInstallFinalize, Condition.NOT_Installed) };

            Compiler.BuildMsi(prj);
        }

        static Dir GetDir(string path, Feature f, string ex = "*.*")
        {
            DirectoryInfo di = new DirectoryInfo(path);
            Dir dir = new Dir(di.Name);

            List<WixSharp.File> files = new List<WixSharp.File>();
            foreach (var item in GetFiles(di.FullName, ex, SearchOption.TopDirectoryOnly))
            {
                files.Add(new WixSharp.File(f, item));
            }
            dir.Files = files.ToArray();

            List<Dir> dirs = new List<Dir>();
            foreach (var item in di.GetDirectories())
            {
                dirs.Add(GetDir(item.FullName, f, ex));
            }
            dir.Dirs = dirs.ToArray();

            return dir;
        }
        static string[] GetFiles(string SourceFolder, string Filter, SearchOption searchOption)
        {
            ArrayList alFiles = new ArrayList();
            string[] MultipleFilters = Filter.Split('|');
            foreach (string FileFilter in MultipleFilters)
                alFiles.AddRange(Directory.GetFiles(SourceFolder, FileFilter, searchOption));
            return (string[])alFiles.ToArray(typeof(string));
        }
    }

    public static class CustonActions
    {
        [CustomAction]
        public static ActionResult RunApp(Session session)
        {
            System.Diagnostics.Process.Start($"{session["INSTALLDIR"]}\\{ Program.ExeNameThisMoment}");
            return ActionResult.Success;
        }
    }
}
