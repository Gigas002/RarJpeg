using System;
using System.IO;
using System.Windows;
using Caliburn.Micro;
using Ookii.Dialogs.Wpf;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace RarJpeg.ViewModels
{
    internal class MainViewModel : PropertyChangedBase
    {
        #region Properties

        //todo Add author and version info

        #region Backing fields

        private string _containerFilePath;

        private string _archiveFilePath;

        private string _outputFilePath;

        #endregion

        public string ContainerFilePath
        {
            get => _containerFilePath;
            set
            {
                _containerFilePath = value;
                NotifyOfPropertyChange(() => ContainerFilePath);
            }
        }

        public string ArchiveFilePath
        {
            get => _archiveFilePath;
            set
            {
                _archiveFilePath = value;
                NotifyOfPropertyChange(() => ArchiveFilePath);
            }
        }

        public string OutputFilePath
        {
            get => _outputFilePath;
            set
            {
                _outputFilePath = value;
                NotifyOfPropertyChange(() => OutputFilePath);
            }
        }

        #endregion

        #region Constructor

        public MainViewModel()
        {
            ContainerFilePath = string.Empty;
            ArchiveFilePath = string.Empty;
            OutputFilePath = string.Empty;
        }

        #endregion

        #region Methods

        #region Buttons

        public void ContainerFileButton()
        {
            VistaOpenFileDialog openFileDialog = new VistaOpenFileDialog();
            ContainerFilePath = openFileDialog.ShowDialog() != true ? ContainerFilePath : openFileDialog.FileName;
        }

        public void ArchiveFileButton()
        {
            VistaOpenFileDialog openFileDialog = new VistaOpenFileDialog();
            ArchiveFilePath = openFileDialog.ShowDialog() != true ? ArchiveFilePath : openFileDialog.FileName;
        }

        public async void StartButton()
        {
            #region Cheks

            if (!CheckData())
                return;

            #endregion

            //todo async
            //todo block interface

            //VistaSaveFileDialog vistaSaveFileDialog = new VistaSaveFileDialog();
            //vistaSaveFileDialog.ShowDialog();

            //ResultingFileInfo = new FileInfo($"{vistaSaveFileDialog.FileName}{InnerFileInfo.Extension}{ContainerFileInfo.Extension}");
            //if (ResultingFileInfo.Exists)
            //    MessageBox.Show("This file already exists.");

            //OutputFilePath = $"{OutputFilePath}{Path.GetExtension(ArchiveFilePath)}" +
            //                 $"{Path.GetExtension(ContainerFilePath)}";

            byte[] containerBytes = File.ReadAllBytes(ContainerFilePath);
            byte[] innerBytes = File.ReadAllBytes(ArchiveFilePath);
            byte[] readyBytes = new byte[containerBytes.Length + innerBytes.Length];

            Array.Copy(containerBytes, 0, readyBytes, 0, containerBytes.Length);
            Array.Copy(innerBytes, 0, readyBytes, containerBytes.Length, innerBytes.Length);

            File.WriteAllBytes(OutputFilePath, readyBytes);

            MessageBox.Show("Done!");

            //todo unblock interface
        }

        #endregion

        #region Other

        private bool CheckData()
        {
            //Check container file
            if (string.IsNullOrWhiteSpace(ContainerFilePath) || !File.Exists(ContainerFilePath))
            {
                MessageBox.Show("Container file doesn't exist or path string is empty.");
                return false;
            }
            //if (string.IsNullOrWhiteSpace(Path.GetExtension(ContainerFilePath)))
            //{
            //    MessageBox.Show("Warning! Container file doesn't have extension.");
            //}

            //Check archive
            if (string.IsNullOrWhiteSpace(ArchiveFilePath) || !File.Exists(ArchiveFilePath))
            {
                MessageBox.Show("Archive doesn't exist or path string is empty.");
                return false;
            }
            //todo check if archive or not
            //if (string.IsNullOrWhiteSpace(Path.GetExtension(ContainerFilePath)))
            //{
            //    MessageBox.Show("Warning! Container file doesn't have extension.");
            //}

            //Check output file
            if (string.IsNullOrWhiteSpace(OutputFilePath))
            {
                MessageBox.Show("Output file's path is empty.");
                return false;
            }
            string previousOutputFilePath = OutputFilePath;
            OutputFilePath = $"{OutputFilePath}{Path.GetExtension(ArchiveFilePath)}" +
                             $"{Path.GetExtension(ContainerFilePath)}";
            if (File.Exists(OutputFilePath))
            {
                MessageBox.Show("Output file is already exists.");
                OutputFilePath = previousOutputFilePath;
                return false;
            }

            return true;
        }

        #endregion

        #endregion
    }
}
