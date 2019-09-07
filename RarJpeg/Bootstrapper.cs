using System.Windows;
using Caliburn.Micro;
using RarJpeg.ViewModels;

namespace RarJpeg
{
    internal sealed class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper() => Initialize();

        protected override void OnStartup(object sender, StartupEventArgs startupEventArgs) => DisplayRootViewFor<MainViewModel>();
    }
}
