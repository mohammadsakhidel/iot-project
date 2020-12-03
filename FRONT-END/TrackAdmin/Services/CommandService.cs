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
    public class CommandService : ICommandService {
        private readonly IConfiguration _configuration;
        public CommandService(IConfiguration configuration) {
            _configuration = configuration;
        }

        public async Task<ApiResult> ConnectAsync(string trackerId) {

            // Http Client Preparation:
            var services = ((App)Application.Current).ServiceProvider;
            using var http = (HttpClient)services.GetService(typeof(HttpClient));

            // Send Request:
            var url = ApiUtils.Combine(_configuration["API:Host"], ApiEndpoints.COMMANDS_CONNECT);
            var endpoint = $"{url}/{trackerId}";
            var response = await http.PostAsync(endpoint, null);

            // Return Result:
            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadAsAsync<ApiResult>();
            } else {
                return new ApiResult {
                    Done = false,
                    Error = await response.Content.ReadAsStringAsync()
                };
            }

        }

        public async Task<ApiResult> ExecuteAsync(ExecuteCommandDto dto) {

            // Http Client Preparation:
            var services = ((App)Application.Current).ServiceProvider;
            using var http = (HttpClient)services.GetService(typeof(HttpClient));

            // Send Request:
            var url = ApiUtils.Combine(_configuration["API:Host"], ApiEndpoints.COMMANDS);
            var response = await http.PostAsJsonAsync(url, dto);

            // Return Result:
            if (response.IsSuccessStatusCode) {
                return await response.Content.ReadAsAsync<ApiResult>();
            } else {
                return new ApiResult {
                    Done = false,
                    Error = await response.Content.ReadAsStringAsync()
                };
            }

        }

        public async Task<List<string>> GetSetCommandsAsync(string setName) {

            // Http Client Preparation:
            var services = ((App)Application.Current).ServiceProvider;
            using var http = (HttpClient)services.GetService(typeof(HttpClient));

            // Send Request:
            var url = ApiUtils.Combine(_configuration["API:Host"], ApiEndpoints.COMMANDS_SETS);
            var response = await http.GetAsync(url);
            response.EnsureSuccessStatusCode();

            // Return Result:
            var result = await response.Content.ReadAsAsync<Dictionary<string, List<string>>>();
            return result[setName];

        }
    }
}
