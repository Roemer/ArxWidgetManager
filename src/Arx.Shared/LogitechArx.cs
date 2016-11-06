using System;
using System.Runtime.InteropServices;

namespace Arx.Shared
{
    public static class LogitechArx
    {
        public const int LOGI_ARX_ORIENTATION_PORTRAIT = 0x01;
        public const int LOGI_ARX_ORIENTATION_LANDSCAPE = 0x10;
        public const int LOGI_ARX_EVENT_FOCUS_ACTIVE = 0x01;
        public const int LOGI_ARX_EVENT_FOCUS_INACTIVE = 0x02;
        public const int LOGI_ARX_EVENT_TAP_ON_TAG = 0x04;
        public const int LOGI_ARX_EVENT_MOBILEDEVICE_ARRIVAL = 0x08;
        public const int LOGI_ARX_EVENT_MOBILEDEVICE_REMOVAL = 0x10;
        public const int LOGI_ARX_DEVICETYPE_IPHONE = 0x01;
        public const int LOGI_ARX_DEVICETYPE_IPAD = 0x02;
        public const int LOGI_ARX_DEVICETYPE_ANDROID_SMALL = 0x03;
        public const int LOGI_ARX_DEVICETYPE_ANDROID_NORMAL = 0x04;
        public const int LOGI_ARX_DEVICETYPE_ANDROID_LARGE = 0x05;
        public const int LOGI_ARX_DEVICETYPE_ANDROID_XLARGE = 0x06;
        public const int LOGI_ARX_DEVICETYPE_ANDROID_OTHER = 0x07;

        public const int LOGI_CUSTOMICON_BYTEARRAY_SIZE = 144 * 144 * 4;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void logiArxCB(int eventType, int eventValue, [MarshalAs(UnmanagedType.LPWStr)]string eventArg, IntPtr context);

        public struct logiArxCbContext
        {
            public logiArxCB arxCallBack;
            public IntPtr arxContext;
        }

        [DllImport("LogitechGArxControlEnginesWrapper.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiArxGetConfigOptionNumber(string configPath, double defaultValue);
        [DllImport("LogitechGArxControlEnginesWrapper.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiArxGetConfigOptionBool(string configPath, bool defaultValue);
        [DllImport("LogitechGArxControlEnginesWrapper.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiArxGetConfigOptionColor(string configPath, int defaultRed, int defaultGreen, int defaultBlue);
        [DllImport("LogitechGArxControlEnginesWrapper.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiArxGetConfigOptionString(string configPath, string defaultValue, int bufferSize);
        [DllImport("LogitechGArxControlEnginesWrapper.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiArxSetConfigOptionLabel(string configPath, string label);
        [DllImport("LogitechGArxControlEnginesWrapper.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiArxInit(string identifier, string friendlyName, ref logiArxCbContext callback);
        [DllImport("LogitechGArxControlEnginesWrapper.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        // Icon should be 144 x 144 in size and 4 bytes per pixel (R,G,B,A).
        public static extern bool LogiArxInitWithIcon(string identifier, string friendlyName, ref logiArxCbContext callback, byte[] iconByteArray);
        [DllImport("LogitechGArxControlEnginesWrapper.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiArxAddFileAs(string filePath, string fileName, string mimeType = "");
        [DllImport("LogitechGArxControlEnginesWrapper.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiArxAddContentAs(byte[] content, int size, string fileName, string mimeType = "");
        [DllImport("LogitechGArxControlEnginesWrapper.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiArxAddUTF8StringAs(string stringContent, string fileName, string mimeType = "");
        [DllImport("LogitechGArxControlEnginesWrapper.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiArxAddImageFromBitmap(byte[] bitmap, int width, int height, string fileName);
        [DllImport("LogitechGArxControlEnginesWrapper.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiArxSetIndex(string fileName);
        [DllImport("LogitechGArxControlEnginesWrapper.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiArxSetTagPropertyById(string tagId, string prop, string newValue);
        [DllImport("LogitechGArxControlEnginesWrapper.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiArxSetTagsPropertyByClass(string tagsClass, string prop, string newValue);
        [DllImport("LogitechGArxControlEnginesWrapper.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiArxSetTagContentById(string tagId, string newContent);
        [DllImport("LogitechGArxControlEnginesWrapper.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool LogiArxSetTagsContentByClass(string tagsClass, string newContent);
        [DllImport("LogitechGArxControlEnginesWrapper.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern int LogiArxGetLastError();
        [DllImport("LogitechGArxControlEnginesWrapper.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        public static extern void LogiArxShutdown();
    }
}
