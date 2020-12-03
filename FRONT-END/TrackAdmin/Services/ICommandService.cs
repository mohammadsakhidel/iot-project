using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackAdmin.DTOs;

namespace TrackAdmin.Services {
    public interface ICommandService {
        Task<ApiResult> ConnectAsync(string trackerId);
        Task<List<string>> GetSetCommandsAsync(string setName);
        Task<ApiResult> ExecuteAsync(ExecuteCommandDto dto);
    }
}
