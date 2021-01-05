using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackAPI.Models;
using TrackDataAccess.Models;
using TrackDataAccess.Repositories;

namespace TrackAPI.Services {
    public class CommandService : ICommandService {

        private readonly ICommandLogRepository _commandLogRepository;
        private readonly IMapper _mapper;
        public CommandService(ICommandLogRepository commandLogRepository, IMapper mapper) {
            _commandLogRepository = commandLogRepository;
            _mapper = mapper;
        }

        public async Task AddLogAsync(CommandLogModel model) {

            var log = _mapper.Map<CommandLog>(model);
            _commandLogRepository.Add(log);
            await _commandLogRepository.SaveAsync();

        }

        public async Task<List<CommandLogModel>> GetLogsAsync(string trackerId, DateTime? date) {

            var logsDate = date ?? DateTime.UtcNow;
            var logs = await Task.Run(() => {
                return _commandLogRepository.Filter(r =>
                            r.TrackerId == trackerId &&
                            r.CreationTime.Date == logsDate.Date
                        ).ToList();
            });

            var models = logs.Select(r => _mapper.Map<CommandLogModel>(r)).ToList();
            return models;

        }
    }
}
