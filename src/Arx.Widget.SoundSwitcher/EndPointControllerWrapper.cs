using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Arx.Widget.SoundSwitcher
{
    public class SoundObject
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1} (Default: {2})", Index, Name, IsDefault);
        }
    }

    public class EndPointControllerWrapper
    {
        private readonly ProcessStartInfo _startInfo;

        public EndPointControllerWrapper()
        {
            _startInfo = new ProcessStartInfo
            {
                FileName = "EndPointController.exe",
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory,
                Arguments = string.Empty,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardInput = true
            };
        }

        public void Activate(int index)
        {
            _startInfo.Arguments = string.Format("{0}", index);
            using (var process = Process.Start(_startInfo))
            {

            }
        }

        public List<SoundObject> GetAll()
        {
            _startInfo.Arguments = "-f \"%d|%ws|%d|%d\"";
            var soundObjects = new List<SoundObject>();
            using (var process = Process.Start(_startInfo))
            {
                using (var reader = process.StandardOutput)
                {
                    var result = reader.ReadToEnd();
                    var devices = result.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var device in devices)
                    {
                        var values = device.Split('|');
                        var obj = new SoundObject
                        {
                            Index = int.Parse(values[0]),
                            IsDefault = values[3] == "1",
                            Name = values[1]
                        };
                        soundObjects.Add(obj);
                    }
                }
            }
            return soundObjects;
        }
    }
}
