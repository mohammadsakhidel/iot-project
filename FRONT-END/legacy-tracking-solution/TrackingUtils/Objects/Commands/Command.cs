using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrackingUtils.Constants;
using TrackingUtils.Objects.Exceptions;

namespace TrackingUtils.Objects.Commands
{
    public abstract class Command
    {
        #region Ctors:
        public Command(string manufacturer, string deviceId, int contentLength, string commandId, string commandData)
        {
            Manufacturer = manufacturer;
            DeviceID = deviceId;
            ContentLength = contentLength;
            CommandID = commandId;
            CommandData = commandData;
        }
        public Command(string terminalId, string commandId, string commandData)
        {
            var spl = terminalId.Split('_');
            Manufacturer = spl[0];
            DeviceID = spl[1];
            ContentLength = commandId.Length + commandData.Length + (!string.IsNullOrEmpty(commandData) ? 1 : 0);
            CommandID = commandId;
            CommandData = commandData;
        }
        #endregion

        #region Props:
        public string Manufacturer { get; set; } = "3G";
        public string DeviceID { get; set; }
        public int ContentLength { get; set; }
        public string CommandID { get; set; }
        public string CommandData { get; set; }
        public string TerminalID
        {
            get
            {
                return $"{Manufacturer}_{DeviceID}";
            }
        }
        #endregion

        #region Overrides:
        public override string ToString()
        {
            return String.Format("[{0}*{1}*{2}*{3}]",
                Manufacturer.ToUpper(),
                DeviceID,
                ContentLength.ToString("x").PadLeft(4, '0').ToUpper(),
                CommandID.ToUpper() + (string.IsNullOrEmpty(CommandData) ? "" : $",{CommandData}"));
        }
        #endregion

        #region Statics:
        /// <summary>
        /// Converts command text to associated command object.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>returns Command object, null if not supported command.</returns>
        public static Command Parse(string command)
        {
            #region Validate:
            var regex = new Regex(Patterns.terminal_command);
            if (!regex.IsMatch(command))
                return null;
            #endregion

            #region Extract Values:
            command = command.Replace("[", "");
            command = command.Replace("]", "");
            var parts = command.Split('*');
            var manufacturer = parts[0];
            var deviceID = parts[1];
            var contentLength = Convert.ToInt32(parts[2], 16);

            string content = parts[3];
            var commandID = content.Contains(',') ? content.Substring(0, content.IndexOf(',')) : content;
            var commandData = content.Length > commandID.Length
                ? content.Substring(content.IndexOf(',') + 1, content.Length - content.IndexOf(',') - 1)
                : "";
            #endregion

            #region Create Object:
            switch (commandID)
            {
                // terminal commands:
                case CommandTypes.LK:
                    return new TerminalLinkCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.UD:
                    return new TerminalPositionCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.UD2:
                    return new TerminalBlindSpotCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.AL:
                    return new TerminalAlarmPositionCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                // platform commands:
                case CommandTypes.UPLOAD:
                    return new PlatformUploadIntervalSetCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.CENTER:
                    return new PlatformCentralNumberSetCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.PW:
                    return new PlatformPasswordSetCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.CALL:
                    return new PlatformMakeCallCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.MONITOR:
                    return new PlatformMonitorCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.SOS1:
                    return new PlatformSOS1SetCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.SOS2:
                    return new PlatformSOS2SetCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.SOS3:
                    return new PlatformSOS3SetCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.SOS:
                    return new PlatformAllSOSSetCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.IP:
                    return new PlatformIPSetCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.FACTORY:
                    return new PlatformResetFactoryCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.LZ:
                    return new PlatformLanguageZoneSetCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.SOSSMS:
                    return new PlatformSOSSMSSetCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.LOWBAT:
                    return new PlatformLowMessageSetCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.VERNO:
                    return new PlatformGetTerminalVersionCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.TS:
                    return new PlatformGetTerminalSettingsCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.RESET:
                    return new PlatformResetCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.CR:
                    return new PlatformWakeupPositioningCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.POWEROFF:
                    return new PlatformPowerOffCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.REMOVE:
                    return new PlatformRemoveAlarmSetCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.REMOVESMS:
                    return new PlatformRemoveSMSAlarmSetCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.PEDO:
                    return new PlatformStepFunctionSwitchCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.WALKTIME:
                    return new PlatformWalkTimeSetCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.SLEEPTIME:
                    return new PlatformSleepTimeSetCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.SILENCETIME:
                    return new PlatformSilenceTimeSetCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.FIND:
                    return new PlatformFindCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.FLOWER:
                    return new PlatformShowFlowersCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.REMIND:
                    return new PlatformReminderSetCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.TK:
                    return new BidirectionalSendVoiceCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.MESSAGE:
                    return new PlatformSendMessageCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.PHB:
                    return new PlatformPhoneBookFirst5SetCommand(manufacturer, deviceID, contentLength, commandID, commandData);
                case CommandTypes.PHB2:
                    return new PlatformPhoneBookLast5SetCommand(manufacturer, deviceID, contentLength, commandID, commandData);
            }
            #endregion

            return null;
        }
        #endregion

        #region Methods:
        public virtual Command GenerateReply()
        {
            throw new NotImplementedException("GenerateReply Method Not Implemented!");
        }
        #endregion
    }
}
