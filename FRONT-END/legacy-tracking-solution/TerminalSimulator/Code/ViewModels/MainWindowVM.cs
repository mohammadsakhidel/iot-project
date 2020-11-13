using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalSimulator.Code.ViewModels
{
    public class MainWindowVM : ViewModelBase
    {
        private bool isConnected = false;
        public bool IsConnected
        {
            get
            {
                return isConnected;
            }
            set
            {
                isConnected = value;
                RaisePropertyChanged("IsConnected");
            }
        }
    }
}
