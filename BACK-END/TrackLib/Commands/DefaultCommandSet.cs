using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackLib.Constants;

namespace TrackLib.Commands {
    public class DefaultCommandSet : CommandSet {

        public override string Name => SET_DEFAULT;

        public override List<(string CommonName, string NativeCommand)> SupportedCommands => 
            new List<(string CommonName, string NativeCommand)> {

                (COMMAND_SET_UPLOAD_INTERVAL, "UPLOAD"),
                (COMMAND_SET_ADMIN_NUMBER, "CENTER"),
                (COMMAND_SET_SECOND_ADMIN_NUMBER, "SLAVE"),
                (COMMAND_SET_PASSWORD, "PW"),
                (COMMAND_CALL, "CALL"),
                (COMMAND_SMS, "SMS"),
                (COMMAND_CALL_ADMIN, "MONITOR"),
                (COMMAND_SET_SOS_NUMBERS, "SOS"),
                (COMMAND_SET_SOS1, "SOS1"),
                (COMMAND_SET_SOS2, "SOS2"),
                (COMMAND_SET_SOS3, "SOS3"),
                (COMMAND_SET_IP_PORT, "IP"),
                (COMMAND_RESET_FACTORY, "FACTORY"),
                (COMMAND_SET_LANG_ZONE, "LZ"),
                (COMMAND_SWITCH_SOS_SMS, "SOSSMS"),
                (COMMAND_SWITCH_LOWBATTERY_SMS, "LOWBAT"),
                (COMMAND_SET_APN, "APN"),
                (COMMAND_RESTART, "RESET"),
                (COMMAND_WAKE_UP, "CR"),
                (COMMAND_POWEROFF, "POWEROFF"),
                (COMMAND_SWITCH_REMOVE_ALARM, "REMOVE"),
                (COMMAND_SWITCH_STEP_FUNC, "PEDO"),
                (COMMAND_SET_WALKTIME, "WALKTIME"),
                (COMMAND_SET_NO_DISTURBANCE, "SILENCETIME"),
                (COMMAND_FIND, "FIND"),
                (COMMAND_SEND_FLOWERS, "FLOWER"),
                (COMMAND_SET_REMINDER, "REMIND"),
                (COMMAND_SEND_VOICE_MESSAGE, "TK"),
                (COMMAND_SEND_TEXT_MESSAGE, "MESSAGE"),
                (COMMAND_SET_CONTACTS1, "PHB"),
                (COMMAND_SET_CONTACTS2, "PHB2"),
                (COMMAND_SWITCH_BLUETOOTH, "BT"),
                (COMMAND_SWITCH_SMS_FUNC, "SMSONOFF"),
                (COMMAND_SWITCH_AUTO_ANSWER, "GSMANT"),

                (COMMAND_GET_VERSION, "VERNO"),
                (COMMAND_GET_CURRENT_SETTINGS, "TS")

            };

    }
}
