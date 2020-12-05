using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using TrackAdmin.Commands;
using TrackAdmin.Constants;
using TrackAdmin.DTOs;
using TrackAdmin.Resources;
using TrackAdmin.Services;
using TrackAdmin.Shared;

namespace TrackAdmin.ViewModels {
    public class UserEditorViewModel : BaseViewModel, IUserEditorViewModel {

        private readonly IUserService _userService;
        public UserEditorViewModel(IUserService userService) {
            _userService = userService;
        }

        #region --------------------- STATE -----------------------
        private UserDto user;
        public UserDto User {
            get { return user; }
            set {
                user = value;
                OnPropertyChanged(nameof(User));
            }
        }

        private bool operationDone;
        public bool OperationDone {
            get { return operationDone; }
            set {
                operationDone = value;
                OnPropertyChanged(nameof(OperationDone));
            }
        }

        private string givenName;
        public string GivenName {
            get { return givenName; }
            set {
                givenName = value;
                RemoveValidationErrors(nameof(GivenName));
                ValidateRequiredProp(nameof(GivenName), value);
                ValidatePropWithRegex(nameof(GivenName), value, Patterns.NAME);
                OnPropertyChanged(nameof(GivenName));
            }
        }

        private string surname;
        public string Surname {
            get { return surname; }
            set {
                surname = value;
                RemoveValidationErrors(nameof(Surname));
                ValidateRequiredProp(nameof(Surname), value);
                ValidatePropWithRegex(nameof(Surname), value, Patterns.NAME);
                OnPropertyChanged(nameof(Surname));
            }
        }

        private string email;
        public string Email {
            get { return email; }
            set {
                email = value;
                RemoveValidationErrors(nameof(Email));
                ValidateRequiredProp(nameof(Email), value);
                ValidatePropWithRegex(nameof(Email), value, Patterns.EMAIL);
                OnPropertyChanged(nameof(Email));
            }
        }

        private string password;
        public string Password {
            get { return password; }
            set {
                password = value;
                RemoveValidationErrors(nameof(Password));
                ValidatePropWithRegex(nameof(Password), value, Patterns.PASSWORD);
                OnPropertyChanged(nameof(Password));
            }
        }

        private string phoneNumber;
        public string PhoneNumber {
            get { return phoneNumber; }
            set {
                phoneNumber = value;
                RemoveValidationErrors(nameof(PhoneNumber));
                ValidatePropWithRegex(nameof(PhoneNumber), value, Patterns.PHONE);
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }

        private string state;
        public string State {
            get { return state; }
            set {
                state = value;
                RemoveValidationErrors(nameof(State));
                ValidatePropWithRegex(nameof(State), value, Patterns.NAME);
                OnPropertyChanged(nameof(State));
            }
        }

        private string city;
        public string City {
            get { return city; }
            set {
                city = value;
                RemoveValidationErrors(nameof(City));
                ValidatePropWithRegex(nameof(City), value, Patterns.NAME);
                OnPropertyChanged(nameof(City));
            }
        }

        private string address;
        public string Address {
            get { return address; }
            set {
                address = value;
                RemoveValidationErrors(nameof(Address));
                ValidatePropWithRegex(nameof(Address), value, Patterns.NAME);
                OnPropertyChanged(nameof(Address));
            }
        }

        private string explanation;
        public string Explanation {
            get { return explanation; }
            set {
                explanation = value;
                OnPropertyChanged(nameof(Explanation));
            }
        }

        private bool isActive;
        public bool IsActive {
            get { return isActive; }
            set {
                isActive = value;
                OnPropertyChanged(nameof(IsActive));
            }
        }
        #endregion

        #region -------------------- COMMANDS ---------------------
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

        private ICommand save;
        public ICommand Save {
            get {
                if (save == null) {
                    save = new RelayCommand(_ => {
                        if (!string.IsNullOrEmpty(User?.Id))
                            _ = UpdateUserAsync();
                        else
                            _ = CreateUserAsync();
                    });
                }

                return save;
            }
        }
        #endregion

        #region Methods:
        private async Task CreateUserAsync() {
            try {

                // UI show Loading:
                (Error, Message, IsLoading) = ("", "", true);

                // Call API:
                var user = new UserDto {
                    GivenName = GivenName,
                    Surname = Surname,
                    Email = Email,
                    Password = Password,
                    PhoneNumber = PhoneNumber,
                    State = State,
                    City = City,
                    Address = Address,
                    Explanation = Explanation,
                    IsActive = IsActive
                };
                (var done, var message) = await _userService.CreateAsync(user);
                if (!done)
                    throw new ApplicationException(message);

                // UI clear loading:
                (IsLoading, OperationDone) = (false, true);
                _ = ShowMessage(Strings.SuccessfullyDone);

            } catch (Exception ex) {
                IsLoading = false;
                _ = ShowError(ex.Message);
            }
        }

        private async Task UpdateUserAsync() {
            try {

                // UI show Loading:
                (Error, Message, IsLoading) = ("", "", true);

                // Call API:
                var user = new UserDto {
                    Id = User.Id,
                    GivenName = GivenName,
                    Surname = Surname,
                    Email = Email,
                    Password = Password,
                    PhoneNumber = PhoneNumber,
                    State = State,
                    City = City,
                    Address = Address,
                    Explanation = Explanation,
                    IsActive = IsActive
                };
                (var done, var message) = await _userService.UpdateAsync(user);
                if (!done)
                    throw new ApplicationException(message);

                // UI clear loading:
                (IsLoading, OperationDone) = (false, true);
                _ = ShowMessage(Strings.SuccessfullyDone);

            } catch (Exception ex) {
                IsLoading = false;
                _ = ShowError(ex.Message);
            }
        }

        public void Init(UserDto user) {
            User = user;
            OperationDone = false;
            GivenName = user?.GivenName;
            Surname = user?.Surname;
            Email = user?.Email;
            Password = string.Empty;
            PhoneNumber = user?.PhoneNumber;
            State = user?.State;
            City = user?.City;
            Address = user?.Address;
            Explanation = user?.Explanation;
            IsActive = user != null && user.IsActive;
        }

        private void ValidateRequiredProp(string propName, string propVal) {
            if (string.IsNullOrEmpty(propVal))
                AddValidationError(propName, $"{propName} is required.");
        }

        private void ValidatePropWithRegex(string propName, string propValue, string pattern) {
            if (!string.IsNullOrEmpty(propValue) && !Regex.IsMatch(propValue, pattern))
                AddValidationError(propName, $"{propName} is invalid.");
        }
        #endregion
    }
}
