using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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
using TerminalSimulator.Code.ViewModels;
using TrackingUtils.Constants;
using TrackingUtils.Objects.Commands;

namespace TerminalSimulator
{
    public partial class MainWindow : Window
    {
        #region Ctors:
        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region Fields:
        TcpClient _client = null;
        string _deviceId = "";
        string _manufacturerId = "";
        #endregion

        #region Event Handlers:
        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region Validation:
                var rgxPort = new Regex(@"^\d+$");
                var rgxDeviceID = new Regex(@"^\d+$");

                if (!IsValidIP(tbServer.Text))
                    throw new Exception("Invalid IP address.");

                if (!rgxPort.IsMatch(tbPort.Text))
                    throw new Exception("Invalid port number");

                if (!rgxDeviceID.IsMatch(tbDeviceID.Text))
                    throw new Exception("‌Invalid device ID");

                if (cmbManufacturer.SelectedItem == null)
                    throw new Exception("Select a manufacturer");
                #endregion

                #region connect:
                var ip = tbServer.Text;
                var port = Convert.ToInt32(tbPort.Text);
                _client = new TcpClient();
                _client.Connect(ip, port);
                #endregion

                #region init listener:
                var bw = new BackgroundWorker();
                bw.DoWork += ConnectionListener_DoWork;
                bw.RunWorkerAsync();
                #endregion

                #region set vars:
                _manufacturerId = cmbManufacturer.SelectedValue.ToString();
                _deviceId = tbDeviceID.Text;
                var vm = DataContext as MainWindowVM;
                vm.IsConnected = true;
                #endregion

                AddToLog("The connection to the server established.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region Validation:
                if (cmbCommandType.SelectedItem == null)
                    throw new Exception("Specify command type");
                #endregion

                #region send command:
                if (_client != null && _client.Connected)
                {
                    string command = "";
                    if (cmbCommandType.SelectedIndex < 4) {
                        // create command:
                        var data = tbCommandData.Text.Replace(" ", "");
                        var commandType = typeof(Command).Assembly.GetType(cmbCommandType.SelectedValue.ToString());
                        var commandId = (
                            commandType.Equals(typeof(TerminalLinkCommand)) ? CommandTypes.LK :
                            commandType.Equals(typeof(TerminalPositionCommand)) ? CommandTypes.UD :
                            commandType.Equals(typeof(TerminalBlindSpotCommand)) ? CommandTypes.UD2 :
                            commandType.Equals(typeof(TerminalAlarmPositionCommand)) ? CommandTypes.AL :
                            commandType.Equals(typeof(PlatformUploadIntervalSetCommand)) ? CommandTypes.UPLOAD :
                            "");
                        var dataLength = (string.IsNullOrEmpty(data) ? commandId.Length : data.Length + commandId.Length + 1);
                        command = string.Format("[{0}*{1}*{2}*{3}]",
                                                _manufacturerId.ToUpper(), _deviceId,
                                                dataLength.ToString("x").ToUpper().PadLeft(4, '0'),
                                                commandId.ToUpper() + (string.IsNullOrEmpty(data) ? "" : $",{data}"));
                    } else {
                        command = tbCommandData.Text;
                    }
                    // send to server:
                    var bytes = Encoding.ASCII.GetBytes(command);
                    _client.Client.Send(bytes);
                }
                else
                {
                    throw new Exception("Not Connected!");
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                tbCommandPayload.Text = @"{""Type"":""UPLOAD"", ""TrackerID"":""3G-123456"", ""Payload"":""100""}";
                cmbManufacturer.ItemsSource = Collections.Manufacturers;

                #region commands:
                var commands = new Dictionary<string, string>();
                commands.Add(typeof(TerminalLinkCommand).ToString(), "Terminal Link Message");
                commands.Add(typeof(TerminalPositionCommand).ToString(), "Terminal Location Message");
                commands.Add(typeof(TerminalBlindSpotCommand).ToString(), "Blind Spot Message");
                commands.Add(typeof(TerminalAlarmPositionCommand).ToString(), "Terminal Alarm Message");
                commands.Add("Custom", "Custom Message");

                cmbCommandType.ItemsSource = commands;
                #endregion

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ConnectionListener_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    if (_client != null && _client.Connected)
                    {
                        var buffer = new byte[1024];
                        _client.Client.Receive(buffer);
                        var srvMessage = Encoding.ASCII.GetString(buffer).Replace("\0", "");

                        #region response:
                        var srvCmd = Command.Parse(srvMessage);
                        if (srvCmd != null) {
                            var cmdTypeName = srvCmd.GetType().Name;
                            bool responseNeeded = !cmdTypeName.StartsWith("Terminal");
                            if (responseNeeded) {
                                srvCmd.CommandData = "";

                                if (srvCmd is PlatformGetTerminalVersionCommand)
                                    srvCmd.CommandData = "G29_BASE_V1.00_2014.04.23_17.46.49";
                                else if (srvCmd is PlatformGetTerminalSettingsCommand)
                                    srvCmd.CommandData = "ver:G29_BASE_V1.00_2014.04.24_09.47.23;ID:5678901234;imei:1234SG-56789012345;url:113.81.229.9;port:5900;11center:;slave:;sos1:;sos2:;sos3:;upload:30S;workmode:1;bat level:3;language:1;zone:8.00;GPS:NO(0);GPRS:OK(89);LED:OFF;pw:123456;";

                                srvCmd.ContentLength = srvCmd.CommandID.Length + srvCmd.CommandData.Length + (string.IsNullOrEmpty(srvCmd.CommandData) ? 0 : 1);
                                var res = srvCmd.ToString();
                                _client.Client.Send(Encoding.ASCII.GetBytes(res));
                            }
                        }
                        #endregion

                        Dispatcher.Invoke(() => {
                            AddToLog($"{srvMessage.Replace("\0", "")}");
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private async void btnSendCommand_Click(object sender, RoutedEventArgs e) {
            try {

                var server = tbCommandServer.Text;
                var port = Convert.ToInt32(tbCommandPortNumber.Text);
                var json = tbCommandPayload.Text;

                await Task.Run(() => {
                    using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)) {
                        var bytes = Encoding.ASCII.GetBytes(json);
                        socket.Connect(server, port);
                        socket.Send(bytes);
                        Dispatcher.Invoke(() => {
                            AddToLog($"Command Sent To {tbCommandServer.Text}:{tbCommandPortNumber.Text}, Waiting for the reponse...");
                        });
                        var buffer = new byte[1024];
                        var count = socket.Receive(buffer);
                        var response = Encoding.ASCII.GetString(buffer, 0, count);
                        Dispatcher.Invoke(() => {
                            AddToLog(response);
                        });
                    }
                });

            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Methods:
        private void AddToLog(string message)
        {
            txtLog.Text += $"{message}{Environment.NewLine}";
            txtLogScroll.ScrollToVerticalOffset(int.MaxValue);
        }
        private bool IsValidIP(string ip)
        {
            var rgx = new Regex(@"^\d+\.\d+\.\d+\.\d+$");
            if (!rgx.IsMatch(ip))
                return false;

            var parts = ip.Split('.');
            foreach (var p in parts)
            {
                if (Convert.ToInt32(p) > 255)
                    return false;
            }

            return true;
        }
        #endregion

        private void btntest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var path = @"D:\temp\convert\switch.exe";
                var input = @"D:\temp\convert\sample.wav";
                var outputFolder = @"D:\temp\convert\result";
                var args = $@"-convert -format .amr -outfolder ""{outputFolder}"" -hide -overwrite ALWAYS ""{input}""";
                Process.Start(path, args);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}