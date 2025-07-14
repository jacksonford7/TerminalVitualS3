using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class P2D_MULTI_Facturacion : Cls_Bil_Base
    {
        private static Int64? lm = -3;

        #region "Propiedades"

        public string xmlCabecera { get ; set ; }
        public string xmlServicios { get; set; }
        public string xmlDetalle { get ; set ; }

        #endregion

        public P2D_MULTI_Facturacion()
        {
            init();

        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        private Int64? Save_factura_manual(out string OnError)
        {

            parametros.Clear();
            parametros.Add("xmlCab", this.xmlCabecera);
            parametros.Add("xmlDet", this.xmlDetalle);
            parametros.Add("xmlServ", this.xmlServicios);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "cfs_bil_procesa_Invoice_fforwarder", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_MultiDespacho(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_factura_manual(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar factura de multidespacho/transporte CFS ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_MultiDespacho), "SaveTransaction_MultiDespacho", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }


    }
}
