using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using ProjectName.Core;
using Screen = Caliburn.Micro.Screen;

namespace ProjectName.ViewModels.Dialogs
{
    public class MessageBoxViewModel : DialogViewModelBase
    {
        private bool _isHelpTextVisible;

        public MessageBoxViewModel()
        {
            HelpCommand = new RelayCommand(o => IsHelpTextVisible = !IsHelpTextVisible);
        }

        public object Result { get; set; }

        public ICollection<MessageBoxButtonViewModel> ButtonItems { get;  set; }

        public bool IsHelpVisible => !string.IsNullOrEmpty(HelpText);

        public string HelpText { get; set; }

        public ICommand HelpCommand { get; private set; }

        public bool IsHelpTextVisible
        {
            get => _isHelpTextVisible;
            private set
            {
                if (value == _isHelpTextVisible) return;
                _isHelpTextVisible = value;
                NotifyOfPropertyChange();
            }
        }

        public MessageBoxImage Icon { get;  set; }

        public string Message { get;  set; }


        /// <summary>
        /// 异步显示一个对话框
        /// </summary>
        /// <typeparam name="TResult">结果类型</typeparam>
        /// <param name="caption">标题</param>
        /// <param name="message">消息</param>
        /// <param name="icon">图标</param>
        /// <param name="helpText">帮助</param>
        /// <param name="defaultResult">默认按钮</param>
        /// <param name="cancelResult">消息按钮或关闭消息框的的结果</param>
        /// <param name="buttons">所有按钮</param>
        /// <returns></returns>
        public static Task<TResult> ShowMessageBoxAsync<TResult>(
            string caption,
            string message,
            MessageBoxImage icon,
            string helpText,
            params MessageBoxButton<TResult>[] buttons)
        {
            var cancelResult = (buttons.FirstOrDefault(x => x.IsCancel) ?? buttons.Last()).Result;
            var vm = new MessageBoxViewModel
            {
                DisplayName = caption,
                Message = message,
                HelpText = helpText,
                Icon = icon,
            };
            vm.ButtonItems = new List<MessageBoxButtonViewModel>(buttons.Select(x =>
                new MessageBoxButtonViewModel(vm, x.DisplayName, x.Result, x.IsDefault, x.IsCancel)));
            // 如果只有一个按钮将其设为默认按钮
            if (vm.ButtonItems.Count(x => x.IsDefault) == 0 && vm.ButtonItems.Count == 1)
            {
                vm.ButtonItems.First().IsDefault = true;
            }

            var completionSource = new TaskCompletionSource<TResult>();
            App.Current.MainWindow.Dispatcher.BeginInvoke(new System.Action(async () =>
            {
                var windowManager = IoC.Get<IWindowManager>();
                var result = await windowManager.ShowDialogAsync(vm);
                completionSource.SetResult((result == true) && (vm.Result != null) ? (TResult)vm.Result : cancelResult);
            }));

            return completionSource.Task;
        }

        public static Task<MessageBoxResult> AskUserYesNoAsync(string caption, string message,
            MessageBoxResult defaultResult = MessageBoxResult.Yes,
            string helpText = null)
        {
            return ShowMessageBoxAsync(caption, message, MessageBoxButton.YesNo, MessageBoxImage.Question,
                defaultResult, helpText);
        }

        public static Task<MessageBoxResult> AskUserOkCancelAsync(string caption, string message,
            MessageBoxResult defaultResult = MessageBoxResult.OK,
            string helpText = null)
        {
            return ShowMessageBoxAsync(caption, message, MessageBoxButton.OKCancel, MessageBoxImage.Question,
                defaultResult, helpText);
        }

        public static Task ShowErrorAsync(string caption, string message, string helpText = null)
        {
            return ShowMessageBoxAsync(caption, message, MessageBoxButton.OK, MessageBoxImage.Error, helpText);
        }

        public static Task ShowErrorAsync(string caption, string message, Exception ex)
        {
            return ShowErrorAsync(caption, message, ex.StackTrace);
        }

        public static Task ShowInfoAsync(string caption, string message, string helpText = null)
        {
            return ShowMessageBoxAsync(caption, message, MessageBoxButton.OK, MessageBoxImage.Information, helpText);
        }

        public static Task ShowWarningAsync(string caption, string message, string helpText = null)
        {
            return ShowMessageBoxAsync(caption, message, MessageBoxButton.OK, MessageBoxImage.Warning, helpText);
        }

        public static Task ShowExceptionInfoDialogAsync(string caption, Exception ex)
        {
            return ShowErrorAsync(caption, ex.Message, ex);
        }

        // 显示确定、跳过和取消按钮，当用户取消时返回MessageBoxResult.No
        public static Task<MessageBoxResult> AskOkSkipCancel(string caption, string message,
            MessageBoxResult defaultResult = MessageBoxResult.OK, string helpText = null)
        {
            return ShowMessageBoxAsync<MessageBoxResult>(
                caption,
                message,
                MessageBoxImage.Question,
                helpText,
                new MessageBoxButton<MessageBoxResult>(Translater.Trans("s_Ok"), MessageBoxResult.OK,
                    defaultResult == MessageBoxResult.OK),
                new MessageBoxButton<MessageBoxResult>(Translater.Trans("s_Skip"), MessageBoxResult.No,
                    defaultResult == MessageBoxResult.No),
                new MessageBoxButton<MessageBoxResult>(Translater.Trans("s_Cancel"), MessageBoxResult.Cancel,
                    defaultResult == MessageBoxResult.Cancel, true)
            );
        }

        public static Task<MessageBoxResult> ShowMessageBoxAsync(
            string caption,
            string message,
            MessageBoxButton button = MessageBoxButton.OK,
            MessageBoxImage image = MessageBoxImage.Information,
            string helpText = null)
        {
            return ShowMessageBoxAsync(caption, message, button, image, MessageBoxResult.None, helpText);
        }

        public static Task<MessageBoxResult> ShowMessageBoxAsync(
            string caption,
            string message,
            MessageBoxButton button,
            MessageBoxImage image,
            MessageBoxResult defaultResult,
            string helpText = null)
        {
            MessageBoxButton<MessageBoxResult>[] buttons = null;
            switch (button)
            {
                case MessageBoxButton.OK:
                    buttons = new[]
                    {
                        new MessageBoxButton<MessageBoxResult>(Translater.Trans("s_Ok"), MessageBoxResult.OK),
                    };
                    break;
                case MessageBoxButton.OKCancel:
                    buttons = new[]
                    {
                        new MessageBoxButton<MessageBoxResult>(Translater.Trans("s_Ok"), MessageBoxResult.OK),
                        new MessageBoxButton<MessageBoxResult>(Translater.Trans("s_Cancel"), MessageBoxResult.Cancel,
                            isCancel: true),
                    };
                    break;
                case MessageBoxButton.YesNoCancel:
                    buttons = new[]
                    {
                        new MessageBoxButton<MessageBoxResult>(Translater.Trans("s_Yes"), MessageBoxResult.Yes),
                        new MessageBoxButton<MessageBoxResult>(Translater.Trans("s_No"), MessageBoxResult.No),
                        new MessageBoxButton<MessageBoxResult>(Translater.Trans("s_Cancel"), MessageBoxResult.Cancel,
                            isCancel: true),
                    };
                    break;
                case MessageBoxButton.YesNo:
                    buttons = new[]
                    {
                        new MessageBoxButton<MessageBoxResult>(Translater.Trans("s_Yes"), MessageBoxResult.Yes),
                        new MessageBoxButton<MessageBoxResult>(Translater.Trans("s_No"), MessageBoxResult.No),
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(button), button, null);
            }


            var defaultBtn = buttons.FirstOrDefault(x => x.Result == defaultResult);
            if (defaultBtn != null)
                defaultBtn.IsDefault = true;

            return ShowMessageBoxAsync<MessageBoxResult>(caption, message, image, helpText, buttons);
        }
    }
}