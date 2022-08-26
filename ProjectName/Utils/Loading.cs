using Caliburn.Micro;
using ProjectName.ViewModels.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectName.Utils
{
    public class Loading
    {
        private BackgroundWorker _backgroundWorker;
        private LoadingViewModel _loadingModel;
        private LoadingEventArgs _args;
        private System.Threading.CancellationTokenSource _cts;

        public Loading(string msg = "", string caption = "", bool canCancel = false, bool showProgress = false)
        {
            _cts = new System.Threading.CancellationTokenSource();
            _loadingModel = new LoadingViewModel(showProgress, canCancel);
            _loadingModel.CancelEvent += OnUserCancel;
            _loadingModel.Message = msg;
            if (!string.IsNullOrEmpty(caption))
                _loadingModel.DisplayName = caption;
            _args = new LoadingEventArgs();
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += _backgroundWorker_DoWork;
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.WorkerReportsProgress = showProgress;
            _backgroundWorker.RunWorkerCompleted += _backgroundWorker_RunWorkerCompleted;
            _backgroundWorker.ProgressChanged += _backgroundWorker_ProgressChanged;
        }

        private void OnUserCancel()
        {
            _args.IsCancelled = true;
            _backgroundWorker.CancelAsync();
            _cts.Cancel();
        }

        public delegate void ReportProgressDelegate(object obj);

        public Action<ReportProgressDelegate, System.Threading.CancellationTokenSource> BackgroundWork { get; set; }



        /// <summary>
        /// 后台任务执行完毕后事件
        /// </summary>
        public event EventHandler<LoadingEventArgs> BackgroundWorkerCompleted;

        public async void Start()
        {
            try
            {
                _backgroundWorker.RunWorkerAsync();
                await IoC.Get<IWindowManager>().ShowDialogAsync(_loadingModel);
            }
            catch (Exception ex)
            {
                _args.Result = ex;
            }
        }

        private void _backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _loadingModel.TryCloseAsync();
            BackgroundWorkerCompleted?.Invoke(null, _args);
        }

        private void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWork?.Invoke(ReportProgress,_cts);
            }
            catch (Exception ex)
            {
                _args.Result = ex;
            }
        }


        private void ReportProgress(object value)
        {
            if (value is string message)
                _loadingModel.Message = message;
            else if (value is int progress)
                _loadingModel.Progress = progress;
            else if (value is Exception ex)
            {
                _args.Result = ex;
                _backgroundWorker.CancelAsync();
            }
            else
            {
                _args.Result = value;
            }
        }
    }
}
