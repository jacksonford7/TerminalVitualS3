using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class ZAL_Asigna_Factura : Cls_Bil_Base
    {



        #region "Propiedades"
        public int id { get; set; }
        public string numero_factura { get; set; }


        private static String v_mensaje = string.Empty;


        #endregion


        #region "Propiedes de factura"
      
        private static Int64? lm = -3;

        #endregion


    

        public ZAL_Asigna_Factura()
        {
            init();

        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        private Int64? Actualiza(out string OnError)
        {
            //OnInit("SERVICE");

            parametros.Clear();

            parametros.Add("id", this.id);
            parametros.Add("numero_factura", this.numero_factura);


            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "zal_asigna_factura", parametros, out OnError);
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
                var id = this.Actualiza(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al actualizar factura de retiro de contenedores ****";
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




    }
}
