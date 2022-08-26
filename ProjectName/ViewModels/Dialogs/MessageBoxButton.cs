namespace ProjectName.ViewModels.Dialogs
{
    public class MessageBoxButton<TResult>
    {
        public string DisplayName { get; set; }

        public TResult Result { get; set; }

        public bool IsDefault { get; set; }

        public bool IsCancel { get; set; }

        public MessageBoxButton(string displayName,
            TResult result,
            bool isDefault = false,
            bool isCancel = false)
        {
            this.DisplayName = displayName;
            this.Result = result;
            this.IsDefault = isDefault;
            this.IsCancel = isCancel;
        }
    }
}