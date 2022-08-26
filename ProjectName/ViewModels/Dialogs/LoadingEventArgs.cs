using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectName.ViewModels.Dialogs
{
    public class LoadingEventArgs : EventArgs
    {
        public object Result { get; set; }

        public bool IsCancelled { get; set; }
    }
}
