using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackAPI.Constants {
    public class Policies {
        #region User Policies:
        public const string CanCreateUser = "CanCreateUser";
        public const string CanReadUser = "CanReadUser";
        public const string CanDeleteUser = "CanDeleteUser";
        public const string CanUpdateUser = "CanUpdateUser";
        #endregion

        #region Tracker Policies:
        public const string CanCreateTracker = "CanCreateTracker";
        public const string CanReadTracker = "CanReadTracker";
        public const string CanDeleteTracker = "CanDeleteTracker";
        public const string CanUpdateTracker = "CanUpdateTracker";
        #endregion
    }
}
