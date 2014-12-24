using ArxWidgetManager.Core;
using System.Collections.ObjectModel;

namespace ArxWidgetManager.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public ObservableCollection<WidgetViewModel> Widgets { get; set; }
        public string StatusText
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public MainViewModel()
        {
            Widgets = new ObservableCollection<WidgetViewModel>();
        }
    }
}
