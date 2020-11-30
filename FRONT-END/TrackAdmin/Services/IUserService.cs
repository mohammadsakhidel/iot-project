using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackAdmin.DTOs;

namespace TrackAdmin.Services {
    public interface IUserService {
        Task<List<UserDto>> ListAsync(int page = 1);
    }
}
