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
using TrackingDesktop.Code.ViewModels;
using TrackingUtils.Constants;
using TrackingUtils.Objects.Exceptions;
using TrackingUxLib.Code.API.Interfaces;
using TrackingUxLib.Code.Utils;
using TrackingUxLib.Resources.Values;
using Microsoft.Practices.Unity;
using System.Text.RegularExpressions;
using System.IO;
using Microsoft.Win32;
using System.Net.Http;

namespace TrackingDesktop.Views.UserControls
{
    public partial class TestTerminal : UserControl
    {
        #region Constructors:
        public TestTerminal()
        {
            InitializeComponent();
        }
        #endregion

        #region Fields:
        ITerminalsEndpoint _apiEndpoint;
        string _terminalId = "";
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
                var vm = DataContext as TestTerminalVM;
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
        private async void CommandButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var cmd = cmbCommand.SelectedValue.ToString();
                switch (cmd)
                {
                    case "MakeCallCommand":
                        await MakeCall(tbMakeCallPhoneNumber.Text);
                        break;
                    case "PowerOffCommand":
                        await PowerOff();
                        break;
                    case "RestartCommand":
                        await Restart();
                        break;
                    case "GetVersionCommand":
                        await GetVersion();
                        break;
                    case "SendFlowersCommand":
                        await SendFlowers(tbFlowersCount.Text);
                        break;
                    case "SendMessageCommand":
                        await SendMessage(tbMessageText.Text);
                        break;
                    case "SendVoiceCommand":
                        await SendVoice(txtSelectedVoiceFileName.Text);
                        break;
                }
            }
            catch (Exception ex)
            {
                progress.IsBusy = false;
                ExceptionManager.Handle(ex);
            }
        }
        private void SelectWavFileButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openfiledialog = new OpenFileDialog();
                openfiledialog.Filter = "Wave Files (*.wav) | *.wav";
                openfiledialog.Multiselect = false;
                var res = openfiledialog.ShowDialog();
                if (res.HasValue && res.Value)
                {
                    txtSelectedVoiceFileName.Text = openfiledialog.FileName;
                }
                else
                {
                    txtSelectedVoiceFileName.Text = "";
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Handle(ex);
            }
        }
        #endregion

        #region Methods:
        async Task MakeCall(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber.Replace(" ", "")))
                throw new ValidationException(Strings.InputRequiredFields);

            if (!(new Regex(Patterns.phonenumber)).IsMatch(phoneNumber))
                throw new ValidationException(Strings.InvalidPhoneNumber);

            progress.IsBusy = true;
            await _apiEndpoint.MakeCallAsync(_terminalId, phoneNumber);
            UxUtil.ShowMessage(Strings.SuccessfullyDone);
            progress.IsBusy = false;
        }
        async Task PowerOff()
        {
            var res = UxUtil.ShowQuestion(Strings.PowerOffWarning);
            if (res == MessageBoxResult.Yes)
            {
                progress.IsBusy = true;
                await _apiEndpoint.PowerOffAsync(_terminalId);
                UxUtil.ShowMessage(Strings.SuccessfullyDone);
                progress.IsBusy = false;
            }
        }
        async Task Restart()
        {
            progress.IsBusy = true;
            await _apiEndpoint.RestartAsync(_terminalId);
            UxUtil.ShowMessage(Strings.SuccessfullyDone);
            progress.IsBusy = false;
        }
        async Task GetVersion()
        {
            progress.IsBusy = true;
            var version = await _apiEndpoint.GetVersionAsync(_terminalId);
            lblVersion.Content = $"Version: {version}";
            progress.IsBusy = false;
        }
        async Task SendFlowers(string flowersCount)
        {
            if (string.IsNullOrEmpty(flowersCount.Replace(" ", "")))
                throw new ValidationException(Strings.InputRequiredFields);

            if (!(new Regex(Patterns.number)).IsMatch(flowersCount))
                throw new ValidationException(Strings.InvalidNumber);

            progress.IsBusy = true;
            await _apiEndpoint.SendFlowersAsync(_terminalId, Convert.ToInt32(flowersCount));
            UxUtil.ShowMessage(Strings.SuccessfullyDone);
            progress.IsBusy = false;
        }
        async Task SendMessage(string message)
        {
            if (string.IsNullOrEmpty(message.Replace(" ", "")))
                throw new ValidationException(Strings.InputRequiredFields);

            progress.IsBusy = true;
            await _apiEndpoint.SendMessageAsync(_terminalId, message);
            UxUtil.ShowMessage(Strings.SuccessfullyDone);
            progress.IsBusy = false;
        }
        async Task SendVoice(string wavFilePath)
        {
            if (string.IsNullOrEmpty(wavFilePath.Replace(" ", "")))
                throw new ValidationException(Strings.SelectFileRequired);

            var wavBytes = File.ReadAllBytes(wavFilePath);
            var base64Wav = Convert.ToBase64String(wavBytes);

            progress.IsBusy = true;
            await _apiEndpoint.SendVoiceAsync(_terminalId, base64Wav);
            UxUtil.ShowMessage(Strings.SuccessfullyDone);
            progress.IsBusy = false;
        }
        #endregion
    }
}