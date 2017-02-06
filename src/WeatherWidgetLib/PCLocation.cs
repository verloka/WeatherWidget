using System;
using System.Device.Location;

namespace WeatherWidgetLib
{
    public class PCLocation
    {
        public event Action<string> PositionChanged;
        GeoCoordinateWatcher watcher;

        public PCLocation()
        {
            watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            watcher.PositionChanged += WatcherPositionChanged;
            watcher.TryStart(false, TimeSpan.FromMilliseconds(5000));
        }

        private void WatcherPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            GeoCoordinate coord = watcher.Position.Location;
            PositionChanged?.Invoke($"{coord.Latitude.ToString().Replace(',', '.')},{coord.Longitude.ToString().Replace(',', '.')}");
        }
        public string GetCoordinate()
        {
            GeoCoordinate coord = watcher.Position.Location;
            return $"{coord.Latitude.ToString().Replace(',', '.')},{coord.Longitude.ToString().Replace(',', '.')}";
        }
        public void Stop()
        {
            watcher.Stop();
            watcher.Dispose();
        }
    }
}
