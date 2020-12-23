using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackDataAccess.Models;
using TrackDataAccess.Models.Identity;
using TrackDataAccess.Repositories.Base;

namespace TrackDataAccess.Repositories {
    public interface IUserRepository : IRepository<AppUser> {
        List<AppUser> Search(Func<AppUser, bool> func, int take = 50);
        List<Tracker> GetTrackers(string userId);
    }
}
