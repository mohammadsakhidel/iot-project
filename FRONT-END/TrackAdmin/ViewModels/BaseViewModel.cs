using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackAdmin.ViewModels {
    public class BaseViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }


        // test:
        private string randomId = DateTime.Now.ToString();
        public string RandomId {
            get {
                return randomId;
            }
            set {
                randomId = value;
                OnPropertyChanged(nameof(RandomId));
            }
        }

    }
}
