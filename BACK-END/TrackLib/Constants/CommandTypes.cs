using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackLib.Constants {
    public class CommandTypes {

        #region Command Constants:
        // SET VALUE COMMANDS:
        private const string SET_UPLOAD_INTERVAL = "UPLOAD";
        private const string SET_ADMIN_NUMBER = "CENTER";
        private const string SET_SECOND_ADMIN_NUMBER = "SLAVE";
        private const string SET_PASSWORD = "PW";
        private const string CALL = "CALL";
        private const string SMS = "SMS";
        private const string CALL_ADMIN = "MONITOR";
        private const string SET_SOS_NUMBERS = "SOS";
        private const string SET_SOS1 = "SOS1";
        private const string SET_SOS2 = "SOS2";
        private const string SET_SOS3 = "SOS3";
        private const string SET_IP_PORT = "IP";
        private const string RESET_FACTORY = "FACTORY";
        private const string SET_LANG_ZONE = "LZ";
        private const string SWITCH_SOS_SMS = "SOSSMS";
        private const string SWITCH_LOWBATTERY_SMS = "LOWBAT";
        private const string SET_APN = "APN";
        private const string RESTART = "RESET";
        private const string WAKE_UP = "CR";
        private const string POWEROFF = "POWEROFF";
        private const string SWITCH_REMOVE_ALARM = "REMOVE";
        private const string SWITCH_STEP_FUNC = "PEDO";
        private const string SET_WALKTIME = "WALKTIME";
        private const string SET_NO_DISTURBANCE = "SILENCETIME";
        private const string FIND = "FIND";
        private const string SEND_FLOWERS = "FLOWER";
        private const string SET_REMINDER = "REMIND";
        private const string SEND_VOICE_MESSAGE = "TK";
        private const string SEND_TEXT_MESSAGE = "MESSAGE";
        private const string SET_CONTACTS1 = "PHB";
        private const string SET_CONTACTS2 = "PHB2";
        private const string SWITCH_BLUETOOTH = "BT";
        private const string SWITCH_SMS_FUNC = "SMSONOFF";
        private const string SWITCH_AUTO_ANSWER = "GSMANT";

        // GET VALUE COMMANDS
        private const string GET_VERSION = "VERNO";
        private const string GET_CURRENT_SETTINGS = "TS";
        #endregion

        public static List<string> All() {
            return AllSendValueCommands().Concat(AllQueryCommands()).ToList();
        }

        public static List<string> AllSendValueCommands() {
            return new List<string> {
                CommandTypes.SET_UPLOAD_INTERVAL, // Sets the interval of oploads
                CommandTypes.SET_ADMIN_NUMBER, // Center number is the number that can send sms commands to the tracker
                CommandTypes.SET_SECOND_ADMIN_NUMBER, // Sets the secondary center number
                CommandTypes.SET_PASSWORD, // Set tracker's password
                CommandTypes.CALL, // Order the tracker to call a number
                CommandTypes.SMS, // Order the tracker to send an SMS to a number
                CommandTypes.SEND_FLOWERS, // Sends a flower to the tracker
                CommandTypes.SEND_TEXT_MESSAGE, // Sends a text message to the tracker
                CommandTypes.CALL_ADMIN, // Orders the tracker to call the center number
                CommandTypes.SET_SOS_NUMBERS, // Sets all SOS numbers on the tracker
                CommandTypes.SET_SOS1, CommandTypes.SET_SOS2, CommandTypes.SET_SOS3,
                CommandTypes.SET_IP_PORT, // Sets tracket's IP and PORT
                CommandTypes.RESET_FACTORY, // Resets to factory settings
                CommandTypes.SET_LANG_ZONE, // Sets language and time zone
                CommandTypes.SWITCH_SMS_FUNC, // Switches all smss on or off on the device
                CommandTypes.SWITCH_AUTO_ANSWER, // Switches automatic answering on or off
                CommandTypes.SWITCH_SOS_SMS, // Payload = {0,1} switches wether send SMS for SOS numbers for alarms
                CommandTypes.SWITCH_LOWBATTERY_SMS, // Payload = {0,1} switches wether send SMS for center number when battery is low
                CommandTypes.SWITCH_REMOVE_ALARM, // Switches the remove watch alarm
                CommandTypes.SET_APN, // Payload = {APN name,user,password,userdata} e.g. APN,cmnet,,,46000
                CommandTypes.RESTART, // Restarts the tracker
                CommandTypes.WAKE_UP, // Wakes up the tracker to send GPS data constantly for 3 minutes
                CommandTypes.SWITCH_BLUETOOTH, // Switches tracker's blutooth on or off
                CommandTypes.POWEROFF, // Shuts down the tracker
                CommandTypes.SWITCH_STEP_FUNC, // Payload = {0,1} Switches trackers step function
                CommandTypes.SET_WALKTIME, // The device only counts steps within the walk time, payload = {HH:mm-HH:mm,HH:mm-HH:mm,HH:mm-HH:mm}
                CommandTypes.SET_NO_DISTURBANCE, // Disables incoming calls within set periods. payload = {HH:mm-HH:mm,HH:mm-HH:mm,HH:mm-HH:mm,HH:mm-HH:mm}
                CommandTypes.FIND, // the lost tracker rings for one minute
                CommandTypes.SET_REMINDER, // Sets alarm clock. Format: time-switch-how often(1： Once； 2:every day;3: self defaulted) e.g. REMIND,08:10-1-1,08:10-1-2, 08:10-1-3-0111110
                CommandTypes.SEND_VOICE_MESSAGE, // Send audio message to the tracker with AMR format. consider the bytes convrsion in the document
                CommandTypes.SET_CONTACTS1, CommandTypes.SET_CONTACTS2 // Sets two sets of 5 of contacts. format is PHB,phone,name{5}
            };
        }

        public static List<string> AllQueryCommands() {
            return new List<string> { 
                CommandTypes.GET_VERSION,
                CommandTypes.GET_CURRENT_SETTINGS
            };
        }
    }
}
