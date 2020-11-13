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
    public class TerminalsEndpoint : ApiEndpoint, ITerminalsEndpoint
    {
        #region Constants:
        const string GET = "terminals/get";
        const string GET_LATESTS = "terminals/getlatests";
        const string CREATE = "terminals/create";
        const string UPDATE = "terminals/update";
        const string DELETE = "terminals/delete";
        const string TEST_CONNECTION = "terminals/testconnection";
        const string SET_PASSWORD = "terminals/setpassword";
        const string SET_CENTER_NUMBER = "terminals/setcenternumber";
        const string SET_SERVER = "terminals/setserver";
        const string SET_UPLOAD_INTERVAL = "terminals/setuploadinterval";
        const string SET_LANGUAGE_ZONE = "terminals/setlanguagezone";
        const string SET_SOS_NUMBERS = "terminals/setsosnumbers";
        const string SAVE_CONTACTS = "terminals/savecontacts";
        const string SWITCH_LOW_BATTERY_SMS = "terminals/switchlowbatterysms";
        const string SWITCH_SMS_TO_SOS = "terminals/switchsmstosos";
        const string SWITCH_REMOVE_ALARM = "terminals/switchremovealarm";
        const string SWITCH_REMOVE_SMS_ALARM = "terminals/switchremovesmsalarm";
        const string FACTORY_RESET = "terminals/factoryreset";
        const string CALL = "terminals/call";
        const string POWER_OFF = "terminals/poweroff";
        const string RESTART = "terminals/restart";
        const string GET_VERSION = "terminals/getversion";
        const string SEND_FLOWERS = "terminals/sendflowers";
        const string SEND_MESSAGE = "terminals/sendmessage";
        const string SEND_VOICE = "terminals/sendvoice";
        const string GET_CUSTOMER_TERMINALS = "terminals/getcustomerterminals";
        #endregion

        #region Get Actions:
        public async Task<TerminalDto> GetAsync(string id)
        {
            var response = await HttpClient.GetAsync($"{GET}/{id}");
            HttpUtil.VerifySuccessStatusCode(response);
            var dto = await response.Content.ReadAsAsync<TerminalDto>();
            return dto;
        }

        public async Task<List<TerminalDto>> GetLatestsAsync(int count = 20)
        {
            var response = await HttpClient.GetAsync($"{GET_LATESTS}?count={count}");
            HttpUtil.VerifySuccessStatusCode(response);
            var dtos = await response.Content.ReadAsAsync<List<TerminalDto>>();
            return dtos;
        }

        public async Task<List<TerminalDto>> GetCustomerTerminals(string userName)
        {
            var response = await HttpClient.GetAsync($"{GET_CUSTOMER_TERMINALS}?username={userName}");
            HttpUtil.VerifySuccessStatusCode(response);
            var dtos = await response.Content.ReadAsAsync<List<TerminalDto>>();
            return dtos;
        }
        #endregion

        #region Post Actions:
        public async Task CreateAsync(TerminalDto dto)
        {
            var response = await HttpClient.PostAsJsonAsync(CREATE, dto);
            HttpUtil.VerifySuccessStatusCode(response);
        }
        #endregion

        #region Put Actions:
        public async Task UpdateAsync(TerminalDto dto)
        {
            var response = await HttpClient.PutAsJsonAsync(UPDATE, dto);
            HttpUtil.VerifySuccessStatusCode(response);
        }
        #endregion

        #region Delete Actions:
        public async Task DeleteAsync(string id)
        {
            var response = await HttpClient.DeleteAsync($"{DELETE}/{id}");
            HttpUtil.VerifySuccessStatusCode(response);
        }
        #endregion

        #region Commands:
        public async Task<bool> TestConnectionAsync(string terminalId)
        {
            var response = await HttpClient.PostAsync($"{TEST_CONNECTION}/{terminalId}", null);
            return response.IsSuccessStatusCode;
        }
        public async Task SetPasswordAsync(string terminalId, string password)
        {
            var dto = new SetPasswordDto { TerminalID = terminalId, Password = password };
            var response = await HttpClient.PostAsJsonAsync($"{SET_PASSWORD}", dto);
            HttpUtil.VerifySuccessStatusCode(response);
        }
        public async Task SetCenterNumberAsync(string terminalId, string centerNumber)
        {
            var dto = new SetCenterNumberDto { TerminalID = terminalId, CenterNumber = centerNumber };
            var response = await HttpClient.PostAsJsonAsync($"{SET_CENTER_NUMBER}", dto);
            HttpUtil.VerifySuccessStatusCode(response);
        }
        public async Task SetServerAsync(string terminalId, string serverAddress, int portNumber)
        {
            var dto = new SetServerDto { TerminalID = terminalId, IP = serverAddress, Port = portNumber };
            var response = await HttpClient.PostAsJsonAsync($"{SET_SERVER}", dto);
            HttpUtil.VerifySuccessStatusCode(response);
        }
        public async Task SetUploadIntervalAsync(string terminalId, int intervalSeconds)
        {
            var dto = new SetUploadIntervalDto { TerminalID = terminalId, IntervalSeconds = intervalSeconds };
            var response = await HttpClient.PostAsJsonAsync($"{SET_UPLOAD_INTERVAL}", dto);
            HttpUtil.VerifySuccessStatusCode(response);
        }
        public async Task SetLanguageZoneAsync(string terminalId, string language, string timeZone)
        {
            var dto = new SetLanguageZoneDto { TerminalID = terminalId, Language = language, TimeZone = timeZone };
            var response = await HttpClient.PostAsJsonAsync($"{SET_LANGUAGE_ZONE}", dto);
            HttpUtil.VerifySuccessStatusCode(response);
        }
        public async Task SetSosNumbersAsync(string terminalId, string sos1, string sos2, string sos3)
        {
            var dto = new SetSOSNumbersDto { TerminalID = terminalId, SOS1 = sos1, SOS2 = sos2, SOS3 = sos3 };
            var response = await HttpClient.PostAsJsonAsync($"{SET_SOS_NUMBERS}", dto);
            HttpUtil.VerifySuccessStatusCode(response);
        }
        public async Task SetContactsAsync(string terminalId, List<KeyValuePair<string, string>> contacts)
        {
            var dto = new SetPhoneBookContactsDto
            {
                TerminalID = terminalId,
                Contacts = contacts.ToArray()
            };
            var response = await HttpClient.PostAsJsonAsync($"{SAVE_CONTACTS}", dto);
            HttpUtil.VerifySuccessStatusCode(response);
        }
        public async Task SwitchLowBatterySmsAsync(string terminalId, bool stat)
        {
            var dto = new SwitchSomethingDto { TerminalID = terminalId, Status = stat };
            var response = await HttpClient.PostAsJsonAsync($"{SWITCH_LOW_BATTERY_SMS}", dto);
            HttpUtil.VerifySuccessStatusCode(response);
        }
        public async Task SwitchSmsToSosAsync(string terminalId, bool stat)
        {
            var dto = new SwitchSomethingDto { TerminalID = terminalId, Status = stat };
            var response = await HttpClient.PostAsJsonAsync($"{SWITCH_SMS_TO_SOS}", dto);
            HttpUtil.VerifySuccessStatusCode(response);
        }
        public async Task SwitchRemoveAlarmAsync(string terminalId, bool stat)
        {
            var dto = new SwitchSomethingDto { TerminalID = terminalId, Status = stat };
            var response = await HttpClient.PostAsJsonAsync($"{SWITCH_REMOVE_ALARM}", dto);
            HttpUtil.VerifySuccessStatusCode(response);
        }
        public async Task SwitchRemoveSmsAlarmAsync(string terminalId, bool stat)
        {
            var dto = new SwitchSomethingDto { TerminalID = terminalId, Status = stat };
            var response = await HttpClient.PostAsJsonAsync($"{SWITCH_REMOVE_SMS_ALARM}", dto);
            HttpUtil.VerifySuccessStatusCode(response);
        }
        public async Task FactoryResetAsync(string terminalId)
        {
            var response = await HttpClient.PostAsync($"{FACTORY_RESET}/{terminalId}", null);
            HttpUtil.VerifySuccessStatusCode(response);
        }
        public async Task MakeCallAsync(string terminalId, string phoneNumber)
        {
            var dto = new MakeCallDto { TerminalID = terminalId, PhoneNumber = phoneNumber };
            var response = await HttpClient.PostAsJsonAsync($"{CALL}", dto);
            HttpUtil.VerifySuccessStatusCode(response);
        }
        public async Task PowerOffAsync(string terminalId)
        {
            var response = await HttpClient.PostAsync($"{POWER_OFF}/{terminalId}", null);
            HttpUtil.VerifySuccessStatusCode(response);
        }
        public async Task RestartAsync(string terminalId)
        {
            var response = await HttpClient.PostAsync($"{RESTART}/{terminalId}", null);
            HttpUtil.VerifySuccessStatusCode(response);
        }
        public async Task<string> GetVersionAsync(string terminalId)
        {
            var response = await HttpClient.PostAsync($"{GET_VERSION}/{terminalId}", null);
            HttpUtil.VerifySuccessStatusCode(response);
            var data = await response.Content.ReadAsAsync<dynamic>();
            return data != null ? data.version : "NULL";
        }
        public async Task SendFlowersAsync(string terminalId, int count)
        {
            var response = await HttpClient.PostAsync($"{SEND_FLOWERS}/{terminalId}?count={count}", null);
            HttpUtil.VerifySuccessStatusCode(response);
        }
        public async Task SendMessageAsync(string terminalId, string message)
        {
            var dto = new SendMessageDto { TerminalID = terminalId, Message = message };
            var response = await HttpClient.PostAsJsonAsync($"{SEND_MESSAGE}", dto);
            HttpUtil.VerifySuccessStatusCode(response);
        }
        public async Task SendVoiceAsync(string terminalId, string base64WavVoice)
        {
            var dto = new SendVoiceDto { TerminalID = terminalId, WavBase64 = base64WavVoice };
            var response = await HttpClient.PostAsJsonAsync($"{SEND_VOICE}", dto);
            HttpUtil.VerifySuccessStatusCode(response);
        }
        #endregion
    }
}
