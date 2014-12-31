using Arx.Shared;
using FlauLib.Tools;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ArxDiskMon
{
    public class ArxDiskMonWidget : WorkerWidgetBase
    {
        [DllImport("kernel32")]
        public static extern int GetDiskFreeSpaceEx(string lpDirectoryName, ref long lpFreeBytesAvailable, ref long lpTotalNumberOfBytes, ref long lpTotalNumberOfFreeBytes);

        protected override string Identifier
        {
            get { return "flauschig.arxdiskmon"; }
        }

        protected override string Name
        {
            get { return "ArxDiskMon"; }
        }

        private readonly AppSettings _settings;

        public ArxDiskMonWidget()
            : base(5000)
        {
            _settings = PortableConfiguration.Load<AppSettings>("General", "ArxDiskMonSettings");
        }

        protected override void DoWork()
        {
            var sb = new StringBuilder();
            foreach (var diskName in _settings.FilePaths)
            {
                if (String.IsNullOrWhiteSpace(diskName)) { continue; }
                var diskSpace = GetDiskSpace(diskName);
                sb.AppendFormat(@"{3}: {0} / {1} ({2}%)\r\n", SizeSuffix(diskSpace.FreeBytes, 2), SizeSuffix(diskSpace.TotalBytes, 2), Math.Round(diskSpace.UsedBytes / (double)diskSpace.TotalBytes * 100, 2), diskName);
            }

            SetContentById("contentDiv", sb.ToString());
        }

        protected override void DeviceConnected(LogiArxDeviceType deviceType)
        {
            bool a = Add(EmbeddedResourceReader.GetBytes("ArxDiskMon.Resources.index.html"), "index.html");
            a = SetIndex("index.html");
        }

        private DiskSpace GetDiskSpace(string diskName)
        {
            string lpDirectoryName = diskName;
            long lpFreeBytesAvailable = 0;
            long lpTotalNumberOfBytes = 0;
            long lpTotalNumberOfFreeBytes = 0;
            int bRC = GetDiskFreeSpaceEx(lpDirectoryName, ref lpFreeBytesAvailable, ref lpTotalNumberOfBytes, ref lpTotalNumberOfFreeBytes);
            return new DiskSpace { FreeBytes = lpFreeBytesAvailable, TotalBytes = lpTotalNumberOfBytes };
        }

        private class DiskSpace
        {
            public long FreeBytes { get; set; }
            public long TotalBytes { get; set; }
            public long UsedBytes { get { return TotalBytes - FreeBytes; } }
        }

        private static readonly string[] SizeSuffixes = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        private static string SizeSuffix(long numBytes, int decimalPlaces = 0)
        {
            if (numBytes < 0)
            {
                throw new ArgumentException("Bytes should not be negative", "numBytes");
            }
            var mag = (int)Math.Max(0, Math.Log(numBytes, 1024));
            var adjustedSize = Math.Round(numBytes / Math.Pow(1024, mag), decimalPlaces);
            return String.Format("{0} {1}", adjustedSize, SizeSuffixes[mag]);
        }
    }
}
