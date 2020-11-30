using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TrackAdmin.Commands {
    public class RelayCommand : ICommand {

        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute) : this(execute, null) {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute) {
            _canExecute = canExecute;
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        #region Methods:
        public bool CanExecute(object parameter) {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter) {
            _execute(parameter);
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
