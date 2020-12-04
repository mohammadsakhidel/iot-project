using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackAPI.Models;

namespace TrackAPI.Services {
    public interface ITrackerService {
        Task<(bool, string)> CreateAsync(TrackerModel model);
        Task<TrackerModel> GetAsync(string id);
        Task<List<TrackerModel>> TakeAsync(int skip, int take);
        Task<List<TrackerModel>> SearchAsync(TrackerSearchModel model);
        Task<(bool, string)> UpdateAsync(TrackerModel model);
        Task<(bool, string)> RemoveAsync(string id);
        Task<List<TrackerReportModel>> GetReportsAsync(string trackerId, DateTime? date);
    }
}
