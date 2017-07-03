using System.Collections.Generic;

namespace WeatherWidget2.Model
{
    public interface IWidgetView
    {
        int Type { get; }

        void Edit(bool mode);
        int GetLeft();
        int GetTop();
        void SetLeft(int left);
        void SetTop(int top);
        void DestroyView();
        void UpdateInfo(Dictionary<string, object> param);
        void UpdateLook(Dictionary<string, object> param);
        void ShowWidget();
    }
}
