using Caliburn.Micro;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;
using Seasail.Extensions;
using Seasail.i18N;

namespace ProjectName.Converters
{
    class StringTranslateConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            if(value is Enum temp)
                return IoC.Get<ITranslater>().Trans(temp.Description());
            if(value is string key)
            {
                // 如果是密度数据项的名称
                var match = Regex.Match(key, @"^(Spectral\w+_At)_(\d+)$");
                if (match.Success)
                {
                    return IoC.Get<ITranslater>().Trans(match.Groups[1].Value + "$0$", match.Groups[2].Value);
                }
            }
            return IoC.Get<ITranslater>().Trans(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
