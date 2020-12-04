using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TrackAdmin.Commands;
using TrackAdmin.Constants;
using TrackAdmin.DTOs;
using TrackAdmin.Services;

namespace TrackAdmin.ViewModels {
    public class TrackerLogsViewModel : BaseViewModel, ITrackerLogsViewModel {

        private readonly ITrackerService _trackerService;
        public TrackerLogsViewModel(ITrackerService trackerService) {
            _trackerService = trackerService;
        }

        #region -------------------- STATE --------------------
        private string trackerId;
        public string TrackerId {
            get { return trackerId; }
            set {
                trackerId = value.ToUpper();
                OnPropertyChanged(nameof(TrackerId));
            }
        }

        private DateTime? selectedDate = DateTime.Now;
        public DateTime? SelectedDate {
            get { return selectedDate; }
            set {
                selectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        private List<TrackerReportDto> reports;
        public List<TrackerReportDto> Reports {
            get { return reports; }
            set {
                reports = value;
                OnPropertyChanged(nameof(Reports));
            }
        }

        private List<CommandLogDto> commandLogs;
        public List<CommandLogDto> CommandLogs {
            get { return commandLogs; }
            set {
                commandLogs = value;
                OnPropertyChanged(nameof(CommandLogs));
            }
        }
        #endregion

        #region ------------------- COMMANDS ------------------
        private ICommand loadData;
        public ICommand LoadData {
            get {
                if (loadData == null) {
                    loadData = new RelayCommand(_ => {
                        _ = LoadDataAsync();
                    });
                }
                return loadData;
            }
        }
        #endregion

        #region METHODS:
        private async Task LoadDataAsync() {
            try {
                Error = "";
                IsLoading = true;

                var date = SelectedDate.HasValue
                ? SelectedDate.Value.ToString(Values.DATE_FORMAT)
                : DateTime.UtcNow.ToString(Values.DATE_FORMAT);

                var taskReports = _trackerService.GetReportsAsync(TrackerId, date);
                var taskLogs = _trackerService.GetCommandLogsAsync(TrackerId, date);

                await Task.WhenAll(taskReports, taskLogs);
                Reports = taskReports.Result;
                CommandLogs = taskLogs.Result;

                IsLoading = false;

            } catch (Exception ex) {
                IsLoading = false;
                _ = ShowError(ex.Message);
            }
        }
        #endregion

    }
}
