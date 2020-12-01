using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TrackAdmin.Constants;
using TrackAdmin.DTOs;
using TrackAdmin.Extensions;
using TrackAdmin.Helpers;

namespace TrackAdmin.Services {
    public class UserService : IUserService {

        private readonly IConfiguration _configuration;
        public UserService(IConfiguration configuration) {
            _configuration = configuration;
        }

        public async Task<List<UserDto>> ListAsync(int page = 1) {

            // Http Client Preparation:
            using var http = new HttpClient();
            http.AddAuthHeader(_configuration["API:Token"]);

            // Send Request:
            var url = ApiUtils.Combine(_configuration["API:Host"], ApiEndpoints.USERS);
            var response = await http.GetAsync(url);
            response.EnsureSuccessStatusCode();

            // Deserialization:
            var users = await response.Content.ReadAsAsync<List<UserDto>>();
            return users;

        }

        public async Task<(bool done, string message)> CreateAsync(UserDto user) {

            // Http Client Preparation:
            using var http = new HttpClient();
            http.AddAuthHeader(_configuration["API:Token"]);

            // Sending Request:
            var url = ApiUtils.Combine(_configuration["API:Host"], ApiEndpoints.USERS);
            var response = await http.PostAsJsonAsync(url, user);
            if (!response.IsSuccessStatusCode)
                return (false, response.Content.ReadAsStringAsync().Result);

            // Result:
            return (true, string.Empty);

        }

        public async Task<(bool done, string message)> UpdateAsync(UserDto user) {

            // Http Client Preparation:
            using var http = new HttpClient();
            http.AddAuthHeader(_configuration["API:Token"]);

            // Sending Request:
            var url = ApiUtils.Combine(_configuration["API:Host"], ApiEndpoints.USERS);
            var response = await http.PutAsJsonAsync(url, user);
            if (!response.IsSuccessStatusCode)
                return (false, response.Content.ReadAsStringAsync().Result);

            // Result:
            return (true, string.Empty);

        }
    }
}
