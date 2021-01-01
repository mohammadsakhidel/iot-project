using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackAPI.Models;

namespace TrackAPI.Services {
    public interface ITrackerService {
        Task<(bool, string)> CreateAsync(TrackerModel model);
        Task<TrackerModel> GetAsync(string id, bool reload = false);
        Task<TrackerModel> FindBySerialAsync(string serialNumber);
        Task<List<TrackerModel>> TakeAsync(int skip, int take);
        Task<List<TrackerModel>> GetUserTrackers(string userId);
        Task<List<TrackerModel>> SearchAsync(TrackerSearchModel model);
        Task<(bool, string)> UpdateAsync(TrackerModel model);
        Task<(bool, string)> RemoveAsync(string id);
        Task<List<TrackerReportModel>> GetReportsAsync(string trackerId, DateTime? date);
        Task<(bool, object)> AssignUser(string trackerId, string userId);
        Task<(bool, string)> UnassignUser(string trackerId, string userId);

    }
}
