using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TrackAdmin.Commands;
using TrackAdmin.Shared;

namespace TrackAdmin.ViewModels {
    public class MainWindowViewModel : BaseViewModel {

        public MainWindowViewModel() {

        }

        #region ----------------------- STATE -------------------------

        //--- CURRENT PAGE
        IPageViewModel _currentPage;
        public IPageViewModel CurrentPage {
            get {
                return _currentPage;
            }
            set {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }



        #endregion

        #region ---------------------- COMMANDS -----------------------
        private ICommand sidebarCommand;
        public ICommand SidebarCommand {
            get {
                if (sidebarCommand == null)
                    sidebarCommand = new RelayCommand(pageName => {
                        var types = this.GetType().Assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IPageViewModel))).ToList();
                        var pageType = types.SingleOrDefault(t => t.Name == $"{pageName}ViewModel");
                        if (pageType != null) {
                            ChangeCurrentPage(pageType);
                        }
                    }, pageName => {
                        return true;
                    });

                return sidebarCommand;
            }
        }
        #endregion

        #region FIELDS:
        private readonly List<IPageViewModel> _pagesCache = new List<IPageViewModel>();
        #endregion

        #region PROPERTIES:

        #endregion

        #region METHODS:
        public void ChangeCurrentPage(Type pageType) {

            var pageVM = _pagesCache.FirstOrDefault(p => p.GetType().Name == pageType.Name);
            if (pageVM == null) {
                pageVM = (IPageViewModel)Activator.CreateInstance(pageType);
                _pagesCache.Add(pageVM);
            }

            if (CurrentPage != pageVM) {
                CurrentPage = pageVM;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }
        #endregion

    }
}
