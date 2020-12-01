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

        #region ------------------------ COMMON STATE -------------------------
        private bool isLoading;
        public bool IsLoading {
            get {
                return isLoading;
            }
            set {
                isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        private string error;
        public string Error {
            get { return error; }
            set {
                error = value;
                OnPropertyChanged(nameof(Error));
            }
        }

        private string message;
        public string Message {
            get { return message; }
            set {
                message = value;
                OnPropertyChanged(nameof(Message));
            }
        }
        #endregion

        #region Methods:
        protected async Task ShowError(string error, int seconds = 5) {
            Error = error;
            await Task.Delay(seconds * 1000);
            Error = "";
        }
        protected async Task ShowMessage(string message, int seconds = 5) {
            Message = message;
            await Task.Delay(seconds * 1000);
            Message = "";
        }
        #endregion

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
