using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TrackAdmin.Constants;
using TrackAdmin.DTOs;
using TrackAdmin.Helpers;

namespace TrackAdmin.Services {
    public class TrackerService : ITrackerService {

        private readonly IConfiguration _configuration;
        public TrackerService(IConfiguration configuration) {
            _configuration = configuration;
        }

        public async Task<List<TrackerDto>> ListAsync() {

            // Http Client Preparation:
            var services = ((App)Application.Current).ServiceProvider;
            using var http = (HttpClient)services.GetService(typeof(HttpClient));

            // Send Request:
            var url = ApiUtils.Combine(_configuration["API:Host"], ApiEndpoints.TRACKERS);
            var response = await http.GetAsync(url);
            response.EnsureSuccessStatusCode();

            // Deserialization:
            var trackers = await response.Content.ReadAsAsync<List<TrackerDto>>();
            return trackers;

        }

        public async Task<List<TrackerDto>> SearchAsync(TrackerSearchDto dto) {
            // Http Client Preparation:
            var services = ((App)Application.Current).ServiceProvider;
            using var http = (HttpClient)services.GetService(typeof(HttpClient));

            // Send Request:
            var url = ApiUtils.Combine(_configuration["API:Host"], ApiEndpoints.TRACKERS_SEARCH);
            var response = await http.PostAsJsonAsync(url, dto);
            response.EnsureSuccessStatusCode();

            // Deserialization:
            var trackers = await response.Content.ReadAsAsync<List<TrackerDto>>();
            return trackers;
        }

        public async Task<(bool done, string message)> DeleteAsync(string trackerId) {

            // Http Client Preparation:
            var services = ((App)Application.Current).ServiceProvider;
            using var http = (HttpClient)services.GetService(typeof(HttpClient));

            // Sending Request:
            var url = ApiUtils.Combine(_configuration["API:Host"], ApiEndpoints.TRACKERS);
            var response = await http.DeleteAsync($"{url}/{trackerId}");
            if (!response.IsSuccessStatusCode)
                return (false, response.Content.ReadAsStringAsync().Result);

            // Result:
            return (true, string.Empty);

        }

        public async Task<(bool done, string message)> CreateAsync(TrackerDto tracker) {

            // Http Client Preparation:
            var services = ((App)Application.Current).ServiceProvider;
            using var http = (HttpClient)services.GetService(typeof(HttpClient));

            // Sending Request:
            var url = ApiUtils.Combine(_configuration["API:Host"], ApiEndpoints.TRACKERS);
            var response = await http.PostAsJsonAsync(url, tracker);
            if (!response.IsSuccessStatusCode)
                return (false, response.Content.ReadAsStringAsync().Result);

            // Result:
            return (true, string.Empty);

        }

        public async Task<(bool done, string message)> UpdateAsync(TrackerDto tracker) {

            // Http Client Preparation:
            var services = ((App)Application.Current).ServiceProvider;
            using var http = (HttpClient)services.GetService(typeof(HttpClient));

            // Sending Request:
            var url = ApiUtils.Combine(_configuration["API:Host"], ApiEndpoints.TRACKERS);
            var response = await http.PutAsJsonAsync(url, tracker);
            if (!response.IsSuccessStatusCode)
                return (false, response.Content.ReadAsStringAsync().Result);

            // Result:
            return (true, string.Empty);

        }

        public async Task<List<TrackerReportDto>> GetReportsAsync(string trackerId, string date) {

            // Http Client Preparation:
            var services = ((App)Application.Current).ServiceProvider;
            using var http = (HttpClient)services.GetService(typeof(HttpClient));

            // Send Request:
            var endpoint = ApiUtils.Combine(_configuration["API:Host"], ApiEndpoints.TRACKERS);
            var url = $"{endpoint}/{trackerId}/reports/{date}";
            var response = await http.GetAsync(url);
            response.EnsureSuccessStatusCode();

            // Deserialization:
            var reports = await response.Content.ReadAsAsync<List<TrackerReportDto>>();
            return reports;

        }

        public async Task<List<CommandLogDto>> GetCommandLogsAsync(string trackerId, string date) {

            // Http Client Preparation:
            var services = ((App)Application.Current).ServiceProvider;
            using var http = (HttpClient)services.GetService(typeof(HttpClient));

            // Send Request:
            var endpoint = ApiUtils.Combine(_configuration["API:Host"], ApiEndpoints.TRACKERS);
            var url = $"{endpoint}/{trackerId}/commandlogs/{date}";
            var response = await http.GetAsync(url);
            response.EnsureSuccessStatusCode();

            // Deserialization:
            var logs = await response.Content.ReadAsAsync<List<CommandLogDto>>();
            return logs;

        }
    }
}
