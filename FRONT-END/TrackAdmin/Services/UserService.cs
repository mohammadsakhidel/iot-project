using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
            using var http = new HttpClient();
            http.AddTokenHeader(_configuration);
            var url = ApiUtils.Combine(_configuration["API:Host"], ApiEndpoints.USERS);
            var response = await http.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var contentString = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<UserDto>>(contentString, new JsonSerializerOptions { 
                PropertyNameCaseInsensitive = true
            });
            return users;
        }
    }
}
