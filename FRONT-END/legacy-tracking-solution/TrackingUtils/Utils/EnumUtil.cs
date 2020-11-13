using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingUtils.Utils
{
    public class EnumUtil
    {
        public static TEnum GetEnum<TEnum>(string value)
        {
            return (TEnum)Enum.Parse(typeof(TEnum), value);
        }

        public static List<TEnum> GetValues<TEnum>()
        {
            var list = new List<TEnum>();
            foreach (TEnum enumValue in Enum.GetValues(typeof(TEnum)))
            {
                list.Add(enumValue);
            }
            return list;
        }
    }
}
