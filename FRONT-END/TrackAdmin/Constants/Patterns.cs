using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackAdmin.Constants {
    public class Patterns {
        public const string NAME = @"^.{1,32}$";
        public const string EMAIL = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        /// <summary>
        /// Password must include both lowercase and uppercase characters and at least one digit.
        /// </summary>
        public const string PASSWORD = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$";
        /// <summary>
        /// Australian phone number format
        /// </summary>
        public const string PHONE = @"^((\+61\s?)?(\((0|02|03|04|07|08)\))?)?\s?\d{1,4}\s?\d{1,4}\s?\d{0,4}$";
    }
}
