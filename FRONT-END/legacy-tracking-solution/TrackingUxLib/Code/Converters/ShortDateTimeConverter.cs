using RamancoLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TrackingUxLib.Code.Converters
{
    public class ShortDateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null)
                    return "";

                var date = (DateTime)value;
                var roji = DateTimeUtils.ToShamsi(date);
                return string.Format("{0:D4}/{1:D2}/{2:D2} {3:D2}:{4:D2}", roji.Year, roji.Month, roji.Day, date.Hour, date.Minute);
            }
            catch
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
