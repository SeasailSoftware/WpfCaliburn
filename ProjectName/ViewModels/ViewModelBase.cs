using Caliburn.Micro;
using Seasail.i18N;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ProjectName.ViewModels
{
    internal class ViewModelBase : INotifyPropertyChanged
    {
        //protected  bool  Set<T>(ref T field, T newValue = default(T), [CallerMemberName] string propertyName = null)
        //{
        //    if (EqualityComparer<T>.Default.Equals(field, newValue))
        //    {
        //        return false;
        //    }
        //    field = newValue;
        //    NotifyOfPropertyChange(propertyName);
        //    return true;
        //}
        public ITranslater Translater => IoC.Get<ITranslater>();
        public IWindowManager WindowManager => IoC.Get<IWindowManager>();

        public IEventAggregator EventAggregator => IoC.Get<IEventAggregator>();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public virtual void NotifyOfPropertyChange<TProperty>(Expression<Func<TProperty>> property)
        {
            OnPropertyChanged(property.GetMemberInfo().Name);
        }
    }
}
