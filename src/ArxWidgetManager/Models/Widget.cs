using ArxWidgetManager.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ArxWidgetManager.Models
{
    public class Widget
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public ImageSource Image { get; set; }

        public event Action<Widget> WidgetStartedEvent;
        public event Action<Widget> WidgetStoppedEvent;

        private Process _widgetProcess;
        private string _executablePath;

        public static Widget LoadFromFile(string widgetExecutable)
        {
            var fileVersionInfo = FileVersionInfo.GetVersionInfo(widgetExecutable);

            var widget = new Widget();
            widget._executablePath = widgetExecutable;
            widget.Name = fileVersionInfo.FileDescription;
            widget.Version = fileVersionInfo.FileVersion;

            var icon = IconLoader.GetNew(widgetExecutable);
            var bmp = icon.ToBitmap();
            var strm = new MemoryStream();
            bmp.Save(strm, System.Drawing.Imaging.ImageFormat.Png);
            var bmpImage = new BitmapImage();
            bmpImage.BeginInit();
            strm.Seek(0, SeekOrigin.Begin);
            bmpImage.StreamSource = strm;
            bmpImage.EndInit();
            widget.Image = bmpImage;

            return widget;
        }

        public void Start()
        {
            if (_widgetProcess == null || _widgetProcess.HasExited)
            {
                _widgetProcess = new Process();
                _widgetProcess.StartInfo.FileName = _executablePath;
                _widgetProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(_executablePath);
                _widgetProcess.StartInfo.Arguments = "";
                _widgetProcess.StartInfo.UseShellExecute = false;
                _widgetProcess.StartInfo.CreateNoWindow = true;
                _widgetProcess.StartInfo.RedirectStandardOutput = true;
                _widgetProcess.StartInfo.RedirectStandardInput = true;
                _widgetProcess.Start();
                OnWidgetStarted();
            }
        }

        public void Stop()
        {
            if (_widgetProcess.HasExited) { return; }
            // Close
            if (!_widgetProcess.CloseMainWindow())
            {
                StreamWriter stdInputWriter = _widgetProcess.StandardInput;
                stdInputWriter.Write("q");
                stdInputWriter.Close();
            }
            if (!_widgetProcess.WaitForExit(1000))
            {
                _widgetProcess.Kill();
            }
            _widgetProcess = null;
            OnWidgetStopped();
        }

        protected void OnWidgetStarted()
        {
            var handler = WidgetStartedEvent;
            if (handler != null) handler(this);
        }

        protected void OnWidgetStopped()
        {
            var handler = WidgetStoppedEvent;
            if (handler != null) handler(this);
        }
    }
}
