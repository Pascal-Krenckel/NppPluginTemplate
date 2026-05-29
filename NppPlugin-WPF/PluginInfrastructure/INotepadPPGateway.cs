// NPP plugin platform for .Net v0.94.00 by Kasper B. Graversen etc.
using System.Drawing;

namespace ____NppPlugin____.PluginInfrastructure;

public interface INotepadPPGateway
{
    void FileNew();
    long GetBufferEncoding(nint bufferId);
    nint GetCurrentBufferId();
    string GetCurrentFilePath();
    string GetFilePath(int bufferId);
    void MakeCurrentBufferDirty();
    void SaveCurrentFile();
    void SendMenuEncoding(NppEncoding enc);
    void SetCurrentLanguage(LangType language);
    void SetMenuItemCheck(FuncItem cmd, bool @checked);
    void SetMenuItemCheck(int cmdIndex, bool @checked);
    void SetMenuItemCheck(string commandName, bool @checked);
    bool SetStatusBar(NppMsg statusBarType, string str);
    void SetToolbarIcon(int cmdId, Bitmap icon);
    void SetToolbarIcon(int cmdId, Bitmap icon, Bitmap iconDarkMode);
    void SwitchToFile(string path);
}