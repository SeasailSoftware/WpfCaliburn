using System;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using ProjectName;
using ProjectName.ViewModels.Dialogs;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Seasail.i18N;
using ProgressDialogController = ProjectName.ViewModels.Dialogs.ProgressDialogController;

namespace ProjectName.Utils
{
    public class DialogHelper
    {
        private static ITranslater Translater = IoC.Get<ITranslater>();
        private static MetroWindow MainWindow => App.Current.MainWindow as MetroWindow;
        public static MetroDialogSettings OkCancel { get; }
            = new MetroDialogSettings
            {
                AffirmativeButtonText = Translater.Trans("s_Ok"),
                NegativeButtonText = Translater.Trans("s_Cancel"),
                DefaultButtonFocus = MessageDialogResult.Affirmative
            };

        public static MetroDialogSettings YesNo { get; }
            = new MetroDialogSettings
            {
                AffirmativeButtonText = Translater.Trans("s_Yes"),
                NegativeButtonText = Translater.Trans("s_No"),
                DefaultButtonFocus = MessageDialogResult.Affirmative
            };

        public static MetroDialogSettings Ok { get; }
            = new MetroDialogSettings { AffirmativeButtonText = Translater.Trans("s_Ok") };


        public static Task<MessageBoxResult> AskUserYesNoAsync(string caption, string message,
            MessageBoxResult defaultResult = MessageBoxResult.Yes,
            string helpText = null)
        {
            return MessageBoxViewModel.AskUserYesNoAsync(caption, message, defaultResult, helpText);
        }

        public static Task<MessageBoxResult> AskUserOkCancelAsync(string caption, string message,
            MessageBoxResult defaultResult = MessageBoxResult.OK,
            string helpText = null)
        {
            return MessageBoxViewModel.AskUserOkCancelAsync(caption, message, defaultResult, helpText);
        }


        // 显示确定、跳过和取消按钮，当用户取消时返回MessageBoxResult.No
        public static Task<MessageBoxResult> AskOkSkipCancel(string caption, string message,
            MessageBoxResult defaultResult = MessageBoxResult.OK, string helpText = null)
        {
            return MessageBoxViewModel.AskOkSkipCancel(caption, message, defaultResult, helpText);
        }

        public static Task ShowErrorAsync(string caption, string message, string helpText = null)
        {
            return ShowMessageBoxAsync(caption, message, MessageBoxButton.OK, MessageBoxImage.Error, helpText);
        }

        public static Task ShowErrorAsync(string caption, string message, Exception ex)
        {
            return ShowErrorAsync(caption, message, ex?.ToString());
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


        public static Task<MessageBoxResult> ShowMessageBoxAsync(
            string caption,
            string message,
            MessageBoxButton button = MessageBoxButton.OK,
            MessageBoxImage image = MessageBoxImage.Information,
            string helpText = null)
        {
            return MessageBoxViewModel.ShowMessageBoxAsync(caption, message, button, image, helpText);
        }

        public static Task<MessageBoxResult> ShowMessageBoxAsync(
            string caption,
            string message,
            MessageBoxButton button,
            MessageBoxImage image,
            MessageBoxResult defaultResult,
            string helpText = null)
        {
            return MessageBoxViewModel.ShowMessageBoxAsync(caption, message, button, image, defaultResult, helpText);
        }

        public static Task<ProgressDialogController> ShowProgressAsync(string title, string message, bool isCancelable = false,
            string cancelButtonText = null)
        {
            return ProgressDialogViewModel.ShowProgressAsync(title, message, isCancelable, cancelButtonText);
        }

        public static Task<bool?> ShowDialogAsync(object viewModel)
        {
            return IoC.Get<IWindowManager>().ShowDialogAsync(viewModel);
        }
    }
}
