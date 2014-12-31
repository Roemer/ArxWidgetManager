using System.Collections.Generic;

namespace ArxDiskMon
{
    public class AppSettings
    {
        public List<string> FilePaths { get; set; }

        public AppSettings()
        {
            FilePaths = new List<string> { @"C:\" };
        }
    }
}
