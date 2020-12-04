using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using TrackAdmin.Constants;

namespace TrackAdmin.Converters {
    public class DateConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            try {

                if (value == null)
                    return string.Empty;

                var date = System.Convert.ToDateTime(value);
                return date.ToString(Values.DATE_FORMAT);

            } catch {
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            try {

                if (value == null || string.IsNullOrEmpty(value.ToString()))
                    return null;

                var dateStr = value.ToString();
                var parsed = DateTime.TryParse(dateStr, out var date);

                return parsed ? date : null;

            } catch {
                return null;
            }
        }
    }
}
