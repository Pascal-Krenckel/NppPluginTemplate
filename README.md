# NppPluginTemplate

A Visual Studio project template for building [Notepad++](https://notepad-plus-plus.org/) plugins in **C# (.NET 6–9)**.

> ⚠️ **.NET 10 is not yet supported** — [DNNE](https://github.com/AaronRobinsonMSFT/DNNE) does not support it at this time.

## Overview

Writing a Notepad++ plugin in C# is non-trivial because Notepad++ loads plugins as native unmanaged DLLs, while .NET assemblies are managed code. This template solves that problem by using [DNNE](https://github.com/AaronRobinsonMSFT/DNNE) to generate a native export shim, allowing your managed C# code to be loaded by Notepad++ as a standard plugin DLL — no COM interop, no manual marshalling boilerplate.

The template is based on [NotepadPlusPlusPluginPack.Net](https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net) and extends it for modern .NET.

## Features

- Targets **.NET 6, 7, 8, or 9** (choose what suits your project)
- Uses **DNNE** to expose managed methods as unmanaged exports, making the DLL compatible with Notepad++'s native plugin loader
- Two UI variants included out of the box:
  - **NppPlugin-WinForms** — for plugins with Windows Forms dialogs
  - **NppPlugin-WPF** — for plugins with WPF-based UI
- Ships as a **NuGet Visual Studio project template** (`Krenckel.NppPlugin`) for easy installation and reuse
- Licensed under **Apache 2.0**

## Requirements

- [Visual Studio 2022](https://visualstudio.microsoft.com/) (or later) with the **.NET desktop development** workload
- .NET SDK 6, 7, 8, or 9 (not 10)
- Notepad++ (64-bit recommended) for testing

## Installation

Install the template from NuGet by running the following inside a VS Developer Command Prompt:

```
dotnet new install Krenckel.NppPlugin
```

Alternatively, clone this repository and run `pack.bat` to build the `.nupkg` locally, then install it:

```
pack.bat
dotnet new install ./Krenckel.NppPlugin.<version>.nupkg
```

Uninstall:
```
dotnet new uninstall Krenckel.NppPlugin
```

## Usage

After installing the template, create a new project in Visual Studio:

1. Open Visual Studio → **Create a new project**
2. Search for **"Notepad++ Plugin"**
3. Choose either the **WinForms** or **WPF** variant
4. Name your project and click **Create**

The plugin name is set automatically from your project name — no manual renaming needed.

### Configuring the Output Directory

The project is configured to copy the built DLLs automatically into your Notepad++ plugins folder after each build. The target path is defined in **`Properties/NppDir.props`** — open that file and adjust the directory to match your Notepad++ installation before building for the first time.

### Debugging

**`Properties/launchSettings.json`** is pre-configured to launch Notepad++ as the debug target. Once `NppDir.props` points at the right directory, press **F5** in Visual Studio to build, deploy, and attach the debugger to Notepad++ in one step. You can then set breakpoints in your plugin code as normal.

### Deploying Manually

If you prefer to copy the files yourself, copy **all files** from the build output directory into a dedicated plugin subfolder. The subfolder name must exactly match the name of the DNNE-generated DLL (i.e. your project name, without the `.dll` extension):

| Installation type | Path |
|---|---|
| 64-bit (standard) | `%ProgramFiles%\Notepad++\plugins\YourPlugin\` |
| 32-bit | `%ProgramFiles(x86)%\Notepad++\plugins\YourPlugin\` |
| Portable edition | `<portable notepad++ directory>\plugins\YourPlugin\` |
| Per-user / AppData | `%AppData%\Notepad++\plugins\YourPlugin\` |

> **Note:** Writing to `%ProgramFiles%` requires administrator permissions. Either run the copy as an administrator (or VS), or point `NppDir.props` at a per-user installation instead.

After copying, restart Notepad++. Your plugin will appear under the **Plugins** menu.

> **Tip:** If Windows blocks a DLL downloaded from the internet, right-click it → **Properties** → check **Unblock** → **Apply**.

---

## How to Write a Plugin

This is the core of what you need to know. The WinForms and WPF templates are identical in plugin structure — the UI variant only affects how you build dialogs.

### Key Files

**`Plugin.cs`** is where you do all your work. It contains:

- `PluginInit()` — registers your plugin's menu commands and toolbar icons
- `CleanUp()` — runs on plugin unload (save settings here)
- `OnNotification(ScNotification)` — receives Notepad++/Scintilla events
- Your own command handler methods (one per menu item)

**`NotepadPPGateway`** is a C# wrapper around the Notepad++ message API. Use it to interact with the editor host — open files, query the current file path, navigate tabs, get the plugin config directory, etc.

**`ScintillaGateway`** is a C# wrapper around the Scintilla text editing component that Notepad++ uses internally. Use it to read and manipulate the text in the current document — get/set text, manage selections, insert content, control undo, and so on.

Both gateways are already instantiated as static properties in `Plugin.cs`:

```csharp
public static NotepadPPGateway NppGateway { get; } = new();
public static ScintillaGateway ScintillaGateway => new(PluginBase.GetCurrentScintilla());
```

Note that `ScintillaGateway` is a property (not a field) — it creates a new instance each time, picking up whichever Scintilla pane is currently active. This is intentional: Notepad++ has two editor panes and the active one can change between calls.

### Step 1 — Register Menu Commands

Inside `PluginInit()`, call `PluginBase.AddCommand` for each item you want to appear in your plugin's submenu. The template already includes a working example with a toolbar icon:

```csharp
internal static void PluginInit()
{
    // Simple command with no icon
    PluginBase.AddCommand("Say Hello", SayHello);

    // Command with a keyboard shortcut (Ctrl+Alt+W)
    PluginBase.AddCommand("Insert Word Count", InsertWordCount,
        shortcut: new ShortcutKey(true, true, false, Keys.W));

    // Command with a toolbar icon (icons must be 16x16 bitmaps)
    Bitmap icon = new Bitmap("myIcon.png");
    PluginBase.AddCommand("My Tool", MyTool, icon);
}
```

Each command handler is a method you define anywhere in the class.

### Step 2 — Write Your Command Handlers

Each command handler is a parameterless method (or lambda). Use `NppGateway` and `ScintillaGateway` to do the actual work.

**Example: Insert text at the cursor**

```csharp
static void SayHello()
{
    Plugin.ScintillaGateway.ReplaceSel("Hello from my plugin!");
}
```

**Example: Read the full document text**

```csharp
static void InsertWordCount()
{
    var editor = Plugin.ScintillaGateway;
    string text = editor.GetText(editor.GetTextLength() + 1);
    int words = text.Split(new[] { ' ', '\n', '\r', '\t' },
        StringSplitOptions.RemoveEmptyEntries).Length;
    editor.ReplaceSel($"Word count: {words}");
}
```

**Example: Get the current file's path**

```csharp
static void ShowCurrentFile()
{
    string path = Plugin.NppGateway.GetCurrentFilePath();
    MessageBox.Show(path, "Current File");
}
```

**Example: Open a new file tab**

```csharp
static void OpenNewTab()
{
    Plugin.NppGateway.FileNew();
    Plugin.ScintillaGateway.SetText("// New file created by MyPlugin");
}
```

**Example: Wrap the current selection in a tag (with undo support)**

```csharp
static void WrapInBold()
{
    var editor = Plugin.ScintillaGateway;
    string selected = editor.GetSelText();
    editor.BeginUndoAction();
    editor.ReplaceSel($"<b>{selected}</b>");
    editor.EndUndoAction();
}
```

### Step 3 — React to Notepad++ Events (Notifications)

The `OnNotification` method is called by Notepad++ whenever something happens — a file is saved, the active tab changes, a character is typed, etc. Filter by notification code:

```csharp
internal static void OnNotification(ScNotification notification)
{
    switch (notification.Header.Code)
    {
        case (uint)NppMsg.NPPN_FILESAVED:
            // A file was just saved
            OnFileSaved();
            break;

        case (uint)NppMsg.NPPN_BUFFERACTIVATED:
            // User switched to a different tab
            OnTabChanged();
            break;

        case (uint)SciMsg.SCN_CHARADDED:
            // A character was typed
            char typed = (char)notification.Character;
            HandleCharAdded(typed);
            break;
    }
}
```

For the full list of notification codes, see the [Plugin Communication docs](https://npp-user-manual.org/docs/plugin-communication/).

### Step 4 — Save and Load Settings

Use `CleanUp()` to persist settings when the plugin unloads, and read them back in `PluginInit()`. Use `NppGateway.GetPluginsConfigDir()` to locate the config directory and store your settings in JSON (or XML) — avoid calling Win32 INI functions directly:

```csharp
static string _configPath;
static MySettings _settings = new();

internal static void PluginInit()
{
    // Get the Notepad++ plugin config directory via the gateway
    string configDir = Plugin.NppGateway.GetPluginsConfigDir();
    _configPath = Path.Combine(configDir, Plugin.PluginName, "settings.json");

    // Load settings if they exist
    if (File.Exists(_configPath))
    {
        string json = File.ReadAllText(_configPath);
        _settings = JsonSerializer.Deserialize<MySettings>(json) ?? new();
    }

    PluginBase.AddCommand("My Option", ToggleMyOption);
}

internal static void CleanUp()
{
    // Ensure the config directory exists, then save settings as JSON
    Directory.CreateDirectory(Path.GetDirectoryName(_configPath)!);
    File.WriteAllText(_configPath, JsonSerializer.Serialize(_settings));
}

class MySettings
{
    public bool MyOption { get; set; } = false;
}
```

### Architecture Overview

```
+-----------+     +-----------+
| Scintilla |     | Notepad++ |
+-----------+     +-----------+
      ^                 ^
      |                 |
+------------------+ +------------------+
| ScintillaGateway | | NotepadPPGateway |
+------------------+ +------------------+
           ^               ^
           |               |
        +----------------------+
        |      Plugin.cs       |
        +----------------------+
                   ^
                   |
        +----------------------+
        |    DNNE shim DLL     |  ← loaded by Notepad++ as a native DLL
        +----------------------+
```

Your code lives entirely in the managed layer (`Plugin.cs` and any classes you add). DNNE handles the boundary between Notepad++'s native plugin loader and your .NET code transparently.

---

## How It Works (Internals)

Notepad++ expects plugins to be native (unmanaged) DLLs that export a specific set of C functions (`isUnicode`, `getName`, `getFuncsArray`, `beNotified`, etc.). Standard .NET assemblies cannot directly satisfy this requirement.

**DNNE** generates a companion native DLL at build time that forwards calls from the unmanaged world into your managed C# code. This means you write your plugin entirely in C#, while DNNE handles the ABI boundary transparently.

## Related Projects

- [DNNE](https://github.com/AaronRobinsonMSFT/DNNE) — Native Exports for .NET
- [NotepadPlusPlusPluginPack.Net](https://github.com/kbilsted/NotepadPlusPlusPluginPack.Net) — The plugin pack this template builds upon
- [Plugin Communication docs](https://npp-user-manual.org/docs/plugin-communication/) — Full list of Notepad++ messages and notifications
- [Notepad++ Plugin List](https://github.com/notepad-plus-plus/nppPluginList) — Official plugin registry

## License

Licensed under the [Apache License 2.0](LICENSE.txt).