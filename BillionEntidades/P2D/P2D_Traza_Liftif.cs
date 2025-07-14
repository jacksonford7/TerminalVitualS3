using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class P2D_Traza_Liftif : Cls_Bil_Base
    {

        #region "Variables"
        private static Int64? lm = -3;
        private Int64 _ID;
        private string _USUARIO = string.Empty;
        private string _TOKEN = string.Empty;
        private string _ORIGEN = string.Empty;
        private string _MENSAJE = string.Empty;
        private Int64 _PASE;
        private string _ESTADO = string.Empty;
        private string _ACCION = string.Empty;
        #endregion

        #region "Propiedades"

        public Int64 ID { get => _ID; set => _ID = value; }
        public string USUARIO { get => _USUARIO; set => _USUARIO = value; }
        public string TOKEN { get => _TOKEN; set => _TOKEN = value; }
        public string ORIGEN { get => _ORIGEN; set => _ORIGEN = value; }
        public string MENSAJE { get => _MENSAJE; set => _MENSAJE = value; }
        public Int64 PASE { get => _PASE; set => _PASE = value; }
        public string ESTADO { get => _ESTADO; set => _ESTADO = value; }
        public string ACCION { get => _ACCION; set => _ACCION = value; }
        #endregion


        public P2D_Traza_Liftif()
        {
            init();

        }


        private int? PreValidationsTransaction(out string msg)
        {

            if (string.IsNullOrEmpty(this.USUARIO))
            {
                msg = "Debe especificar el usuario";
                return 0;

            }

            msg = string.Empty;
            return 1;
        }

        private Int64? Save(out string OnError)
        {
          
            parametros.Clear();

            parametros.Add("USUARIO", this.USUARIO);
            parametros.Add("TOKEN", this.TOKEN);
            parametros.Add("ORIGEN", this.ORIGEN);
            parametros.Add("MENSAJE", this.MENSAJE);
            parametros.Add("PASE", this.PASE);
            parametros.Add("ESTADO", this.ESTADO);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "bil_inserta_traza_liftif", parametros, out OnError);
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
                    OnError = "*** Error: al registrar traza de P2D liftif ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction), "P2D_Traza_Liftif", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }


        #region "pase para reprocesar"
        private Int64? Save_Reproceso(out string OnError)
        {

            parametros.Clear();

            parametros.Add("USUARIO", this.USUARIO);
            parametros.Add("ACCION", this.ACCION);
            parametros.Add("PASE", this.PASE);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "P2D_INSERTA_PASES_PROCESAR", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_Reprocesar(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_Reproceso(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al registrar reproceso de pase a liftit ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Reprocesar), "SaveTransaction_Reprocesar", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }


        #endregion

    }
}
