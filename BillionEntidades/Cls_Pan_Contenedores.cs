using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;


namespace BillionEntidades
{
    public class Cls_Pan_Contenedores : Cls_Bil_Base
    {
        #region "Variables"



        private Int64? _GKEY;
        private string _CONTENEDOR = string.Empty;
        private string _ACCION = string.Empty;
        private string _TIPO = string.Empty;
        private string _REFERENCIA = string.Empty;
        private string _AISV = string.Empty;
        private string _FECHA_ACCION = string.Empty;
        private string _RUC = string.Empty;
        private string _EMPRESA = string.Empty;
        private string _ESTADO = string.Empty;
        private int _tiene_conteendores;

        #endregion

        #region "Propiedades"
        public Int64? GKEY { get => _GKEY; set => _GKEY = value; }

        public string CONTENEDOR { get => _CONTENEDOR; set => _CONTENEDOR = value; }
        public string ACCION { get => _ACCION; set => _ACCION = value; }
        public string TIPO { get => _TIPO; set => _TIPO = value; }
        public string REFERENCIA { get => _REFERENCIA; set => _REFERENCIA = value; }
        public string AISV { get => _AISV; set => _AISV = value; }
        public string FECHA_ACCION { get => _FECHA_ACCION; set => _FECHA_ACCION = value; }
        public string RUC { get => _RUC; set => _RUC = value; }
        public string EMPRESA { get => _EMPRESA; set => _EMPRESA = value; }
        public string ESTADO { get => _ESTADO; set => _ESTADO = value; }
        public int tiene_conteendores { get => _tiene_conteendores; set => _tiene_conteendores = value; }
        #endregion

        public Cls_Pan_Contenedores()
        {
            init();

        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }


        public static List<Cls_Pan_Contenedores> Pan_Contenedores(DateTime fecha_desde, DateTime fecha_hasta, string Contenedor ,  string ruc, out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("fecha_desde", fecha_desde);
            parametros.Add("fecha_hasta", fecha_hasta);
            parametros.Add("criterio", Contenedor);
            parametros.Add("ruc", ruc);
            return sql_puntero.ExecuteSelectControl<Cls_Pan_Contenedores>(nueva_conexion, 6000, "pan_listado_contenedores", parametros, out OnError);

        }

        public static List<Cls_Pan_Contenedores> Pan_Tienen_Contenedores( string ruc, out string OnError)
        {
            OnInit("N4Middleware");
            parametros.Clear();
            parametros.Add("ruc", ruc);
            return sql_puntero.ExecuteSelectControl<Cls_Pan_Contenedores>(nueva_conexion, 6000, "pan_tiene_contenedores", parametros, out OnError);

        }


    }
}
