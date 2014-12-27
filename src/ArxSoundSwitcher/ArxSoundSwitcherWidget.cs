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
            a = Add(EmbeddedResourceReader.GetBytes("ArxSoundSwitcher.Resources.jquery-1.11.1.min.js"), "jquery-1.11.1.min.js");
            a = SetIndex("index.html");
        }

        protected override void WidgetFocus(LogiArxOrientation orientation)
        {
            base.WidgetFocus(orientation);

            var devices = _endPointControllerWrapper.GetAll();
            var jsonString = JsonConvert.SerializeObject(devices);

            SetContentById("hiddenData", HttpUtility.HtmlEncode(jsonString));
        }

        protected override void Tapped(string tappedElementId)
        {
            Console.WriteLine(tappedElementId);
            if (tappedElementId.StartsWith("device_"))
            {
                _endPointControllerWrapper.Activate(Convert.ToInt32(tappedElementId.Substring(7)));
            }
        }
    }
}
