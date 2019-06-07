using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using ICSharpCode.SharpZipLib.Zip;
using Ookii.Dialogs.Wpf;
using RarJpeg.Models;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace RarJpeg.ViewModels
{
    internal class MainViewModel : PropertyChangedBase
    {
        //todo move strings to enums
        //todo rename model's class
        //todo localize strings
        //todo xml-doc

        #region Properties

        #region UI

        public string ContainerHint { get; } = Enums.MainViewModel.ContainerHint;

        public string ArchiveHint { get; } = Enums.MainViewModel.ArchiveHint;

        public string ReadyHint { get; } = Enums.MainViewModel.ReadyHint;

        public string Start { get; } = Enums.MainViewModel.Start;

        public string Copyright { get; } = Enums.MainViewModel.Copyright;

        public string Version { get; } = Enums.MainViewModel.Version;

        #endregion

        #region Backing fields

        private bool _isGridEnabled;

        private string _containerPath;

        private string _archivePath;

        private string _readyPath;

        #endregion

        public bool IsGridEnabled
        {
            get => _isGridEnabled;
            set
            {
                _isGridEnabled = value;
                NotifyOfPropertyChange(() => IsGridEnabled);
            }
        }

        public string ContainerPath
        {
            get => _containerPath;
            set
            {
                _containerPath = value;
                NotifyOfPropertyChange(() => ContainerPath);
            }
        }

        public string ArchivePath
        {
            get => _archivePath;
            set
            {
                _archivePath = value;
                NotifyOfPropertyChange(() => ArchivePath);
            }
        }

        public string ReadyPath
        {
            get => _readyPath;
            set
            {
                _readyPath = value;
                NotifyOfPropertyChange(() => ReadyPath);
            }
        }

        #endregion

        #region Constructor

        public MainViewModel()
        {
            IsGridEnabled = true;
            ContainerPath = string.Empty;
            ArchivePath = string.Empty;
            ReadyPath = string.Empty;
        }

        #endregion

        #region Methods

        #region Buttons

        public void SelectContainerButton()
        {
            VistaOpenFileDialog openFileDialog = new VistaOpenFileDialog();
            ContainerPath = openFileDialog.ShowDialog() == true ? openFileDialog.FileName : ContainerPath;
        }

        public void SelectArchiveButton()
        {
            VistaOpenFileDialog openFileDialog = new VistaOpenFileDialog();
            ArchivePath = openFileDialog.ShowDialog() == true ? openFileDialog.FileName : ArchivePath;
        }

        public void ReadyPathButton()
        {
            VistaSaveFileDialog saveFileDialog = new VistaSaveFileDialog();
            ReadyPath = saveFileDialog.ShowDialog() == true ? saveFileDialog.FileName : ReadyPath;
        }

        public async void StartButton()
        {
            #region Cheсks

            try
            {
                CheckData();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return;
            }

            #endregion

            //Block the UI while working.
            IsGridEnabled = false;

            try
            {
                await Task.Run(() => MainModel.RarJpeg(ContainerPath, ArchivePath, ReadyPath));
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Error occured: {exception.Message}. Inner exception: {exception.InnerException?.Message}.");
                IsGridEnabled = true;
                return;
            }

            MessageBox.Show("Done!");

            IsGridEnabled = true;
        }

        #endregion

        #region Other

        private void CheckData()
        {
            #region Check container file

            if (string.IsNullOrWhiteSpace(ContainerPath) || !File.Exists(ContainerPath))
                throw new Exception("Container file doesn't exist or path string is empty.");

            #endregion

            #region Check archive

            if (string.IsNullOrWhiteSpace(ArchivePath) || !File.Exists(ArchivePath))
                throw new Exception("Archive doesn't exist or path string is empty.");
            try
            {
                ZipFile zipFile = new ZipFile(ArchivePath);
                if (!zipFile.TestArchive(true))
                    throw new Exception("Archive is corrupted.");
            }
            catch (Exception)
            {
                throw new Exception("Selected archive file is corrupted or not an archive.");
            }

            #endregion

            #region Check output file

            if (string.IsNullOrWhiteSpace(ReadyPath))
                throw new Exception("Output file's path is empty.");
            ReadyPath = $"{ReadyPath}{Path.GetExtension(ArchivePath)}" +
                             $"{Path.GetExtension(ContainerPath)}";
            if (File.Exists(ReadyPath))
                throw new Exception("Output file is already exists.");

            #endregion
        }

        #endregion

        #endregion
    }
}
