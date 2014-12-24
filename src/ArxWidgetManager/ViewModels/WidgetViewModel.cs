﻿using ArxWidgetManager.Core;
using ArxWidgetManager.Models;
using System.Windows.Input;
using System.Windows.Media;

namespace ArxWidgetManager.ViewModels
{
    public class WidgetViewModel
    {
        private readonly Widget _widget;

        public string Name { get { return _widget.Name; } }
        public string Version { get { return _widget.Version; } }
        public ImageSource Image { get { return _widget.Image; } }

        public ICommand ClickCommand { get; private set; }

        public WidgetViewModel(Widget widget)
        {
            _widget = widget;
            ClickCommand = new RelayCommand(param =>
            {
                var isChecked = (bool)param;
                if (isChecked)
                {
                    _widget.Start();
                }
                else
                {
                    _widget.Stop();
                }
            });
        }
    }
}