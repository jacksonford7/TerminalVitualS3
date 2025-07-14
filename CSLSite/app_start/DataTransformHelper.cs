using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using csl_log;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;
using System.Data;
using System.Xml;

namespace CSLSite
{
    public class DataTransformHelper
    {
        //metodo que convertira cualquier clase .NET a Json valido para el form
        public static string SerializeObjectIntoJson<T>(T objeto) 
        {
            var mensaje = string.Empty;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(objeto.GetType());
            try
            {
                 using (MemoryStream ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, objeto);
                    ms.Flush(); byte[] bytes = ms.GetBuffer();
                    string jsonString = Encoding.UTF8.GetString(bytes, 0, bytes.Length).Trim('\0');
                    return jsonString;
                }
            }
            catch (IOException ex)
            {
                var ticket = log_csl.save_log<Exception>(ex, "DataTransformHelper", "SerializeObjectIntoJson", objeto.GetType().ToString(), "usuario");
                if (HttpContext.Current.Cache["display"] != null)
                {
                    mensaje = string.Format(HttpContext.Current.Cache["display"].ToString(), "\\n-CODIGO IO00ID", ticket + "-\\n");
                }
                else
                {
                    mensaje = string.Format("Comunique a CGSA \\n-CODIGO IO00ID{0}\\", ticket);
                }
                return jsonParserError(mensaje, string.Empty, false);
            }
            catch (Exception ex)
            {
               var ticket = log_csl.save_log<Exception>(ex, "DataTransformHelper", "SerializeObjectIntoJson", objeto.GetType().ToString(), "usuario");
               if (HttpContext.Current.Cache["display"] != null)
               {
                   mensaje = string.Format(HttpContext.Current.Cache["display"].ToString(), "\\n-CODIGO E00ID", ticket + "-\\n");
               }
               else
               {
                   mensaje = string.Format("Comunique a CGSA \\n-CODIGO E00ID{0}\\", ticket);
               }
                return jsonParserError(mensaje, string.Empty, false);
            }
        }

        //parsea el error a json
        public static string jsonParserError(string mensaje, string data, bool estatus = false)
        {
            string msge = "{{\"mensaje\": \"{0}\",  \"resultado\": {1},\"data\": \"{2}\"  }}";
            return string.Format(msge, removeStringAtacks(mensaje,null),data,  estatus.ToString().ToLower());
        }

        //limpia todas las propiedades de un objeto de posibles ataques
        public static void  CleanProperties<T>(T o)
        {
            foreach (MemberInfo mi in o.GetType().GetMembers())
            {
                try
                {
                    if (mi.MemberType == MemberTypes.Property)
                    {
                        PropertyInfo pi = mi as PropertyInfo;
                        if (pi != null)
                        {
                            if (pi.PropertyType.Name.ToLower().Contains("string"))
                            {
                                string antes = pi.GetValue(o, null) as string;
                                if (!string.IsNullOrEmpty(antes))
                                {
                                    var r = removeStringAtacks(antes, null);
                                    pi.SetValue(o, r, null);
                                }
                            }
                        }
                    }
                }
                catch
                {
                    continue;
                }
            }
      }

        //limpia las cadenas de string
        public static string removeStringAtacks( string salir, Func<string, string> errorMethod , string usuario = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(salir))
                {
                    DateTime fecha;
                    if (!DateTime.TryParse(salir, out fecha))
                    {
                        salir = salir.Replace("/", "-");
                    }
                    salir = salir.Replace("[", "(");
                    salir = salir.Replace("]", ")");
                    Regex replace_vars = new Regex(@"[&|*|?|""""|'|#|%|<|>|¡|{|}|~|\|!|¿]", RegexOptions.Compiled);
                    salir = replace_vars.Replace(salir, string.Empty);
                    //nuevo eliminar XML_MALOS
                    string re = @"[^\x09\x0A\x0D\x20-\uD7FF\uE000-\uFFFD\u10000-\u10FFFF]";
                    salir = Regex.Replace(salir, re,string.Empty);
                }
                return !string.IsNullOrEmpty(salir) ? salir.Trim().ToUpper() : null;
            }
            catch (Exception ex)
            {
                if (errorMethod != null)
                {
                    errorMethod(ex.Message);
                }
                log_csl.save_log<Exception>(ex, "DataTransformHelper", "removeStringAtacks", salir, usuario);
                return string.Format("Se produjo una problema de conversión de data.");
            }

        }

        //validador de iso de container.
        public static bool ContainerISOValidate(string container, ref string mensaje)
        {
            if (string.IsNullOrEmpty(container))
            {
                mensaje = "Cadena vacía";
                return false;
            }
            double result = 0;
            var numcont = container.Trim();
            var verficador = container.Substring(numcont.Length - 1);
            //valida el largo de la cadena
            if (numcont.Length > 0 && numcont.Length != 11)
            {
                mensaje = "El número debe tener 11 caracteres";
                return false;
            }
            //valida el verficador
            double x = 0;
            double v = 0;
            if (!double.TryParse(verficador, out x))
            {
                mensaje = string.Format("El verificador [{0}] incorrecto!", verficador);
                return false;
            }
            v = x;
            try
            {
                var param = new Dictionary<string, int>();
                param.Add("A", 10);
                param.Add("B", 12);
                param.Add("C", 13);
                param.Add("D", 14);
                param.Add("E", 15);
                param.Add("F", 16);
                param.Add("G", 17);
                param.Add("H", 18);
                param.Add("I", 19);
                param.Add("J", 20);
                param.Add("K", 21);
                param.Add("L", 23);
                param.Add("M", 24);
                param.Add("N", 25);
                param.Add("O", 26);
                param.Add("P", 27);
                param.Add("Q", 28);
                param.Add("R", 29);
                param.Add("S", 30);
                param.Add("T", 31);
                param.Add("U", 32);
                param.Add("V", 34);
                param.Add("W", 35);
                param.Add("X", 36);
                param.Add("Y", 37);
                param.Add("Z", 38);
                var potencia = 0;
                var xchar = numcont.Split();
                if (double.TryParse(xchar[0], out x) || double.TryParse(xchar[1], out x) || double.TryParse(xchar[2], out x))
                {
                    mensaje = "Propietario inválido";
                    return false;
                }
                if (xchar[3].ToLower() != "u" && xchar[3].ToLower() != "z" && xchar[3].ToLower() != "j")
                {
                    mensaje = "Tipo de equipo inválido";
                    return false;
                }
                double sumador = 0;
                for (int i = 0; i <= xchar.Length - 2; i++)
                {
                    if (!double.TryParse(xchar[i], out x))
                    {
                        int u = 0;
                        param.TryGetValue(xchar[i], out u);
                        sumador = sumador + u * Math.Pow(2, potencia);
                    }
                    else
                    {
                        sumador = sumador + int.Parse(xchar[i]) * Math.Pow(2, potencia);
                    }
                    potencia++;
                }
                var nuevo = (sumador / 11) * 11;
                var original = sumador;
                result = v - (original - nuevo);
            }
            catch (Exception e)
            {
                mensaje = e.Message;
                return false;
            }
            if (result != 0)
            {
                mensaje = "ISO no válido";
                return false;
            }
            else
            {
                mensaje = string.Empty;
                return true;
            }
        }

        //Leer archivo CSV separado por comas.
        private static DataTable GetDataFromCSVFile(string csv_file_path,string usuario)
        {
            var csvData = new DataTable();
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields = csvReader.ReadFields();
                    foreach (string column in colFields)
                    {
                        DataColumn datecolumn = new DataColumn(column);
                        datecolumn.AllowDBNull = true;
                        csvData.Columns.Add(datecolumn);
                    }
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            if (fieldData[i] == "")
                            {
                                fieldData[i] = null;
                            }
                        }
                        csvData.Rows.Add(fieldData);
                    }
                }
            }
            catch (Exception ex)
            {
                log_csl.save_log<Exception>(ex, "DataTransformHelper", "GetDataFromCSVFile", csv_file_path , usuario);
            }
            return csvData;
        }

        //valida lo basico del container 11 4 letras 7 numeros
        public static bool ContainerBasicValidate(string container)
        {
            if (string.IsNullOrEmpty(container))
            {
                return false;
            }
            var arx = container.Trim().ToCharArray();
            //menos de 11 chao
            if (arx.Length != 11)
            {
                return false;
            }
            for (var i = 0; i <= arx.Length - 1; i++)
            {
                if (i <= 3)
                {
                    if (!char.IsLetter(arx[i]))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!char.IsNumber(arx[i]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public static string hideData(string cadena)
        {
            if (string.IsNullOrEmpty(cadena) || cadena.Length <= 4) { return cadena; }
            var clencadena = cadena.Trim();
            try
            {
                int mitad = clencadena.Length / 2;
                int mitad_1 = mitad + 1;
                StringBuilder s = new StringBuilder();
                var inicial = 1;
                foreach (char a in clencadena)
                {
                    char b = a;
                    if (inicial == mitad)
                    {
                        b = '*';
                    }
                    if (inicial == mitad_1)
                    {
                        b = '*';
                    }
                    s.Append(b);
                    inicial++;
                }
                return s.ToString();
            }
            catch
            {

                int xc = clencadena.Length / 2;
                var cc = clencadena.Substring(0, xc);
                return cc.PadRight(clencadena.Length, '*');
            }
        }
        public static bool ValidarAduDoc(string documento,string tipo,  out string mensaje)
        {
            mensaje = string.Empty;
            if (string.IsNullOrEmpty(documento) || string.IsNullOrEmpty(tipo))
            {
                mensaje = "Debe escribir el número de documento de aduana y su tipo";
                return false;
            }
            documento = documento.Trim();
           documento = documento.Replace("-", string.Empty);
            documento =   documento.Replace(" ", string.Empty);
            var dtipo = tipo.Trim().ToUpper();
           //si es cualquiera----> 17 caracteres
            if (dtipo.Contains("DAE") || dtipo.Contains("DAS") || dtipo.Contains("DJT"))
            {
                //->17 caracteres
                if (documento.Length != 17)
                {
                    mensaje = string.Format("El No. documento de exportación [{0}] debe tener 17 caracteres que no incluyen espacios", tipo);
                    return false; 
                }
            }
            if ( dtipo.Contains("TRS"))
            {
                if (documento.Length == 21)
                {
                    mensaje = string.Format("El No. documento de exportación [{0}] debe tener máximo 21 caracteres que no incluyen espacios", tipo);
                    return false;
                }
            }
            mensaje = string.Empty;
            return true;
        }


        //nuevo 2017---->
        public static bool ValidarAduDoc(string documento, string tipo, string ruc , string tipoc, int total, out bool consultar, out string mensaje)
        {
            mensaje = string.Empty;
            consultar = false;
            if (string.IsNullOrEmpty(ruc) )
            {
                mensaje = "Fue imposible leer el número de RUC del usuario actual, para comprobor el documento de aduana o su formato es incorrecto";
                return false;
            }

            if (string.IsNullOrEmpty(tipoc))
            {
                mensaje = "Fue imposible encontrar el tipo de carga para comprobar el documento de aduana, el proceso no podrá continuar";
                return false;
            }

            if (string.IsNullOrEmpty(documento) || string.IsNullOrEmpty(tipo))
            {
                mensaje = "Debe escribir el número de documento de aduana y su tipo";
                
                return false;
            }
            ruc = ruc.Trim();
            documento = documento.Trim();
            documento = documento.Replace("-", string.Empty);
            documento = documento.Replace(" ", string.Empty);
            var dtipo = tipo.Trim().ToUpper();
            //si es cualquiera----> 17 caracteres
            if (dtipo.Contains("DAE") || dtipo.Contains("DAS") || dtipo.Contains("DJT"))
            {
                //->17 caracteres
                if (documento.Length != 17)
                {
                    mensaje = "La cantidad de dígitos de la DAE/DAS se encuentra incompleta o supera los requeridos (deben ser 17 dígitos).";
                    return false;
                }
            }
            if (dtipo.Contains("TRS"))
            {
                if (documento.Length == 21)
                {
                    mensaje = string.Format("El No. documento de exportación [{0}] debe tener máximo 21 caracteres que no incluyen espacios", tipo);
                    return false;
                }
            }

            //nueva validaciones 2017--> comprobar el RUC/DAE/TIPO DAE
            if (!jAisvContainer.DaeCheck(tipo, ruc, tipoc, documento, total,out consultar,  out mensaje))
            {
                return false;
            }

            mensaje = string.Empty;
            return true;
        }

        public static DataTable TableFromXML(string Name, string XMLString)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(new StringReader(XMLString));
            DataTable Dt = new DataTable(Name);
            try
            {

                XmlNode NodoEstructura = doc.FirstChild.FirstChild;
                //  Table structure (columns definition) 
                foreach (XmlNode columna in NodoEstructura.ChildNodes)
                {
                    Dt.Columns.Add(columna.Name, typeof(String));
                }

                XmlNode Filas = doc.FirstChild;
                //  Data Rows 
                foreach (XmlNode Fila in Filas.ChildNodes)
                {
                    List<string> Valores = new List<string>();
                    foreach (XmlNode Columna in Fila.ChildNodes)
                    {
                        Valores.Add(Columna.InnerText);
                    }
                    Dt.Rows.Add(Valores.ToArray());
                }
            }
            catch (Exception)
            {

            }

            return Dt;
        }

        //nuevo
        public static string CleanInvalidXmlChars(string text)
        {
            if (string.IsNullOrEmpty(text)) { return string.Empty; }

            // From xml spec valid chars: 
            // #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD] | [#x10000-#x10FFFF]     
            // any Unicode character, excluding the surrogate blocks, FFFE, and FFFF. 
            string re = @"[^\x09\x0A\x0D\x20-\uD7FF\uE000-\uFFFD\u10000-\u10FFFF]";
            return Regex.Replace(text, re, "");
        }

        //esta funcion usa un patron para buscar dentro de una cadena.
        /*Patron para buscar teca y teka*/
        // (\W)teca|teka(\W\B)
        public static bool IsstringPattern(string pattern, string text)
        {
            if (string.IsNullOrEmpty(pattern)) { return true; }
            if (string.IsNullOrEmpty(text)) { return false; }
            text = text.Trim().ToLower();
            //var x = Regex.Matches(text, pattern, RegexOptions.IgnoreCase).Count;
            return Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase);

            //return Regex.Matches(text, pattern, RegexOptions.IgnoreCase).Count > 0 ? true : false;
        }
      }
}