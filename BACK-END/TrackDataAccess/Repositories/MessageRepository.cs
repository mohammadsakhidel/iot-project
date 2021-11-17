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
    public class MessageRepository : Repository<Message>, IMessageRepository {
        public MessageRepository(TrackDbContext context) : base(context) {
        }

        public List<GpsTrackerMessage> GetLocationMessages(string trackerId, DateTime start) {
            return Context.Set<Message>()
                .OfType<GpsTrackerMessage>()
                .Where(m => m.TrackerId == trackerId && m.CreationTime > start)
                .OrderBy(m => m.CreationTime)
                .ToList();
        }
    }
}
