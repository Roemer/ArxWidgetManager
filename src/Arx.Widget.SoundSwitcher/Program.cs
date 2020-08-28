using System;
using Arx.Shared;

namespace Arx.Widget.SoundSwitcher
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
