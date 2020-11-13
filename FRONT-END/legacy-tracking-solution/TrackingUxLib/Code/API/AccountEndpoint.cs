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
    public class AccountEndpoint : ApiEndpoint, IAccountEndpoint
    {
        #region Constants:
        const string VALIDATE_USER = "account/validateuser";
        #endregion

        #region Post Actions:
        public async Task<ApiResultDto> ValidateUserAsync(string userName, string password)
        {
            var dto = new SignInDto { UserName = userName, Password = password };
            var response = await HttpClient.PostAsJsonAsync(VALIDATE_USER, dto);
            HttpUtil.VerifySuccessStatusCode(response);
            var result = await response.Content.ReadAsAsync<ApiResultDto>();
            return result;
        }
        #endregion
    }
}
