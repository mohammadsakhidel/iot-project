using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using TrackAdmin.Commands;
using TrackAdmin.DTOs;
using TrackAdmin.Resources;
using TrackAdmin.Services;

namespace TrackAdmin.ViewModels {
    public class ConfigTrackerViewModel : BaseViewModel, IConfigTrackerViewModel {

        private readonly ICommandService _commandService;
        public ConfigTrackerViewModel(ICommandService commandService) {
            _commandService = commandService;
        }

        #region ------------------- STATE ---------------------
        private bool isConnected;
        public bool IsConnected {
            get { return isConnected; }
            set {
                isConnected = value;
                OnPropertyChanged(nameof(IsConnected));
            }
        }

        private string trackerId;
        public string TrackerId {
            get { return trackerId; }
            set {
                trackerId = value.ToUpper();
                OnPropertyChanged(nameof(TrackerId));
            }
        }

        private TrackerDto connectedTracker;
        public TrackerDto ConnectedTracker {
            get { return connectedTracker; }
            set {
                connectedTracker = value;
                OnPropertyChanged(nameof(ConnectedTracker));
            }
        }

        private Dictionary<string, string> commands;
        public Dictionary<string, string> Commands {
            get { return commands; }
            set {
                commands = value;
                OnPropertyChanged(nameof(Commands));
            }
        }

        private string commandType;
        public string CommandType {
            get { return commandType; }
            set {
                commandType = value;
                OnPropertyChanged(nameof(CommandType));
            }
        }

        private string commandPayload;
        public string CommandPayload {
            get { return commandPayload; }
            set {
                commandPayload = value;
                OnPropertyChanged(nameof(CommandPayload));
            }
        }

        private string executionResultData;
        public string ExecutionResultData {
            get { return executionResultData; }
            set {
                executionResultData = value;
                OnPropertyChanged(nameof(ExecutionResultData));
            }
        }
        #endregion

        #region ------------------ COMMANDS -------------------
        private ICommand connect;
        public ICommand Connect {
            get {
                if (connect == null) {
                    connect = new RelayCommand(c => {
                        if (!string.IsNullOrEmpty(TrackerId))
                            _ = ConnectAsync();
                    });
                }
                return connect;
            }
        }

        private ICommand disconnect;
        public ICommand Disconnect {
            get {
                if (disconnect == null) {
                    disconnect = new RelayCommand(_ => {
                        ResetState();
                    });
                }
                return disconnect;
            }
        }

        private ICommand execute;
        public ICommand Execute {
            get {
                if (execute == null) {
                    execute = new RelayCommand(_ => {
                        _ = SendCommandAsync();
                    });
                }
                return execute;
            }
        }
        #endregion

        #region Methods:
        private async Task ConnectAsync() {
            try {
                Error = "";
                IsLoading = true;

                var result = await _commandService.ConnectAsync(TrackerId);
                if (result.Done) {

                    IsConnected = true;
                    ConnectedTracker = JsonSerializer.Deserialize<TrackerDto>(result.Data);
                    var commandList = await _commandService.GetSetCommandsAsync(ConnectedTracker.CommandSet);
                    Commands = commandList.ToDictionary(c => c, c => c);

                } else {
                    throw new ApplicationException(result.Error);
                }

                IsLoading = false;

            } catch (Exception ex) {
                IsLoading = false;
                _ = ShowError(ex.Message);
            }
        }
        private async Task SendCommandAsync() {
            try {
                Error = "";
                IsLoading = true;

                var dto = new ExecuteCommandDto {
                    TrackerId = TrackerId,
                    CommandType = CommandType,
                    Payload = CommandPayload
                };
                var result = await _commandService.ExecuteAsync(dto);
                if (result.Done) {

                    ExecutionResultData = !string.IsNullOrEmpty(result.Data) ? result.Data 
                        : Strings.ExecutionResultEmpty;

                } else {
                    throw new ApplicationException(result.Error);
                }

                IsLoading = false;

            } catch (Exception ex) {
                IsLoading = false;
                _ = ShowError(ex.Message);
            }
        }
        private void ResetState() {
            IsConnected = false;
            TrackerId = string.Empty;
            ConnectedTracker = null;
            Commands = new Dictionary<string, string>();
            CommandType = string.Empty;
            CommandPayload = string.Empty;
            ExecutionResultData = string.Empty;
        }
        #endregion

    }
}
