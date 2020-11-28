using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackAPI.Constants;
using TrackAPI.Models;
using TrackDataAccess.Models;
using TrackDataAccess.Repositories;

namespace TrackAPI.Services {
    public class TrackerService : ITrackerService {

        private readonly ITrackerRepository _trackerRepository;
        public TrackerService(ITrackerRepository trackerRepository) {
            _trackerRepository = trackerRepository;
        }

        public async Task CreateAsync(TrackerModel model) {
            var tracker = new Tracker {
                Id = $"{model.Manufacturer}-{model.RawID}".ToUpper(),
                CreationTime = DateTime.UtcNow,
                RawID = model.RawID,
                Manufacturer = model.Manufacturer,
                CommandSet = model.CommandSet,
                ProductId = model.ProductId,
                UserId = model.UserId
            };
            _trackerRepository.Add(tracker);
            await _trackerRepository.SaveAsync();
        }

        public async Task<TrackerModel> GetAsync(string id) {
            var tracker = await _trackerRepository.GetAsync(id);
            if (tracker == null)
                return null;

            return mapEntityToModel(tracker);
        }

        public async Task<List<TrackerModel>> TakeAsync(int skip, int take) {

            var trackers = await _trackerRepository.TakeAsync(skip, take, t => t.CreationTime, true);
            var models = trackers.Select(t => mapEntityToModel(t));
            return models.ToList();

        }

        public async Task<(bool, string)> UpdateAsync(TrackerModel model) {

            var tracker = await _trackerRepository.GetAsync(model.Id);
            if (tracker == null)
                return (false, "Tracker not found.");

            tracker.RawID = model.RawID;
            tracker.Manufacturer = model.Manufacturer;
            tracker.CommandSet = model.CommandSet;
            tracker.ProductId = model.ProductId;
            tracker.UserId = model.UserId;

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

        #region Private Methods:
        private static TrackerModel mapEntityToModel(Tracker model) {
            return new TrackerModel {
                Id = model.Id,
                RawID = model.RawID,
                Manufacturer = model.Manufacturer,
                CommandSet = model.CommandSet,
                ProductId = model.ProductId,
                UserId = model.UserId,
                CreationTime = model.CreationTime.ToString(Values.DATETIME_FORMAT)
            };
        }
        #endregion

    }
}
