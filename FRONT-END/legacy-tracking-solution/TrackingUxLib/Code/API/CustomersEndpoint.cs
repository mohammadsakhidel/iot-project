using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TrackingModels.Dtos;
using TrackingUtils.Utils;
using TrackingUxLib.Code.API.Interfaces;

namespace TrackingUxLib.Code.API
{
    public class CustomersEndpoint : ApiEndpoint, ICustomersEndpoint
    {
        #region Constants:
        const string GET = "customers/get";
        const string GET_LATESTS = "customers/getlatests";
        const string CREATE = "customers/create";
        const string UPDATE = "customers/update";
        const string DELETE = "customers/delete";
        #endregion

        #region Get Actions:
        public async Task<CustomerDto> GetAsync(int id)
        {
            var response = await HttpClient.GetAsync($"{GET}/{id}");
            HttpUtil.VerifySuccessStatusCode(response);
            var dto = await response.Content.ReadAsAsync<CustomerDto>();
            return dto;
        }

        public async Task<List<CustomerDto>> GetLatestsAsync(int count = 20)
        {
            var response = await HttpClient.GetAsync($"{GET_LATESTS}?count={count}");
            HttpUtil.VerifySuccessStatusCode(response);
            var dtos = await response.Content.ReadAsAsync<List<CustomerDto>>();
            return dtos;
        }
        #endregion

        #region Post Actions:
        public async Task CreateAsync(CustomerDto dto)
        {
            var response = await HttpClient.PostAsJsonAsync(CREATE, dto);
            HttpUtil.VerifySuccessStatusCode(response);
        }
        #endregion

        #region Put Actions:
        public async Task UpdateAsync(CustomerDto dto)
        {
            var response = await HttpClient.PutAsJsonAsync(UPDATE, dto);
            HttpUtil.VerifySuccessStatusCode(response);
        }
        #endregion

        #region Delete Actions:
        public async Task DeleteAsync(int id)
        {
            var response = await HttpClient.DeleteAsync($"{DELETE}/{id}");
            HttpUtil.VerifySuccessStatusCode(response);
        }
        #endregion
    }
}