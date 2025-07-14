using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;

using System.Xml;

namespace ClsOrdenesP2D
{
    public class ToStringBase
    {

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                StringEscapeHandling = StringEscapeHandling.EscapeHtml
            });
        }

        public static T FromString<T>(string json)
        {
            var sett = new JsonSerializerSettings();
            sett.NullValueHandling = NullValueHandling.Ignore;
            sett.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            sett.StringEscapeHandling = StringEscapeHandling.EscapeHtml;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(json);
                json = JsonConvert.SerializeXmlNode(doc);
            }
            catch
            {
                json = json.Trim();
            }

            var obj = JsonConvert.DeserializeObject<T>(json, sett);
            return obj;
        }

        public static List<T> ListFromString<T>(string json)
        {
            var sett = new JsonSerializerSettings();
            sett.NullValueHandling = NullValueHandling.Ignore;
            sett.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            sett.StringEscapeHandling = StringEscapeHandling.EscapeHtml;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(json);
                json = JsonConvert.SerializeXmlNode(doc);
            }
            catch
            {
                json = json.Trim();
            }
            var obj = JsonConvert.DeserializeObject<List<T>>(json, sett);
            return obj;
        }

        public static object FromString(string json)
        {
            var sett = new JsonSerializerSettings();
            sett.NullValueHandling = NullValueHandling.Ignore;
            sett.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            sett.StringEscapeHandling = StringEscapeHandling.EscapeHtml;
            var obj = JsonConvert.DeserializeObject(json, sett);
            return obj;
        }

    }
}
