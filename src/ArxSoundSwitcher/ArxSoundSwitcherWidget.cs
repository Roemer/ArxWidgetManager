using Arx.Shared;
using Newtonsoft.Json;
using System;
using System.Web;

namespace ArxSoundSwitcher
{
    public class ArxSoundSwitcherWidget : WidgetBase
    {
        private readonly EndPointControllerWrapper _endPointControllerWrapper = new EndPointControllerWrapper();

        protected override string Identifier
        {
            get { return "flauschig.arxsoundswitcher"; }
        }

        protected override string Name
        {
            get { return "ArxSndSwitch"; }
        }

        protected override void DeviceConnected(LogiArxDeviceType deviceType)
        {
            base.DeviceConnected(deviceType);
            bool a = Add(EmbeddedResourceReader.GetBytes("ArxSoundSwitcher.Resources.index.html"), "index.html");
            a = Add(EmbeddedResourceReader.GetBytes("ArxSoundSwitcher.Resources.checkmark.png"), "checkmark.png");
            a = Add(EmbeddedResourceReader.GetBytes("ArxSoundSwitcher.Resources.jquery-1.11.1.min.js"), "jquery-1.11.1.min.js");
            a = SetIndex("index.html");
        }

        protected override void WidgetFocus(LogiArxOrientation orientation)
        {
            base.WidgetFocus(orientation);
            SendCurrent();
        }

        protected override void Tapped(string tappedElementId)
        {
            if (tappedElementId.StartsWith("device_"))
            {
                var deviceId = Convert.ToInt32(tappedElementId.Substring(7));
                Console.WriteLine("Activating Device {0}", deviceId);
                _endPointControllerWrapper.Activate(deviceId);
                System.Threading.Thread.Sleep(1000);
                SendCurrent();
            }
        }

        private void SendCurrent()
        {
            var devices = _endPointControllerWrapper.GetAll();
            var jsonString = JsonConvert.SerializeObject(devices);
            SetContentById("hiddenData", HttpUtility.HtmlEncode(jsonString));
        }
    }
}
