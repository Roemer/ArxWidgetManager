using System;
using System.Diagnostics;
using System.IO;

namespace ArxSoundSwitcher
{
    class Program
    {
        static void Main(string[] args)
        {
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = "EndPointController.exe";
            startInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            startInfo.Arguments = "";
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardInput = true;

            using (var process = Process.Start(startInfo))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                }
            }

            Console.ReadKey();
        }
    }
}
