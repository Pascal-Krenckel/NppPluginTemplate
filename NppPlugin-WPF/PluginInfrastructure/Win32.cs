// NPP plugin platform for .Net v0.94.00 by Kasper B. Graversen etc.
using System.Drawing;
using System.Runtime.InteropServices;

namespace ____NppPlugin____.PluginInfrastructure;

public partial class Win32
{

    private static partial class User32
    {
#if NET7_0_OR_GREATER

        //
        // Raw pointer version (most flexible)
        //
        [LibraryImport(nameof(User32), EntryPoint = "SendMessageW")]
        internal static partial nint SendMessageW(
            nint hWnd,
            uint msg,
            nint wParam,
            nint lParam);

        [LibraryImport(nameof(User32), EntryPoint = "SendMessageW")]
        internal static partial nint SendMessageW(
    nint hWnd,
    uint msg,
    nint wParam,
    out nint lParam);

        //
        // UTF-16 string input (read-only)
        //
        [LibraryImport(
            nameof(User32),
            EntryPoint = "SendMessageW",
            StringMarshalling = StringMarshalling.Utf16)]
        internal static partial nint SendMessageW(
            nint hWnd,
            uint msg,
            nint wParam,
            string lParam);

        //
        // Writable UTF-16 buffer (char[])
        //
        [LibraryImport(
            nameof(User32),
            EntryPoint = "SendMessageW",
            StringMarshalling = StringMarshalling.Utf16)]
        internal static partial nint SendMessageW(
            nint hWnd,
            uint msg,
            nint wParam,
            [Out] char[] lParam);

        //
        // Unsafe pointer version
        //
        [LibraryImport(nameof(User32), EntryPoint = "SendMessageW")]
        internal static unsafe partial nint SendMessageW(
            nint hWnd,
            uint msg,
            nint wParam,
            char* lParam);

        //
        // Integer lParam overloads
        //
        [LibraryImport(nameof(User32), EntryPoint = "SendMessageW")]
        internal static partial nint SendMessageW(
            nint hWnd,
            uint msg,
            nint wParam,
            int lParam);

        [LibraryImport(nameof(User32), EntryPoint = "SendMessageW")]
        internal static partial nint SendMessageW(
            nint hWnd,
            uint msg,
            nint wParam,
            uint lParam);

        [LibraryImport(nameof(User32), EntryPoint = "SendMessageW")]
        internal static partial nint SendMessageW(
            nint hWnd,
            uint msg,
            nint wParam,
            long lParam);

        [LibraryImport(nameof(User32), EntryPoint = "SendMessageW")]
        internal static partial nint SendMessageW(
            nint hWnd,
            uint msg,
            nint wParam,
            ulong lParam);

        [LibraryImport(nameof(User32))]
        internal static partial nint GetMenu(nint hWnd);

        [LibraryImport(nameof(User32))]
        internal static partial int CheckMenuItem(
            nint hMenu,
            int uIDCheckItem,
            int uCheck);

        [LibraryImport(nameof(User32))]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static partial bool ClientToScreen(
            nint hWnd,
            ref Point lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        internal struct Point(int x, int y)
        {
            public int x = x;
            public int y = y;

            public static implicit operator Point(System.Drawing.Point pt) => new(pt.X, pt.Y);
            public static implicit operator System.Drawing.Point(Point pt) => new(pt.x, pt.y);
        }

        [LibraryImport(nameof(User32))]
        internal static partial int GetScrollInfo(
            nint hwnd,
            int nBar,
            ref ScrollInfo scrollInfo);

#else

    [DllImport(nameof(User32), EntryPoint = "SendMessageW")]
    internal static extern nint SendMessageW(
        nint hWnd,
        uint msg,
        nint wParam,
        nint lParam);
        
    [DllImport(nameof(User32), EntryPoint = "SendMessageW")]
    internal static extern nint SendMessageW(
        nint hWnd,
        uint msg,
        nint wParam,
        out nint lParam);

    [DllImport(nameof(User32), EntryPoint = "SendMessageW")]
    internal static extern nint SendMessageW(
        nint hWnd,
        uint msg,
        nint wParam,
        string lParam);

    [DllImport(nameof(User32), EntryPoint = "SendMessageW")]
    internal static extern nint SendMessageW(
        nint hWnd,
        uint msg,
        nint wParam,
        [Out] char[] lParam);

    [DllImport(nameof(User32), EntryPoint = "SendMessageW")]
    internal static extern unsafe nint SendMessageW(
        nint hWnd,
        uint msg,
        nint wParam,
        char* lParam);

    [DllImport(nameof(User32), EntryPoint = "SendMessageW")]
    internal static extern nint SendMessageW(
        nint hWnd,
        uint msg,
        nint wParam,
        int lParam);

    [DllImport(nameof(User32), EntryPoint = "SendMessageW")]
    internal static extern nint SendMessageW(
        nint hWnd,
        uint msg,
        nint wParam,
        uint lParam);

    [DllImport(nameof(User32), EntryPoint = "SendMessageW")]
    internal static extern nint SendMessageW(
        nint hWnd,
        uint msg,
        nint wParam,
        long lParam);

    [DllImport(nameof(User32), EntryPoint = "SendMessageW")]
    internal static extern nint SendMessageW(
        nint hWnd,
        uint msg,
        nint wParam,
        ulong lParam);

          [DllImport(nameof(User32))]
    internal static extern nint GetMenu(nint hWnd);

    [DllImport(nameof(User32))]
    internal static extern int CheckMenuItem(
        nint hMenu,
        int uIDCheckItem,
        int uCheck);

    [DllImport(nameof(User32))]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool ClientToScreen(
        nint hWnd,
        ref Point lpPoint);

    [DllImport(nameof(User32))]
    internal static extern int GetScrollInfo(
        nint hwnd,
        int nBar,
        ref ScrollInfo scrollInfo);

#endif
    }

    /// <summary>
    /// Get the scroll information of a scroll bar or window with scroll bar
    /// @see https://msdn.microsoft.com/en-us/library/windows/desktop/bb787537(v=vs.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ScrollInfo
    {
        /// <summary>
        /// Specifies the size, in bytes, of this structure. The caller must set this to sizeof(SCROLLINFO).
        /// </summary>
        public uint cbSize;
        /// <summary>
        /// Specifies the scroll bar parameters to set or retrieve.
        /// @see ScrollInfoMask
        /// </summary>
        public uint fMask;
        /// <summary>
        /// Specifies the minimum scrolling position.
        /// </summary>
        public int nMin;
        /// <summary>
        /// Specifies the maximum scrolling position.
        /// </summary>
        public int nMax;
        /// <summary>
        /// Specifies the page size, in device units. A scroll bar uses this value to determine the appropriate size of the proportional scroll box.
        /// </summary>
        public uint nPage;
        /// <summary>
        /// Specifies the position of the scroll box.
        /// </summary>
        public int nPos;
        /// <summary>
        /// Specifies the immediate position of a scroll box that the user is dragging. 
        /// An application can retrieve this value while processing the SB_THUMBTRACK request code. 
        /// An application cannot set the immediate scroll position; the SetScrollInfo function ignores this member.
        /// </summary>
        public int nTrackPos;
    }

    /// <summary>
    /// Used for the ScrollInfo fMask
    /// SIF_ALL             => Combination of SIF_PAGE, SIF_POS, SIF_RANGE, and SIF_TRACKPOS.
    /// SIF_DISABLENOSCROLL => This value is used only when setting a scroll bar's parameters. If the scroll bar's new parameters make the scroll bar unnecessary, disable the scroll bar instead of removing it.
    /// SIF_PAGE            => The nPage member contains the page size for a proportional scroll bar.
    /// SIF_POS             => The nPos member contains the scroll box position, which is not updated while the user drags the scroll box.
    /// SIF_RANGE           => The nMin and nMax members contain the minimum and maximum values for the scrolling range.
    /// SIF_TRACKPOS        => The nTrackPos member contains the current position of the scroll box while the user is dragging it.
    /// </summary>
    public enum ScrollInfoMask
    {
        SIF_RANGE = 0x1,
        SIF_PAGE = 0x2,
        SIF_POS = 0x4,
        SIF_DISABLENOSCROLL = 0x8,
        SIF_TRACKPOS = 0x10,
        SIF_ALL = SIF_RANGE + SIF_PAGE + SIF_POS + SIF_TRACKPOS
    }

    /// <summary>
    /// Used for the GetScrollInfo() nBar parameter
    /// </summary>
    public enum ScrollInfoBar
    {
        SB_HORZ = 0,
        SB_VERT = 1,
        SB_CTL = 2,
        SB_BOTH = 3
    }

    /// <summary>
    /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
    /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
    /// If gateways are missing or incomplete, please help extend them and send your code to the project 
    /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// </summary>
    public static nint SendMessage(nint hWnd, uint Msg, nint wParam, string lParam) => User32.SendMessageW(hWnd, Msg, wParam, lParam);

    /// <summary>
    /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
    /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
    /// If gateways are missing or incomplete, please help extend them and send your code to the project 
    /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// </summary>
    public static nint SendMessage(nint hWnd, uint Msg, nint wParam, char[] lParam) => User32.SendMessageW(hWnd, Msg, wParam, lParam);

    /// <summary>
    /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
    /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
    /// If gateways are missing or incomplete, please help extend them and send your code to the project 
    /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// </summary>
    public static nint SendMessage(nint hWnd, uint Msg, nint wParam, nint lParam) => User32.SendMessageW(hWnd, Msg, wParam, lParam);

    /// <summary>
    /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
    /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
    /// If gateways are missing or incomplete, please help extend them and send your code to the project 
    /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// </summary>
    public static nint SendMessage(nint hWnd, uint Msg, nint wParam, out nint lParam) => User32.SendMessageW(hWnd, Msg, wParam, out lParam);

    /// <summary>
    /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
    /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
    /// If gateways are missing or incomplete, please help extend them and send your code to the project 
    /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// </summary>
    public static nint SendMessage(nint hWnd, uint Msg, int wParam, NppMenuCmd lParam) => SendMessage(hWnd, Msg, new nint(wParam), new nint((uint)lParam));

    /// <summary>
    /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
    /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
    /// If gateways are missing or incomplete, please help extend them and send your code to the project 
    /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// </summary>
    public static nint SendMessage(nint hWnd, uint Msg, int wParam, nint lParam) => SendMessage(hWnd, Msg, new nint(wParam), lParam);

    /// <summary>
    /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
    /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
    /// If gateways are missing or incomplete, please help extend them and send your code to the project 
    /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// </summary>
    public static nint SendMessage(nint hWnd, uint Msg, int wParam, int lParam) => SendMessage(hWnd, Msg, new nint(wParam), new nint(lParam));

    /// <summary>
    /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
    /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
    /// If gateways are missing or incomplete, please help extend them and send your code to the project 
    /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// </summary>
    public static nint SendMessage(nint hWnd, uint Msg, int wParam, out int lParam)
    {
        nint retval = SendMessage(hWnd, Msg, new nint(wParam), out nint outVal);
        lParam = outVal.ToInt32();
        return retval;
    }

    /// <summary>
    /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
    /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
    /// If gateways are missing or incomplete, please help extend them and send your code to the project 
    /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// </summary>
    public static nint SendMessage(nint hWnd, uint Msg, nint wParam, int lParam) => SendMessage(hWnd, Msg, wParam, new nint(lParam));

    /// <summary>
    /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
    /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
    /// If gateways are missing or incomplete, please help extend them and send your code to the project 
    /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// </summary>
    public static nint SendMessage(nint hWnd, uint Msg, int wParam, char[] lParam) => SendMessage(hWnd, Msg, new nint(wParam), lParam);

    /// <summary>
    /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
    /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
    /// If gateways are missing or incomplete, please help extend them and send your code to the project 
    /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// </summary>
    public static nint SendMessage(nint hWnd, uint Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam) => SendMessage(hWnd, Msg, new nint(wParam), lParam);

    /// <summary>
    /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
    /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
    /// If gateways are missing or incomplete, please help extend them and send your code to the project 
    /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// </summary>
    public static nint SendMessage(nint hWnd, SciMsg Msg, nint wParam, int lParam) => SendMessage(hWnd, (uint)Msg, wParam, new nint(lParam));

    /// <summary>
    /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
    /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
    /// If gateways are missing or incomplete, please help extend them and send your code to the project 
    /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// </summary>
    public static nint SendMessage(nint hWnd, SciMsg Msg, int wParam, nint lParam) => SendMessage(hWnd, (uint)Msg, new nint(wParam), lParam);

    /// <summary>
    /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
    /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
    /// If gateways are missing or incomplete, please help extend them and send your code to the project 
    /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// </summary>
    public static nint SendMessage(nint hWnd, SciMsg Msg, int wParam, string lParam) => SendMessage(hWnd, (uint)Msg, new nint(wParam), lParam);

    /// <summary>
    /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
    /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
    /// If gateways are missing or incomplete, please help extend them and send your code to the project 
    /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// </summary>
    public static nint SendMessage(nint hWnd, SciMsg Msg, int wParam, char[] lParam) => SendMessage(hWnd, (uint)Msg, new nint(wParam), lParam);

    /// <summary>
    /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
    /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
    /// If gateways are missing or incomplete, please help extend them and send your code to the project 
    /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// </summary>
    public static nint SendMessage(nint hWnd, SciMsg Msg, int wParam, int lParam) => SendMessage(hWnd, (uint)Msg, new nint(wParam), new nint(lParam));

    /// <summary>
    /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
    /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
    /// If gateways are missing or incomplete, please help extend them and send your code to the project 
    /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// </summary>
    public static nint SendMessage(nint hWnd, SciMsg Msg, nint wParam, nint lParam) => SendMessage(hWnd, (uint)Msg, wParam, lParam);

    /// <summary>
    /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as
    /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.
    /// If gateways are missing or incomplete, please help extend them and send your code to the project
    /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// </summary>
    public static nint SendMessage(nint hWnd, uint Msg, int wParam, ref LangType lParam)
    {
        nint retval = SendMessage(hWnd, Msg, new nint(wParam), out nint outVal);
        lParam = (LangType)outVal;
        return retval;
    }

    /// <summary>
    /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
    /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
    /// If gateways are missing or incomplete, please help extend them and send your code to the project 
    /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// </summary>
    public static nint SendMessage(nint hWnd, NppMsg Msg, nint wParam, int lParam) => SendMessage(hWnd, (uint)Msg, wParam, lParam);

    /// <summary>
    /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
    /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
    /// If gateways are missing or incomplete, please help extend them and send your code to the project 
    /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// </summary>
    public static nint SendMessage(nint hWnd, NppMsg Msg, int wParam, int lParam) => SendMessage(hWnd, (uint)Msg, wParam, lParam);

    /// <summary>
    /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
    /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
    /// If gateways are missing or incomplete, please help extend them and send your code to the project 
    /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// </summary>
    public static nint SendMessage(nint hWnd, NppMsg Msg, int wParam, string lParam) => SendMessage(hWnd, (uint)Msg, wParam, lParam);

    /// <summary>
    /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
    /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
    /// If gateways are missing or incomplete, please help extend them and send your code to the project 
    /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// </summary>
    public static nint SendMessage(nint hWnd, NppMsg Msg, int wParam, char[] lParam) => SendMessage(hWnd, (uint)Msg, wParam, lParam);

    /// <summary>
    /// You should try to avoid calling this method in your plugin code. Rather use one of the gateways such as 
    /// <see cref="ScintillaGateway"/> or <see cref="NotepadPPGateway"/>.  
    /// If gateways are missing or incomplete, please help extend them and send your code to the project 
    /// at https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net
    /// </summary>
    public static nint SendMessage(nint hWnd, NppMsg Msg, nint wParam, nint lParam) => SendMessage(hWnd, (uint)Msg, wParam, lParam);

    public const int MAX_PATH = Int16.MaxValue;


    public const int MF_BYCOMMAND = 0;
    public const int MF_CHECKED = 8;
    public const int MF_UNCHECKED = 0;

    public static nint GetMenu(nint hWnd) => User32.GetMenu(hWnd);

    public static int CheckMenuItem(nint hmenu, int uIDCheckItem, int uCheck) => User32.CheckMenuItem(hmenu, uIDCheckItem, uCheck);

    public const int WM_CREATE = 1;

    public static bool ClientToScreen(nint hWnd, ref Point lpPoint)
    {
        User32.Point p = lpPoint;
        var ret = User32.ClientToScreen(hWnd, ref p);
        lpPoint = p;
        return ret;
    }


    /// <summary>
    /// @see https://msdn.microsoft.com/en-us/library/windows/desktop/bb787583(v=vs.85).aspx
    /// </summary>
    /// <param name="hwnd"></param>
    /// <param name="nBar"></param>
    /// <param name="scrollInfo"></param>
    /// <returns></returns>
    public static int GetScrollInfo(nint hwnd, int nBar, ref ScrollInfo scrollInfo) => User32.GetScrollInfo(hwnd, nBar, ref scrollInfo);
}
