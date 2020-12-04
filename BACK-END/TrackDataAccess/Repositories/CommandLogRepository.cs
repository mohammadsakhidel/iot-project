using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackDataAccess.Database;
using TrackDataAccess.Models;
using TrackDataAccess.Repositories.Base;

namespace TrackDataAccess.Repositories {
    public class CommandLogRepository : Repository<CommandLog>, ICommandLogRepository {
        public CommandLogRepository(TrackDbContext context) : base(context) {
        }
    }
}
