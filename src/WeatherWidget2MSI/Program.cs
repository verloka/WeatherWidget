using System;
using WixSharp;

namespace WeatherWidget2MSI
{
    class Program
    {
        static void Main(string[] args)
        {
            ProductInfo pi = new ProductInfo();
            pi.Manufacturer = "Verloka";
            pi.Name = "Weather Widget 2";
            pi.Contact = "Verloka Vadim";
            pi.Id = Guid.NewGuid().ToString();

            Project prj = new Project();

            prj.Name = "Weather Widget 2";
            prj.UI = WUI.WixUI_Mondo;
            prj.ControlPanelInfo = pi;
            prj.GUID = Guid.NewGuid();
            prj.Version = new Version(2, 0, 0, 0);

            
            Compiler.BuildMsi(prj);
        }
    }
}
