using Caliburn.Micro;
using ProjectName.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seasail.i18N;
using Action = System.Action;

namespace ProjectName.ViewModels.Dialogs
{
    class LoadingViewModel : Screen
    {
        private ITranslater Translater => IoC.Get<ITranslater>();
        public LoadingViewModel(bool showProgress = false, bool canCancel = false)
        {
            CanCancelExcuted = canCancel;
            IsShowingProgress = showProgress;
        }
        public override string DisplayName => Translater.Trans(nameof(LoadingViewModel));

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

        private bool _isShowingProgress;
        public bool IsShowingProgress
        {
            get => _isShowingProgress;
            set
            {
                _isShowingProgress = value;
                NotifyOfPropertyChange(() => IsShowingProgress);
            }
        }

        private int _progress;
        public int Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                NotifyOfPropertyChange(() => Progress);
            }
        }

        public event Action CancelEvent;


        public bool CanCancelExcuted { get; set; }

        public RelayCommand CancelCommand => new RelayCommand(x => Cancel(), y => CanCancelExcuted);

        private void Cancel()
        {
            CancelEvent?.Invoke();
        }
    }
}
