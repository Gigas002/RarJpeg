using System.Reflection;

namespace RarJpeg.Enums
{
    /// <summary>
    /// Some const values for <see cref="ViewModels.MainViewModel"/> class.
    /// </summary>
    internal static class MainViewModel
    {
        #region UI

        /// <summary>
        /// Copyright string.
        /// </summary>
        internal const string Copyright = "© Gigas002 2019";

        /// <summary>
        /// Info about current version. Pattern: {MAJOR}.{MINOR}.{PATCH}.{BUILD}
        /// </summary>
        internal static string Version { get; } = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        #endregion
    }
}
