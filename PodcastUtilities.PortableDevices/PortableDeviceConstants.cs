using System;

namespace PodcastUtilities.PortableDevices
{
    public static class PortableDeviceConstants
    {
        // From PortableDevice.h in the windows SDK
        public const string WPD_DEVICE_OBJECT_ID = "DEVICE";

        public const int VT_LPWSTR = 31;

        public static Guid WPD_CONTENT_TYPE_FOLDER;
        public static Guid WPD_CONTENT_TYPE_FUNCTIONAL_OBJECT;

        static PortableDeviceConstants()
        {
            WPD_CONTENT_TYPE_FOLDER = new Guid(0x27E2E392, 0xA111, 0x48E0, 0xAB, 0x0C, 0xE1, 0x77, 0x05, 0xA0, 0x5F, 0x85);
            WPD_CONTENT_TYPE_FUNCTIONAL_OBJECT = new Guid(0x99ED0160, 0x17FF, 0x4C44, 0x9D, 0x98, 0x1D, 0x7A, 0x6F, 0x94, 0x19, 0x21);
        }
    }
}