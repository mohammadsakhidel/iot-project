using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackAdmin.ViewModels {
    public class BaseViewModel : INotifyPropertyChanged, INotifyDataErrorInfo {

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

        #region INotifyPropertyChanged Implementation:
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region INotifyDataErrorInfo Implementation:
        public bool HasErrors => _validationErrors.Any();

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName) {
            var errors = _validationErrors.ContainsKey(propertyName) ? _validationErrors[propertyName] : null;
            return errors;
        }
        #endregion

        #region Fields:
        private readonly Dictionary<string, List<string>> _validationErrors = new Dictionary<string, List<string>>();
        #endregion

        #region Methods:
        protected void OnPropertyChanged(string propName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        private void OnValidationErrorsChanged(string propName) {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propName));
        }

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

        protected void AddValidationError(string propName, string error) {
            if (!_validationErrors.ContainsKey(propName))
                _validationErrors[propName] = new List<string>();

            if (!_validationErrors[propName].Contains(error)) {
                _validationErrors[propName].Add(error);
                OnValidationErrorsChanged(propName);
                OnPropertyChanged(nameof(HasErrors)); // This is for binding Save button enability
            }
        }

        protected void RemoveValidationErrors(string propName) {
            if (_validationErrors.ContainsKey(propName)) {
                _validationErrors.Remove(propName);
                OnValidationErrorsChanged(propName);
                OnPropertyChanged(nameof(HasErrors)); // This is for binding Save button enability
            }
        }
        #endregion

    }
}
