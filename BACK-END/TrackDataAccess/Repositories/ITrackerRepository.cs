using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrackDataAccess.Models;
using TrackDataAccess.Repositories.Base;

namespace TrackDataAccess.Repositories {
    public interface ITrackerRepository : IRepository<Tracker> {
        Task<Tracker> GetWithIncludeAsync(params object[] id);
        Task AddPermittedUser(string trackerId, string userId, string permissions);
        Task RemovePermittedUser(string trackerId, string userId);
    }
}
