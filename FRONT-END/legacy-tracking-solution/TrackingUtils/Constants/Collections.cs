using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingUtils.Constants
{
    public static class Collections
    {
        public static Dictionary<string, string> Manufacturers
        {
            get
            {
                var d = new Dictionary<string, string>();
                d.Add("3G", "3G Electronics");
                return d;
            }
        }
        public static Dictionary<string, string> Marketers
        {
            get
            {
                var d = new Dictionary<string, string>();
                d.Add("behrang", "Behrang Chenaghlou");
                d.Add("moradi", "Roya Moradi");
                d.Add("habibpour", "Ramin Habibpour");
                d.Add("saray", "Reza Saray");
                d.Add("javani", "Ali Javani");
                d.Add("none", "-----None-----");
                return d;
            }
        }
        public static Dictionary<string, string> ApiClients
        {
            get
            {
                var dic = new Dictionary<string, string>();
                dic.Add("sroSVqFq9Y", "rLVX7PCdTWnYBzuF5T9qrPCT9meRV2wL");
                //Authorization: Basic c3JvU1ZxRnE5WTpyTFZYN1BDZFRXbllCenVGNVQ5cXJQQ1Q5bWVSVjJ3TA==
                return dic;
            }
        }
    }
}
