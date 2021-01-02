using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackWorker.Models;

namespace TrackWorker.Services {
    public interface IAccessCodeService {

        Task<AccessCodeModel> GetAsync(string code);

    }
}
