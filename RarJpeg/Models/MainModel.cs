using System;
using System.IO;

namespace RarJpeg.Models
{
    internal static class MainModel
    {
        internal static void RarJpeg(string containerFilePath, string archiveFilePath, string outputFilePath)
        {
            byte[] containerBytes = File.ReadAllBytes(containerFilePath);
            byte[] archiveBytes = File.ReadAllBytes(archiveFilePath);
            byte[] outputBytes = new byte[containerBytes.Length + archiveBytes.Length];

            Array.Copy(containerBytes, 0, outputBytes, 0, containerBytes.Length);
            Array.Copy(archiveBytes, 0, outputBytes, containerBytes.Length, archiveBytes.Length);

            File.WriteAllBytes(outputFilePath, outputBytes);
        }
    }
}
