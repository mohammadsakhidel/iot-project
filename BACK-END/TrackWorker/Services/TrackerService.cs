using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackDataAccess.Repositories;
using TrackWorker.Models;

namespace TrackWorker.Services {
    public class TrackerService : ITrackerService {

        private readonly ITrackerRepository _trackerRepository;
        private readonly IMapper _mapper;
        public TrackerService(ITrackerRepository trackerRepository, IMapper mapper) {
            _trackerRepository = trackerRepository;
            _mapper = mapper;
        }

        public TrackerModel Get(string id, bool freshData = false) {
            var tracker = _trackerRepository.Get(id);
            if (tracker == null)
                return null;

            if (freshData)
                _trackerRepository.Reload(tracker);

            return _mapper.Map<TrackerModel>(tracker);
        }

        public async Task<TrackerModel> GetWithIncludeAsync(string id, bool freshData = false) {
            var tracker = await _trackerRepository.GetWithIncludeAsync(id);
            if (tracker == null)
                return null;

            if (freshData)
                _trackerRepository.Reload(tracker);

            return _mapper.Map<TrackerModel>(tracker);
        }

        public async Task UpdateLastConnectAsync(string trackerId, string status, string server, DateTime? lastConnectDate) {
            var tracker = _trackerRepository.Get(trackerId);
            if (tracker != null) {

                if (!string.IsNullOrEmpty(status))
                    tracker.Status = status;

                if (!string.IsNullOrEmpty(server))
                    tracker.LastConnectedServer = server;

                if (lastConnectDate.HasValue)
                    tracker.LastConnection = lastConnectDate;

                await _trackerRepository.SaveAsync();
            }
        }

        public async Task UpdateStatusAsync(string trackerId, string status) {
            var tracker = _trackerRepository.Get(trackerId);
            if (tracker != null) {
                tracker.Status = status;
                await _trackerRepository.SaveAsync();
            }
        }
    }
}
