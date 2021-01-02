using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackAPI.Models;

namespace TrackAPI.Services {
    public interface IAccessCodeService {
        Task AddAsync(AccessCodeModel model);
    }
}
