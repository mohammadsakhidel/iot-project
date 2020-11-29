using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TrackAdmin.Commands;
using TrackAdmin.Shared;

namespace TrackAdmin.ViewModels {
    public class MainWindowViewModel : BaseViewModel {

        public MainWindowViewModel() {

        }

        IPageViewModel _currentPageViewModel;
        public IPageViewModel CurrentPageViewModel {
            get {
                return _currentPageViewModel;
            }
            set {
                _currentPageViewModel = value;
                OnPropertyChanged(nameof(CurrentPageViewModel));
            }
        }

    }
}
