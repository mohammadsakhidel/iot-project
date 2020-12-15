using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackAPI.Models;

namespace TrackAPI.Services {
    public interface IImageService {
        Task<ImageModel> GetAsync(string id);
        Task<(bool, string)> CreateAsync(ImageModel model);
        Task<(bool, string)> RemoveAsync(string id);
    }
}
