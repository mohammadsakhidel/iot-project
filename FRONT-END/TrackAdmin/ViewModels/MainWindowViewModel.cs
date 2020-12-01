using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TrackAdmin.Commands;
using TrackAdmin.Constants;
using TrackAdmin.DTOs;
using TrackAdmin.Shared;

namespace TrackAdmin.ViewModels {
    public class MainWindowViewModel : BaseViewModel {

        public MainWindowViewModel() {
            ConfigMediatorSubscriptions();
        }

        #region ----------------------- STATE -------------------------

        //--- CURRENT PAGE
        IViewModel _currentPage;
        public IViewModel CurrentPage {
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
                        var types = this.GetType().Assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(IViewModel))).ToList();
                        var pageType = types.SingleOrDefault(t => t.Name == $"I{pageName}ViewModel");
                        if (pageType != null) {
                            var pageVM = (IViewModel)((App)Application.Current).ServiceProvider.GetService(pageType);
                            ChangeCurrentPage(pageVM);
                        }
                    }, pageName => {
                        return true;
                    });

                return sidebarCommand;
            }
        }
        #endregion

        #region FIELDS:

        #endregion

        #region PROPERTIES:

        #endregion

        #region METHODS:
        public void ChangeCurrentPage(IViewModel pageVM) {
            if (CurrentPage != pageVM)
                CurrentPage = pageVM;
        }

        private void ConfigMediatorSubscriptions() {
            
            // Go To User Editor:
            Mediator.Subscribe(MediatorTokens.GoToUserEditor, user => {
                var services = ((App)Application.Current).ServiceProvider;
                var userEditorVM = (IUserEditorViewModel)services.GetService(typeof(IUserEditorViewModel));
                userEditorVM.Init(user as UserDto);
                ChangeCurrentPage(userEditorVM);
            });

            // Back To Users Page:
            Mediator.Subscribe(MediatorTokens.BackToUsers, result => {
                var services = ((App)Application.Current).ServiceProvider;
                var done = Convert.ToBoolean(result);
                var usersVM = (IUsersViewModel)services.GetService(typeof(IUsersViewModel));
                if (done)
                    _ = usersVM.GetDataAsync();
                
                ChangeCurrentPage(usersVM);
            });

        }
        #endregion

    }
}
