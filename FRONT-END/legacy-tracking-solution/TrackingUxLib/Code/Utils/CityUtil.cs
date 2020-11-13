using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TrackingUxLib.Resources.Values;

namespace TrackingUxLib.Code.Utils
{
    public class CityUtil
    {
        #region Public Methods:
        public static Dictionary<int, string> GetProvinces()
        {
            var xml = GetXmlContent();
            var doc = XDocument.Parse(xml);
            var dic = doc.Root.Elements().Select(el => new
            {
                ID = Convert.ToInt32(el.Attribute("ID").Value),
                Name = el.Attribute("Name").Value.ToString()
            }).ToDictionary(p => p.ID, p => p.Name);
            return dic;
        }

        public static Dictionary<int, string> GetProvinceCities(int provId)
        {
            var xml = GetXmlContent();
            var doc = XDocument.Parse(xml);
            var dic = doc.Root.Elements()
                .SingleOrDefault(p => Convert.ToInt32(p.Attribute("ID").Value) == provId)
                .Elements().Select(el => new
                {
                    ID = Convert.ToInt32(el.Attribute("ID").Value),
                    Name = el.Attribute("Name").Value.ToString()
                }).ToDictionary(c => c.ID, c => c.Name);
            return dic;
        }

        public static KeyValuePair<int, string> FindProvinceByCityID(int cityId)
        {
            var xml = GetXmlContent();
            var doc = XDocument.Parse(xml);
            var province = doc.Root.Elements()
                .FirstOrDefault(p => p.Elements().Any(c => Convert.ToInt32(c.Attribute("ID").Value) == cityId));
            return new KeyValuePair<int, string>(Convert.ToInt32(province.Attribute("ID").Value),
                province.Attribute("Name").Value.ToString());
        }

        /// <summary>
        /// Returns city by it's ID, Tuple ::: ProvinceID, CityID, CityName.
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public static Tuple<int, int, string> GetCity(int cityId)
        {
            var xml = GetXmlContent();
            var doc = XDocument.Parse(xml);
            var city = doc.Root.Descendants("City")
                .SingleOrDefault(c => Convert.ToInt32(c.Attribute("ID").Value) == cityId);
            return new Tuple<int, int, string>(Convert.ToInt32(city.Parent.Attribute("ID").Value),
                Convert.ToInt32(city.Attribute("ID").Value), city.Attribute("Name").Value.ToString());
        }
        #endregion

        #region Private Methods:
        private static string GetXmlContent()
        {
            using (var stream = typeof(Strings).Assembly.GetManifestResourceStream("TrackingUxLib.Resources.XML.ir-cities.xml"))
            {
                using (var sr = new StreamReader(stream))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        #endregion
    }
}
