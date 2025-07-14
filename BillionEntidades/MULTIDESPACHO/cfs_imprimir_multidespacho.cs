using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class cfs_imprimir_multidespacho : Cls_Bil_Base
    {

        #region "Variables"

        private Int64 _IV_ID;
        private string _IV_NUMERO = string.Empty;
        private string _IV_COMPROBANTE = string.Empty;
        private string _IV_LIQUIDACION = string.Empty;

        private string _IV_FECHA = string.Empty;
        private string _IV_VENCIMIENTO = string.Empty;
        private string _IV_TIPO_CARGA = string.Empty;
        private string _IV_GLOSA = string.Empty;
        private string _IV_CONTENEDOR = string.Empty;
        private string _IV_ID_AGENTE = string.Empty;
        private string _IV_DESC_AGENTE = string.Empty;
        private string _IV_ID_CLIENTE = string.Empty;
        private string _IV_DESC_CLIENTE = string.Empty;
        private string _IV_ID_FACTURADO = string.Empty;
        private string _IV_DESC_FACTURADO = string.Empty;
        private string _IV_DIR_FACTURADO = string.Empty;
        private string _IV_EMAIL_FACTURADO = string.Empty;
        private string _IV_CIUDAD_FACTURADO = string.Empty;
        private string _IV_TIPO = string.Empty;
        private string _IV_BUQUE = string.Empty;
        private string _IV_VIAJE = string.Empty;
        private string _IV_FECHA_ARRIBO = string.Empty;
        private Int64 _IV_DIAS_CREDITO = 0;

        private string _IV_NUMERO_CARGA = string.Empty;

        private string _IV_ID_SERVICIO = string.Empty;
        private string _IV_DESC_SERVICIO = string.Empty;
        private decimal _IV_CANTIDAD = 0;
        private decimal _IV_PRECIO = 0;
        private decimal _IV_SUBTOTAL = 0;
        private decimal _IV_IVA = 0;


        private decimal _TOT_IV_SUBTOTAL = 0;
        private decimal _TOT_IV_IVA = 0;
        private decimal _TOT_IV_TOTAL = 0;

        private string _IV_DOCUMENTO = string.Empty;
        private string _NUMERO_DESPACHO = string.Empty;
        #endregion

        #region "Propiedades"

        public Int64 IV_ID { get => _IV_ID; set => _IV_ID = value; }
        public string IV_NUMERO { get => _IV_NUMERO; set => _IV_NUMERO = value; }
        public string IV_COMPROBANTE { get => _IV_COMPROBANTE; set => _IV_COMPROBANTE = value; }
        public string IV_LIQUIDACION { get => _IV_LIQUIDACION; set => _IV_LIQUIDACION = value; }


        public string IV_FECHA { get => _IV_FECHA; set => _IV_FECHA = value; }
        public string IV_VENCIMIENTO { get => _IV_VENCIMIENTO; set => _IV_VENCIMIENTO = value; }
        public string IV_GLOSA { get => _IV_GLOSA; set => _IV_GLOSA = value; }
        public string IV_TIPO_CARGA { get => _IV_TIPO_CARGA; set => _IV_TIPO_CARGA = value; }
        public string IV_CONTENEDOR { get => _IV_CONTENEDOR; set => _IV_CONTENEDOR = value; }

        public string IV_ID_AGENTE { get => _IV_ID_AGENTE; set => _IV_ID_AGENTE = value; }
        public string IV_DESC_AGENTE { get => _IV_DESC_AGENTE; set => _IV_DESC_AGENTE = value; }
        public string IV_ID_CLIENTE { get => _IV_ID_CLIENTE; set => _IV_ID_CLIENTE = value; }
        public string IV_DESC_CLIENTE { get => _IV_DESC_CLIENTE; set => _IV_DESC_CLIENTE = value; }
        public string IV_ID_FACTURADO { get => _IV_ID_FACTURADO; set => _IV_ID_FACTURADO = value; }
        public string IV_DESC_FACTURADO { get => _IV_DESC_FACTURADO; set => _IV_DESC_FACTURADO = value; }
        public string IV_DIR_FACTURADO { get => _IV_DIR_FACTURADO; set => _IV_DIR_FACTURADO = value; }
        public string IV_EMAIL_FACTURADO { get => _IV_EMAIL_FACTURADO; set => _IV_EMAIL_FACTURADO = value; }
        public string IV_CIUDAD_FACTURADO { get => _IV_CIUDAD_FACTURADO; set => _IV_CIUDAD_FACTURADO = value; }
        public string IV_TIPO { get => _IV_TIPO; set => _IV_TIPO = value; }
        public string IV_BUQUE { get => _IV_BUQUE; set => _IV_BUQUE = value; }
        public string IV_VIAJE { get => _IV_VIAJE; set => _IV_VIAJE = value; }
        public string IV_FECHA_ARRIBO { get => _IV_FECHA_ARRIBO; set => _IV_FECHA_ARRIBO = value; }
        public Int64 IV_DIAS_CREDITO { get => _IV_DIAS_CREDITO; set => _IV_DIAS_CREDITO = value; }

        public string IV_NUMERO_CARGA { get => _IV_NUMERO_CARGA; set => _IV_NUMERO_CARGA = value; }

        public string IV_ID_SERVICIO { get => _IV_ID_SERVICIO; set => _IV_ID_SERVICIO = value; }
        public string IV_DESC_SERVICIO { get => _IV_DESC_SERVICIO; set => _IV_DESC_SERVICIO = value; }
        public decimal IV_CANTIDAD { get => _IV_CANTIDAD; set => _IV_CANTIDAD = value; }
        public decimal IV_PRECIO { get => _IV_PRECIO; set => _IV_PRECIO = value; }
        public decimal IV_SUBTOTAL { get => _IV_SUBTOTAL; set => _IV_SUBTOTAL = value; }
        public decimal IV_IVA { get => _IV_IVA; set => _IV_IVA = value; }


        public decimal TOT_IV_SUBTOTAL { get => _TOT_IV_SUBTOTAL; set => _TOT_IV_SUBTOTAL = value; }
        public decimal TOT_IV_IVA { get => _TOT_IV_IVA; set => _TOT_IV_IVA = value; }
        public decimal TOT_IV_TOTAL { get => _TOT_IV_TOTAL; set => _TOT_IV_TOTAL = value; }

        public string IV_DOCUMENTO { get => _IV_DOCUMENTO; set => _IV_DOCUMENTO = value; }
        public string NUMERO_DESPACHO { get => _NUMERO_DESPACHO; set => _NUMERO_DESPACHO = value; }
        private static String v_mensaje = string.Empty;


        #endregion

        public cfs_imprimir_multidespacho()
        {
            init();

        }

        /*carga todas las facturas*/
        public static List<cfs_imprimir_multidespacho> Datos_Factura(Int64 IV_ID, out string OnError)
        {
            parametros.Clear();
            parametros.Add("IV_ID", IV_ID);
            return sql_puntero.ExecuteSelectControl<cfs_imprimir_multidespacho>(sql_puntero.Conexion_Local, 4000, "cfs_Rpt_ImprimirFactura_MultiDespacho", parametros, out OnError);

        }

        public static List<cfs_imprimir_multidespacho> Listado_Facturas(DateTime FECHA_DESDE, DateTime FECHA_HASTA, out string OnError)
        {

         
            parametros.Clear();
            parametros.Add("IV_FECHA_DESDE", FECHA_DESDE);
            parametros.Add("IV_FECHA_HASTA", FECHA_HASTA);
            return sql_puntero.ExecuteSelectControl<cfs_imprimir_multidespacho>(sql_puntero.Conexion_Local, 6000, "CFS_RPT_LISTADO_FACTURAS_MULTIEDESPACHO", parametros, out OnError);

        }
    }
}
