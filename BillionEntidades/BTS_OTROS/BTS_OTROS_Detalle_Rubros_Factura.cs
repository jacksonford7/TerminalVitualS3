using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
   public  class BTS_OTROS_Detalle_Rubros_Factura : Cls_Bil_Base
    {
        private decimal _quantity_billed = 0;
        private decimal _quantity = 0;
        private decimal _rate_billed = 0;
        private decimal _amount = 0;
        private decimal _tax = 0;
        private decimal _amount_taxt = 0;

        #region "Propiedades"

        public Int64 ID { get; set; }
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

        public decimal quantity_billed { get => _quantity_billed; set => _quantity_billed = value; }
        public decimal quantity { get => _quantity; set => _quantity = value; }
        public decimal rate_billed { get => _rate_billed; set => _rate_billed = value; }
        public decimal amount { get => _amount; set => _amount = value; }

        public string description { get; set; }
        public DateTime? created { get; set; }
        public decimal tax { get => _tax; set => _tax = value; }
        public decimal amount_taxt { get => _amount_taxt; set => _amount_taxt = value; }

        public string notes { get; set; }
        #endregion

        public BTS_OTROS_Detalle_Rubros_Factura()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }



        public static List<BTS_OTROS_Detalle_Rubros_Factura> Carga_Servicios_Draf(string draft_nbr, out string OnError)
        {
            OnInit("BILLING");

            parametros.Clear();
            parametros.Add("draft_nbr", draft_nbr);
            return sql_puntero.ExecuteSelectControl<BTS_OTROS_Detalle_Rubros_Factura>(nueva_conexion, 7000, "BTS_CONSULTAR_DRAF", parametros, out OnError);

            //parametros.Clear();
            //parametros.Add("draft_nbr", draft_nbr);
            //return sql_puntero.ExecuteSelectControl<BTS_OTROS_Detalle_Rubros_Factura>(nueva_conexion, 7000, "BTS_CONUSLTAR_DRAF", parametros, out OnError);
        }




    }
}
