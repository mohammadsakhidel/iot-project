using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackAPI.Helpers;
using TrackLib.Commands;

namespace TrackAPI.Sockets {
    public interface ICommandExecutor {
        Task<CommandResponse> ExecuteAsync(CommandRequest request, WorkerServerInfo host);
    }
}
