using System;
using Arx.Shared;

namespace Arx.Widget.DiskMon
{
    class Program
    {
        static void Main(string[] args)
        {
            WidgetBase widget = new ArxDiskMonWidget();
            widget.Start();
            Console.Read();
            widget.Stop();
        }
    }
}
