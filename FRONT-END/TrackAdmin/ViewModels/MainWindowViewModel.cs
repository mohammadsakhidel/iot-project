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

        #region TestCommand:
        private ICommand testCommand;
        public ICommand TestCommand {
            get {
                if (testCommand == null)
                    testCommand = new RelayCommand(() => {
                        TestMessage = new Random().Next(1000000, 10000000).ToString();
                        OnPropertyChanged(nameof(TestMessage));
                    }, () => Convert.ToInt32(TestMessage) < 5000000);

                return testCommand;
            }
        }

        private ICommand testCommand2;
        public ICommand TestCommand2 {
            get {
                if (testCommand2 == null)
                    testCommand2 = new RelayCommand(() => {
                        TestMessage = 0.ToString();
                        OnPropertyChanged(nameof(TestMessage));
                    });

                return testCommand2;
            }
        }

        public string TestMessage { get; set; }
        #endregion

    }
}
