using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackAPI.Models;

namespace TrackAPI.Services {
    public interface ICommandService {

        Task AddLogAsync(CommandLogModel model);

        Task<List<CommandLogModel>> GetLogsAsync(string trackerId, DateTime? date);

    }
}
