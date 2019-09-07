using System;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using RarJpeg.ViewModels;

namespace RarJpeg.Helpers
{
    /// <summary>
    /// That class helps to print exceptions and custom errors.
    /// </summary>
    internal static class ErrorHelper
    {
        /// <summary>
        /// Shows current exception.
        /// </summary>
        /// <param name="exception">Exception.</param>
        /// <returns></returns>
        public static async ValueTask ShowExceptionAsync(Exception exception)
        {
            await DialogHost.Show(new MessageBoxDialogViewModel(exception.Message)).ConfigureAwait(false);

            #if DEBUG
            await DialogHost.Show(new MessageBoxDialogViewModel(exception.InnerException?.Message))
                            .ConfigureAwait(false);
            #endif
        }

        /// <summary>
        /// Shows current error and exception, if it was thrown.
        /// </summary>
        /// <param name="errorMessage">Error message.</param>
        /// <param name="exception">Exception.</param>
        /// <returns><see langword="false"/>.</returns>
        // ReSharper disable once UnusedMember.Global
        public static async ValueTask<bool> ShowErrorAsync(string errorMessage, Exception exception)
        {
            await DialogHost.Show(new MessageBoxDialogViewModel(errorMessage)).ConfigureAwait(false);

            #if DEBUG
            await DialogHost.Show(new MessageBoxDialogViewModel(exception?.Message)).ConfigureAwait(false);
            await DialogHost.Show(new MessageBoxDialogViewModel(exception?.InnerException?.Message))
                            .ConfigureAwait(false);
            #endif

            return await new ValueTask<bool>(false).ConfigureAwait(false);
        }
    }
}
