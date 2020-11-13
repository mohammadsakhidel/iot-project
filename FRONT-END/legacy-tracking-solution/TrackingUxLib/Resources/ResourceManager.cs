using TrackingUxLib.Resources.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingUtils.Utils;

namespace TrackingUxLib.Resources
{
    public class ResourceManager
    {
        public static string GetValue(string key, string className = "Strings")
        {
            var resManager = new System.Resources.ResourceManager($"TrackingUxLib.Resources.Values.{className}", typeof(Strings).Assembly);
            return resManager.GetString(key, System.Threading.Thread.CurrentThread.CurrentUICulture);
        }

        public static string GetEnumDisplayText<TEnum>(TEnum enumVal)
        {
            var key = $"{typeof(TEnum).Name}_{enumVal.ToString()}";
            var resManager = new System.Resources.ResourceManager($"TrackingUxLib.Resources.Values.Enums", typeof(Enums).Assembly);
            return resManager.GetString(key, System.Threading.Thread.CurrentThread.CurrentUICulture);
        }

        public static Dictionary<string, string> EnumToDic<TEnum>()
        {
            var dic = new Dictionary<string, string>();
            var values = EnumUtil.GetValues<TEnum>();
            foreach (TEnum enumVal in values)
            {
                dic.Add(enumVal.ToString(), GetEnumDisplayText<TEnum>(enumVal));
            }
            return dic;
        }
    }
}
