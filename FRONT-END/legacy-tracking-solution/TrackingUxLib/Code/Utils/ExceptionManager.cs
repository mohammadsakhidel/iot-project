using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using TrackingUtils.Objects.Exceptions;
using TrackingUxLib.Resources.Values;

namespace TrackingUxLib.Code.Utils
{
    public class ExceptionManager
    {
        public static void Handle(Exception ex)
        {
            var message = GetMessage(ex);

            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                UxUtil.ShowError($"{ex.GetType().FullName}:{Environment.NewLine}{message}");
            });
        }

        public static void Log(Exception ex)
        {
            // log exception here
        }

        public static string GetMessage(Exception ex)
        {
            var message = "";

            if (ex is HttpException)
            {
                var httpex = (HttpException)ex;
                message = httpex.Response.Content.ReadAsStringAsync().Result;
            }
            else if (ex is HttpRequestException)
            {
                message = "replace ensureSuccessCode with verifySuccessCode fro details";
            }
            else
            {
                message = ex.Message;
            }

            return message;
        }
    }
}
