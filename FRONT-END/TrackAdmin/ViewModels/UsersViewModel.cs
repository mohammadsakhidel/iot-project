using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TrackAdmin.Commands;
using TrackAdmin.Constants;
using TrackAdmin.DTOs;
using TrackAdmin.Helpers;
using TrackAdmin.Services;
using TrackAdmin.Shared;

namespace TrackAdmin.ViewModels {
    public class UsersViewModel : BaseViewModel, IUsersViewModel {

        private readonly IUserService _userService;
        public UsersViewModel(IUserService userService) {
            _userService = userService;
            _ = GetDataAsync();
        }

        #region ----------------- STATE ------------------
        private List<UserDto> users;
        public List<UserDto> Users {
            get { return users; }
            set {
                users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        private UserDto selectedRecord;
        public UserDto SelectedRecord {
            get { return selectedRecord; }
            set {
                selectedRecord = value;
                OnPropertyChanged(nameof(selectedRecord));
            }
        }
        #endregion

        #region -------------------- COMMANDS ---------------------
        // ---------------------- TestCommand:
        private ICommand loadData;
        public ICommand LoadData {
            get {
                if (loadData == null)
                    loadData = new RelayCommand(async p => {
                        await GetDataAsync();
                    });

                return loadData;
            }
        }

        private ICommand goToUserEditor;

        public ICommand GoToUserEditor {
            get {
                if (goToUserEditor == null) {
                    goToUserEditor = new RelayCommand(action => {
                        if (action.ToString().ToLower() == "add") {
                            Mediator.Notify(MediatorTokens.GoToUserEditor, null);
                        } else if (action.ToString().ToLower() == "edit") {
                            if (SelectedRecord == null)
                                _ = ShowError("Please select a user to edit.");
                            else
                                Mediator.Notify(MediatorTokens.GoToUserEditor, SelectedRecord);
                        }
                    });
                }

                return goToUserEditor;
            }
        }


        #endregion

        #region Methods:
        public async Task GetDataAsync() {
            try {
                Error = "";
                IsLoading = true;

                Users = await _userService.ListAsync();

                IsLoading = false;

            } catch (Exception ex) {
                IsLoading = false;
                _ = ShowError(ex.Message);
            }
        }
        #endregion

    }
}
