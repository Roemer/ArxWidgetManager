using Arx.Shared;
using System;

namespace ArxSoundSwitcher
{
    class Program
    {
        static void Main(string[] args)
        {
            WidgetBase widget = new ArxSoundSwitcherWidget();
            widget.Start();
            Console.Read();
            widget.Stop();
        }
    }
}
