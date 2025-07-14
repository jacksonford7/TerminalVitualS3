using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class BTS_Prev_Cabecera : Cls_Bil_Base
    {

    

        #region "Propiedades"

        public Int64 ID { get; set ; }
        public string GLOSA { get ; set; }
        public DateTime? FECHA { get ; set ; }      
        public string TIPO_CARGA { get; set; }    
        public string ID_CLIENTE { get; set; }
        public string DESC_CLIENTE { get ; set ; }
        public string ID_FACTURADO { get; set ; }
        public string DESC_FACTURADO { get ; set ; }
        public string REFERENCIA { get; set ; }
        
        public decimal SUBTOTAL { get ; set ; }
        public decimal IVA { get ; set; }
        public decimal TOTAL { get ; set ; }


     
        public string DIR_FACTURADO { get ; set; }
        public string EMAIL_FACTURADO { get ; set; }
        public string CIUDAD_FACTURADO { get ; set; }
        public Int64 DIAS_CREDITO { get ; set ; }
        public string SESION { get ; set ; }
     
        private static String v_mensaje = string.Empty;

        public string INVOICE_TYPE { get; set ; }

        public string RUC_USUARIO { get ; set ; }
        public string DESC_USUARIO { get ; set ; }

        public int TOTAL_CAJAS_BODEGA { get; set; }
        public int TOTAL_CAJAS_MUELLE { get; set; }
        public string DRAF { get; set; }

        public string xmlCantidades { get; set; }
        private static Int64? lm = -3;

        #endregion



        public List<Cls_Prev_Detalle_Bodega> Detalle_bodega { get; set; }
        public List<Cls_Prev_Detalle_Muelle> Detalle_muelle { get; set; }
        public List<Cls_Prev_Detalle_Servicios> Detalle_Servicios { get; set; }

        public BTS_Prev_Cabecera()
        {
            init();

            this.Detalle_bodega = new List<Cls_Prev_Detalle_Bodega>();
            this.Detalle_muelle = new List<Cls_Prev_Detalle_Muelle>();
            this.Detalle_Servicios = new List<Cls_Prev_Detalle_Servicios>();

        }


        #region "Actualizar Cantidades de Muelle"
        private Int64? Save_Cantidad(out string OnError)
        {

            parametros.Clear();

            parametros.Add("xmlCantidades", this.xmlCantidades);
            parametros.Add("usuario", this.IV_USUARIO_CREA);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "Bts_Actualiza_Cajas_Muelle", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_Muelle(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_Cantidad(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al confirmar cantidades de cajas en muelle ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Muelle), "SaveTransaction_Muelle", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }

        #endregion



    }
}
