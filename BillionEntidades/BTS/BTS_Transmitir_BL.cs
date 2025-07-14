using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class BTS_Transmitir_BL : Cls_Bil_Base
    {


        #region "Propiedades"


        public Int64 ID { get; set; }
        public string NBR { get; set; }
        public string LINE { get; set; }
        public string VISIT { get; set; }
        public string CATEGORY { get; set; }
        public string SHIPPER { get; set; }
        public string CONSIGNEE { get; set; }
        public string CARGOFIRST { get; set; }
        public string CARGOSECOND { get; set; }
        public string OPERATION { get; set; }
        public string WEIGHT { get; set; }
        public string VOLUME { get; set; }
        public string CANWEIGHT { get; set; }
        public string POL { get; set; }
        public string NOTES { get; set; }
        public string ITEMNBR { get; set; }
        public string COMMODITY { get; set; }
        public string PRODUCT { get; set; }
        public string PACKAGING { get; set; }
        public string TOTALWEIGHT { get; set; }
        public string QTY { get; set; }
        public string MARKS { get; set; }
        public string POSITION { get; set; }
        public string USER { get; set; }
        private static String v_mensaje = string.Empty;
        public string MENSAJE { get; set; }

        #endregion

        public BTS_Transmitir_BL()
        {
            init();

        }

        /*carga todas las facturas*/
        public static List<BTS_Transmitir_BL> Informacion_BL(Int64 ID, out string OnError)
        {
            parametros.Clear();
            parametros.Add("ID", ID);
            return sql_puntero.ExecuteSelectControl<BTS_Transmitir_BL>(sql_puntero.Conexion_Local, 4000, "BTS_TRANSMITIR_BL", parametros, out OnError);

        }

        public Int64? Save(out string OnError)
        {
           

            parametros.Clear();
            parametros.Add("NBR", this.NBR);
            parametros.Add("ID", this.ID);
            parametros.Add("MENSAJE", this.MENSAJE);
            parametros.Add("USUARIO", this.USER);
         

            using (var scope = new System.Transactions.TransactionScope())
            {

                var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "bts_registra_bl_generados", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {

                    return null;
                }
                OnError = string.Empty;
                scope.Complete();
                return db.Value;
            }

        }

    }
}
