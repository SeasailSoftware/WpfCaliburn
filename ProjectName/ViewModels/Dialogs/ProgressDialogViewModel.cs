using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using Caliburn.Micro;

namespace ProjectName.ViewModels.Dialogs
{
    public class ProgressDialogController
    {
        private ProgressDialogViewModel _progress;
        private TaskCompletionSource<bool> _closeTaskCompletionSource;

        internal ProgressDialogController(ProgressDialogViewModel progressDialogViewModel)
        {
            _progress = progressDialogViewModel;
            _progress.Canceled += (sender, args) =>
                Canceled?.Invoke(this, EventArgs.Empty);
            _progress.Closed += (sender, args) =>
            {
                _closeTaskCompletionSource?.SetResult(true);
                Closed?.Invoke(this, EventArgs.Empty);
            };
        }

        //
        // 摘要:
        //     Gets if the Cancel button has been pressed.
        public bool IsCanceled
        {
            get => _progress.IsCanceled;
        }

        //
        // 摘要:
        //     Gets if the wrapped ProgressDialog is open.
        public bool IsOpen
        {
            get => _progress.IsOpen;
        }

        //
        // 摘要:
        //     Gets/Sets the minimum restriction of the progress Value property
        public double Minimum
        {
            get => _progress.Minimum;
            set => _progress.Minimum = value;
        }

        //
        // 摘要:
        //     Gets/Sets the maximum restriction of the progress Value property
        public double Maximum
        {
            get => _progress.Maximum;
            set => _progress.Maximum = value;
        }

        //
        // 摘要:
        //     This event is raised when the associated MahApps.Metro.Controls.Dialogs.ProgressDialog
        //     was closed programmatically.
        public event EventHandler Closed;

        //
        // 摘要:
        //     This event is raised when the associated MahApps.Metro.Controls.Dialogs.ProgressDialog
        //     was cancelled by the user.
        public event EventHandler Canceled;

        //
        // 摘要:
        //     Begins an operation to close the ProgressDialog.
        //
        // 返回结果:
        //     A task representing the operation.
        public Task CloseAsync()
        {
            _closeTaskCompletionSource = new TaskCompletionSource<bool>();
            App.Current.MainWindow.Dispatcher.BeginInvoke(new System.Action(async () => await _progress.TryCloseAsync(true)));
            return _closeTaskCompletionSource.Task;
        }

        //
        // 摘要:
        //     Sets if the Cancel button is visible.
        //
        // 参数:
        //   value:
        public void SetCancelable(bool value)
        {
            _progress.IsCancelable = value;
        }

        //
        // 摘要:
        //     Sets the ProgressBar's IsIndeterminate to true. To set it to false, call SetProgress.
        public void SetIndeterminate()
        {
            _progress.IsIndeterminate = true;
        }

        //
        // 摘要:
        //     Sets the dialog's message content.
        //
        // 参数:
        //   message:
        //     The message to be set.
        public void SetMessage(string message)
        {
            _progress.Message = message;
        }

        //
        // 摘要:
        //     Sets the dialog's progress bar value and sets IsIndeterminate to false.
        //
        // 参数:
        //   value:
        //     The percentage to set as the value.
        public void SetProgress(double value)
        {
            _progress.IsIndeterminate = false;
            _progress.Value = value;
        }

        //
        // 摘要:
        //     Sets the dialog's progress bar brush
        //
        // 参数:
        //   brush:
        //     The brush to use for the progress bar's foreground
        public void SetProgressBarForegroundBrush(Brush brush)
        {
            _progress.ProgressBarForegroundBrush = brush;
        }

        //
        // 摘要:
        //     Sets the dialog's title.
        //
        // 参数:
        //   title:
        //     The title to be set.
        public void SetTitle(string title)
        {
            _progress.DisplayName = title;
        }
    }

    /// <summary>
    /// 进度条
    /// </summary>
    class ProgressDialogViewModel : DialogViewModelBase
    {
        private string _message;
        private double _minimum = 0;
        private double _maximum = 100;
        private bool _isCancelable;
        private bool _isIndeterminate = true;
        private double _value = 0;
        private string _cancelButtonText;
        private Brush _progressBarForegroundBrush = Brushes.LimeGreen;

        public static Task<ProgressDialogController> ShowProgressAsync(string caption, string message,
            bool isCancelable = false, string cancelButtonText = null)
        {
            var completionSource = new TaskCompletionSource<ProgressDialogController>();

            App.Current.MainWindow.Dispatcher.BeginInvoke(new System.Action(async () =>
            {
                var progress = new ProgressDialogViewModel
                {
                    DisplayName = caption,
                    Message = message,
                    IsCancelable = isCancelable,
                    CancelButtonText = cancelButtonText ?? Translater.Trans("s_Cancel")
                };

                var controller = new ProgressDialogController(progress);
                completionSource.SetResult(controller);
                if (!(await IoC.Get<IWindowManager>().ShowDialogAsync(progress)).GetValueOrDefault())
                {
                    progress.IsCanceled = true;
                    progress.Canceled?.Invoke(progress, EventArgs.Empty);
                }
                else
                {
                    progress.Closed?.Invoke(progress, EventArgs.Empty);
                }
            }));

            return completionSource.Task;
        }

        #region Properties

        //
        // 摘要:
        //     This event is raised when the associated MahApps.Metro.Controls.Dialogs.ProgressDialog
        //     was closed programmatically.
        public event EventHandler Closed;

        //
        // 摘要:
        //     This event is raised when the associated MahApps.Metro.Controls.Dialogs.ProgressDialog
        //     was cancelled by the user.
        public event EventHandler Canceled;

        /// <summary>
        /// 进度条的消息
        /// </summary>
        public string Message
        {
            get => _message;
            set
            {
                if (value == _message) return;
                _message = value;
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// 进度条最小值
        /// </summary>
        public double Minimum
        {
            get => _minimum;
            set
            {
                if (value.Equals(_minimum)) return;
                _minimum = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange();
            }
        }

        /// <summary>
        /// 进度条最大值
        /// </summary>
        public double Maximum
        {
            get => _maximum;
            set
            {
                if (value.Equals(_maximum)) return;
                _maximum = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange();
            }
        }

        public bool IsCancelable
        {
            get => _isCancelable;
            set
            {
                if (value == _isCancelable) return;
                _isCancelable = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsIndeterminate
        {
            get => _isIndeterminate;
            set
            {
                if (value == _isIndeterminate) return;
                _isIndeterminate = value;
                NotifyOfPropertyChange();
            }
        }

        public double Value
        {
            get => _value;
            set
            {
                if (value.Equals(_value)) return;
                _value = value;
                NotifyOfPropertyChange();
            }
        }

        public string CancelButtonText
        {
            get => _cancelButtonText;
            set
            {
                if (value == _cancelButtonText) return;
                _cancelButtonText = value;
                NotifyOfPropertyChange();
            }
        }

        public Brush ProgressBarForegroundBrush
        {
            get => _progressBarForegroundBrush;
            set
            {
                if (Equals(value, _progressBarForegroundBrush)) return;
                _progressBarForegroundBrush = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsOpen { get; private set; }

        public bool IsCanceled { get; private set; }

        #endregion

        #region Overrides

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            IsOpen = true;
            return base.OnActivateAsync(cancellationToken);
        }


        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            if (close)
            {
                IsOpen = false;
            }
            return base.OnDeactivateAsync(close, cancellationToken);
        }

        #endregion
    }
}