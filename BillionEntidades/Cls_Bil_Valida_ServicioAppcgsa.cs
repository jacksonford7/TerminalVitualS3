using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Bil_Valida_ServicioAppcgsa : Cls_Bil_Base
    {
        #region "Variables"

        private Int64 _gkey;
        private string _contenedor = string.Empty;
        private int _servicio;

        #endregion

        #region "Propiedades"

        public Int64 gkey { get => _gkey; set => _gkey = value; }
        public string contenedor { get => _contenedor; set => _contenedor = value; }
        public int servicio { get => _servicio; set => _servicio = value; }

        #endregion

        public Cls_Bil_Valida_ServicioAppcgsa()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<Cls_Bil_Valida_ServicioAppcgsa> Validacion_ServicioAppCgsa(string XmlContenedor, out string OnError)
        {
            OnInit("N5");

            parametros.Clear();
            parametros.Add("XmlContenedor", XmlContenedor);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Valida_ServicioAppcgsa>(nueva_conexion, 6000, "[Bill].[Validacion_ServicioAppCgsa]", parametros, out OnError);

        }

        public static List<Cls_Bil_Valida_ServicioAppcgsa> Validacion_ServicioAppCgsa_Cfs(string XmlContenedor, out string OnError)
        {
            OnInit("N5");

            parametros.Clear();
            parametros.Add("XmlContenedor", XmlContenedor);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Valida_ServicioAppcgsa>(nueva_conexion, 6000, "[Bill].[Validacion_ServicioAppCgsa_CFS]", parametros, out OnError);

        }

        public static List<Cls_Bil_Valida_ServicioAppcgsa> Validacion_ServicioAppCgsa_Brbk(string XmlContenedor, out string OnError)
        {
            OnInit("N5");

            parametros.Clear();
            parametros.Add("XmlContenedor", XmlContenedor);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Valida_ServicioAppcgsa>(nueva_conexion, 6000, "[Bill].[Validacion_ServicioAppCgsa_Brbk]", parametros, out OnError);

        }


    }
}
