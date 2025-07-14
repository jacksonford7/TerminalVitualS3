using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Bil_Consulta_Casos : Cls_Bil_Base
    {
        #region "Variables"


        private string _USUARIO = string.Empty;
        private string _CATEGORIA = string.Empty;
        private string _RUC = string.Empty;
        private string _ASUNTO = string.Empty;
        private string _CONTENIDO = string.Empty;
        private string _FECHA_REGISTRO = string.Empty;
        #endregion

        #region "Propiedades"

        public string CATEGORIA { get => _CATEGORIA; set => _CATEGORIA = value; }
        public string USUARIO { get => _USUARIO; set => _USUARIO = value; }
        public string RUC { get => _RUC; set => _RUC = value; }
        public string ASUNTO { get => _ASUNTO; set => _ASUNTO = value; }
        public string CONTENIDO { get => _CONTENIDO; set => _CONTENIDO = value; }
        public string FECHA_REGISTRO { get => _FECHA_REGISTRO; set => _FECHA_REGISTRO = value; }

        private static String v_mensaje = string.Empty;

        #endregion


        public Cls_Bil_Consulta_Casos()
        {
            init();


        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("PORTAL_BILLION");
        }

        public static List<Cls_Bil_Consulta_Casos> Usuarios(out string OnError)
        {
            OnInit();
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Consulta_Casos>(sql_puntero.Conexion_Local, 5000, "sp_Bil_Extrae_Usuarios", null, out OnError);

        }

        public static List<Cls_Bil_Consulta_Casos> Listado_Casos(string Usuario, DateTime FECHA_DESDE, DateTime FECHA_HASTA, out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("Usuario", Usuario);
            parametros.Add("FECHA_DESDE", FECHA_DESDE);
            parametros.Add("FECHA_HASTA", FECHA_HASTA);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Consulta_Casos>(sql_puntero.Conexion_Local, 5000, "sp_Bil_Consulta_Casos", parametros, out OnError);

        }

    }


}
