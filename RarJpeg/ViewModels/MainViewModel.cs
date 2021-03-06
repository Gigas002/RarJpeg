﻿using System;
using System.IO;
using System.Threading.Tasks;
using Caliburn.Micro;
using ICSharpCode.SharpZipLib.Zip;
using MaterialDesignExtensions.Controls;
using MaterialDesignThemes.Wpf;
using RarJpeg.Enums;
using RarJpeg.Helpers;
using RarJpeg.Models;
using RarJpeg.Localization;
using RarJpeg.Properties;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace RarJpeg.ViewModels
{
    /// <inheritdoc />
    /// <summary>
    /// ViewModel for <see cref="T:RarJpeg.Views.MainView" />.
    /// </summary>
    internal class MainViewModel : PropertyChangedBase
    {
        #region Properties

        #region Settings

        /// <summary>
        /// Shows if dark theme selected.
        /// </summary>
        public bool IsDarkTheme { get; } = Settings.Default.IsDarkTheme;

        #endregion

        #region UI

        /// <summary>
        /// String, that displays on container's path textbox as hint.
        /// </summary>
        public string ContainerHint { get; } = Strings.ContainerHint;

        /// <summary>
        /// String, that displays on archive's path textbox as hint.
        /// </summary>
        public string ArchiveHint { get; } = Strings.ArchiveHint;

        /// <summary>
        /// String, that displays on output's path textbox as hint.
        /// </summary>
        public string OutputHint { get; } = Strings.OutputHint;

        /// <summary>
        /// Start button's text.
        /// </summary>
        public string Start { get; } = Strings.Start;

        /// <summary>
        /// Hey, it's me!
        /// </summary>
        public string Copyright { get; } = Enums.MainViewModel.Copyright;

        /// <summary>
        /// Info about current version. Pattern: {MAJOR}.{MINOR}.{PATCH}.{BUILD}
        /// </summary>
        public string Version { get; } = Enums.MainViewModel.Version;

        #endregion

        /// <summary>
        /// Identifier of DialogHost on <see cref="Views.MainView"/>.
        /// </summary>
        public string DialogHostId { get; } = Enums.MainViewModel.DialogHostId;

        /// <summary>
        /// Theme string for DialogHosts.
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string Theme { get; }

        /// <summary>
        /// Property to store real output path.
        /// </summary>
        private string TempOutputPath { get; set; }

        #region Backing fields

        private bool _isGridEnabled;

        private string _containerPath;

        private string _archivePath;

        private string _outputPath;

        #endregion

        /// <summary>
        /// Controls inner grid.
        /// </summary>
        public bool IsGridEnabled
        {
            get => _isGridEnabled;
            set
            {
                _isGridEnabled = value;
                NotifyOfPropertyChange(() => IsGridEnabled);
            }
        }

        /// <summary>
        /// Container file's path.
        /// </summary>
        public string ContainerPath
        {
            get => _containerPath;
            set
            {
                _containerPath = value;
                NotifyOfPropertyChange(() => ContainerPath);
            }
        }

        /// <summary>
        /// Archive's path.
        /// </summary>
        public string ArchivePath
        {
            get => _archivePath;
            set
            {
                _archivePath = value;
                NotifyOfPropertyChange(() => ArchivePath);
            }
        }

        /// <summary>
        /// Output file's path.
        /// </summary>
        public string OutputPath
        {
            get => _outputPath;
            set
            {
                _outputPath = value;
                NotifyOfPropertyChange(() => OutputPath);
            }
        }

        #endregion

        #region Constructor

        public MainViewModel()
        {
            //Setting the theme.
            SetThemeModel.SetTheme(IsDarkTheme);
            Theme = IsDarkTheme ? Themes.Dark : Themes.Light;

            IsGridEnabled = true;
            ContainerPath = string.Empty;
            ArchivePath = string.Empty;
            OutputPath = string.Empty;
            TempOutputPath = string.Empty;
        }

        #endregion

        #region Methods

        #region Buttons

        /// <summary>
        /// Button for selecting container file through Windows file explorer.
        /// </summary>
        /// <returns></returns>
        public async ValueTask SelectContainerButtonAsync()
        {
            try
            {
                OpenFileDialogResult dialogResult = await OpenFileDialog
                                                         .ShowDialogAsync(Enums.MainViewModel.DialogHostId,
                                                                          new OpenFileDialogArguments()).ConfigureAwait(true);
                ContainerPath = dialogResult.Canceled ? ContainerPath : dialogResult.FileInfo.FullName;
            }
            catch (Exception exception) { await ErrorHelper.ShowExceptionAsync(exception).ConfigureAwait(true); }
        }

        /// <summary>
        /// Button for selecting archive through Windows file explorer.
        /// </summary>
        /// <returns></returns>
        public async ValueTask SelectArchiveButtonAsync()
        {
            try
            {
                OpenFileDialogResult dialogResult = await OpenFileDialog
                                                         .ShowDialogAsync(Enums.MainViewModel.DialogHostId,
                                                                          new OpenFileDialogArguments()).ConfigureAwait(true);
                ArchivePath = dialogResult.Canceled ? ArchivePath : dialogResult.FileInfo.FullName;
            }
            catch (Exception exception) { await ErrorHelper.ShowExceptionAsync(exception).ConfigureAwait(true); }
        }

        /// <summary>
        /// Button for selecting output file's path through Windows file explorer.
        /// </summary>
        /// <returns></returns>
        public async ValueTask OutputPathButtonAsync()
        {
            try
            {
                SaveFileDialogResult dialogResult = await SaveFileDialog
                                                         .ShowDialogAsync(Enums.MainViewModel.DialogHostId,
                                                                          new SaveFileDialogArguments
                                                                          {
                                                                              CreateNewDirectoryEnabled = true
                                                                          }).ConfigureAwait(true);
                OutputPath = dialogResult.Canceled ? OutputPath : dialogResult.FileInfo.FullName;
            }
            catch (Exception exception) { await ErrorHelper.ShowExceptionAsync(exception).ConfigureAwait(true); }
        }

        /// <summary>
        /// Button for starting the work.
        /// </summary>
        /// <returns></returns>
        public async ValueTask StartButtonAsync()
        {
            //Do some checks before running.
            if (!await StartWorkAsync().ConfigureAwait(true)) return;

            //Shows, if completed successfuly.
            bool isSuccessful = true;

            try
            {
                await MainModel.RarJpegAsync(ContainerPath, ArchivePath, TempOutputPath).ConfigureAwait(true);
            }
            catch (Exception exception)
            {
                await ErrorHelper.ShowExceptionAsync(exception).ConfigureAwait(true);
                isSuccessful = false;
            }

            //Do some stuff, like unblocking UI.
            await CancelWorkAsync(isSuccessful).ConfigureAwait(true);
        }

        #endregion

        #region Other

        /// <summary>
        /// Preform data checks before starting the work.
        /// <para>Also blocks the UI if everything is OK.</para>
        /// </summary>
        /// <returns><see langword="true"/> if no errors occured, <see langword="false"/> otherwise.</returns>
        private async ValueTask<bool> StartWorkAsync()
        {
            #region Cheсks

            try { await CheckDataAsync().ConfigureAwait(true); }
            catch (Exception exception)
            {
                //Exception with string.Empty is returned, when container file doesn't have extension.
                if (string.IsNullOrWhiteSpace(exception.Message)) return false;

                //Show other errors.
                await ErrorHelper.ShowExceptionAsync(exception).ConfigureAwait(true);

                return false;
            }

            #endregion

            //Block the UI while working.
            IsGridEnabled = false;

            return true;
        }

        /// <summary>
        /// Enable UI and return inner <see cref="TempOutputPath"/> to correct value.
        /// </summary>
        /// <param name="isSuccessful">Was the task successful?</param>
        /// <returns></returns>
        private async ValueTask CancelWorkAsync(bool isSuccessful)
        {
            IsGridEnabled = true;
            TempOutputPath = string.Empty;
            if (isSuccessful) await DialogHost.Show(new MessageBoxDialogViewModel(Strings.Done)).ConfigureAwait(true);
        }

        /// <summary>
        /// Check all paths and files before the work actually starts.
        /// </summary>
        /// <returns></returns>
        private async ValueTask CheckDataAsync()
        {
            #region Check container file

            //Check if container file's path is empty or container file doesn't exist.
            if (string.IsNullOrWhiteSpace(ContainerPath) || !File.Exists(ContainerPath))
                throw new Exception(Strings.ContainerExistEmpty);

            //Check if container file doesn't have extension.
            if (string.IsNullOrWhiteSpace(Path.GetExtension(ContainerPath)))
                //You can actually continue, if it doesn't have extension, just click "OK" on MessageBox.
                if (!(bool) await DialogHost.Show(new MessageBoxDialogViewModel(Strings.ContainerExtension, true))
                                            .ConfigureAwait(true))
                    throw new Exception(string.Empty);

            #endregion

            #region Check archive

            //Check if archive's path is empty or archive doesn't exist.
            if (string.IsNullOrWhiteSpace(ArchivePath) || !File.Exists(ArchivePath))
                throw new Exception(Strings.ArchiveExistEmpty);

            //Check if archive doesn't contain extension.
            if (string.IsNullOrWhiteSpace(Path.GetExtension(ArchivePath)))
                throw new Exception(Strings.ArchiveExtension);

            //Check if selected file is archive and is archive corrupted.
            using ZipFile zipFile = new ZipFile(ArchivePath);

            if (!zipFile.TestArchive(true)) throw new Exception(Strings.ArchiveCorrupted);

            #endregion

            #region Check output file

            //Check if output file's path isn't empty.
            if (string.IsNullOrWhiteSpace(OutputPath)) throw new Exception(Strings.OutputEmpty);

            //Check if output file's path contain extension.
            if (!string.IsNullOrWhiteSpace(Path.GetExtension(OutputPath))) throw new Exception(Strings.OutputExtension);

            //Change TempOutputPath's' value: adding needed extensions.
            TempOutputPath = $"{OutputPath}{Path.GetExtension(ArchivePath)}{Path.GetExtension(ContainerPath)}";

            //Check if output file already exists.
            if (File.Exists(TempOutputPath)) throw new Exception(Strings.OutputExist);

            #endregion
        }

        #endregion

        #endregion
    }
}
