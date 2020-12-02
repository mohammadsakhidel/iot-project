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
using TrackAdmin.Resources;
using TrackAdmin.Services;
using TrackAdmin.Shared;

namespace TrackAdmin.ViewModels {
    public class UsersViewModel : BaseViewModel, IUsersViewModel {

        private readonly IUserService _userService;
        public UsersViewModel(IUserService userService) {
            _userService = userService;
            _ = GetDataAsync();
            ConfigMediatorSubscriptions();
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
                OnPropertyChanged(nameof(SelectedRecord));
            }
        }
        #endregion

        #region -------------------- COMMANDS ---------------------
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
                        Mediator.Notify(MediatorTokens.GoToUserEditor,
                            (action.ToString().ToLower() == "add" ? null : SelectedRecord));
                    });
                }

                return goToUserEditor;
            }
        }

        private ICommand deleteUser;
        public ICommand DeleteUser {
            get {
                if (deleteUser == null) {
                    deleteUser = new RelayCommand(p => {
                        var result = MessageBox.Show(Strings.AreYouSure, "???", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes) {
                            _ = DeleteUserAsync(SelectedRecord.Id);
                        }
                    });
                }

                return deleteUser;
            }
        }

        private ICommand search;
        public ICommand Search {
            get {
                if (search == null) {
                    search = new RelayCommand(_ => Mediator.Notify(MediatorTokens.GoToSearchUser));
                }

                return search;
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
        public async Task SearchAsync(UserSearchDto dto) {
            try {
                Error = "";
                IsLoading = true;

                Users = await _userService.SearchAsync(dto);

                IsLoading = false;

            } catch (Exception ex) {
                IsLoading = false;
                _ = ShowError(ex.Message);
            }
        }
        private async Task DeleteUserAsync(string userId) {
            try {

                // UI show Loading:
                (Error, Message, IsLoading) = ("", "", true);

                // Call API:
                (var done, var message) = await _userService.DeleteAsync(userId);
                if (!done)
                    throw new ApplicationException(message);

                // UI clear loading:
                IsLoading = false;
                _ = ShowMessage(Strings.SuccessfullyDone);

                _ = GetDataAsync();

            } catch (Exception ex) {
                IsLoading = false;
                _ = ShowError(ex.Message);
            }
        }
        private void ConfigMediatorSubscriptions() {

            // Search users subscription:
            Mediator.Subscribe(MediatorTokens.SearchUsers, data => {
                _ = SearchAsync(data as UserSearchDto);
            });

        }
        #endregion

    }
}
