using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class BTS_Draf_Impresion : Cls_Bil_Base
    {


        #region "Propiedades"


        public Int64 ID { get; set; }
        public string GLOSA { get; set; }
        public DateTime? FECHA { get; set; }
        public string TIPO_CARGA { get; set; }
        public string ID_CLIENTE { get; set; }
        public string DESC_CLIENTE { get; set; }
        public string ID_FACTURADO { get; set; }
        public string DESC_FACTURADO { get; set; }
        public string REFERENCIA { get; set; }

        public decimal SUBTOTAL { get; set; }
        public decimal IVA { get; set; }
        public decimal TOTAL { get; set; }



        public string DIR_FACTURADO { get; set; }
        public string EMAIL_FACTURADO { get; set; }
        public string CIUDAD_FACTURADO { get; set; }
        public Int64 DIAS_CREDITO { get; set; }
        public string SESION { get; set; }

  
        public string INVOICE_TYPE { get; set; }

        public string RUC_USUARIO { get; set; }
        public string DESC_USUARIO { get; set; }

        public int TOTAL_CAJAS_BODEGA { get; set; }
        public int TOTAL_CAJAS_MUELLE { get; set; }
        public string DRAF { get; set; }

        public int Fila { get; set; }
        public string final_nbr { get; set; }
        public Int64? draft_nbr { get; set; }
        public string payee_customer_id { get; set; }
        public string payee_customer_name { get; set; }
        public string currency_id { get; set; }
        public DateTime? finalized_date { get; set; }
        public Int64 gkey { get; set; }
        public string event_type_id { get; set; }
        public string event_id { get; set; }

        public decimal? quantity_billed { get; set; }
        public decimal? quantity { get; set; }
        public decimal? rate_billed { get; set; }
        public decimal? amount { get; set; }

        public string description { get; set; }
        public DateTime? created { get; set; }
        public decimal? tax { get; set; }
        public decimal? amount_taxt { get; set; }
        public string notes { get; set; }

        public string INVOICETYPE { get; set; }

        private static String v_mensaje = string.Empty;


        #endregion

        public BTS_Draf_Impresion()
        {
            init();

        }

        /*carga todas las facturas*/
        public static List<BTS_Draf_Impresion> Datos_Factura(Int64 ID, out string OnError)
        {
            parametros.Clear();
            parametros.Add("ID", ID);
            return sql_puntero.ExecuteSelectControl<BTS_Draf_Impresion>(sql_puntero.Conexion_Local, 4000, "Bts_Rpt_Formato_ImprimirFactura", parametros, out OnError);

        }

        
    }
}
