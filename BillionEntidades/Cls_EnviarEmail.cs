using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlConexion;


namespace BillionEntidades
{
    public class Cls_EnviarEmail : Cls_Bil_Base
    {
        #region "Variables"
        private Int64 _id = 0;
        private static Int64? lm = -3;

        private string _mail_cliente;
        private string _Cliente;
        private decimal _SaldoPendiente;
        private decimal _ValorVencido;
        private decimal _ValorPendiente;

        #endregion

        #region "Propiedades"

        public Int64 id { get => _id; set => _id = value; }

        public string mail_cliente { get => _mail_cliente; set => _mail_cliente = value; }
        public string Cliente { get => _Cliente; set => _Cliente = value; }

        public decimal SaldoPendiente { get => _SaldoPendiente; set => _SaldoPendiente = value; }
        public decimal ValorVencido { get => _ValorVencido; set => _ValorVencido = value; }
        public decimal ValorPendiente { get => _ValorPendiente; set => _ValorPendiente = value; }


        #endregion

        public Cls_EnviarEmail()
        {
            init();
        }

        private Int64? Save(out string OnError)
        {

            parametros.Clear();
            parametros.Add("mail_cliente", this.mail_cliente);
            parametros.Add("SaldoPendiente", this.SaldoPendiente);
            parametros.Add("ValorVencido", this.ValorVencido);
            parametros.Add("ValorPendiente", this.ValorPendiente);
            parametros.Add("Cliente", this.Cliente);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 8000, "Notificacion_Cartera_Sap", parametros, out OnError);
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

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction), "Cls_EnviarEmail:SaveTransaction", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }

    }
}
