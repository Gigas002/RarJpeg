using System;
using System.IO;
using System.Threading.Tasks;

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
        internal static async ValueTask RarJpegAsync(string containerFilePath, string archivePath, string outputFilePath)
        {
            byte[] containerBytes = await File.ReadAllBytesAsync(containerFilePath).ConfigureAwait(false);
            byte[] archiveBytes = await File.ReadAllBytesAsync(archivePath).ConfigureAwait(false);
            byte[] outputBytes = new byte[containerBytes.Length + archiveBytes.Length];

            await Task.Run(() =>
            {
                Array.Copy(containerBytes, 0, outputBytes, 0, containerBytes.Length);
                Array.Copy(archiveBytes, 0, outputBytes, containerBytes.Length, archiveBytes.Length);
            }).ConfigureAwait(false);

            await File.WriteAllBytesAsync(outputFilePath, outputBytes).ConfigureAwait(false);
        }
    }
}
