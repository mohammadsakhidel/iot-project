using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrackDataAccess.Database;
using TrackDataAccess.Models;
using TrackDataAccess.Repositories.Base;

namespace TrackDataAccess.Repositories {
    public class TrackerRepository : Repository<Tracker>, ITrackerRepository {
        public TrackerRepository(TrackDbContext context) : base(context) {
        }

        public async Task<Tracker> GetWithIncludeAsync(params object[] id) {
            var tracker = await Context.Set<Tracker>()
                .Include(t => t.Users)
                .Include(t => t.AllowedUsers)
                .SingleOrDefaultAsync(t => t.Id == id[0].ToString());

            return tracker;
        }
    }
}
