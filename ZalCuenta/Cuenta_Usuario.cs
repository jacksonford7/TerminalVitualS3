using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLHandler;

namespace ZalCuenta
{
    /// <summary>
    /// Login de usuario que una cuenta
    /// </summary>
    class Cuenta_Usuario:BaseInit
    {
        public string loginname { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
        public string estado { get; set; }
        private static void initialize()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
        }
        /// <summary>
        /// Retorna una lista de usuarios de un ru (base Portal_Master)
        /// </summary>
        /// <param name="ruc"></param>
        /// <returns></returns>
        internal static List<Cuenta_Usuario> UsuarioxRuc(string ruc)
        {
            //esto en base de datos cgwdb01
            //zec_usuariosxruc '0990872511001'
            initialize();
            var v_conexion = app_configurations.get_configuration("csl_services");
            if (string.IsNullOrEmpty(v_conexion?.value))
            {
                return null;
            }
            return sql_pointer.ExecuteSelect<Cuenta_Usuario>(v_conexion.value, 2000, "zec_usuariosxruc", new Dictionary<string, object>() { { "ruc", ruc } },null);
        }


    }



}
