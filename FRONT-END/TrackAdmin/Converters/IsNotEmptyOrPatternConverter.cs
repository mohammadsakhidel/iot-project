using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TrackAdmin.Converters {
    public class IsNotEmptyOrPatternConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return false;

            if (parameter != null) {
                if (Regex.IsMatch(value.ToString(), parameter.ToString()))
                    return false;
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
