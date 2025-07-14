using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class BTS_Procesa_Draf : Cls_Bil_Base
    {
        public string xmlCabecera { get ; set ; }
        public string xmlBodega { get ; set ; }
        public string xmlMuelle { get ; set ; }
        public string xmlServicios { get; set ; }
        public Int64 ID { get; set; }
        private static Int64? lm = -3;

        public string USUARIO_FINALIZA { get; set; }
        public string FACTURA { get; set; }
     

        public BTS_Procesa_Draf()
        {
            init();

        }


        #region "Graba factura BTS"

        private Int64? Save_Factura(out string OnError)
        {

            parametros.Clear();

            parametros.Add("xmlCabecera", this.xmlCabecera);
            parametros.Add("xmlBodega", this.xmlBodega);
            parametros.Add("xmlMuelle", this.xmlMuelle);
            parametros.Add("xmlServicios", this.xmlServicios);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "Bts_Procesa_Draf", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_BTS(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_Factura(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar factura de BTS ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_BTS), "SaveTransaction_BTS", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion


        #region "Anular draf"

        private Int64? Anular_Update(out string OnError)
        {

            parametros.Clear();
            parametros.Add("ID", this.ID);
            parametros.Add("USUARIO_MOD", this.IV_USUARIO_CREA);
 

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "Bts_Anula_FacturaAgencia", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_Anular_Update(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Anular_Update(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al anular draf ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Anular_Update), "SaveTransaction_Anular_Update", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }

        #endregion

        #region "actualiza factura"

        private Int64? Actualiza_Draf(out string OnError)
        {

            parametros.Clear();
            parametros.Add("ID", this.ID);
            parametros.Add("USUARIO_FINALIZA", this.USUARIO_FINALIZA);
            parametros.Add("FACTURA", this.FACTURA);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "Bts_Actualiza_FacturaAgencia", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_Actualizar_Draf(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Actualiza_Draf(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al finalizar draf, asignación de factura  ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Actualizar_Draf), "SaveTransaction_Actualizar_Draf", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }

        #endregion

    }
}
