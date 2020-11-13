using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TrackingDesktop.Code.ViewModels;
using TrackingUxLib.Code.Utils;

namespace TrackingDesktop.Views.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var mi = e.Source as Button;
                var vm = DataContext as MainWindowVM;
                var ty = typeof(App).Assembly.GetType($"TrackingDesktop.Views.UserControls.{mi.Tag.ToString()}");
                var uc = (UserControl)Activator.CreateInstance(ty);
                vm.Content = uc;
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex);
            }
        }
    }
}
