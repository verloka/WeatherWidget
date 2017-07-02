namespace WeatherWidget2.Model
{
    public interface IWidgetView
    {
        void Edit(bool mode);
        int GetLeft();
        int GetTop();
        void SetLeft();
        void SetTop();
        void DestroyView();
        void UpdateInfo();
        void UpdateIcon();
        void UpdateIconKind();
        void ShowWidget();
    }
}
