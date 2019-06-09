using System;
using System.IO;

namespace RarJpeg.Models
{
    /// <summary>
    /// Class with main logics for <see cref="ViewModels.MainViewModel"/> class.
    /// </summary>
    internal static class MainModel
    {
        /// <summary>
        /// Creates .{archiveExtension}.{containerExtension} output file.
        /// </summary>
        /// <param name="containerFilePath">Full path to the container file.</param>
        /// <param name="archivePath">Full path to the archive.</param>
        /// <param name="outputFilePath">Full path to the output file.</param>
        internal static void RarJpeg(string containerFilePath, string archivePath, string outputFilePath)
        {
            byte[] containerBytes = File.ReadAllBytes(containerFilePath);
            byte[] archiveBytes = File.ReadAllBytes(archivePath);
            byte[] outputBytes = new byte[containerBytes.Length + archiveBytes.Length];

            Array.Copy(containerBytes, 0, outputBytes, 0, containerBytes.Length);
            Array.Copy(archiveBytes, 0, outputBytes, containerBytes.Length, archiveBytes.Length);

            File.WriteAllBytes(outputFilePath, outputBytes);
        }
    }
}
