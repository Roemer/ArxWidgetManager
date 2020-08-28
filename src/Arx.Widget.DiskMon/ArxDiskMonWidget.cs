using Arx.Shared;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Arx.Widget.DiskMon
{
    public class ArxDiskMonWidget : WorkerWidgetBase
    {
        [DllImport("kernel32")]
        public static extern int GetDiskFreeSpaceEx(string lpDirectoryName, ref long lpFreeBytesAvailable, ref long lpTotalNumberOfBytes, ref long lpTotalNumberOfFreeBytes);

        protected override string Identifier => "flauschig.arxdiskmon";

        protected override string Name => "ArxDiskMon";

        private readonly AppSettings _settings;

        public ArxDiskMonWidget()
            : base(5000)
        {
            _settings = new AppSettings();
        }

        protected override void DoWork()
        {
            var sb = new StringBuilder();
            foreach (var diskName in _settings.FilePaths)
            {
                if (string.IsNullOrWhiteSpace(diskName)) { continue; }
                var diskSpace = GetDiskSpace(diskName);
                sb.AppendFormat(@"{3}: {0} / {1} ({2}%)\r\n<br/>", SizeSuffix(diskSpace.FreeBytes, 2), SizeSuffix(diskSpace.TotalBytes, 2), Math.Round(diskSpace.UsedBytes / (double)diskSpace.TotalBytes * 100, 2), diskName);
            }

            SetContentById("contentDiv", sb.ToString());
        }

        protected override void DeviceConnected(LogiArxDeviceType deviceType)
        {
            var a = Add(EmbeddedResourceReader.GetBytes("Arx.Widget.DiskMon.Resources.index.html"), "index.html");
            a = SetIndex("index.html");
        }

        private DiskSpace GetDiskSpace(string diskName)
        {
            var lpDirectoryName = diskName;
            long lpFreeBytesAvailable = 0;
            long lpTotalNumberOfBytes = 0;
            long lpTotalNumberOfFreeBytes = 0;
            var bRC = GetDiskFreeSpaceEx(lpDirectoryName, ref lpFreeBytesAvailable, ref lpTotalNumberOfBytes, ref lpTotalNumberOfFreeBytes);
            return new DiskSpace { FreeBytes = lpFreeBytesAvailable, TotalBytes = lpTotalNumberOfBytes };
        }

        private class DiskSpace
        {
            public long FreeBytes { get; set; }
            public long TotalBytes { get; set; }
            public long UsedBytes => TotalBytes - FreeBytes;
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
            return string.Format("{0} {1}", adjustedSize, SizeSuffixes[mag]);
        }
    }
}
