// NPP plugin platform for .Net v0.94.00 by Kasper B. Graversen etc.

// NPP plugin platform for .Net v0.94.00 by Kasper B. Graversen etc.
using System.Runtime.InteropServices;

namespace ____NppPlugin____.PluginInfrastructure;

public class ClikeStringArray : IDisposable
{
    private readonly List<nint> _nativeItems;
    private bool _disposed = false;

    public ClikeStringArray(int num, int stringCapacity)
    {
        NativePointer = Marshal.AllocHGlobal((num + 1) * nint.Size);
        _nativeItems = [];
        for (int i = 0; i < num; i++)
        {
            nint item = Marshal.AllocHGlobal(stringCapacity);
            Marshal.WriteIntPtr((int)NativePointer + (i * nint.Size), item);
            _nativeItems.Add(item);
        }
        Marshal.WriteIntPtr((int)NativePointer + (num * nint.Size), nint.Zero);
    }
    public ClikeStringArray(List<string> lstStrings)
    {
        NativePointer = Marshal.AllocHGlobal((lstStrings.Count + 1) * nint.Size);
        _nativeItems = [];
        for (int i = 0; i < lstStrings.Count; i++)
        {
            nint item = Marshal.StringToHGlobalUni(lstStrings[i]);
            Marshal.WriteIntPtr((int)NativePointer + (i * nint.Size), item);
            _nativeItems.Add(item);
        }
        Marshal.WriteIntPtr((int)NativePointer + (lstStrings.Count * nint.Size), nint.Zero);
    }

    public nint NativePointer { get; }
    public List<string> ManagedStringsAnsi => GetManagedItems(false);
    public List<string> ManagedStringsUnicode => GetManagedItems(true);

    private List<string> GetManagedItems(bool unicode)
    {
        List<string> _managedItems = [];
        for (int i = 0; i < _nativeItems.Count; i++)
            if (unicode)
                _managedItems.Add(Marshal.PtrToStringUni(_nativeItems[i])!);
            else
                _managedItems.Add(Marshal.PtrToStringAnsi(_nativeItems[i])!);
        return _managedItems;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            for (int i = 0; i < _nativeItems.Count; i++)
                if (_nativeItems[i] != nint.Zero)
                    Marshal.FreeHGlobal(_nativeItems[i]);
            if (NativePointer != nint.Zero)
                Marshal.FreeHGlobal(NativePointer);
            _disposed = true;
            GC.SuppressFinalize(this);
        }
    }
    ~ClikeStringArray()
    {
        Dispose();
    }
}