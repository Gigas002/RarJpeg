using System;
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
    /// <summary>
    /// ViewModel for <see cref="Views.MainView"/>.
    /// </summary>
    internal class MainViewModel : PropertyChangedBase
    {
        //todo rename ready to output
        //todo material dialogs
        //todo add tests
        //todo take a look at project settings
        //todo take a look at publish

        #region Properties

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
        public string ReadyHint { get; } = Strings.ReadyHint;

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

        #region Backing fields

        private bool _isGridEnabled;

        private string _containerPath;

        private string _archivePath;

        private string _readyPath;

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

        /// <summary>
        /// Button for selecting container file through Windows file explorer.
        /// </summary>
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

        /// <summary>
        /// Button for selecting archive through Windows file explorer.
        /// </summary>
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

        /// <summary>
        /// Button for selecting output file's path through Windows file explorer.
        /// </summary>
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

        /// <summary>
        /// Button for starting the work.
        /// </summary>
        public async ValueTask StartButton()
        {
            //Do some checks before running.
            if (!await StartWork()) return;

            //Shows, if completed successfuly.
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

            //Do some stuff, like unblocking UI.
            await CancelWork(isSuccessful);
        }

        #endregion

        #region Other

        /// <summary>
        /// Preform data checks before starting the work.
        /// <para>Also blocks the UI if everything is OK.</para>
        /// </summary>
        /// <returns><see langword="true"/> if no errors occured, <see langword="false"/> otherwise.</returns>
        private async ValueTask<bool> StartWork()
        {
            #region Cheсks

            try
            {
                await CheckData();
            }
            catch (Exception exception)
            {
                //Exception with string.Empty is returned, when container file doesn't have extension.
                if (string.IsNullOrWhiteSpace(exception.Message)) return false;

                //Show other errors.
                await DialogHost.Show(new MessageBoxDialogViewModel(exception.Message));
                return false;
            }

            #endregion

            //Block the UI while working.
            IsGridEnabled = false;

            return true;
        }

        /// <summary>
        /// Enable UI and return inner <see cref="ReadyPath"/> to correct value.
        /// </summary>
        /// <param name="isSuccessful">Was the task successful?</param>
        private async ValueTask CancelWork(bool isSuccessful)
        {
            IsGridEnabled = true;
            _readyPath = ReadyPath;
            if (isSuccessful) await DialogHost.Show(new MessageBoxDialogViewModel(Strings.Done));
        }

        /// <summary>
        /// Check all paths and files before the work actually starts.
        /// </summary>
        /// <returns></returns>
        private async ValueTask CheckData()
        {
            #region Check container file

            //Check if container file's path is empty or container file doesn't exist.
            if (string.IsNullOrWhiteSpace(ContainerPath) || !File.Exists(ContainerPath))
                throw new Exception(Strings.ContainerExistEmpty);

            //Check if container file doesn't have extension.
            if (string.IsNullOrWhiteSpace(Path.GetExtension(ContainerPath)))
            {
                //You can actually continue, if it doesn't have extension, just tap "OK" on MessageBox.
                if (!(bool)
                        await DialogHost.Show(new MessageBoxDialogViewModel(Strings.ContainerExtension,
                                                                            true)))
                    throw new Exception(string.Empty);
            }

            #endregion

            #region Check archive

            //Check if archive's path is empty or archive doesn't exist.
            if (string.IsNullOrWhiteSpace(ArchivePath) || !File.Exists(ArchivePath))
                throw new Exception(Strings.ArchiveExistEmpty);

            //Check if archive doesn't contain extension.
            if (string.IsNullOrWhiteSpace(Path.GetExtension(ArchivePath)))
                throw new Exception(Strings.ArchiveExtension);

            //Check if selected file is archive and is archive corrupted.
            if (!new ZipFile(ArchivePath).TestArchive(true))
                throw new Exception(Strings.ArchiveCorrupted);

            #endregion

            #region Check ready file

            //Check if output file's path isn't empty.
            if (string.IsNullOrWhiteSpace(ReadyPath))
                throw new Exception(Strings.ReadyEmpty);

            //Check if output file's path contain extension.
            if (!string.IsNullOrWhiteSpace(Path.GetExtension(ReadyPath)))
                throw new Exception(Strings.ReadyExtension);

            //Change inner ReadyPath's value: adding needed extensions.
            _readyPath = $"{ReadyPath}{Path.GetExtension(ArchivePath)}" +
                             $"{Path.GetExtension(ContainerPath)}";

            //Check if output file already exists.
            if (File.Exists(_readyPath))
                throw new Exception(Strings.ReadyExist);

            #endregion
        }

        #endregion

        #endregion
    }
}
