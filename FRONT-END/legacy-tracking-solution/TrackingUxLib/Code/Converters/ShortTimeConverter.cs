using TrackingUxLib.Code.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TrackingUxLib.Code.Converters
{
    public class ShortTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    var time = (DateTime)value;
                    return $"{time.Hour.ToString("D2")}:{time.Minute.ToString("D2")}";
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex);
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
