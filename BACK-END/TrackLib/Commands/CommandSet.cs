using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackLib.Constants;

namespace TrackLib.Commands {
    public abstract class CommandSet {

        #region Contants:
        public const string SET_DEFAULT = "default";

        // SET VALUE COMMANDS:
        public const string COMMAND_SET_UPLOAD_INTERVAL = "UPLOAD_INTERVAL";
        public const string COMMAND_SET_ADMIN_NUMBER = "CENTER_NUMBER";
        public const string COMMAND_SET_SECOND_ADMIN_NUMBER = "SLAVE_NUMBER";
        public const string COMMAND_SET_PASSWORD = "PASSWORD";
        public const string COMMAND_CALL = "MAKE_CALL";
        public const string COMMAND_SMS = "SEND_SMS";
        public const string COMMAND_CALL_ADMIN = "MONITOR";
        public const string COMMAND_SET_SOS_NUMBERS = "SOS_NUMBERS";
        public const string COMMAND_SET_SOS1 = "SOS1";
        public const string COMMAND_SET_SOS2 = "SOS2";
        public const string COMMAND_SET_SOS3 = "SOS3";
        public const string COMMAND_SET_IP_PORT = "IP_PORT";
        public const string COMMAND_RESET_FACTORY = "RESET_FACTORY";
        public const string COMMAND_SET_LANG_ZONE = "LANG_ZONE";
        public const string COMMAND_SWITCH_SOS_SMS = "SOS_SMS";
        public const string COMMAND_SWITCH_LOWBATTERY_SMS = "LOWBAT_SMS";
        public const string COMMAND_SET_APN = "APN";
        public const string COMMAND_RESTART = "RESTART";
        public const string COMMAND_WAKE_UP = "WAKEUP";
        public const string COMMAND_POWEROFF = "POWEROFF";
        public const string COMMAND_SWITCH_REMOVE_ALARM = "REMOVE_ALARM";
        public const string COMMAND_SWITCH_STEP_FUNC = "PEDO_FUNC";
        public const string COMMAND_SET_WALKTIME = "WALK_TIME";
        public const string COMMAND_SET_NO_DISTURBANCE = "SILENCE_TIME";
        public const string COMMAND_FIND = "FIND";
        public const string COMMAND_SEND_FLOWERS = "SEND_FLOWERS";
        public const string COMMAND_SET_REMINDER = "REMINDER";
        public const string COMMAND_SEND_VOICE_MESSAGE = "VOICE_MESSAGE";
        public const string COMMAND_SEND_TEXT_MESSAGE = "TEXT_MESSAGE";
        public const string COMMAND_SET_CONTACTS1 = "CONTACTS1";
        public const string COMMAND_SET_CONTACTS2 = "CONTACTS2";
        public const string COMMAND_SWITCH_BLUETOOTH = "BLUETOOTH";
        public const string COMMAND_SWITCH_SMS_FUNC = "SMSONOFF";
        public const string COMMAND_SWITCH_AUTO_ANSWER = "AUTO_ANSWER";

        // GET VALUE COMMANDS
        public const string COMMAND_GET_VERSION = "VERSION";
        public const string COMMAND_GET_CURRENT_SETTINGS = "CURRENT_SETTINGS";
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
            return name switch {
                SET_DEFAULT => sp.GetService(typeof(DefaultCommandSet)) as CommandSet,
                _ => null
            };
        }

        public static List<CommandSet> GetAllSets(IServiceProvider sp) {
            return new List<CommandSet> {
                Get(CommandSet.SET_DEFAULT, sp)
            };
        }

        public static List<string> GetAllPostCommands() {
            return new List<string> {
                COMMAND_SET_UPLOAD_INTERVAL, // Sets the interval of oploads
                COMMAND_SET_ADMIN_NUMBER, // Center number is the number that can send sms commands to the tracker
                COMMAND_SET_SECOND_ADMIN_NUMBER, // Sets the secondary center number
                COMMAND_SET_PASSWORD, // Set tracker's password
                COMMAND_CALL, // Order the tracker to call a number
                COMMAND_SMS, // Order the tracker to send an SMS to a number
                COMMAND_SEND_FLOWERS, // Sends a flower to the tracker
                COMMAND_SEND_TEXT_MESSAGE, // Sends a text message to the tracker
                COMMAND_CALL_ADMIN, // Orders the tracker to call the center number
                COMMAND_SET_SOS_NUMBERS, // Sets all SOS numbers on the tracker
                COMMAND_SET_SOS1,
                COMMAND_SET_SOS2,
                COMMAND_SET_SOS3,
                COMMAND_SET_IP_PORT, // Sets tracket's IP and PORT
                COMMAND_RESET_FACTORY, // Resets to factory settings
                COMMAND_SET_LANG_ZONE, // Sets language and time zone
                COMMAND_SWITCH_SMS_FUNC, // Switches all smss on or off on the device
                COMMAND_SWITCH_AUTO_ANSWER, // Switches automatic answering on or off
                COMMAND_SWITCH_SOS_SMS, // Payload = {0,1} switches wether send SMS for SOS numbers for alarms
                COMMAND_SWITCH_LOWBATTERY_SMS, // Payload = {0,1} switches wether send SMS for center number when battery is low
                COMMAND_SWITCH_REMOVE_ALARM, // Switches the remove watch alarm
                COMMAND_SET_APN, // Payload = {APN name,user,password,userdata} e.g. APN,cmnet,,,46000
                COMMAND_RESTART, // Restarts the tracker
                COMMAND_WAKE_UP, // Wakes up the tracker to send GPS data constantly for 3 minutes
                COMMAND_SWITCH_BLUETOOTH, // Switches tracker's blutooth on or off
                COMMAND_POWEROFF, // Shuts down the tracker
                COMMAND_SWITCH_STEP_FUNC, // Payload = {0,1} Switches trackers step function
                COMMAND_SET_WALKTIME, // The device only counts steps within the walk time, payload = {HH:mm-HH:mm,HH:mm-HH:mm,HH:mm-HH:mm}
                COMMAND_SET_NO_DISTURBANCE, // Disables incoming calls within set periods. payload = {HH:mm-HH:mm,HH:mm-HH:mm,HH:mm-HH:mm,HH:mm-HH:mm}
                COMMAND_FIND, // the lost tracker rings for one minute
                COMMAND_SET_REMINDER, // Sets alarm clock. Format: time-switch-how often(1： Once； 2:every day;3: self defaulted) e.g. REMIND,08:10-1-1,08:10-1-2, 08:10-1-3-0111110
                COMMAND_SEND_VOICE_MESSAGE, // Send audio message to the tracker with AMR format. consider the bytes convrsion in the document
                COMMAND_SET_CONTACTS1,
                COMMAND_SET_CONTACTS2 // Sets two sets of 5 of contacts. format is PHB,phone,name{5}
            };
        }

        public static List<string> GetAllQueryCommands() {
            return new List<string> {
                COMMAND_GET_VERSION,
                COMMAND_GET_CURRENT_SETTINGS
            };
        }

        public static List<string> GetAllCommands() {
            return GetAllPostCommands()
                .Concat(GetAllQueryCommands())
                .ToList();
        }
        #endregion
    }
}
