using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TrackingDesktop.Views.UserControls;

namespace TrackingDesktop.Code.ViewModels
{
    public class MainWindowVM : ViewModelBase
    {
        #region Ctors:
        public MainWindowVM()
        {
            Content = new DefaultPage();
        }
        #endregion

        #region Content:
        private UserControl content = null;
        public UserControl Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
                RaisePropertyChanged("Content");
            }
        }
        #endregion
    }
}
