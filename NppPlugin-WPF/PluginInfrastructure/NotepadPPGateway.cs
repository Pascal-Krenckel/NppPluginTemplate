// NPP plugin platform for .Net v0.94.00 by Kasper B. Graversen etc.
using System.Buffers;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ____NppPlugin____.PluginInfrastructure;



/// <summary>
/// This class holds helpers for sending messages defined in the Msgs_h.cs file. It is at the moment
/// incomplete. Please help fill in the blanks.
/// </summary>
public class NotepadPPGateway : INotepadPPGateway
{
    private const int Unused = 0;
    private static nint NppHandle => PluginBase.nppData._nppHandle;

    public void FileNew() => Win32.SendMessage(NppHandle, (uint)NppMsg.NPPM_MENUCOMMAND, Unused, NppMenuCmd.IDM_FILE_NEW);

    /// <summary>
    /// Gets the path of the current document.
    /// </summary>
    public string GetCurrentFilePath()
    {
        char[] characters = ArrayPool<char>.Shared.Rent(Win32.MAX_PATH);
        _ = Win32.SendMessage(NppHandle, (uint)NppMsg.NPPM_GETFULLCURRENTPATH, characters.Length, characters);
        int i = characters.AsSpan().IndexOf('\0');
        if (i == -1)
            i = characters.Length;
        string str = new(characters.AsSpan(0, i));
        ArrayPool<char>.Shared.Return(characters);
        return str;
    }

    /// <summary>
    /// Gets the path of the current document.
    /// </summary>
    public string GetFilePath(int bufferId)
    {
        int length = Win32.SendMessage(NppHandle, (uint)NppMsg.NPPM_GETFULLPATHFROMBUFFERID, bufferId, IntPtr.Zero).ToInt32();
        char[] characters = ArrayPool<char>.Shared.Rent(length + 1);
        _ = Win32.SendMessage(NppHandle, (uint)NppMsg.NPPM_GETFULLPATHFROMBUFFERID, bufferId, characters);
        string path = new(characters.AsSpan(0, length));
        return path;
    }

    public void SetCurrentLanguage(LangType language) => Win32.SendMessage(NppHandle, (uint)NppMsg.NPPM_SETCURRENTLANGTYPE, Unused, (int)language);

    public bool SetStatusBar(NppMsg statusBarType, string str) => Win32.SendMessage(NppHandle, (uint)NppMsg.NPPM_SETSTATUSBAR, (int)statusBarType, str) == nint.Zero;

    public long GetBufferEncoding(nint bufferId) => Win32.SendMessage(NppHandle, NppMsg.NPPM_GETBUFFERENCODING, bufferId, 0).ToInt64();

    public void SetMenuItemCheck(int cmdIndex, bool @checked) => Win32.SendMessage(NppHandle, NppMsg.NPPM_SETMENUITEMCHECK, PluginBase._funcItems.Items[cmdIndex]._cmdID, @checked ? 1 : 0);

    public void SendMenuEncoding(NppEncoding enc)
    {
        switch (enc)
        {
            case NppEncoding.ANSI:
                _ = Win32.SendMessage(NppHandle, NppMsg.NPPM_MENUCOMMAND, 0, (int)NppMenuCmd.IDM_FORMAT_ANSI);
                break;
            case NppEncoding.UTF8_BOM:
                _ = Win32.SendMessage(NppHandle, NppMsg.NPPM_MENUCOMMAND, 0, (int)NppMenuCmd.IDM_FORMAT_UTF_8);
                break;
            case NppEncoding.UTF16_LE:
                _ = Win32.SendMessage(NppHandle, NppMsg.NPPM_MENUCOMMAND, 0, (int)NppMenuCmd.IDM_FORMAT_UCS_2LE);
                break;
            case NppEncoding.UTF16_BE:
                _ = Win32.SendMessage(NppHandle, NppMsg.NPPM_MENUCOMMAND, 0, (int)NppMenuCmd.IDM_FORMAT_UCS_2BE);
                break;
            case NppEncoding.UTF8:
                _ = Win32.SendMessage(NppHandle, NppMsg.NPPM_MENUCOMMAND, 0, (int)NppMenuCmd.IDM_FORMAT_AS_UTF_8);
                break;
        }
    }

    public void SetMenuItemCheck(string commandName, bool @checked) => Win32.SendMessage(NppHandle, NppMsg.NPPM_SETMENUITEMCHECK, PluginBase._funcItems.Items.First(cmd => cmd._itemName == commandName)._cmdID, @checked ? 1 : 0);
    public void SetMenuItemCheck(FuncItem cmd, bool @checked) => Win32.SendMessage(NppHandle, NppMsg.NPPM_SETMENUITEMCHECK, cmd._cmdID, @checked ? 1 : 0);


    public nint GetCurrentBufferId() => Win32.SendMessage(NppHandle, NppMsg.NPPM_GETCURRENTBUFFERID, 0, 0);

    public void MakeCurrentBufferDirty() => Win32.SendMessage(NppHandle, NppMsg.NPPM_MAKECURRENTBUFFERDIRTY, 0, 0);

    public void SwitchToFile(string path) => Win32.SendMessage(NppHandle, NppMsg.NPPM_SWITCHTOFILE, 0, path);

    public void SaveCurrentFile() => Win32.SendMessage(NppHandle, NppMsg.NPPM_SAVECURRENTFILE, 0, 0);


    /// <summary>
    /// Set an icon for a command. Use the right <see cref="PluginBase.AddCommand(string, NppFuncItemDelegate, Bitmap, Bitmap)"/> function.
    /// </summary>
    /// <param name="cmdId">The command id. Make sure it is the right id <see cref="FuncItem._cmdID"/></param>
    /// <param name="icon"></param>
    /// <param name="iconDarkMode"></param>
    public void SetToolbarIcon(int cmdId, Bitmap icon, Bitmap iconDarkMode)
    {
        ToolbarIcons toolbar = new();
        toolbar.hToolbarBmp = icon.GetHbitmap();
        toolbar.hToolbarIcon = icon.GetHicon();
        toolbar.hToolbarIconDarkMode = iconDarkMode.GetHicon();
        IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf<ToolbarIcons>());
        Marshal.StructureToPtr(toolbar, pTbIcons, false);
        _ = Win32.SendMessage(NppHandle, (uint)NppMsg.NPPM_ADDTOOLBARICON_FORDARKMODE, cmdId, pTbIcons);
        Marshal.FreeHGlobal(pTbIcons);
    }

    /// <summary>
    /// Set an icon for a command. Use the right <see cref="PluginBase.AddCommand(string, NppFuncItemDelegate, Bitmap, Bitmap)"/> function.
    /// </summary>
    /// <param name="cmdId">The command id. Make sure it is the right id <see cref="FuncItem._cmdID"/></param>
    /// <param name="icon"></param>
    /// <param name="iconDarkMode"></param>
    public void SetToolbarIcon(int cmdId, Bitmap icon)
    {
        ToolbarIcons toolbar = new();
        toolbar.hToolbarBmp = icon.GetHbitmap();
        toolbar.hToolbarIconDarkMode = toolbar.hToolbarIcon = icon.GetHicon();
        IntPtr pTbIcons = Marshal.AllocHGlobal(Marshal.SizeOf<ToolbarIcons>());
        Marshal.StructureToPtr(toolbar, pTbIcons, false);
        _ = Win32.SendMessage(NppHandle, (uint)NppMsg.NPPM_ADDTOOLBARICON_FORDARKMODE, cmdId, pTbIcons);
        Marshal.FreeHGlobal(pTbIcons);
    }
}

/// <summary>
/// This class holds helpers for sending messages defined in the Resource_h.cs file. It is at the moment
/// incomplete. Please help fill in the blanks.
/// </summary>
internal class NppResource
{
    private const int Unused = 0;

    public void ClearIndicator() => Win32.SendMessage(PluginBase.nppData._nppHandle, (uint)Resource.NPPM_INTERNAL_CLEARINDICATOR, Unused, Unused);
}
