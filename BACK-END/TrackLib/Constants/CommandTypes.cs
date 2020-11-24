using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackLib.Constants {
    public class CommandTypes {
        public const string LK = "LK";
        public const string UD = "UD";
        public const string UD2 = "UD2";
        public const string AL = "AL";
        public const string UPLOAD = "UPLOAD";
        public const string CENTER = "CENTER";
        public const string SLAVE = "SLAVE";
        public const string PW = "PW";
        public const string CALL = "CALL";
        public const string SMS = "SMS";
        public const string MONITOR = "MONITOR";
        public const string SOS = "SOS";
        public const string SOS1 = "SOS1";
        public const string SOS2 = "SOS2";
        public const string SOS3 = "SOS3";
        public const string IP = "IP";
        public const string FACTORY = "FACTORY";
        public const string LZ = "LZ";
        public const string SOSSMS = "SOSSMS";
        public const string LOWBAT = "LOWBAT";
        public const string APN = "APN";
        public const string VERNO = "VERNO";
        public const string RESET = "RESET";
        public const string CR = "CR";
        public const string POWEROFF = "POWEROFF";
        public const string REMOVE = "REMOVE";
        public const string REMOVESMS = "REMOVESMS";
        public const string PEDO = "PEDO";
        public const string WALKTIME = "WALKTIME";
        public const string SLEEPTIME = "SLEEPTIME";
        public const string SILENCETIME = "SILENCETIME";
        public const string FIND = "FIND";
        public const string FLOWER = "FLOWER";
        public const string REMIND = "REMIND";
        public const string TK = "TK";
        public const string MESSAGE = "MESSAGE";
        public const string PHB = "PHB";
        public const string PHB2 = "PHB2";
        public const string BT = "BT";
        public const string SMSONOFF = "SMSONOFF";
        public const string GSMANT = "GSMANT";

        public static List<string> AllSendValueCommands() {
            return new List<string> {
                CommandTypes.UPLOAD, // Sets the interval of oploads
                CommandTypes.CENTER, // Center number is the number that can send sms commands to the tracker
                CommandTypes.SLAVE, // Sets the secondary center number
                CommandTypes.PW, // Set tracker's password
                CommandTypes.CALL, // Order the tracker to call a number
                CommandTypes.SMS, // Order the tracker to send an SMS to a number
                CommandTypes.FLOWER, // Sends a flower to the tracker
                CommandTypes.MESSAGE, // Sends a text message to the tracker
                CommandTypes.MONITOR, // Orders the tracker to call the center number
                CommandTypes.SOS, // Sets all SOS numbers on the tracker
                CommandTypes.SOS1, CommandTypes.SOS2, CommandTypes.SOS3,
                CommandTypes.IP, // Sets tracket's IP and PORT
                CommandTypes.FACTORY, // Resets to factory settings
                CommandTypes.LZ, // Sets language and time zone
                CommandTypes.SMSONOFF, // Switches all smss on or off on the device
                CommandTypes.GSMANT, // Switches automatic answering on or off
                CommandTypes.SOSSMS, // Payload = {0,1} switches wether send SMS for SOS numbers for alarms
                CommandTypes.LOWBAT, // Payload = {0,1} switches wether send SMS for center number when battery is low
                CommandTypes.REMOVE, // Switches the remove watch alarm
                CommandTypes.APN, // Payload = {APN name,user,password,userdata} e.g. APN,cmnet,,,46000
                CommandTypes.RESET, // Restarts the tracker
                CommandTypes.CR, // Wakes up the tracker to send GPS data constantly for 3 minutes
                CommandTypes.BT, // Switches tracker's blutooth on or off
                CommandTypes.POWEROFF, // Shuts down the tracker
                CommandTypes.PEDO, // Payload = {0,1} Switches trackers step function
                CommandTypes.WALKTIME, // The device only counts steps within the walk time, payload = {HH:mm-HH:mm,HH:mm-HH:mm,HH:mm-HH:mm}
                CommandTypes.SILENCETIME, // Disables incoming calls within set periods. payload = {HH:mm-HH:mm,HH:mm-HH:mm,HH:mm-HH:mm,HH:mm-HH:mm}
                CommandTypes.FIND, // the lost tracker rings for one minute
                CommandTypes.REMIND, // Sets alarm clock. Format: time-switch-how often(1： Once； 2:every day;3: self defaulted) e.g. REMIND,08:10-1-1,08:10-1-2, 08:10-1-3-0111110
                CommandTypes.TK, // Send audio message to the tracker with AMR format. consider the bytes convrsion in the document
                CommandTypes.PHB, CommandTypes.PHB2 // Sets two sets of 5 of contacts. format is PHB,phone,name{5}
            };
        }

    }
}
