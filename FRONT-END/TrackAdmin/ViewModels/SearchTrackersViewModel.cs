using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TrackAdmin.Commands;
using TrackAdmin.Constants;
using TrackAdmin.DTOs;
using TrackAdmin.Helpers;
using TrackAdmin.Services;
using TrackAdmin.Shared;

namespace TrackAdmin.ViewModels {
    public class SearchTrackersViewModel : BaseViewModel, ISearchTrackersViewModel {

        private readonly IUserService _userService;
        public SearchTrackersViewModel(IUserService userService) {
            _userService = userService;
        }

        #region --------------------- STATE ----------------------
        private string userId;
        public string UserId {
            get { return userId; }
            set {
                userId = value;
                OnPropertyChanged(nameof(UserId));
            }
        }

        private string rawId;
        public string RawID {
            get { return rawId; }
            set {
                rawId = value;
                OnPropertyChanged(nameof(RawID));
            }
        }

        private string manufacturer;
        public string Manufacturer {
            get { return manufacturer; }
            set {
                manufacturer = value;
                OnPropertyChanged(nameof(Manufacturer));
            }
        }

        private string serialNumber;
        public string SerialNumber {
            get { return serialNumber; }
            set {
                serialNumber = value;
                OnPropertyChanged(nameof(SerialNumber));
            }
        }

        private List<UserDto> users;
        public List<UserDto> Users {
            get { return users; }
            set {
                users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        private string userIdInput;
        public string UserIdInput {
            get { return userIdInput; }
            set {
                userIdInput = value;
                UpdateUsersList();
                OnPropertyChanged(nameof(UserIdInput));
            }
        }
        #endregion

        #region -------------------- COMMANDS ---------------------
        private ICommand search;
        public ICommand Search {
            get {
                if (search == null) {
                    search = new RelayCommand(_ => {
                        Mediator.Notify(MediatorTokens.BackToTrackers, false);
                        var model = new TrackerSearchDto {
                            UserId = UserId,
                            Manufacturer = Manufacturer,
                            RawID = RawID,
                            SerialNumber = SerialNumber
                        };
                        Mediator.Notify(MediatorTokens.SearchTrackers, model);
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
                        Mediator.Notify(MediatorTokens.BackToTrackers, Convert.ToBoolean(p));
                    });
                }

                return backToUsers;
            }
        }
        #endregion

        #region Fields:
        CancellationTokenSource _cts = null;
        #endregion

        #region Methods:
        private void UpdateUsersList() {

            if (_cts != null)
                TaskUtils.ClearTimeout(_cts);

            if (!string.IsNullOrEmpty(UserIdInput) && UserIdInput.Length > 2 &&
                !Regex.IsMatch(UserIdInput, Patterns.USER_DESC)) {

                _cts = TaskUtils.SetTimeout(async () => {
                    Users = await _userService.QueryAsync(UserIdInput);
                }, 1000);

            }

        }
        #endregion

    }
}
