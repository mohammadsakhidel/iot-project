using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TrackAdmin.Commands;
using TrackAdmin.Constants;
using TrackAdmin.DTOs;
using TrackAdmin.Resources;
using TrackAdmin.Services;
using TrackAdmin.Shared;

namespace TrackAdmin.ViewModels {
    public class TrackersViewModel : BaseViewModel, ITrackersViewModel {

        private readonly ITrackerService _trackerService;
        public TrackersViewModel(ITrackerService trackerService) {
            _trackerService = trackerService;
            _ = GetDataAsync();
            ConfigMediatorSubscriptions();
        }

        #region --------------------------- STATE ----------------------------
        private List<TrackerDto> trackers;
        public List<TrackerDto> Trackers {
            get { return trackers; }
            set {
                trackers = value;
                OnPropertyChanged(nameof(Trackers));
            }
        }

        private TrackerDto selectedRecord;
        public TrackerDto SelectedRecord {
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

        private ICommand goToTrackerEditor;
        public ICommand GoToTrackerEditor {
            get {
                if (goToTrackerEditor == null) {
                    goToTrackerEditor = new RelayCommand(action => {
                        Mediator.Notify(MediatorTokens.GoToTrackerEditor,
                            (action.ToString().ToLower() == "add" ? null : SelectedRecord));
                    });
                }

                return goToTrackerEditor;
            }
        }

        private ICommand deleteTracker;
        public ICommand DeleteTracker {
            get {
                if (deleteTracker == null) {
                    deleteTracker = new RelayCommand(p => {
                        var result = MessageBox.Show(Strings.AreYouSure, "???", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                        if (result == MessageBoxResult.Yes) {
                            _ = DeleteAsync(SelectedRecord.Id);
                        }
                    });
                }

                return deleteTracker;
            }
        }

        private ICommand search;
        public ICommand Search {
            get {
                if (search == null) {
                    search = new RelayCommand(_ => Mediator.Notify(MediatorTokens.GoToSearchTracker));
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

                Trackers = await _trackerService.ListAsync();

                IsLoading = false;

            } catch (Exception ex) {
                IsLoading = false;
                _ = ShowError(ex.Message);
            }
        }
        public async Task SearchAsync(TrackerSearchDto dto) {
            try {
                Error = "";
                IsLoading = true;

                Trackers = await _trackerService.SearchAsync(dto);

                IsLoading = false;

            } catch (Exception ex) {
                IsLoading = false;
                _ = ShowError(ex.Message);
            }
        }
        private async Task DeleteAsync(string trackerId) {
            try {

                // UI show Loading:
                (Error, Message, IsLoading) = ("", "", true);

                // Call API:
                (var done, var message) = await _trackerService.DeleteAsync(trackerId);
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
            Mediator.Subscribe(MediatorTokens.SearchTrackers, data => {
                _ = SearchAsync(data as TrackerSearchDto);
            });

        }
        #endregion

    }
}
