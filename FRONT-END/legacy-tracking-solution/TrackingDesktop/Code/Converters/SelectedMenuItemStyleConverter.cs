using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TrackingDesktop.Views.UserControls;

namespace TrackingDesktop.Code.Converters
{
    public class SelectedMenuItemStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && parameter != null)
            {
                var content = value as UserControl;
                var senderName = parameter.ToString();

                if (senderName == "customers")
                {
                    var style = (content is Customers ? App.Current.FindResource("menu_item_customers_selected") as Style : App.Current.FindResource("menu_item_customers") as Style);
                    return style;
                }
                else if (senderName == "terminals")
                {
                    var style = (content is Terminals ? App.Current.FindResource("menu_item_devices_selected") as Style : App.Current.FindResource("menu_item_devices") as Style);
                    return style;
                }
                else if (senderName == "configterminal")
                {
                    var style = (content is ConfigTerminal ? App.Current.FindResource("menu_item_config_device_selected") as Style : App.Current.FindResource("menu_item_config_device") as Style);
                    return style;
                }
                else if (senderName == "testterminal")
                {
                    var style = (content is TestTerminal ? App.Current.FindResource("menu_item_test_device_selected") as Style : App.Current.FindResource("menu_item_test_device") as Style);
                    return style;
                }

            }

            return App.Current.FindResource("menu_item");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
