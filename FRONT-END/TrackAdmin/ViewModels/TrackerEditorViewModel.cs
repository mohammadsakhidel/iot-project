using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TrackAdmin.Commands;
using TrackAdmin.Constants;
using TrackAdmin.DTOs;
using TrackAdmin.Helpers;
using TrackAdmin.Resources;
using TrackAdmin.Services;
using TrackAdmin.Shared;

namespace TrackAdmin.ViewModels {
    public class TrackerEditorViewModel : BaseViewModel, ITrackerEditorViewModel {

        private readonly ITrackerService _trackerService;
        private readonly IUserService _userService;
        public TrackerEditorViewModel(ITrackerService trackerService, IUserService userService) {
            _trackerService = trackerService;
            _userService = userService;
        }

        #region ------------------------ STATE ----------------------
        private TrackerDto tracker;
        public TrackerDto Tracker {
            get { return tracker; }
            set {
                tracker = value;
                OnPropertyChanged(nameof(Tracker));
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

        private string manufacturer;
        public string Manufacturer {
            get { return manufacturer; }
            set {
                manufacturer = value;
                RemoveValidationErrors(nameof(Manufacturer));
                ValidateRequiredProp(nameof(Manufacturer), value);
                OnPropertyChanged(nameof(Manufacturer));
            }
        }

        private string rawId;
        public string RawID {
            get { return rawId; }
            set {
                rawId = value;
                RemoveValidationErrors(nameof(RawID));
                ValidateRequiredProp(nameof(RawID), value);
                OnPropertyChanged(nameof(RawID));
            }
        }

        private string serialNumber;
        public string SerialNumber {
            get { return serialNumber; }
            set {
                serialNumber = value;
                RemoveValidationErrors(nameof(SerialNumber));
                ValidateRequiredProp(nameof(SerialNumber), value);
                OnPropertyChanged(nameof(SerialNumber));
            }
        }

        private string displayName;
        public string DisplayName {
            get { return displayName; }
            set {
                displayName = value;
                RemoveValidationErrors(nameof(DisplayName));
                ValidateRequiredProp(nameof(DisplayName), value);
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        private string userId;
        public string UserId {
            get { return userId; }
            set {
                userId = value;
                OnPropertyChanged(nameof(UserId));
            }
        }

        private string commandSet;
        public string CommandSet {
            get { return commandSet; }
            set {
                commandSet = value;
                RemoveValidationErrors(nameof(CommandSet));
                ValidateRequiredProp(nameof(CommandSet), value);
                OnPropertyChanged(nameof(CommandSet));
            }
        }

        private int? productId;
        public int? ProductId {
            get { return productId; }
            set {
                productId = value;
                OnPropertyChanged(nameof(ProductId));
            }
        }

        private string productModel;
        public string ProductModel {
            get { return productModel; }
            set {
                productModel = value;
                OnPropertyChanged(nameof(ProductModel));
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

        #region ----------------------- COMMANDS --------------------
        private ICommand backToTrackers;
        public ICommand BackToTrackers {
            get {
                if (backToTrackers == null) {
                    backToTrackers = new RelayCommand(p => {
                        Mediator.Notify(MediatorTokens.BackToTrackers, Convert.ToBoolean(p));
                    });
                }

                return backToTrackers;
            }
        }

        private ICommand save;
        public ICommand Save {
            get {
                if (save == null) {
                    save = new RelayCommand(_ => {
                        if (!string.IsNullOrEmpty(Tracker?.Id))
                            _ = UpdateTrackerAsync();
                        else
                            _ = CreateTrackerAsync();
                    });
                }

                return save;
            }
        }

        private ICommand testCommand;
        public ICommand TestCommand {
            get {
                if (testCommand == null) {
                    testCommand = new RelayCommand(x => {
                        var ss = "";
                    });
                }
                return testCommand;
            }

        }

        #endregion

        #region Fields:
        CancellationTokenSource _cts = null;
        #endregion

        #region Properties:
        public Dictionary<string, string> CommandSetItems {
            get {
                return new Dictionary<string, string> {
                    { CommandSetNames.DEFAULT, "Default" }
                };
            }
        }

        public Dictionary<string, string> ManufacturerItems {
            get {
                return new Dictionary<string, string> {
                    { Manufacturers.ThreeGElec, "3G" }
                };
            }
        }
        #endregion

        #region Methods:
        private async Task CreateTrackerAsync() {
            try {

                // UI show Loading:
                (Error, Message, IsLoading) = ("", "", true);

                // Call API:
                var tracker = new TrackerDto {
                    Manufacturer = Manufacturer,
                    RawID = RawID,
                    SerialNumber = SerialNumber,
                    DisplayName = DisplayName,
                    UserId = UserId,
                    CommandSet = CommandSet,
                    ProductId = ProductId,
                    ProductModel = ProductModel,
                    Explanation = Explanation
                };
                (var done, var message) = await _trackerService.CreateAsync(tracker);
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

        private async Task UpdateTrackerAsync() {
            try {

                // UI show Loading:
                (Error, Message, IsLoading) = ("", "", true);

                // Call API:
                var tracker = new TrackerDto {
                    Id = Tracker.Id,
                    Manufacturer = Manufacturer,
                    RawID = RawID,
                    SerialNumber = SerialNumber,
                    DisplayName = DisplayName,
                    UserId = UserId,
                    CommandSet = CommandSet,
                    ProductId = ProductId,
                    ProductModel = ProductModel,
                    Explanation = Explanation
                };
                (var done, var message) = await _trackerService.UpdateAsync(tracker);
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

        public void Init(TrackerDto tracker) {
            Tracker = tracker;
            Users = new List<UserDto>();
            OperationDone = false;
            UserIdInput = tracker?.UserId;

            Manufacturer = tracker?.Manufacturer;
            RawID = tracker?.RawID;
            SerialNumber = tracker?.SerialNumber;
            DisplayName = tracker?.DisplayName;
            UserId = tracker?.UserId;
            CommandSet = tracker?.CommandSet;
            ProductId = tracker?.ProductId;
            ProductModel = tracker?.ProductModel;
            Explanation = tracker?.Explanation;
        }

        private void ValidateRequiredProp(string propName, string propVal) {
            if (string.IsNullOrEmpty(propVal))
                AddValidationError(propName, $"{propName} is required.");
        }

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
