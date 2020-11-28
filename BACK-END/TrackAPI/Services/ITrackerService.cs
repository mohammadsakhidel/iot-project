using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackAPI.Models;

namespace TrackAPI.Services {
    public interface ITrackerService {
        Task CreateAsync(TrackerModel model);
        Task<TrackerModel> GetAsync(string id);
        Task<List<TrackerModel>> TakeAsync(int skip, int take);
        Task<(bool, string)> UpdateAsync(TrackerModel model);
        Task<(bool, string)> RemoveAsync(string id);
    }
}
