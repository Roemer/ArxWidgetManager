using System;
using Arx.Shared;

namespace Arx.Widget.Clock
{
    class Program
    {
        static void Main(string[] args)
        {
            WidgetBase widget = new ArxClockWidget();
            widget.Start();
            Console.Read();
            widget.Stop();
        }
    }
}
