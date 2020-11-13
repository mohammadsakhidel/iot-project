using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TrackingUtils.Constants;
using TrackingUxLib.Code.Utils;
using Microsoft.Practices.Unity;
using TrackingUxLib.Code.API.Interfaces;
using TrackingUtils.Objects.Exceptions;
using TrackingUxLib.Resources.Values;
using TrackingDesktop.Code.ViewModels;
using System.Text.RegularExpressions;

namespace TrackingDesktop.Views.UserControls
{
    public partial class ConfigTerminal : UserControl
    {
        #region Fields:
        ITerminalsEndpoint _apiEndpoint;
        string _terminalId = "";
        #endregion

        #region Ctors:
        public ConfigTerminal()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Handlers:
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                cmbManufacturer.ItemsSource = Collections.Manufacturers;
                cmbManufacturer.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex);
            }
        }
        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region Validation:
                if (cmbManufacturer.SelectedItem == null || string.IsNullOrEmpty(tbDeviceID.Text))
                    throw new ValidationException(Strings.InputRequiredFields);

                var manufacturer = cmbManufacturer.SelectedValue.ToString();
                var deviceId = tbDeviceID.Text;
                var terminalId = $"{manufacturer}_{deviceId}";
                #endregion

                #region Call Server:
                var vm = DataContext as ConfigTerminalVM;
                vm.Connected = false;
                progress.IsBusy = true;

                _apiEndpoint = App.Container.Resolve<ITerminalsEndpoint>();
                var isConnected = await _apiEndpoint.TestConnectionAsync(terminalId);
                if (!isConnected)
                    throw new Exception(Strings.DeviceNotConnectedMessage);

                _terminalId = terminalId;
                vm.Connected = isConnected;
                progress.IsBusy = false;
                #endregion
            }
            catch (Exception ex)
            {
                progress.IsBusy = false;
                ExceptionManager.Handle(ex);
            }
        }
        private async void SendCommand_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var tag = ((Button)e.Source).Tag.ToString();
                switch (tag)
                {
                    case "SetPassword":
                        await CallSetPassword(tbSetPasswordParameter.Text);
                        break;
                    case "SetCenterNumber":
                        await CallSetCenterNumber(tbSetCenterNumberParameter.Text);
                        break;
                    case "SetServer":
                        await CallSetServer(tbServerAddress.Text, tbServerPort.Text);
                        break;
                    case "SetUploadInterval":
                        await CallSetUploadInterval(tbUploadInterval.Text);
                        break;
                    case "SetLanguageAndTimeZone":
                        await CallSetLanguageZone((cmbLanguage.SelectedItem != null ? cmbLanguage.SelectedValue.ToString() : ""),
                            (cmbTimeZone.SelectedItem != null ? cmbTimeZone.SelectedValue.ToString() : ""));
                        break;
                    case "SetSosNumbers":
                        await CallSetSosNumbers(tbSos1.Text, tbSos2.Text, tbSos3.Text);
                        break;
                    case "SetContacts":
                        await CallSetContacts(tbContact1.Text, tbContact1PhoneNumber.Text,
                            tbContact2.Text, tbContact2PhoneNumber.Text, tbContact3.Text, tbContact3PhoneNumber.Text,
                            tbContact4.Text, tbContact4PhoneNumber.Text, tbContact5.Text, tbContact5PhoneNumber.Text);
                        break;
                    case "SetLowBatterySms":
                        await CallSwitchLowBatterySms(chLowBatterySms.IsChecked.HasValue ? chLowBatterySms.IsChecked.Value : false);
                        break;
                    case "SetSosSms":
                        await CallSwitchSmsToSos(chSmsToSOS.IsChecked.HasValue ? chSmsToSOS.IsChecked.Value : false);
                        break;
                    case "SetRemoveAlarm":
                        await CallSwitchRemoveAlarm(chRemoveAlarm.IsChecked.HasValue ? chRemoveAlarm.IsChecked.Value : false);
                        break;
                    case "SetRemoveAlarmSms":
                        await CallSwitchRemoveSmsAlarm(chRemoveAlarmSms.IsChecked.HasValue ? chRemoveAlarmSms.IsChecked.Value : false);
                        break;
                    case "FactoryReset":
                        await CallFactoryReset();
                        break;
                }
            }
            catch (Exception ex)
            {
                progress.IsBusy = false;
                ExceptionManager.Handle(ex);
            }
        }
        #endregion

        #region Api Commands:
        async Task CallSetPassword(string password)
        {
            if (string.IsNullOrEmpty(password.Replace(" ", "")))
                throw new ValidationException(Strings.InputRequiredFields);

            progress.IsBusy = true;
            await _apiEndpoint.SetPasswordAsync(_terminalId, password);
            UxUtil.ShowMessage(Strings.SuccessfullyDone);
            progress.IsBusy = false;
        }
        async Task CallSetCenterNumber(string centerNumber)
        {
            if (string.IsNullOrEmpty(centerNumber.Replace(" ", "")))
                throw new ValidationException(Strings.InputRequiredFields);

            if (!(new Regex(Patterns.phonenumber)).IsMatch(centerNumber))
                throw new ValidationException(Strings.InvalidPhoneNumber);

            progress.IsBusy = true;
            await _apiEndpoint.SetCenterNumberAsync(_terminalId, centerNumber);
            UxUtil.ShowMessage(Strings.SuccessfullyDone);
            progress.IsBusy = false;
        }
        async Task CallSetServer(string serverAddress, string port)
        {
            if (string.IsNullOrEmpty(serverAddress.Replace(" ", "")) || string.IsNullOrEmpty(port.Replace(" ", "")))
                throw new ValidationException(Strings.InputRequiredFields);

            if (!(new Regex(Patterns.number)).IsMatch(port))
                throw new ValidationException(Strings.InvalidPortNumber);

            progress.IsBusy = true;
            await _apiEndpoint.SetServerAsync(_terminalId, serverAddress, Convert.ToInt32(port));
            UxUtil.ShowMessage(Strings.SuccessfullyDone);
            progress.IsBusy = false;
        }
        async Task CallSetUploadInterval(string intervalSeconds)
        {
            if (string.IsNullOrEmpty(intervalSeconds.Replace(" ", "")))
                throw new ValidationException(Strings.InputRequiredFields);

            if (!(new Regex(Patterns.number)).IsMatch(intervalSeconds))
                throw new ValidationException(Strings.InvalidNumber);

            progress.IsBusy = true;
            await _apiEndpoint.SetUploadIntervalAsync(_terminalId, Convert.ToInt32(intervalSeconds));
            UxUtil.ShowMessage(Strings.SuccessfullyDone);
            progress.IsBusy = false;
        }
        async Task CallSetLanguageZone(string language, string zone)
        {
            if (string.IsNullOrEmpty(language.Replace(" ", "")) || string.IsNullOrEmpty(zone.Replace(" ", "")))
                throw new ValidationException(Strings.InputRequiredFields);

            progress.IsBusy = true;
            await _apiEndpoint.SetLanguageZoneAsync(_terminalId, language, zone);
            UxUtil.ShowMessage(Strings.SuccessfullyDone);
            progress.IsBusy = false;
        }
        async Task CallSetSosNumbers(string sos1, string sos2, string sos3)
        {
            var rgxPhone = new Regex(Patterns.phonenumber);

            if (!string.IsNullOrEmpty(sos1) && !rgxPhone.IsMatch(sos1))
                throw new ValidationException(Strings.InvalidPhoneNumber);
            if (!string.IsNullOrEmpty(sos2) && !rgxPhone.IsMatch(sos2))
                throw new ValidationException(Strings.InvalidPhoneNumber);
            if (!string.IsNullOrEmpty(sos3) && !rgxPhone.IsMatch(sos3))
                throw new ValidationException(Strings.InvalidPhoneNumber);

            progress.IsBusy = true;
            await _apiEndpoint.SetSosNumbersAsync(_terminalId, sos1, sos2, sos3);
            UxUtil.ShowMessage(Strings.SuccessfullyDone);
            progress.IsBusy = false;
        }
        async Task CallSetContacts(string c1Name, string c1Number, string c2Name, string c2Number,
            string c3Name, string c3Number, string c4Name, string c4Number, string c5Name, string c5Number)
        {
            #region Validation:
            var rgxPhone = new Regex(Patterns.phonenumber);

            if ((!string.IsNullOrEmpty(c1Number) && !rgxPhone.IsMatch(c1Number))
                || (!string.IsNullOrEmpty(c2Number) && !rgxPhone.IsMatch(c2Number))
                || (!string.IsNullOrEmpty(c3Number) && !rgxPhone.IsMatch(c3Number))
                || (!string.IsNullOrEmpty(c4Number) && !rgxPhone.IsMatch(c4Number))
                || (!string.IsNullOrEmpty(c5Number) && !rgxPhone.IsMatch(c5Number)))
                throw new ValidationException(Strings.InvalidPhoneNumber);

            if ((!string.IsNullOrEmpty(c1Number) && string.IsNullOrEmpty(c1Name.Replace(" ", "")))
                || (!string.IsNullOrEmpty(c2Number) && string.IsNullOrEmpty(c2Name.Replace(" ", "")))
                || (!string.IsNullOrEmpty(c3Number) && string.IsNullOrEmpty(c3Name.Replace(" ", "")))
                || (!string.IsNullOrEmpty(c4Number) && string.IsNullOrEmpty(c4Name.Replace(" ", "")))
                || (!string.IsNullOrEmpty(c5Number) && string.IsNullOrEmpty(c5Name.Replace(" ", ""))))
                throw new ValidationException(Strings.InputRequiredFields);
            #endregion

            var contacts = new List<KeyValuePair<string, string>>();
            if (!string.IsNullOrEmpty(c1Number))
                contacts.Add(new KeyValuePair<string, string>(c1Number, c1Name));
            if (!string.IsNullOrEmpty(c2Number))
                contacts.Add(new KeyValuePair<string, string>(c2Number, c2Name));
            if (!string.IsNullOrEmpty(c3Number))
                contacts.Add(new KeyValuePair<string, string>(c3Number, c3Name));
            if (!string.IsNullOrEmpty(c4Number))
                contacts.Add(new KeyValuePair<string, string>(c4Number, c4Name));
            if (!string.IsNullOrEmpty(c5Number))
                contacts.Add(new KeyValuePair<string, string>(c5Number, c5Name));

            progress.IsBusy = true;
            await _apiEndpoint.SetContactsAsync(_terminalId, contacts);
            UxUtil.ShowMessage(Strings.SuccessfullyDone);
            progress.IsBusy = false;
        }
        async Task CallSwitchLowBatterySms(bool stat)
        {
            progress.IsBusy = true;
            await _apiEndpoint.SwitchLowBatterySmsAsync(_terminalId, stat);
            UxUtil.ShowMessage(Strings.SuccessfullyDone);
            progress.IsBusy = false;
        }
        async Task CallSwitchSmsToSos(bool stat)
        {
            progress.IsBusy = true;
            await _apiEndpoint.SwitchSmsToSosAsync(_terminalId, stat);
            UxUtil.ShowMessage(Strings.SuccessfullyDone);
            progress.IsBusy = false;
        }
        async Task CallSwitchRemoveAlarm(bool stat)
        {
            progress.IsBusy = true;
            await _apiEndpoint.SwitchRemoveAlarmAsync(_terminalId, stat);
            UxUtil.ShowMessage(Strings.SuccessfullyDone);
            progress.IsBusy = false;
        }
        async Task CallSwitchRemoveSmsAlarm(bool stat)
        {
            progress.IsBusy = true;
            await _apiEndpoint.SwitchRemoveSmsAlarmAsync(_terminalId, stat);
            UxUtil.ShowMessage(Strings.SuccessfullyDone);
            progress.IsBusy = false;
        }
        async Task CallFactoryReset()
        {
            progress.IsBusy = true;
            await _apiEndpoint.FactoryResetAsync(_terminalId);
            UxUtil.ShowMessage(Strings.SuccessfullyDone);
            progress.IsBusy = false;
        }
        #endregion
    }
}
