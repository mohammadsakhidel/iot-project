using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackLib.Commands;

namespace TrackAPI.Sockets {
    public interface ICommandExecutor {
        Task<CommandResponse> ExecuteAsync(CommandRequest request, string host = "");
    }
}
