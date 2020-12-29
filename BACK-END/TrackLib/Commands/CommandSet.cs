using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TrackLib.Constants;

namespace TrackLib.Commands {
    public abstract class CommandSet {

        #region Contants:
        // COMMAND SET NAMES:
        public const string SET_GPSWATCH = "gpswatch";

        // SET VALUE COMMANDS:
        public const string COMMAND_UPLOAD_INTERVAL = "UPLOAD_INTERVAL";
        public const string COMMAND_CENTER_NUMBER = "CENTER_NUMBER";                    // Center number is the number that can send sms commands to the tracker
        public const string COMMAND_SECOND_CENTER_NUMBER = "SECOND_CENTER_NUMBER";
        public const string COMMAND_PASSWORD = "PASSWORD";
        public const string COMMAND_MAKE_CALL = "MAKE_CALL";
        public const string COMMAND_SEND_SMS = "SEND_SMS";
        public const string COMMAND_CALL_CENTER = "CALL_CENTER";                        // Orders the tracker to call the center number
        public const string COMMAND_SOS_NUMBERS = "SOS_NUMBERS";
        public const string COMMAND_SOS_FIRST = "SOS_FIRST";
        public const string COMMAND_SOS_SECOND = "SOS_SECOND";
        public const string COMMAND_SOS_THIRD = "SOS_THIRD";
        public const string COMMAND_IP_PORT = "IP_PORT";
        public const string COMMAND_RESET_FACTORY = "RESET_FACTORY";
        public const string COMMAND_LANG_ZONE = "LANG_ZONE";
        public const string COMMAND_SOS_SMS_ALARM = "SOS_SMS_ALARM";                    // Payload = {0,1} switches wether send SMS for SOS numbers for alarms
        public const string COMMAND_LOWBATTERY_SMS_ALARM = "LOWBATTERY_SMS_ALARM";      // Payload = {0,1} switches wether send SMS for center number when battery is low
        public const string COMMAND_APN = "APN";
        public const string COMMAND_RESTART = "RESTART";
        public const string COMMAND_WAKEUP = "WAKEUP";                                  // Wakes up the tracker to send GPS data constantly for 3 minutes
        public const string COMMAND_POWEROFF = "POWEROFF";
        public const string COMMAND_REMOVE_ALARM = "REMOVE_ALARM";
        public const string COMMAND_PEDO_FUNC = "PEDO_FUNC";
        public const string COMMAND_WALK_TIME = "WALK_TIME";                            // The device only counts steps within the walk time, payload = {HH:mm-HH:mm,HH:mm-HH:mm,HH:mm-HH:mm}
        public const string COMMAND_NO_DISTURBANCE = "NO_DISTURBANCE";                  // Disables incoming calls within set periods. payload = {HH:mm-HH:mm,HH:mm-HH:mm,HH:mm-HH:mm,HH:mm-HH:mm}
        public const string COMMAND_FIND = "FIND";
        public const string COMMAND_SEND_FLOWERS = "SEND_FLOWERS";
        public const string COMMAND_REMINDER = "REMINDER";                              // Sets alarm clock. Format: time-switch-how often(1： Once； 2:every day;3: self defaulted) e.g. REMIND,08:10-1-1,08:10-1-2, 08:10-1-3-0111110
        public const string COMMAND_VOICE_MESSAGE = "VOICE_MESSAGE";                    // Send audio message to the tracker with AMR format. consider the bytes convrsion in the document
        public const string COMMAND_TEXT_MESSAGE = "TEXT_MESSAGE";
        public const string COMMAND_CONTACTS1 = "CONTACTS1";                            // Sets two sets of 5 of contacts. format is PHB,phone,name{5}
        public const string COMMAND_CONTACTS2 = "CONTACTS2";
        public const string COMMAND_BLUETOOTH = "BLUETOOTH";
        public const string COMMAND_SWITCH_SMS = "SWITCH_SMS";                          // Switches all smss on or off on the device
        public const string COMMAND_SWITCH_AUTO_ANSWER = "SWITCH_AUTO_ANSWER";          // Switches automatic answering on or off

        // GET VALUE COMMANDS
        public const string COMMAND_GET_STATUS = "GET_STATUS";
        public const string COMMAND_GET_VERSION = "GET_VERSION";
        public const string COMMAND_GET_CURRENT_SETTINGS = "GET_CURRENT_SETTINGS";
        #endregion

        #region Properties:
        public abstract string Name { get; }

        /// <summary>
        /// Returns the list of supported commands' common name.
        /// </summary>
        public abstract List<(string CommonName, string NativeCommand)> SupportedCommands { get; }

        public string this[string commonName] {
            get {
                if (!IsCommandSupported(commonName))
                    throw new ArgumentException("The specified command is not supported by this command set.");

                return SupportedCommands
                    .Where(c => c.CommonName == commonName)
                    .Select(c => c.NativeCommand)
                    .FirstOrDefault();
            }
        }
        #endregion

        #region Methods:
        public bool IsCommandSupported(string commonName) {
            return SupportedCommands.Any(c => c.CommonName == commonName);
        }
        #endregion

        #region Static Member:
        public static CommandSet Get(string name, IServiceProvider sp) {
            var set = name switch {
                SET_GPSWATCH => sp.GetService(typeof(GpsWatchCommandSet)) as CommandSet,
                _ => null
            };
            return set;
        }

        public static List<CommandSet> GetAllSets(IServiceProvider sp) {
            return new List<CommandSet> {
                Get(SET_GPSWATCH, sp)
            };
        }
        #endregion

    }
}
