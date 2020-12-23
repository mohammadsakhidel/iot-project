using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackDataAccess.Database;
using TrackDataAccess.Models;
using TrackDataAccess.Models.Identity;
using TrackDataAccess.Repositories.Base;

namespace TrackDataAccess.Repositories {
    public class UserRepository : Repository<AppUser>, IUserRepository {
        public UserRepository(TrackDbContext context) : base(context) {
        }

        public List<Tracker> GetTrackers(string userId) {
            var recs = Context.Set<AppUser>()
                .Include(u => u.Trackers)
                .ThenInclude(ut => ut.Tracker)
                .Where(u => u.Id == userId)
                .SelectMany(u => u.Trackers.Select(ut => ut.Tracker))
                .ToList();

            return recs;
        }

        public List<AppUser> Search(Func<AppUser, bool> predicate, int take = 50) {
            var recs = Context.Set<AppUser>()
                .Include(u => u.Claims)
                .Where(predicate)
                .OrderByDescending(u => u.CreationTime)
                .Take(take)
                .ToList();
            return recs;
        }
    }
}
