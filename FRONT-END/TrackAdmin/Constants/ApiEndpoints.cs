using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackAdmin.Constants {
    public class ApiEndpoints {
        public const string USERS = "users";
        public const string USERS_SEARCH = "users/search";

        public const string TRACKERS = "trackers";
        public const string TRACKERS_SEARCH = "trackers/search";

        public const string COMMANDS = "commands";
        public const string COMMANDS_CONNECT = "commands/connect";
        public const string COMMANDS_SETS = "commands/sets";
    }
}
