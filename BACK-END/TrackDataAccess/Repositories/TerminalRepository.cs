using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TrackDataAccess.Models;
using TrackDataAccess.Repositories.Base;

namespace TrackDataAccess.Repositories {
    public class TerminalRepository : Repository<Terminal>, ITerminalRepository {
        public TerminalRepository(DbContext context) : base(context) {
        }
    }
}
