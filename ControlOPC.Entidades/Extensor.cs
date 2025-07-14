using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlOPC.Entidades
{
    public static class  Extension
    {
        /// <summary>
        /// Check string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="cadena"></param>
        /// <returns></returns>
        public static int check_string<T>(this T source,string cadena,  int min=0, int max=100) 
        {
            if (string.IsNullOrEmpty(cadena)) { return -1; }
            if (cadena.Length < min || cadena.Length > max) { return 0; }
            return 1;
        }
        /// <summary>
        /// Check is valida date value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="dateString"></param>
        /// <param name="formater">Format Value os string sample dd/MM/yyyy, yyyy/MM/dd </param>
        /// <returns></returns>
        public static int check_is_date<T>(this T source, string dateString, string formater="dd/MM/yyyy")
        {
            var  enUS = new System.Globalization.CultureInfo("en-US");
            DateTime dt;
            if (string.IsNullOrEmpty(dateString)) { return -1; }
            return DateTime.TryParseExact(dateString, formater, enUS, System.Globalization.DateTimeStyles.None, out dt)? 0:1;
        }
        /// <summary>
        /// Check is values is time format 00:00 to 23:59
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="timeValue"></param>
        /// <returns></returns>
        public static int check_is_time<T>(this T source, string timeValue)
        {
            if (string.IsNullOrEmpty(timeValue)) { return -1; }
            TimeSpan dummyOutput;
            return TimeSpan.TryParse(timeValue, out dummyOutput) ? 1 : 0;
        }
        /// <summary>
        /// Check Object Instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int check_object<T>(this T source)
        {
            return source == null ? 0 : 1;
        }
        /// <summary>
        /// Check for numeric conversion
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int TryParse<T,TD>(this T source,string text, out TD value)
        {
            value = default(TD);
            try
            {
                value = (TD)Convert.ChangeType(text, typeof(TD));
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public static string Nueva_Conexion (string pTipo)
        {   
            try
            {
                var v_conexion = app_configurations.get_configuration(pTipo);
                return v_conexion.value;
            }
            catch
            {
                return string.Empty;
            }


        }

    }
}
