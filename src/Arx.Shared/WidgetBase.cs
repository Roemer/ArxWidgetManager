using Arx.Shared.Win32;
using System;
using System.IO;

namespace Arx.Shared
{
    /// <summary>
    /// Simple widget for complete freedom
    /// </summary>
    public abstract class WidgetBase
    {
        /// <summary>
        /// Flag to indicate if the widget currently has focus or not
        /// </summary>
        public bool HasFocus { get; private set; }

        /// <summary>
        /// Unique identifier of the widget (like "yourcompany.widgetname")
        /// </summary>
        protected abstract string Identifier { get; }
        /// <summary>
        /// Friendly (readable) name of the widget
        /// </summary>
        protected abstract string Name { get; }

        /// <summary>
        /// Holds a reference to the wrapper library
        /// </summary>
        private IntPtr _arxLibrary;

        public event Action BeforeStopEvent;

        /// <summary>
        /// Start the widget
        /// </summary>
        public bool Start()
        {
            // Load the 3rd-party dll
            var dllDir = Path.GetDirectoryName(typeof(WidgetBase).Assembly.Location);
            dllDir = Path.Combine(dllDir, Environment.Is64BitProcess ? "x64" : "x86");
            _arxLibrary = LibraryLoader.LoadLibraryEx(Path.Combine(dllDir, "LogitechGArxControlEnginesWrapper.dll"), IntPtr.Zero, LibraryLoader.LoadLibraryFlags.LOAD_WITH_ALTERED_SEARCH_PATH);

            // Setup the callback
            LogitechArx.logiArxCbContext contextCallback;
            contextCallback.arxCallBack = ArxCallback;
            contextCallback.arxContext = IntPtr.Zero;
            // Initialize the callback
            var retVal = LogitechArx.LogiArxInit(Identifier, Name, ref contextCallback);
            return retVal;
        }

        /// <summary>
        /// Stop and clean up the widget
        /// </summary>
        public void Stop()
        {
            OnBeforeStopEvent();
            BeforeStop();
            // Shutdown
            LogitechArx.LogiArxShutdown();
            // Free the library
            LibraryLoader.FreeLibrary(_arxLibrary);
            _arxLibrary = IntPtr.Zero;
        }

        #region Wrapper Methods

        /// <summary>
        /// Send a file to the device
        /// </summary>
        public bool Add(string filePath, string internalName, string mimeType = "")
        {
            return LogitechArx.LogiArxAddFileAs(filePath, internalName, mimeType);
        }

        /// <summary>
        /// Send a byte array to the device
        /// </summary>
        public bool Add(byte[] content, string internalName, string mimeType = "")
        {
            return LogitechArx.LogiArxAddContentAs(content, content.Length, internalName, mimeType);
        }

        /// <summary>
        /// Sets the index to the defined page
        /// </summary>
        public bool SetIndex(string internalName)
        {
            return LogitechArx.LogiArxSetIndex(internalName);
        }

        public bool SetPropertyById(string id, string property, string newValue)
        {
            return LogitechArx.LogiArxSetTagPropertyById(id, property, newValue);
        }

        public bool SetPropertyByClass(string @class, string property, string newValue)
        {
            return LogitechArx.LogiArxSetTagsPropertyByClass(@class, property, newValue);
        }

        public bool SetContentById(string id, string newValue)
        {
            return LogitechArx.LogiArxSetTagContentById(id, newValue);
        }

        public bool SetContentByClass(string @class, string newValue)
        {
            return LogitechArx.LogiArxSetTagsContentByClass(@class, newValue);
        }

        public LogiArxError GetLastError()
        {
            var error = LogitechArx.LogiArxGetLastError();
            return (LogiArxError)error;
        }

        #endregion

        #region Callback Methods

        /// <summary>
        /// A mobile device is now connected
        /// Use this to send your files
        /// </summary>
        /// <param name="deviceType">The type of the device which connected</param>
        protected virtual void DeviceConnected(LogiArxDeviceType deviceType) { }
        /// <summary>
        /// No more devices connected to Logitech Gaming Software
        /// </summary>
        protected virtual void DeviceDisconnected() { }
        /// <summary>
        /// The applet has received focus and is now in active status
        /// </summary>
        /// <param name="orientation">The orientation of the applet</param>
        protected virtual void WidgetFocus(LogiArxOrientation orientation) { }
        /// <summary>
        /// The applet is now in background
        /// </summary>
        protected virtual void WidgetFocusLost() { }
        /// <summary>
        /// The user has tapped on an element in the applet HTML active page
        /// </summary>
        /// <param name="tappedElementId">The id of the element which was tapped</param>
        protected virtual void Tapped(string tappedElementId) { }
        /// <summary>
        /// Called before the widget is completely stopped
        /// </summary>
        protected virtual void BeforeStop() { }
        /// <summary>
        /// Internal callback which is called before the widget is completely stopped
        /// </summary>
        internal virtual void BeforeStopInternal() { }
        #endregion

        /// <summary>
        /// Callback method of Arx
        /// </summary>
        private void ArxCallback(int eventType, int eventValue, String eventArg, IntPtr context)
        {
            if (eventType == LogitechArx.LOGI_ARX_EVENT_MOBILEDEVICE_ARRIVAL)
            {
                DeviceConnected((LogiArxDeviceType)eventValue);
            }
            else if (eventType == LogitechArx.LOGI_ARX_EVENT_MOBILEDEVICE_REMOVAL)
            {
                DeviceDisconnected();
            }
            else if (eventType == LogitechArx.LOGI_ARX_EVENT_FOCUS_ACTIVE)
            {
                HasFocus = true;
                WidgetFocus((LogiArxOrientation)eventValue);
            }
            else if (eventType == LogitechArx.LOGI_ARX_EVENT_FOCUS_INACTIVE)
            {
                HasFocus = false;
                WidgetFocusLost();
            }
            else if (eventType == LogitechArx.LOGI_ARX_EVENT_TAP_ON_TAG)
            {
                Tapped(eventArg);
            }
        }

        private void OnBeforeStopEvent()
        {
            var handler = BeforeStopEvent;
            if (handler != null) handler();
        }
    }
}
