using System.Windows;
using Caliburn.Micro;
using RarJpeg.ViewModels;

namespace RarJpeg
{
    internal class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper() => Initialize();

        protected override void OnStartup(object sender, StartupEventArgs e) => DisplayRootViewFor<MainViewModel>();
    }
}
