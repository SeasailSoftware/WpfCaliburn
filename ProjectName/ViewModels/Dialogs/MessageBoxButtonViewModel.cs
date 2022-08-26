using System.Windows.Input;
using Caliburn.Micro;
using ProjectName.Core;

namespace ProjectName.ViewModels.Dialogs
{
    public class MessageBoxButtonViewModel : IHaveDisplayName
    {
        public MessageBoxButtonViewModel(
            MessageBoxViewModel owner,
            string displayName,
            object result,
            bool isDefault,
            bool isCancel)
        {
            Owner = owner;
            Result = result;
            DisplayName = displayName;
            IsDefault = isDefault;
            IsCancel = isCancel;
            Command = new RelayCommand( async o =>
            {
                Owner.Result = result;
               await  Owner.TryCloseAsync(!IsCancel);
            });
        }

        internal MessageBoxViewModel Owner { get; set; }

        public object Result { get; set; }

        public ICommand Command { get; private set; }

        public bool IsDefault { get; set; }

        public bool IsCancel { get; set; }

        #region Implementation of IHaveDisplayName

        public string DisplayName { get; set; }

        #endregion
    }
}