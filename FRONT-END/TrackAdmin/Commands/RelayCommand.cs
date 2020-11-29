using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TrackAdmin.Commands {
    public class RelayCommand : ICommand {

        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public RelayCommand(Action execute) : this(execute, null) {
        }

        public RelayCommand(Action execute, Func<bool> canExecute) {
            _canExecute = canExecute;
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        #region Methods:
        public bool CanExecute(object parameter) {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter) {
            _execute();
        }

        public void RaiseCanExecuteChanged() {
            CanExecuteChangedInternal?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Events:
        public event EventHandler CanExecuteChanged {
            add {
                CommandManager.RequerySuggested += value;
                CanExecuteChangedInternal += value;
            }
            remove {
                CommandManager.RequerySuggested -= value;
                CanExecuteChangedInternal -= value;
            }
        }
        private event EventHandler CanExecuteChangedInternal;
        #endregion
    }
}
