using ____NppPlugin____.PluginInfrastructure;
using System.Drawing;
using System.Windows;

namespace ____NppPlugin____;

public class Plugin
{
    public static readonly string PluginName = "____NppPlugin____";
    public static NotepadPPGateway NppGateway { get; } = new();
    public static ScintillaGateway ScintillaGateway => new(PluginBase.GetCurrentScintilla());

    internal static void OnNotification(ScNotification notification)
    {
        //switch(notification.Header.Code)
        //{
        //    case ...:
        //}
    }


    internal static void PluginInit()
    {
        // you have to specify at least one command, no icon needed, but as an example, here is how to create a rainbow colored toolbar icon for your command
        Color[] rainbow = [Color.Red,Color.OrangeRed,Color.Orange,Color.Yellow,Color.Yellow, Color.GreenYellow,Color.Lime,Color.Lime,
            Color.LimeGreen,Color.LimeGreen,Color.DarkTurquoise,Color.Blue,Color.Blue,Color.BlueViolet, Color.DarkViolet,Color.Transparent];
        Bitmap bitmap = new Bitmap(16, 16); // ToolbarIcons are 16x16
        for (int i = 0; i < 16; i++)
            for (int j = 0; j < 16; j++)
                bitmap.SetPixel(i, j, rainbow[i]);


        PluginBase.AddCommand("MyMenuCommand", () => MessageBox.Show("Hello World!!!"), bitmap);
    }
    internal static void CleanUp()
    {

    }


}
