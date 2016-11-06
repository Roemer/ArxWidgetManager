using Arx.Shared;

namespace ArxClock
{
    public class ArxClockWidget : WidgetBase
    {
        protected override string Identifier => "flauschig.arxclock";

        protected override string Name => "ArxClock";

        protected override void DeviceConnected(LogiArxDeviceType deviceType)
        {
            var a = Add(EmbeddedResourceReader.GetBytes("ArxClock.Resources.index.html"), "index.html");
            a = Add(EmbeddedResourceReader.GetBytes("ArxClock.Resources.jquery-1.11.1.min.js"), "jquery-1.11.1.min.js");
            a = SetIndex("index.html");
        }
    }
}
