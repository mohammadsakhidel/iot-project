using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackLib.Constants;

namespace TrackLib.Commands {
    public class GpsWatchCommandSet : CommandSet {

        public override string Name => SET_GPSWATCH;

        public override List<(string CommonName, string NativeCommand)> SupportedCommands => 
            new List<(string CommonName, string NativeCommand)> {

                (COMMAND_UPLOAD_INTERVAL, "UPLOAD"),
                (COMMAND_CENTER_NUMBER, "CENTER"),
                (COMMAND_SECOND_CENTER_NUMBER, "SLAVE"),
                (COMMAND_PASSWORD, "PW"),
                (COMMAND_MAKE_CALL, "CALL"),
                (COMMAND_SEND_SMS, "SMS"),
                (COMMAND_CALL_CENTER, "MONITOR"),
                (COMMAND_SOS_NUMBERS, "SOS"),
                (COMMAND_SOS_FIRST, "SOS1"),
                (COMMAND_SOS_SECOND, "SOS2"),
                (COMMAND_SOS_THIRD, "SOS3"),
                (COMMAND_IP_PORT, "IP"),
                (COMMAND_RESET_FACTORY, "FACTORY"),
                (COMMAND_LANG_ZONE, "LZ"),
                (COMMAND_SOS_SMS_ALARM, "SOSSMS"),
                (COMMAND_LOWBATTERY_SMS_ALARM, "LOWBAT"),
                (COMMAND_APN, "APN"),
                (COMMAND_RESTART, "RESET"),
                (COMMAND_WAKEUP, "CR"),
                (COMMAND_POWEROFF, "POWEROFF"),
                (COMMAND_REMOVE_ALARM, "REMOVE"),
                (COMMAND_PEDO_FUNC, "PEDO"),
                (COMMAND_WALK_TIME, "WALKTIME"),
                (COMMAND_NO_DISTURBANCE, "SILENCETIME"),
                (COMMAND_FIND, "FIND"),
                (COMMAND_SEND_FLOWERS, "FLOWER"),
                (COMMAND_REMINDER, "REMIND"),
                (COMMAND_VOICE_MESSAGE, "TK"),
                (COMMAND_TEXT_MESSAGE, "MESSAGE"),
                (COMMAND_CONTACTS1, "PHB"),
                (COMMAND_CONTACTS2, "PHB2"),
                (COMMAND_BLUETOOTH, "BT"),
                (COMMAND_SWITCH_SMS, "SMSONOFF"),
                (COMMAND_SWITCH_AUTO_ANSWER, "GSMANT"),

                (COMMAND_GET_STATUS, "VERNO"),
                (COMMAND_GET_VERSION, "VERNO"),
                (COMMAND_GET_CURRENT_SETTINGS, "TS")

            };

    }
}
