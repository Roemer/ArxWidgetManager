
namespace Arx.Shared
{
    public enum LogiArxDeviceType
    {
        IPhone = LogitechArx.LOGI_ARX_DEVICETYPE_IPHONE,
        IPad = LogitechArx.LOGI_ARX_DEVICETYPE_IPAD,
        AndroidSmall = LogitechArx.LOGI_ARX_DEVICETYPE_ANDROID_SMALL,
        AndroidNormal = LogitechArx.LOGI_ARX_DEVICETYPE_ANDROID_NORMAL,
        AndroidLarge = LogitechArx.LOGI_ARX_DEVICETYPE_ANDROID_LARGE,
        AndroidXLarge = LogitechArx.LOGI_ARX_DEVICETYPE_ANDROID_XLARGE,
        AndroidOther = LogitechArx.LOGI_ARX_DEVICETYPE_ANDROID_OTHER,
    }

    public enum LogiArxOrientation
    {
        Portrait = LogitechArx.LOGI_ARX_ORIENTATION_PORTRAIT,
        Landscame = LogitechArx.LOGI_ARX_ORIENTATION_LANDSCAPE,
    }

    public enum LogiArxError
    {
        Success = 0,
        WrongParameterFOrmat = 1,
        NullParameterNotSupported = 2,
        WrongFilePath = 3,
        SdkNotInitialized = 4,
        SdkAlreadyInitialized = 5,
        ConnectionWithGamingSoftwareBroken = 6,
        ErrorCreatingThread = 7,
        ErrorCopyingMemory = 8,
    }
}
