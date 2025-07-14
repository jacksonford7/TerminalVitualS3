using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using csl_log;

namespace CSLSite.XmlTool
{
    public class xmlHelper
    {
        public static string SerializeAsString<T>(T tipo, out Int64 ticket)
        {
            Int64 tic = 0;
            XmlSerializer ser = new XmlSerializer(typeof(T));
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");
            StringBuilder texto = new StringBuilder();
            try
            {
                using (var m = new MemoryStream())
                {
                    ser.Serialize(m, tipo, namespaces);
                    m.Position = 0;
                    using (var z = new StreamReader(m))
                    {
                        texto.Append(z.ReadToEnd());
                    }
                    var f = String.Join("\n", texto.ToString().Split('\n').Skip(1).ToArray());
                    texto.Clear();
                    texto.AppendLine("<argo:snx xmlns:argo=\"http://www.navis.com/argo\" >");
                    texto.Append(CleanString(f));
                    texto.AppendLine("</argo:snx>");

                   }
            }
            catch(Exception ex)
            {
                tic= log_csl.save_log<Exception>(ex, "xmlHelper", "SerializeAsString", tipo!=null?tipo.GetType().ToString():"El tipo es null", "N4");
                texto = null;
            }
            ticket = tic;
            return texto.ToString();
        }
        public static T DesSerializeAsT<T>(string inputString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringReader rdr = new StringReader(inputString);
            T resultingMessage = (T)serializer.Deserialize(rdr);
            rdr.Dispose();
            return resultingMessage;
        }
        public static string CleanString(string salir)
        {
            Regex replace_a_Accents = new Regex("[á|à|ä|â]", RegexOptions.Compiled);
            Regex replace_e_Accents = new Regex("[é|è|ë|ê]", RegexOptions.Compiled);
            Regex replace_i_Accents = new Regex("[í|ì|ï|î]", RegexOptions.Compiled);
            Regex replace_o_Accents = new Regex("[ó|ò|ö|ô]", RegexOptions.Compiled);
            Regex replace_u_Accents = new Regex("[ú|ù|ü|û]", RegexOptions.Compiled);
            salir = Regex.Replace(salir, @"\&#xA", string.Empty);
            salir = Regex.Replace(salir, "ñ", "n");
            salir = Regex.Replace(salir, "&#xA", string.Empty);
            salir = Regex.Replace(salir, @"\&#xA;", string.Empty);
            salir = Regex.Replace(salir, "&#xA;", string.Empty);
            salir = Regex.Replace(salir, "&amp;", " and ");
            salir = Regex.Replace(salir, "&", " and ");
            salir = Regex.Replace(salir, "#", " No. ");
            salir = Regex.Replace(salir, "Ñ", "N");
            salir = replace_a_Accents.Replace(salir, "a");
            salir = replace_e_Accents.Replace(salir, "e");
            salir = replace_i_Accents.Replace(salir, "i");
            salir = replace_o_Accents.Replace(salir, "o");
            salir = replace_u_Accents.Replace(salir, "u");
            return salir;
        }
        public static string ICUSetProperty(string carriervisit, string unit, string propiedad, string valor)
        {
            StringBuilder texto = new StringBuilder();
            texto.AppendLine("<icu><units>");
            texto.AppendFormat("<unit-identity id=\"{0}\" type=\"CONTAINERIZED\" ><carrier direction=\"IB\" mode=\"VESSEL\" id=\"{1}\" /></unit-identity></units>", unit, carriervisit);
            texto.AppendFormat("<properties>");
            texto.AppendFormat("<property tag=\"{0}\" value=\"{1}\"/>", propiedad,valor);
            texto.AppendLine("</properties></icu>");
            return texto.ToString();
        }
    }
}