using Caliburn.Micro;
using ProjectName.Core;
using Microsoft.Win32;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Seasail.Extensions;

namespace ProjectName.ViewModels
{
    [Export(typeof(IShell))]
    internal class ShellViewModel : ViewModelBase, IShell
    {
        private BackgroundWorker _connectionWorker;
        public ShellViewModel()
        {
            EventAggregator.SubscribeOnUIThread(this);
        }
        public override string DisplayName => Translater.Trans(nameof(ShellViewModel));

        public Options Options => IoC.Get<Options>();

        private string _name;
        public string Name
        {
            get => _name;
            set=>Set(ref _name, value);
        }

        public void MouseDown(object obj)
        {
            try { Application.Current.MainWindow.DragMove(); } catch { }
        }




    }
}
