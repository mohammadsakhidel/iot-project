using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackLib.Constants {
    public class CommandSets {
        public static Dictionary<string, Dictionary<string, string>> All() {

            var allCommandsDic = CommandTypes.All().ToDictionary(k => k, v => v);
            return new Dictionary<string, Dictionary<string, string>> {
                { CommandSetNames.DEFAULT, allCommandsDic }
            };

        }
    }
}
