using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackAdmin.Constants {
    public class ProductTypes {
        public const string KIDSWATCH = "kidswatch";
        public const string CARTRACKER = "cartracker";

        public static List<string> All() {
            return new List<string> {
                KIDSWATCH,
                CARTRACKER
            };
        }
    }
}
