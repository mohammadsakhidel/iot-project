using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackLib.DataContracts;

namespace TrackAPI.Sockets {
    public interface ICommandExecutor {
        Task<CommandResponse> SendAsync(CommandRequest request, string host = "");
    }
}
