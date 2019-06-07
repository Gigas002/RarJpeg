using System.Windows;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;

namespace RarJpeg.ViewModels
{
    public class MessageBoxDialogViewModel : PropertyChangedBase
    {
        #region Properties and fields

        #region UI

        

        #endregion


        private string _message;

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                NotifyOfPropertyChange(() => Message);
            }
        }

        private Visibility _cancelButtonVisibility = Visibility.Collapsed;

        public Visibility CancelButtonVisibility
        {
            get => _cancelButtonVisibility;
            set
            {
                _cancelButtonVisibility = value;
                NotifyOfPropertyChange(() => CancelButtonVisibility);
            }
        }

        #endregion

        #region Constructors

        public MessageBoxDialogViewModel(string message) => Message = message;

        public MessageBoxDialogViewModel(string message, Visibility cancelButtonVisibility)
        {
            Message = message;
            CancelButtonVisibility = cancelButtonVisibility;
        }

        #endregion

        #region Buttons methods

        /// <summary>
        /// Method for CancelButton on View. Closes the UserControl and returns false.
        /// </summary>
        public void CancelButton() => DialogHost.CloseDialogCommand.Execute(false, null);

        /// <summary>
        /// Method for AcceptButton on View. Closes the UserControl and returns true.
        /// </summary>
        public void AcceptButton() => DialogHost.CloseDialogCommand.Execute(true, null);

        /// <summary>
        /// Method for CopyToClipboard button. Copies the message.
        /// </summary>
        public void CopyToClipboardButton() => Clipboard.SetText(Message);

        #endregion
    }
}
