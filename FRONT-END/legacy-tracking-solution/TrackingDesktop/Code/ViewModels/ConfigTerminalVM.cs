using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingDesktop.Code.ViewModels
{
    public class ConfigTerminalVM : ViewModelBase
    {
        private bool connected = false;
        public bool Connected
        {
            get
            {
                return connected;
            }
            set
            {
                connected = value;
                RaisePropertyChanged("Connected");
            }
        }
    }
}
