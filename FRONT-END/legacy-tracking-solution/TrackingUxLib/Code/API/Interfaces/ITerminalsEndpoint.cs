using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingModels.Dtos;

namespace TrackingUxLib.Code.API.Interfaces
{
    public interface ITerminalsEndpoint : IDisposable
    {
        #region Command Actions:
        Task<bool> TestConnectionAsync(string terminalId);
        Task SetPasswordAsync(string terminalId, string password);
        Task SetCenterNumberAsync(string terminalId, string centerNumber);
        Task SetServerAsync(string terminalId, string serverAddress, int portNumber);
        Task SetUploadIntervalAsync(string terminalId, int intervalSeconds);
        Task SetLanguageZoneAsync(string terminalId, string language, string timeZone);
        Task SetSosNumbersAsync(string terminalId, string sos1, string sos2, string sos3);
        Task SetContactsAsync(string terminalId, List<KeyValuePair<string, string>> contacts);
        Task SwitchLowBatterySmsAsync(string terminalId, bool stat);
        Task SwitchSmsToSosAsync(string terminalId, bool stat);
        Task SwitchRemoveAlarmAsync(string terminalId, bool stat);
        Task SwitchRemoveSmsAlarmAsync(string terminalId, bool stat);
        Task FactoryResetAsync(string terminalId);
        Task MakeCallAsync(string terminalId, string phoneNumber);
        Task PowerOffAsync(string terminalId);
        Task RestartAsync(string terminalId);
        Task<string> GetVersionAsync(string terminalId);
        Task SendFlowersAsync(string terminalId, int count);
        Task SendMessageAsync(string terminalId, string message);
        Task SendVoiceAsync(string terminalId, string base64WavVoice);
        #endregion
        #region Terminal Management Actions:
        Task<TerminalDto> GetAsync(string id);
        Task<List<TerminalDto>> GetLatestsAsync(int count = 20);
        Task<List<TerminalDto>> GetCustomerTerminals(string userName);
        Task UpdateAsync(TerminalDto dto);
        Task CreateAsync(TerminalDto dto);
        Task DeleteAsync(string id);
        #endregion
    }
}
