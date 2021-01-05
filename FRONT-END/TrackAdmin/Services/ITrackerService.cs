using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackAdmin.DTOs;

namespace TrackAdmin.Services {
    public interface ITrackerService {
        Task<List<TrackerDto>> ListAsync();
        Task<List<TrackerDto>> SearchAsync(TrackerSearchDto dto);
        Task<(bool done, string message)> DeleteAsync(string trackerId);
        Task<(bool done, string message)> CreateAsync(TrackerDto tracker);
        Task<(bool done, string message)> UpdateAsync(TrackerDto tracker);
        Task<List<TrackerReportDto>> GetReportsAsync(string trackerId, string date);
        Task<List<CommandLogDto>> GetCommandLogsAsync(string trackerId, string date);
        Task<string[]> GetDefaultIconNamesAsync();
    }
}
