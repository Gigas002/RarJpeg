﻿using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using ICSharpCode.SharpZipLib.Zip;
using MaterialDesignThemes.Wpf;
using Ookii.Dialogs.Wpf;
using RarJpeg.Models;
using RarJpeg.Properties;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace RarJpeg.ViewModels
{
    internal class MainViewModel : PropertyChangedBase
    {
        //todo xml-doc
        //todo material dialogs
        //todo add tests
        //todo take a look at project settings
        //todo take a look at publish

        #region Properties

        #region UI

        public string ContainerHint { get; } = Strings.ContainerHint;

        public string ArchiveHint { get; } = Strings.ArchiveHint;

        public string ReadyHint { get; } = Strings.ReadyHint;

        public string Start { get; } = Strings.Start;

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

        public async ValueTask SelectContainerButton()
        {
            try
            {
                VistaOpenFileDialog openFileDialog = new VistaOpenFileDialog();
                ContainerPath = openFileDialog.ShowDialog() == true ? openFileDialog.FileName : ContainerPath;
            }
            catch (Exception exception)
            {
                await DialogHost.Show(new MessageBoxDialogViewModel(exception.Message));
            }
        }

        public async ValueTask SelectArchiveButton()
        {
            try
            {
                VistaOpenFileDialog openFileDialog = new VistaOpenFileDialog();
                ArchivePath = openFileDialog.ShowDialog() == true ? openFileDialog.FileName : ArchivePath;
            }
            catch (Exception exception)
            {
                await DialogHost.Show(new MessageBoxDialogViewModel(exception.Message));
            }
        }

        public async ValueTask ReadyPathButton()
        {
            try
            {
                VistaSaveFileDialog saveFileDialog = new VistaSaveFileDialog();
                ReadyPath = saveFileDialog.ShowDialog() == true ? saveFileDialog.FileName : ReadyPath;
            }
            catch (Exception exception)
            {
                await DialogHost.Show(new MessageBoxDialogViewModel(exception.Message));
            }
        }

        public async ValueTask StartButton()
        {
            if (!await StartWork()) return;

            bool isSuccessful = true;
            try
            {
                await Task.Run(() => MainModel.RarJpeg(ContainerPath, ArchivePath, _readyPath));
            }
            catch (Exception exception)
            {
                await DialogHost.Show(new MessageBoxDialogViewModel(exception.Message));
                isSuccessful = false;
            }

            await CancelWork(isSuccessful);
        }

        #endregion

        #region Other

        private async ValueTask<bool> StartWork()
        {
            #region Cheсks

            try
            {
                await CheckData();
            }
            catch (Exception exception)
            {
                if (string.IsNullOrWhiteSpace(exception.Message)) return false;
                await DialogHost.Show(new MessageBoxDialogViewModel(exception.Message));
                return false;
            }

            #endregion

            //Block the UI while working.
            IsGridEnabled = false;

            return true;
        }

        private async ValueTask CancelWork(bool isSuccessful)
        {
            IsGridEnabled = true;
            _readyPath = ReadyPath;
            if (isSuccessful)
                await DialogHost.Show(new MessageBoxDialogViewModel(Strings.Done));
        }

        private async ValueTask CheckData()
        {
            #region Check container file

            if (string.IsNullOrWhiteSpace(ContainerPath) || !File.Exists(ContainerPath))
                throw new Exception(Strings.ContainerExistEmpty);
            if (string.IsNullOrWhiteSpace(Path.GetExtension(ContainerPath)))
            {
                if (!(bool)
                        await DialogHost.Show(new MessageBoxDialogViewModel(Strings.ContainerExtension,
                                                                            Visibility.Visible)))
                    throw new Exception(string.Empty);
            }

            #endregion

            #region Check archive

            if (string.IsNullOrWhiteSpace(ArchivePath) || !File.Exists(ArchivePath))
                throw new Exception(Strings.ArchiveExistEmpty);
            if (string.IsNullOrWhiteSpace(Path.GetExtension(ArchivePath)))
                throw new Exception(Strings.ArchiveExtension);
            if (!new ZipFile(ArchivePath).TestArchive(true))
                throw new Exception(Strings.ArchiveCorrupted);

            #endregion

            #region Check ready file

            if (string.IsNullOrWhiteSpace(ReadyPath))
                throw new Exception(Strings.ReadyEmpty);
            if (!string.IsNullOrWhiteSpace(Path.GetExtension(ReadyPath)))
                throw new Exception(Strings.ReadyExtension);
            _readyPath = $"{ReadyPath}{Path.GetExtension(ArchivePath)}" +
                             $"{Path.GetExtension(ContainerPath)}";
            if (File.Exists(ReadyPath))
                throw new Exception(Strings.ReadyExist);

            #endregion
        }

        #endregion

        #endregion
    }
}
