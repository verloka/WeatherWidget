using System;
using System.Device.Location;

namespace WeatherWidgetLib
{
    public class PCLocation
    {
        GeoCoordinateWatcher watcher;

        public PCLocation()
        {
            watcher = new GeoCoordinateWatcher();
            //watcher.TryStart(false, TimeSpan.FromMilliseconds(2000));
        }

        public string GetCoordinate()
        {
            watcher.TryStart(false, TimeSpan.FromMilliseconds(2000));
            GeoCoordinate coord = watcher.Position.Location;
            return $"{coord.Latitude.ToString().Replace(',','.')},{coord.Longitude.ToString().Replace(',', '.')}";
        }
    }
}
