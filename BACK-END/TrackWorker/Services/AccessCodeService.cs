using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackDataAccess.Repositories;
using TrackWorker.Models;

namespace TrackWorker.Services {
    public class AccessCodeService : IAccessCodeService {

        private readonly IAccessCodeRepository _accessCodeRepository;
        private readonly IMapper _mapper;

        public AccessCodeService(IAccessCodeRepository accessCodeRepository, IMapper mapper) {
            _accessCodeRepository = accessCodeRepository;
            _mapper = mapper;
        }

        public async Task<AccessCodeModel> GetAsync(string code) {
            var accessCode = await _accessCodeRepository.GetAsync(code);
            if (accessCode == null)
                return null;

            return _mapper.Map<AccessCodeModel>(accessCode);
        }

    }
}
