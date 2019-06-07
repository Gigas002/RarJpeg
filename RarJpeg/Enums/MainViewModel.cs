using System.Reflection;

namespace RarJpeg.Enums
{
    internal static class MainViewModel
    {
        #region UI

        internal const string ContainerHint = "Select container file";

        internal const string ArchiveHint = "Select archive";

        internal const string ReadyHint = "Select ready file's path";

        internal const string Start = "Start";

        internal const string Copyright = "© Gigas002 2019";

        internal static string Version { get; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        #endregion

        #region Exceptions and MessageBoxes text

        internal const string Done = "Done!";
        internal const string ContainerExistEmpty = "Container file doesn't exist or path string is empty.";
        internal const string ContainerExtension = "Selected container file doesn't have extension. Continue?";
        internal const string ArchiveExistEmpty = "Archive doesn't exist or path string is empty.";
        internal const string ArchiveExtension = "Archive doesn't contain extension.";
        internal const string ArchiveCorrupted = "Archive is corrupted or not an archive.";
        internal const string ReadyEmpty = "Ready file's path is empty.";
        internal const string ReadyExtension = "Ready file contain extension. Please, enter file's name without extension.";
        internal const string ReadyExist = "Ready file is already exists.";

        #endregion
    }
}
