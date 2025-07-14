using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class BTS_Cabecera_Eventos : Cls_Bil_Base
    {



        #region "Propiedades"

        public Int64 id { get; set; }
        public string referencia { get; set; }
        public DateTime? fecha { get; set; }
        public Int64 id_servicio { get; set; }
        public int id_exportador { get; set; }
        public string desc_exportador { get; set; }
        public int cajas { get; set; }
        public int idModalidad { get; set; }
        public string comentario { get; set; }
        public bool estado { get; set; }
        public string usuario_reg { get; set; }
        public string usuario_mod { get; set; }
        public DateTime? fecha_mod { get; set; }

        private static Int64? lm = -3;

        #endregion



        public List<BTS_Detalle_Eventos> Detalle_Eventos { get; set; }
      

        public BTS_Cabecera_Eventos()
        {
            init();

            this.Detalle_Eventos = new List<BTS_Detalle_Eventos>();
          

        }


        #region "Grabar Eventos"
        private Int64? Save(out string OnError)
        {

            parametros.Clear();

            parametros.Add("referencia", this.referencia);
            parametros.Add("id_servicio", this.id_servicio);
            parametros.Add("id_exportador", this.id_exportador);
            parametros.Add("desc_exportador", this.desc_exportador);
            parametros.Add("cajas", this.cajas);
            parametros.Add("comentario", this.comentario);
            parametros.Add("usuario_reg", this.usuario_reg);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "Bts_Registra_Eventos_Exportador", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_Eventos(out string OnError)
        {

            Int64 ID = 0;
            try
            {
                using (var scope = new System.Transactions.TransactionScope())
                {

                    var id = this.Save(out OnError);
                    if (!id.HasValue)
                    {
                        OnError = "*** Error: al registrar eventos de exportador ****";
                        return 0;
                    }

                    ID = id.Value;
                    scope.Complete();
                }

                   
                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Eventos), "SaveTransaction_Eventos", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }

        #endregion


        public bool Delete(out string OnError)
        {
            parametros.Clear();
            parametros.Add("id", this.id);
            parametros.Add("usuario_mod", this.usuario_mod);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_puntero.ExecuteInsertUpdateDelete(sql_puntero.Conexion_Local, 6000, "BTS_ELIMINAR_EVENTO", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }

    }
}
