using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackAPI.Models;

namespace TrackAPI.Services {
    public interface IUserService {
        Task<(bool, string)> CreateAsync(UserModel model);
        Task<(bool succeeded, string error)> UpdateAsync(UserModel model);
        Task<UserModel> GetAsync(string id);
        Task<List<UserModel>> GetAsync(int skip, int take);
        Task<UserModel> FindByUsernameAsync(string userName);
        Task<(bool succeeded, string error)> RemoveAsync(string id);
        Task<(bool isValid, string token)> ValidateUserAsync(string userName, string password);
    }
}
