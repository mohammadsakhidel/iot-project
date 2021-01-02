using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackAPI.Models;
using TrackDataAccess.Models;
using TrackDataAccess.Repositories;

namespace TrackAPI.Services {
    public class AccessCodeService : IAccessCodeService {

        private readonly IAccessCodeRepository _accessCodeRepository;
        private readonly IMapper _mapper;
        public AccessCodeService(IAccessCodeRepository accessCodeRepository, IMapper mapper) {
            _accessCodeRepository = accessCodeRepository;
            _mapper = mapper;
        }

        public async Task AddAsync(AccessCodeModel model) {
            var accessCode = _mapper.Map<AccessCode>(model);
            _accessCodeRepository.Add(accessCode);
            await _accessCodeRepository.SaveAsync();
        }
    }
}
