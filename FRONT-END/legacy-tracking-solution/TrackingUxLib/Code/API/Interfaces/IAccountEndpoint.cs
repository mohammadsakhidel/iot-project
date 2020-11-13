using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingModels.Dtos;

namespace TrackingUxLib.Code.API.Interfaces
{
    public interface IAccountEndpoint : IDisposable
    {
        Task<ApiResultDto> ValidateUserAsync(string userName, string password);
    }
}
