using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TrackingUtils.Utils;

namespace TrackingUxLib.Code.API
{
    public class ApiEndpoint : IDisposable
    {

        private HttpClient httpClient = null;
        protected HttpClient HttpClient
        {
            get
            {
                if (httpClient == null)
                {
                    httpClient = HttpUtil.CreateClient();
                }

                return httpClient;
            }
            set
            {
                httpClient = value;
            }
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}
