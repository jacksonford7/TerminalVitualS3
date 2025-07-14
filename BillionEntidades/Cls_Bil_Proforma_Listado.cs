using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Bil_Proforma_Listado : Cls_Bil_Base
    {

        #region "Variables"

        private Int64 _PF_ID;
        private string _PF_NUMERO = string.Empty;
        private string _PF_FECHA = null;
        private string _PF_TIPO_CARGA = string.Empty;
        private string _PF_GLOSA = string.Empty;
        private string _PF_DESC_AGENTE = string.Empty;
        private string _PF_DESC_CLIENTE = string.Empty;
        private string _PF_DESC_FACTURADO = string.Empty;
        private string _PF_NUMERO_CARGA = string.Empty;
        private string _PF_FECHA_HASTA = string.Empty;
        private decimal _TOT_PF_SUBTOTAL = 0;
        private decimal _TOT_PF_IVA = 0;
        private decimal _TOT_PF_TOTAL = 0;
        private string _PF_USUARIO_CREA = string.Empty;

        #endregion

        #region "Propiedades"

        public Int64 PF_ID { get => _PF_ID; set => _PF_ID = value; }
        public string PF_NUMERO { get => _PF_NUMERO; set => _PF_NUMERO = value; }
        public string PF_FECHA { get => _PF_FECHA; set => _PF_FECHA = value; }
        public string PF_FECHA_HASTA { get => _PF_FECHA_HASTA; set => _PF_FECHA_HASTA = value; }
        public string PF_GLOSA { get => _PF_GLOSA; set => _PF_GLOSA = value; }
        public string PF_TIPO_CARGA { get => _PF_TIPO_CARGA; set => _PF_TIPO_CARGA = value; }
        public string PF_DESC_AGENTE { get => _PF_DESC_AGENTE; set => _PF_DESC_AGENTE = value; }
        public string PF_DESC_CLIENTE { get => _PF_DESC_CLIENTE; set => _PF_DESC_CLIENTE = value; }
        public string PF_DESC_FACTURADO { get => _PF_DESC_FACTURADO; set => _PF_DESC_FACTURADO = value; }
        public string PF_NUMERO_CARGA { get => _PF_NUMERO_CARGA; set => _PF_NUMERO_CARGA = value; }
        public decimal TOT_PF_SUBTOTAL { get => _TOT_PF_SUBTOTAL; set => _TOT_PF_SUBTOTAL = value; }
        public decimal TOT_PF_IVA { get => _TOT_PF_IVA; set => _TOT_PF_IVA = value; }
        public decimal TOT_PF_TOTAL { get => _TOT_PF_TOTAL; set => _TOT_PF_TOTAL = value; }
        public string PF_USUARIO_CREA { get => _PF_USUARIO_CREA; set => _PF_USUARIO_CREA = value; }

        private static String v_mensaje = string.Empty;


        #endregion

        public Cls_Bil_Proforma_Listado()
        {
            init();

        }
        /*
        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("PORTAL_BILLION");
        }
        */

        /*lista todas las proformas por rango de fechas*/
       /* public static List<Cls_Bil_Proforma_Listado> Listado_Proformas(DateTime PF_FECHA_DESDE, DateTime PF_FECHA_HASTA, string Usuario, string Agente, out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("PF_FECHA_DESDE", PF_FECHA_DESDE);
            parametros.Add("PF_FECHA_HASTA", PF_FECHA_HASTA);
            parametros.Add("PF_USUARIO", Usuario);
            parametros.Add("PF_RUC", Agente);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Proforma_Listado>(sql_puntero.Conexion_Local, 4000, "sp_Bil_Rpt_Listado_Proformas", parametros, out OnError);

        }*/
    }
}
