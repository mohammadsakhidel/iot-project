using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackAPI.Constants;
using TrackAPI.Models;
using TrackDataAccess.Models;
using TrackDataAccess.Models.Identity;
using TrackDataAccess.Repositories;

namespace TrackAPI.Services {
    public class TrackerService : ITrackerService {

        private readonly UserManager<AppUser> _userManager;
        private readonly ITrackerRepository _trackerRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;
        public TrackerService(ITrackerRepository trackerRepository, IMapper mapper,
            IReportRepository reportRepository, UserManager<AppUser> userManager) {
            _trackerRepository = trackerRepository;
            _reportRepository = reportRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<(bool, string)> CreateAsync(TrackerModel model) {

            // Validate UserId:
            if (!string.IsNullOrEmpty(model.UserId)) {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                    return (false, "Invalid UserId.");
            }

            var tracker = _mapper.Map<Tracker>(model);
            tracker.Id = $"{model.Manufacturer}-{model.RawID}".ToUpper();
            tracker.CreationTime = DateTime.UtcNow;

            _trackerRepository.Add(tracker);
            await _trackerRepository.SaveAsync();

            return (true, tracker.Id);
        }

        public async Task<TrackerModel> GetAsync(string id) {
            var tracker = await _trackerRepository.GetAsync(id);
            if (tracker == null)
                return null;

            return _mapper.Map<TrackerModel>(tracker);
        }

        public async Task<List<TrackerModel>> TakeAsync(int skip, int take) {

            var trackers = await _trackerRepository.TakeAsync(skip, take, t => t.CreationTime, true);
            var models = trackers.Select(t => _mapper.Map<TrackerModel>(t));
            return models.ToList();

        }

        public async Task<(bool, string)> UpdateAsync(TrackerModel model) {

            var tracker = await _trackerRepository.GetAsync(model.Id);
            if (tracker == null)
                return (false, "Tracker not found.");

            // Validate UserId:
            if (!string.IsNullOrEmpty(model.UserId)) {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                    return (false, "Invalid UserId.");
            }

            tracker.RawID = model.RawID;
            tracker.Manufacturer = model.Manufacturer;
            tracker.CommandSet = model.CommandSet;
            tracker.ProductType = model.ProductType;
            tracker.ProductModel = model.ProductModel;
            tracker.UserId = model.UserId;
            tracker.Explanation = model.Explanation;
            tracker.DisplayName = model.DisplayName;
            tracker.SerialNumber = model.SerialNumber;
            if (!string.IsNullOrEmpty(model.IconImageId))
                tracker.IconImageId = model.IconImageId;

            await _trackerRepository.SaveAsync();

            return (true, string.Empty);
        }

        public async Task<(bool, string)> RemoveAsync(string id) {
            var tracker = await _trackerRepository.GetAsync(id);
            if (tracker == null)
                return (false, "Tracker not found.");

            _trackerRepository.Remove(tracker);
            await _trackerRepository.SaveAsync();
            return (true, string.Empty);
        }

        public async Task<List<TrackerModel>> SearchAsync(TrackerSearchModel model) {
            var trackers = await Task.Run(() => {

                // Return Empty list if all parameters are empty:
                if (string.IsNullOrEmpty(model.UserId) &&
                    string.IsNullOrEmpty(model.RawID) &&
                    string.IsNullOrEmpty(model.SerialNumber) &&
                    string.IsNullOrEmpty(model.Manufacturer)) {

                    return new List<Tracker> { };

                }

                return _trackerRepository.Filter(t =>
                            (string.IsNullOrEmpty(model.UserId) || t.UserId == model.UserId) &&
                            (string.IsNullOrEmpty(model.RawID) || t.RawID == model.RawID) &&
                            (string.IsNullOrEmpty(model.SerialNumber) || t.SerialNumber.Contains(model.SerialNumber)) &&
                            (string.IsNullOrEmpty(model.Manufacturer) || t.Manufacturer.ToLower() == model.Manufacturer.ToLower())
                        ).OrderByDescending(t => t.CreationTime)
                        .Take(Values.PAGESIZE)
                        .ToList();
            });

            var models = trackers.Select(t => _mapper.Map<TrackerModel>(t));
            return models.ToList();
        }

        public async Task<List<TrackerReportModel>> GetReportsAsync(string trackerId, DateTime? date) {

            var reportDate = date ?? DateTime.UtcNow;
            var reports = await Task.Run(() => {
                return _reportRepository.Filter(r =>
                            r.TrackerId == trackerId &&
                            r.CreationTime.Date == reportDate.Date
                        ).ToList();
            });

            var models = reports.Select(r => _mapper.Map<TrackerReportModel>(r)).ToList();
            return models;

        }

        public async Task<List<TrackerModel>> GetUserTrackers(string userId) {
            var trackers = await Task.Run(() => {
                return _trackerRepository.Filter(t => t.UserId == userId);
            });
            return trackers.Select(t => _mapper.Map<Tracker, TrackerModel>(t)).ToList();
        }

        public async Task<(bool, string)> AssignUser(string trackerId, string userId) {
            var tracker = await _trackerRepository.GetAsync(trackerId);
            if (tracker == null)
                return (false, "Tracker not found.");

            if (!string.IsNullOrEmpty(tracker.UserId))
                return (false, ErrorCodes.USER_ASSIGNED);

            tracker.UserId = userId;
            await _trackerRepository.SaveAsync();

            return (true, string.Empty);
        }

        public async Task<(bool, string)> UnassignUser(string trackerId) {
            var tracker = await _trackerRepository.GetAsync(trackerId);
            if (tracker == null)
                return (false, "Tracker not found.");

            tracker.UserId = string.Empty;
            await _trackerRepository.SaveAsync();
            return (true, string.Empty);
        }
    }
}
