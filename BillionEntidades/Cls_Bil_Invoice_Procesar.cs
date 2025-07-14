using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
using System.Xml;

namespace BillionEntidades
{
    public class Cls_Bil_Invoice_Procesar : Cls_Bil_Base
    {

        private  string _xmlCabecera;
        private string  _xmlContenedor;
        private string  _xmlServicios;
        private string _xmlClientes;
        private static Int64? lm = -3;

        public string xmlCabecera { get => _xmlCabecera; set => _xmlCabecera = value; }
        public string xmlContenedor { get => _xmlContenedor; set => _xmlContenedor = value; }
        public string xmlServicios { get => _xmlServicios; set => _xmlServicios = value; }
        public string xmlClientes { get => _xmlClientes; set => _xmlClientes = value; }

        public Cls_Bil_Invoice_Procesar()
        {
            init();
           
        }

      

        #region "Graba Contenedor"
        private Int64? Save(out string OnError)
        {

            parametros.Clear();

            parametros.Add("xmlCab", this.xmlCabecera);
            parametros.Add("xmlContenedor", this.xmlContenedor);
            parametros.Add("xmlServicios", this.xmlServicios);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "sp_Bil_Procesa_Invoice", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar factura ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction), "SaveTransaction", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion

        #region "Graba CFS"
        private Int64? Save_cfs(out string OnError)
        {

            parametros.Clear();

            parametros.Add("xmlCab", this.xmlCabecera);
            parametros.Add("xmlContenedor", this.xmlContenedor);
            parametros.Add("xmlServicios", this.xmlServicios);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "sp_Bil_Procesa_Invoice_cfs", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_cfs(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_cfs(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar factura de carga suelta ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_cfs), "SaveTransaction_cfs", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion


        #region "Graba Contenedor Certificado"
        private Int64? Save_New(out string OnError)
        {

            parametros.Clear();

            parametros.Add("xmlCab", this.xmlCabecera);
            parametros.Add("xmlContenedor", this.xmlContenedor);
            parametros.Add("xmlServicios", this.xmlServicios);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "sp_Bil_Procesa_Invoice_New", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_New(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_New(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar factura ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction), "SaveTransaction_New", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion


        #region "Graba CFS factura"

        private Int64? Save_cfs_new(out string OnError)
        {

            parametros.Clear();

            parametros.Add("xmlCab", this.xmlCabecera);
            parametros.Add("xmlContenedor", this.xmlContenedor);
            parametros.Add("xmlServicios", this.xmlServicios);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "sp_Bil_Procesa_Invoice_cfs_new", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_cfs_new(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_cfs_new(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar factura de carga suelta ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_cfs_new), "SaveTransaction_cfs_new", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion


        #region "Graba Contenedor Freight Forwarder"
        private Int64? Save_Freight_Forwarder(out string OnError)
        {

            parametros.Clear();

            parametros.Add("xmlCab", this.xmlCabecera);
            parametros.Add("xmlContenedor", this.xmlContenedor);
            parametros.Add("xmlServicios", this.xmlServicios);
            parametros.Add("xmlClientes", this.xmlClientes);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "sp_Bil_Procesa_Invoice_FF", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_Freight_Forwarder(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_Freight_Forwarder(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar factura Freight_Forwarder****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Freight_Forwarder), "SaveTransaction_Freight_Forwarder", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion

        #region "Graba CFS Port to Door"
        private Int64? Save_cfs_p2d(out string OnError)
        {

            parametros.Clear();

            parametros.Add("xmlCab", this.xmlCabecera);
            parametros.Add("xmlContenedor", this.xmlContenedor);
            parametros.Add("xmlServicios", this.xmlServicios);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "p2d_Bil_Procesa_Invoice_cfs", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_cfs_p2d(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_cfs_p2d(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar factura de carga suelta (Port to Door) ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_cfs_new), "SaveTransaction_cfs_p2d", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion


        #region "Graba factura BREAK BULK"

        private Int64? Save_break_bulk(out string OnError)
        {

            parametros.Clear();

            parametros.Add("xmlCab", this.xmlCabecera);
            parametros.Add("xmlContenedor", this.xmlContenedor);
            parametros.Add("xmlServicios", this.xmlServicios);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "sp_Bil_Procesa_Invoice_break_bulk", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_break_bulk(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_break_bulk(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar factura de carga break bulk ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_break_bulk), "SaveTransaction_break_bulk", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion
    }



}
