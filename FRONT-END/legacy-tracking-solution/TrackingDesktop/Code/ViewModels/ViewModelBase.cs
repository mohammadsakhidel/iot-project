using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingDesktop.Code.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        #region Token:
        private string token;
        public string Token
        {
            get
            {
                return token;
            }
            set
            {
                token = value;
                RaisePropertyChanged("Token");
            }
        }
        #endregion
    }
}
