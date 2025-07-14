using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Bil_Invoice_Listado : Cls_Bil_Base
    {

        #region "Variables"

        private Int64 _IV_ID;
        private string _IV_NUMERO = string.Empty;
        private string _IV_COMPROBANTE = string.Empty;
        private string _IV_LIQUIDACION = string.Empty;
        private string _IV_FECHA = string.Empty;
        private string _IV_VENCIMIENTO = string.Empty;
        private string _IV_FECHA_HASTA = string.Empty;
        private string _IV_TIPO_CARGA = string.Empty;
        private string _IV_GLOSA = string.Empty;
        private string _IV_DESC_AGENTE = string.Empty;
        private string _IV_DESC_CLIENTE = string.Empty;
        private string _IV_DESC_FACTURADO = string.Empty;   
        private string _IV_NUMERO_CARGA = string.Empty;
        private decimal _TOT_IV_SUBTOTAL = 0;
        private decimal _TOT_IV_IVA = 0;
        private decimal _TOT_IV_TOTAL = 0;
      //  private string _IV_USUARIO_CREA = string.Empty;

        #endregion

        #region "Propiedades"

        public Int64 IV_ID { get => _IV_ID; set => _IV_ID = value; }
        public string IV_NUMERO { get => _IV_NUMERO; set => _IV_NUMERO = value; }
        public string IV_COMPROBANTE { get => _IV_COMPROBANTE; set => _IV_COMPROBANTE = value; }
        public string IV_LIQUIDACION { get => _IV_LIQUIDACION; set => _IV_LIQUIDACION = value; }

        public string IV_FECHA { get => _IV_FECHA; set => _IV_FECHA = value; }
        public string IV_VENCIMIENTO { get => _IV_VENCIMIENTO; set => _IV_VENCIMIENTO = value; }
        public string IV_FECHA_HASTA { get => _IV_FECHA_HASTA; set => _IV_FECHA_HASTA = value; }
        public string IV_GLOSA { get => _IV_GLOSA; set => _IV_GLOSA = value; }
        public string IV_TIPO_CARGA { get => _IV_TIPO_CARGA; set => _IV_TIPO_CARGA = value; }
      
        public string IV_DESC_AGENTE { get => _IV_DESC_AGENTE; set => _IV_DESC_AGENTE = value; }
        public string IV_DESC_CLIENTE { get => _IV_DESC_CLIENTE; set => _IV_DESC_CLIENTE = value; }
        public string IV_DESC_FACTURADO { get => _IV_DESC_FACTURADO; set => _IV_DESC_FACTURADO = value; }
        public string IV_NUMERO_CARGA { get => _IV_NUMERO_CARGA; set => _IV_NUMERO_CARGA = value; }
        public decimal TOT_IV_SUBTOTAL { get => _TOT_IV_SUBTOTAL; set => _TOT_IV_SUBTOTAL = value; }
        public decimal TOT_IV_IVA { get => _TOT_IV_IVA; set => _TOT_IV_IVA = value; }
        public decimal TOT_IV_TOTAL { get => _TOT_IV_TOTAL; set => _TOT_IV_TOTAL = value; }
        //public string IV_USUARIO_CREA { get => _IV_USUARIO_CREA; set => _IV_USUARIO_CREA = value; }

        private static String v_mensaje = string.Empty;


        #endregion

        public Cls_Bil_Invoice_Listado()
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

        /*carga todas las facturas por rango de fechas*/
      /*  public static List<Cls_Bil_Invoice_Listado> Listado_Facturas(DateTime IV_FECHA_DESDE, DateTime IV_FECHA_HASTA,string Usuario , string Agente,out string OnError)
        {
            OnInit();

            parametros.Clear();
            parametros.Add("IV_FECHA_DESDE", IV_FECHA_DESDE);
            parametros.Add("IV_FECHA_HASTA", IV_FECHA_HASTA);
            parametros.Add("IV_USUARIO", Usuario);
            parametros.Add("IV_RUC", Agente);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_Invoice_Listado>(sql_puntero.Conexion_Local, 4000, "sp_Bil_Rpt_Listado_Factura", parametros, out OnError);

        }*/
    }
}
