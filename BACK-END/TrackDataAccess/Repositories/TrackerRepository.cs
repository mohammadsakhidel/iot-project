using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TrackDataAccess.Database;
using TrackDataAccess.Models;
using TrackDataAccess.Repositories.Base;

namespace TrackDataAccess.Repositories {
    public class TrackerRepository : Repository<Tracker>, ITrackerRepository {
        public TrackerRepository(TrackDbContext context) : base(context) {
        }
    }
}
