using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Bil_Invoice_Grafico : Cls_Bil_Base
    {

        #region "Variables"

        private Decimal _vta_01=0;
        private Decimal _vta_02=0;
        private Decimal _vta_03 = 0;
        private Decimal _vta_04 = 0;
        private Decimal _vta_05 = 0;
        private Decimal _vta_06 = 0;
        private Decimal _vta_07 = 0;
        private Decimal _vta_08 = 0;
        private Decimal _vta_09 = 0;
        private Decimal _vta_10 = 0;
        private Decimal _vta_11 = 0;
        private Decimal _vta_12 = 0;
        private Decimal _total = 0;
        private string _tipo = string.Empty;

        private string _ruc = string.Empty;
        private string _mes_letra = string.Empty;
        private Decimal _dolares = 0;
        private Int32? _mes = 0;
        private Int32 _totales = 0;
        private Int32 _idUsuario = 0;
        private string _Usuario = string.Empty;
        private string _Nombres = string.Empty;
        private string _NombreEmpresa = string.Empty;
        #endregion

        #region "Propiedades"

        public Decimal vta_01 { get => _vta_01; set => _vta_01 = value; }
        public Decimal vta_02 { get => _vta_02; set => _vta_02 = value; }
        public Decimal vta_03 { get => _vta_03; set => _vta_03 = value; }
        public Decimal vta_04 { get => _vta_04; set => _vta_04 = value; }
        public Decimal vta_05 { get => _vta_05; set => _vta_05 = value; }
        public Decimal vta_06 { get => _vta_06; set => _vta_06 = value; }
        public Decimal vta_07 { get => _vta_07; set => _vta_07 = value; }
        public Decimal vta_08 { get => _vta_08; set => _vta_08 = value; }
        public Decimal vta_09 { get => _vta_09; set => _vta_09 = value; }
        public Decimal vta_10 { get => _vta_10; set => _vta_10 = value; }
        public Decimal vta_11 { get => _vta_11; set => _vta_11 = value; }
        public Decimal vta_12 { get => _vta_12; set => _vta_12 = value; }
        public Decimal total { get => _total; set => _total = value; }
        public string tipo { get => _tipo; set => _tipo = value; }


        public string RUC { get => _ruc; set => _ruc = value; }
        public string MES_LETRA { get => _mes_letra; set => _mes_letra = value; }
        public Decimal DOLARES { get => _dolares; set => _dolares = value; }
        public Int32? MES { get => _mes; set => _mes = value; }
        public Int32 TOTALES { get => _totales; set => _totales = value; }

        public Int32 idUsuario { get => _idUsuario; set => _idUsuario = value; }
        public string Usuario { get => _Usuario; set => _Usuario = value; }
        public string Nombres { get => _Nombres; set => _Nombres = value; }
        public string NombreEmpresa { get => _NombreEmpresa; set => _NombreEmpresa = value; }
        #endregion



        public Cls_Bil_Invoice_Grafico()
        {
            init();

        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

     

        /*carga todas las facturas por rango de fechas*/
        public static List<Cls_Bil_Invoice_Grafico> Listado_Ventas(string Usuario,  out string OnError)
        {
            OnInit("PORTAL_BILLION");

            parametros.Clear();
            parametros.Add("IV_USUARIO_CREA", Usuario);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Invoice_Grafico>(sql_puntero.Conexion_Local, 4000, "sp_Bil_Rpt_Grafico_Ventas", parametros, out OnError);

        }

        /*carga total mensual de facturas emitidas por agente*/
        public static List<Cls_Bil_Invoice_Grafico> Movimientos_Mensuales(string ruc, out string OnError)
        {
            OnInit("PORTAL_BILLION");

            parametros.Clear();
            parametros.Add("RUC", ruc);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Invoice_Grafico>(sql_puntero.Conexion_Local, 5000, "sp_Bil_Rpt_Movimientos_Mes_Agente", parametros, out OnError);

        }
        /*carga total mensual de facturas emitidas por usuarios*/
        public static List<Cls_Bil_Invoice_Grafico> Movimientos_Usuarios(string ruc, out string OnError)
        {
            OnInit("PORTAL_BILLION");

            parametros.Clear();
            parametros.Add("RUC", ruc);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Invoice_Grafico>(sql_puntero.Conexion_Local, 5000, "sp_Bil_Rpt_Movimientos_Usuario", parametros, out OnError);

        }

        /*carga total mensual de facturas emitidas por usuarios*/
        public static List<Cls_Bil_Invoice_Grafico> Listado_Usuarios(out string OnError)
        {
            OnInit("PORTAL_MASTER");

            return sql_puntero.ExecuteSelectControl<Cls_Bil_Invoice_Grafico>(nueva_conexion, 5000, "sp_Bil_Listado_Usuarios", null, out OnError);

        }

        /*carga total mensual de facturas emitidas por usuarios*/
        public static List<Cls_Bil_Invoice_Grafico> Movimientos_Usuarios_Fechas(DateTime desde, DateTime hasta,out string OnError)
        {
            OnInit("PORTAL_BILLION");

            parametros.Clear();
            parametros.Add("fecha_desde", desde);
            parametros.Add("fecha_hata", hasta);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Invoice_Grafico>(sql_puntero.Conexion_Local, 5000, "sp_Bil_Rpt_Movimientos_Usuario_fechas", parametros, out OnError);

        }
    }
}
