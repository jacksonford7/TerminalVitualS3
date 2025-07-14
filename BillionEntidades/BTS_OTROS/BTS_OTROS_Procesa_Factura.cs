using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class BTS_OTROS_Procesa_Factura : Cls_Bil_Base
    {

    

        #region "Propiedades"

        public Int64 ID { get; set ; }
       

        public string xmlCabecera { get; set; }
        public string xmlExportador { get; set; }
        public string xmlRubros { get; set; }
        public string xmlServicios { get; set; }
        private static Int64? lm = -3;

        #endregion



      
        public BTS_OTROS_Procesa_Factura()
        {
            init();

         
        }



        #region "Graba factura BTS del Exportador"
        private Int64? Save_Factura(out string OnError)
        {

            parametros.Clear();

            parametros.Add("xmlCabecera", this.xmlCabecera);
            parametros.Add("xmlExportador", this.xmlExportador);
            parametros.Add("xmlRubros", this.xmlRubros);
            parametros.Add("xmlServicios", this.xmlServicios);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "Bts_Procesa_Factura_Exportador", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_BTS_Exportador(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_Factura(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar factura de BTS del Exportador ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_BTS_Exportador), "SaveTransaction_BTS_Exportador", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }

        #endregion



        #region "Graba factura BTS del Exportador Otros Servicios"
        private Int64? Save_Factura_Otros(out string OnError)
        {

            parametros.Clear();

            parametros.Add("xmlCabecera", this.xmlCabecera);
            parametros.Add("xmlExportador", this.xmlExportador);
            parametros.Add("xmlRubros", this.xmlRubros);
            parametros.Add("xmlServicios", this.xmlServicios);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "Bts_Procesa_Factura_Exportador_Otros", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_BTS_Exportador_Otros(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_Factura_Otros(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar factura de BTS del Exportador Otros Servicios ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_BTS_Exportador_Otros), "SaveTransaction_BTS_Exportador_Otros", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }

        #endregion



        #region "Graba factura BTS del Exportador Eventos"
        private Int64? Save_Factura_Eventos(out string OnError)
        {

            parametros.Clear();

            parametros.Add("xmlCabecera", this.xmlCabecera);
            parametros.Add("xmlExportador", this.xmlExportador);
            parametros.Add("xmlRubros", this.xmlRubros);
            parametros.Add("xmlServicios", this.xmlServicios);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "Bts_Procesa_Factura_Exportador_Eventos", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_BTS_Exportador_Eventos(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_Factura_Eventos(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar factura de BTS del Exportador Otros Servicios adicionales ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_BTS_Exportador_Eventos), "SaveTransaction_BTS_Exportador_Eventos", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }

        #endregion
    }
}
