using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackDataAccess.Models;
using TrackDataAccess.Repositories;
using TrackWorker.Models;

namespace TrackWorker.Services {
    public class ReportService : IReportService {

        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;

        public ReportService(IReportRepository reportRepository, IMapper mapper) {
            _reportRepository = reportRepository;
            _mapper = mapper;
        }

        public async Task AddAsync(ReportModel reportModel) {
            var report = _mapper.Map<Report>(reportModel);
            _reportRepository.Add(report);
            await _reportRepository.SaveAsync();
        }
    }
}
