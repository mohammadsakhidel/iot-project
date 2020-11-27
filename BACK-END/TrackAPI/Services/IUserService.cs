using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackAPI.Models;

namespace TrackAPI.Services {
    public interface IUserService {
        Task<string> CreateAsync(UserModel model);
        Task<UserModel> GetAsync(string id);
        Task<List<UserModel>> GetAsync(int skip, int take);
        Task<(bool succeeded, string error)> RemoveAsync(string id);
    }
}
