using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TrackAdmin.Commands;
using TrackAdmin.Constants;
using TrackAdmin.DTOs;
using TrackAdmin.Shared;

namespace TrackAdmin.ViewModels {
    public class SearchUsersViewModel : BaseViewModel, ISearchUsersViewModel {

        #region --------------------- STATE ----------------------
        private string userId;
        public string UserId {
            get { return userId; }
            set {
                userId = value;
                OnPropertyChanged(nameof(UserId));
            }
        }

        private string givenName;
        public string GivenName {
            get { return givenName; }
            set {
                givenName = value;
                OnPropertyChanged(nameof(GivenName));
            }
        }

        private string surname;
        public string Surname {
            get { return surname; }
            set {
                surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }

        private string email;
        public string Email {
            get { return email; }
            set {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        #endregion

        #region -------------------- COMMANDS ---------------------
        private ICommand search;
        public ICommand Search {
            get {
                if (search == null) {
                    search = new RelayCommand(_ => {
                        Mediator.Notify(MediatorTokens.BackToUsers, false);
                        var model = new UserSearchDto { 
                            UserId = UserId,
                            GivenName = GivenName,
                            Surname = Surname,
                            Email = email
                        };
                        Mediator.Notify(MediatorTokens.SearchUsers, model);
                    });
                }

                return search;
            }
        }

        private ICommand backToUsers;
        public ICommand BackToUsers {
            get {
                if (backToUsers == null) {
                    backToUsers = new RelayCommand(p => {
                        Mediator.Notify(MediatorTokens.BackToUsers, Convert.ToBoolean(p));
                    });
                }

                return backToUsers;
            }
        }
        #endregion

    }
}
