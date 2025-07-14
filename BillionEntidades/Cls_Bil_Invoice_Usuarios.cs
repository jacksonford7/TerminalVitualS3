using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
using System.Xml;

namespace BillionEntidades
{
    public class Cls_Bil_Invoice_Usuarios : Cls_Bil_Base
    {

       
        private string _xmlClientes;
        private static Int64? lm = -3;

     
        public string xmlClientes { get => _xmlClientes; set => _xmlClientes = value; }

        public Cls_Bil_Invoice_Usuarios()
        {
            init();
           
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }



        #region "Enviar email Freight Forwarder"
        private Int64? Save_Usuarios_Freight_Forwarder(out string OnError)
        {

            OnInit("PORTAL_MASTER");

            parametros.Clear();

            parametros.Add("xmlClientes", this.xmlClientes);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 8000, "sp_Bil_Registra_Usuarios_FF", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_Usuarios_Freight_Forwarder(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_Usuarios_Freight_Forwarder(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al grabar Usuarios Freight_Forwarder****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Usuarios_Freight_Forwarder), "SaveTransaction_Usuarios_Freight_Forwarder", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion
    }



}
