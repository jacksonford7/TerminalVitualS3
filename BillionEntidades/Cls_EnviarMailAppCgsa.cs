using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlConexion;


namespace BillionEntidades
{
    public class Cls_EnviarMailAppCgsa : Cls_Bil_Base
    {
        #region "Variables"
        private Int64 _IdCliente = 0;
        private string _Ruc ;
        private static Int64? lm = -3;
        private string _Email;
        #endregion

        #region "Propiedades"

        public Int64 IdCliente { get => _IdCliente; set => _IdCliente = value; }
        public string Ruc { get => _Ruc; set => _Ruc = value; }
        public string Email { get => _Email; set => _Email = value; }


        #endregion

        public Cls_EnviarMailAppCgsa()
        {
            init();
        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("PORTAL_MASTER");
        }

        private Int64? Save(out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("IdCliente", this.IdCliente);
           
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 8000, "mail_appcgsa_contenedores", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                if (!string.IsNullOrEmpty(OnError))
                {
                    return null;
                }
                else
                {
                    db = 1;
                }

            }
            OnError = string.Empty;
            return db.Value;

        }

        private Int64? Save_mail_nuevo(out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("ruc", this.Ruc);
            parametros.Add("email", this.Email);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 8000, "mail_appcgsa_contenedores_ruc", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                if (!string.IsNullOrEmpty(OnError))
                {
                    return null;
                }
                else
                {
                    db = 1;
                }

            }
            OnError = string.Empty;
            return db.Value;

        }

        private Int64? Save_Cancelar(out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("Ruc", this.Ruc);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(nueva_conexion, 8000, "mail_appcgsa_inactiva_servicio", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                if (!string.IsNullOrEmpty(OnError))
                {
                    return null;
                }
                else
                {
                    db = 1;
                }

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
                    OnError = "*** Error: al enviar email ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction), "Cls_EnviarMailAppCgsa:SaveTransaction", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }

        public Int64? SaveTransactionMail(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_mail_nuevo(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al enviar email ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransactionMail), "Cls_EnviarMailAppCgsa:SaveTransactionMail", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }

        public Int64? SaveTransactionCancelar(out string OnError)
        {

            Int64 ID = 0;
            try
            {

                //grabar transaccion.
                var id = this.Save_Cancelar(out OnError);
                if (!id.HasValue)
                {
                    OnError = "*** Error: al enviar email ****";
                    return 0;
                }
                ID = id.Value;

                return ID;

            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction), "Cls_EnviarMailAppCgsa:SaveTransaction", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
    }
}
