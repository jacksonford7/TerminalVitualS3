using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
using System.Xml;

namespace BillionEntidades
{
    public class cfs_procesa_subsecuencias : Cls_Bil_Base
    {

        private  string _xmlCabecera;
      
        private static Int64? lm = -3;

        public string xmlCabecera { get => _xmlCabecera; set => _xmlCabecera = value; }
      

        public cfs_procesa_subsecuencias()
        {
            init();
           
        }

      

        #region "Graba Subsecuencias"
        private Int64? Save(out string OnError)
        {

            parametros.Clear();

            parametros.Add("xmlCab", this.xmlCabecera);
          
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "sp_Bil_Procesa_SubSecuencias", parametros, out OnError);
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
                    OnError = "*** Error: al grabar subsecuencias ****";
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

      
    }



}
