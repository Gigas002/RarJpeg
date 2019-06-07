using System.Reflection;

namespace RarJpeg.Enums
{
    internal static class MainViewModel
    {
        internal const string Copyright = "© Gigas002 2019";

        internal static string Version { get; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }
}
