using ArxWidgetManager.Models;
using ArxWidgetManager.ViewModels;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ArxWidgetManager
{
    public static class SampleDataContext
    {
        public static MainViewModel SampleViewModel { get; private set; }

        static SampleDataContext()
        {
            var uriSource = new Uri(@"pack://application:,,,/ArxWidgetManager;component/LogitechG.ico");
            ImageSource img = new BitmapImage(uriSource);
            SampleViewModel = new MainViewModel();
            SampleViewModel.StatusText = "Some status text...";
            SampleViewModel.Widgets.Add(new WidgetViewModel(new Widget { Name = "Widget1", Version = "1.0.0.0", Image = img }));
            SampleViewModel.Widgets.Add(new WidgetViewModel(new Widget { Name = "Widget2", Version = "1.0.0.0", Image = img }));
            SampleViewModel.Widgets.Add(new WidgetViewModel(new Widget { Name = "Widget3", Version = "1.0.0.0", Image = img }));
            SampleViewModel.Widgets.Add(new WidgetViewModel(new Widget { Name = "Widget4", Version = "1.0.0.0", Image = img }));
            SampleViewModel.Widgets.Add(new WidgetViewModel(new Widget { Name = "Widget5", Version = "1.0.0.0", Image = img }));
        }
    }
}
