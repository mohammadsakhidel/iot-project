using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackAdmin.DTOs;

namespace TrackAdmin.Services {
    public interface IUserService {
        Task<List<UserDto>> ListAsync();
        Task<List<UserDto>> SearchAsync(UserSearchDto dto);
        Task<List<UserDto>> QueryAsync(string query);
        Task<(bool done, string message)> CreateAsync(UserDto user);
        Task<(bool done, string message)> UpdateAsync(UserDto user);
        Task<(bool done, string message)> DeleteAsync(string userId);
    }
}
