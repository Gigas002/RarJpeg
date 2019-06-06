using System;
using System.IO;
using System.Windows;
using Ookii.Dialogs.Wpf;

namespace RarJpeg
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Properties

        private FileInfo ContainerFileInfo { get; set; }

        private FileInfo InnerFileInfo { get; set; }

        private FileInfo ResultingFileInfo { get; set; }

        #endregion

        public MainWindow() => InitializeComponent();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            VistaOpenFileDialog vistaOpenFileDialog = new VistaOpenFileDialog();
            vistaOpenFileDialog.ShowDialog();
            ContainerFileInfo = new FileInfo(vistaOpenFileDialog.FileName);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            VistaOpenFileDialog vistaOpenFileDialog = new VistaOpenFileDialog();
            vistaOpenFileDialog.ShowDialog();
            InnerFileInfo = new FileInfo(vistaOpenFileDialog.FileName);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (!ContainerFileInfo.Exists)
                MessageBox.Show("Container file is not exist.");
            if (!InnerFileInfo.Exists)
                MessageBox.Show("Inner file is not exist.");

            //TODO check extensions

            VistaSaveFileDialog vistaSaveFileDialog = new VistaSaveFileDialog();
            vistaSaveFileDialog.ShowDialog();

            ResultingFileInfo = new FileInfo($"{vistaSaveFileDialog.FileName}{InnerFileInfo.Extension}{ContainerFileInfo.Extension}");
            if (ResultingFileInfo.Exists)
                MessageBox.Show("This file already exists.");

            var containerBytes = File.ReadAllBytes(ContainerFileInfo.FullName);
            var innerBytes = File.ReadAllBytes(InnerFileInfo.FullName);
            var readyBytes = new byte[containerBytes.Length + innerBytes.Length];

            Array.Copy(containerBytes, 0, readyBytes, 0, containerBytes.Length);
            Array.Copy(innerBytes, 0, readyBytes, containerBytes.Length, innerBytes.Length);

            File.WriteAllBytes(ResultingFileInfo.FullName, readyBytes);

            MessageBox.Show("Done!");
        }
    }
}
