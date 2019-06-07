using System.Reflection;

namespace RarJpeg.Enums
{
    internal static class MainViewModel
    {
        internal const string ContainerHint = "Select container file";
        internal const string ArchiveHint = "Select archive";
        internal const string ReadyHint = "Select ready file's path";
        internal const string Start = "Start";

        internal const string Copyright = "© Gigas002 2019";

        internal static string Version { get; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }
}
