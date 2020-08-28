using System.Collections.Generic;

namespace Arx.Widget.DiskMon
{
    public class AppSettings
    {
        public List<string> FilePaths { get; set; }

        public AppSettings()
        {
            FilePaths = new List<string> { @"C:\", @"E:\", @"F:\" };
        }
    }
}
