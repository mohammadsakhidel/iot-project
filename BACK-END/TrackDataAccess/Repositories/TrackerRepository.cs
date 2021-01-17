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

        public async Task AddPermittedUser(string trackerId, string userId, string permissions) {

            var set = Context.Set<TrackerAllowedUser>();
            var current = await set.SingleOrDefaultAsync(p => p.TrackerId == trackerId && p.UserId == userId);

            if (current == null) {
                var entity = new TrackerAllowedUser {
                    TrackerId = trackerId,
                    UserId = userId,
                    Permissions = permissions
                };
                set.Add(entity);
            } else {
                current.Permissions = permissions;
            }

            await Context.SaveChangesAsync();

        }

        public async Task<Tracker> GetWithIncludeAsync(params object[] id) {
            var tracker = await Context.Set<Tracker>()
                .Include(t => t.Users).ThenInclude(tu => tu.User)
                .Include(t => t.AllowedUsers).ThenInclude(au => au.User).ThenInclude(u => u.Claims)
                .SingleOrDefaultAsync(t => t.Id == id[0].ToString());

            return tracker;
        }

        public async Task RemovePermittedUser(string trackerId, string userId) {

            var set = Context.Set<TrackerAllowedUser>();
            var entity = await set.SingleOrDefaultAsync(p => p.TrackerId == trackerId && p.UserId == userId);
            if (entity != null) {
                Context.Remove(entity);
                await Context.SaveChangesAsync();
            }

        }
    }
}
