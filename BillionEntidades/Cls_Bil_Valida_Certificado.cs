using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Bil_Valida_Certificado : Cls_Bil_Base
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

        public Cls_Bil_Valida_Certificado()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<Cls_Bil_Valida_Certificado> Validacion_Certificado(string XmlContenedor, out string OnError)
        {
            OnInit("N5");

            parametros.Clear();
            parametros.Add("XmlContenedor", XmlContenedor);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Valida_Certificado>(nueva_conexion, 6000, "[Bill].[Validacion_Certificado]", parametros, out OnError);

        }

        public static List<Cls_Bil_Valida_Certificado> Validacion_Certificado_Cfs(string XmlContenedor, out string OnError)
        {
            OnInit("N5");

            parametros.Clear();
            parametros.Add("XmlContenedor", XmlContenedor);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Valida_Certificado>(nueva_conexion, 6000, "[Bill].[Validacion_Certificado_CFS]", parametros, out OnError);

        }

        public static List<Cls_Bil_Valida_Certificado> Validacion_Certificado_Expo(string XmlContenedor, out string OnError)
        {
            OnInit("N5");

            parametros.Clear();
            parametros.Add("XmlContenedor", XmlContenedor);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Valida_Certificado>(nueva_conexion, 6000, "[Bill].[Validacion_Certificado_Expo]", parametros, out OnError);

        }

        public static List<Cls_Bil_Valida_Certificado> Validacion_Certificado_Brbk(string XmlContenedor, out string OnError)
        {
            OnInit("N5");

            parametros.Clear();
            parametros.Add("XmlContenedor", XmlContenedor);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Valida_Certificado>(nueva_conexion, 6000, "[Bill].[Validacion_Certificado_Brbk]", parametros, out OnError);

        }

        public static List<Cls_Bil_Valida_Certificado> Validacion_ImagenesSellos(string XmlContenedor, out string OnError)
        {
            OnInit("N5");

            parametros.Clear();
            parametros.Add("XmlContenedor", XmlContenedor);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Valida_Certificado>(nueva_conexion, 6000, "[Bill].[Validacion_ImagenesSellos]", parametros, out OnError);

        }

    }
}
