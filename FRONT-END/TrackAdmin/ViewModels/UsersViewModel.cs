using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TrackAdmin.Commands;

namespace TrackAdmin.ViewModels {
    public class UsersViewModel : BaseViewModel, IPageViewModel {

        // ----------------------- IsLoading
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

        // ---------------------- TestCommand:
        private ICommand testCommand;
        public ICommand TestCommand {
            get {
                return testCommand ?? new RelayCommand(async p => {
                    IsLoading = true;
                    await Task.Delay(3000);
                    IsLoading = false;
                });
            }
        }

    }
}
