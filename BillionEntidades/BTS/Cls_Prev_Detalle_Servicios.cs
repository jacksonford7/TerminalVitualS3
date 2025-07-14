using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
   public  class Cls_Prev_Detalle_Servicios : Cls_Bil_Base
    {

  

        #region "Propiedades"

        public Int64 ID { get; set ; }
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
        #endregion

        public Cls_Prev_Detalle_Servicios()
        {
            init();
        }


        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        public static List<Cls_Prev_Detalle_Servicios> Carga_Servicios_Draf(string draft_nbr,  out string OnError)
        {
            OnInit("BILLING");

            parametros.Clear();
            parametros.Add("draft_nbr", draft_nbr);
            return sql_puntero.ExecuteSelectControl<Cls_Prev_Detalle_Servicios>(nueva_conexion, 7000, "BTS_CONSULTAR_DRAF", parametros, out OnError);
        }
    }
}
