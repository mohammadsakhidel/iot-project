using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWorker.Models;

namespace TrackWorker.Services {
    public interface ITrackerService {
        TrackerModel Get(string id, bool freshData = false);
        Task<TrackerModel> GetWithIncludeAsync(string id, bool freshData = false);
        Task UpdateLastConnectAsync(string trackerId, string status, string server, DateTime? lastConnectDate);
        Task UpdateStatusAsync(string trackerId, string status);
    }
}
