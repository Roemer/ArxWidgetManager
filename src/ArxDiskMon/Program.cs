using Arx.Shared;
using System;

namespace ArxDiskMon
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
