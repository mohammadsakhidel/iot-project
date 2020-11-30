using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using TrackAdmin.Commands;
using TrackAdmin.Constants;
using TrackAdmin.DTOs;
using TrackAdmin.Helpers;
using TrackAdmin.Services;

namespace TrackAdmin.ViewModels {
    public class UsersViewModel : BaseViewModel, IUsersViewModel {

        private readonly IUserService _userService;
        public UsersViewModel(IUserService userService) {
            _userService = userService;
            _ = GetDataAsync();
        }

        #region ----------------- STATE ------------------
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

        private string error;
        public string Error {
            get { return error; }
            set {
                error = value;
                OnPropertyChanged(nameof(Error));
            }
        }
        // ---------------------- Users:
        private List<UserDto> users;
        public List<UserDto> Users {
            get { return users; }
            set {
                users = value;
                OnPropertyChanged(nameof(Users));
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

        #endregion

        #region Methods:
        private async Task GetDataAsync() {
            try {
                Error = "";
                IsLoading = true;

                Users = await _userService.ListAsync();

                IsLoading = false;

            } catch (Exception ex) {
                IsLoading = false;
                Error = ex.Message;
            }
        }
        #endregion

    }
}
