using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ArxWidgetManager.Win32
{
    /// <summary>
    /// Loads a Win32 icon
    /// </summary>
    public static class IconLoader
    {
        public enum SystemCursors
        {
            IDC_APPSTARTING = 32650, // Standard arrow and small hourglass cursor.
            IDC_ARROW = 32512, // Standard arrow cursor.
            IDC_CROSS = 32515, // Crosshair cursor.
            IDC_HAND = 32649, // Hand cursor.
            IDC_HELP = 32651, // Arrow and question mark cursor.
            IDC_IBEAM = 32513, // I-beam cursor.
            IDC_NO = 32648, // Slashed circle cursor.
            IDC_SIZEALL = 32646, // Four-pointed arrow cursor pointing north, south, east, and west.
            IDC_SIZENESW = 32643, // Double-pointed arrow cursor pointing northeast and southwest.
            IDC_SIZENS = 32645, // Double-pointed arrow cursor pointing north and south.
            IDC_SIZENWSE = 32642, // Double-pointed arrow cursor pointing northwest and southeast.
            IDC_SIZEWE = 32644, // Double-pointed arrow cursor pointing west and east.
            IDC_UPARROW = 32516, // Vertical arrow cursor.
            IDC_WAIT = 32514, // Hourglass cursor.
        }

        public enum SystemIcons
        {
            IDI_APPLICATION = 32512, // Application icon.
            IDI_ASTERISK = 32516, // Asterisk icon.
            IDI_EXCLAMATION = 32515, // Exclamation point icon.
            IDI_HAND = 32513, // Stop sign icon.
            IDI_QUESTION = 32514, // Question-mark icon.
            IDI_WINLOGO = 32517, // Application icon. Windows 2000:  Windows logo icon.
            IDI_WARNING = IDI_EXCLAMATION,
            IDI_ERROR = IDI_HAND,
            IDI_INFORMATION = IDI_ASTERISK,
        }

        [DllImport("user32.dll")]
        private static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr iconName);
        [DllImport("comctl32.dll")]
        private static extern IntPtr LoadIconWithScaleDown(IntPtr hInstance, IntPtr iconName, int cx, int cy, IntPtr[] phico);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr LoadImage(IntPtr hinst, IntPtr lpszName, uint uType, int cxDesired, int cyDesired, uint fuLoad);
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string moduleName);
        [DllImport("shell32.dll")]
        private static extern UInt32 ExtractIconEx(string libName, int iconIndex, IntPtr[] largeIcon, IntPtr[] smallIcon, uint nIcons);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern UInt32 PrivateExtractIcons(String lpszFile, int nIconIndex, int cxIcon, int cyIcon, IntPtr[] phicon, IntPtr[] piconid, UInt32 nIcons, UInt32 flags);
        [DllImport("user32.dll")]
        private static extern int DestroyIcon(IntPtr hIcon);

        public static uint GetIconCount(string moduleName)
        {
            return ExtractIconEx(moduleName, -1, null, null, uint.MaxValue);
        }

        public static Icon GetNew(string moduleName = null)
        {
            IntPtr[] phicon = new IntPtr[] { IntPtr.Zero };
            IntPtr[] piconid = new IntPtr[] { IntPtr.Zero };
            var ret = PrivateExtractIcons(moduleName, 0, 128, 128, phicon, piconid, 1, 0);
            var icon = Icon.FromHandle(phicon[0]);
            return icon;


            var hInstance = GetModuleHandle(moduleName);
            var hIcon = LoadImage(hInstance, new IntPtr((int)SystemIcons.IDI_APPLICATION), 1, 64, 64, 0);


            //IntPtr[] icOut = new IntPtr[1];
            //var hIcon = LoadIconWithScaleDown(hInstance, new IntPtr((int)SystemIcons.IDI_APPLICATION), 64, 64, icOut);
            if (hIcon == IntPtr.Zero) { return null; }
            //var icon = Icon.FromHandle(hIcon);
            return icon;
        }

        public static Icon Get(string moduleName)
        {
            IntPtr[] largeIcon = new IntPtr[1];
            IntPtr[] smallIcon = new IntPtr[1];
            var ret = ExtractIconEx(moduleName, 0, largeIcon, smallIcon, 1);
            var icon = Icon.FromHandle(largeIcon[0]);
            //DestroyIcon(largeIcon[0]);
            return icon;
        }

        public static Icon GetIcon(SystemCursors cursor)
        {
            return GetIcon((int)cursor);
        }

        public static Icon GetIcon(SystemIcons icon)
        {
            return GetIcon((int)icon);
        }

        private static Icon GetIcon(int iconId)
        {
            var hInstance = GetModuleHandle(null);
            var hIcon = LoadIcon(hInstance, new IntPtr(iconId));
            if (hIcon == IntPtr.Zero) { return null; }
            var icon = Icon.FromHandle(hIcon);
            return icon;
        }
    }
}
