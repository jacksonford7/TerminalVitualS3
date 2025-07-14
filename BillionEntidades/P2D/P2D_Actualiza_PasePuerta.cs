using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;


namespace BillionEntidades
{
    public class P2D_Actualiza_PasePuerta : Cls_Bil_Base
    {
        private Int64 _ID_PASE;
        private string _ORDER_ID = string.Empty;
        private string _TRACKING_NUMBER = string.Empty;

        public Int64 ID_PASE { get => _ID_PASE; set => _ID_PASE = value; }
        public string ORDER_ID { get => _ORDER_ID; set => _ORDER_ID = value; }
        public string TRACKING_NUMBER { get => _TRACKING_NUMBER; set => _TRACKING_NUMBER = value; }

        public P2D_Actualiza_PasePuerta()
        {
            init();
        }

        private int? PreValidations_Update(out string msg)
        {
            if (this.ID_PASE <= 0)
            {
                msg = "Especifique el id del pase";
                return 0;
            }

            if (string.IsNullOrEmpty(this.ORDER_ID))
            {
                msg = "Especifique el # de orden";
                return 0;
            }

            if (string.IsNullOrEmpty(this.TRACKING_NUMBER))
            {
                msg = "Especifique el TRACKING_NUMBER";
                return 0;
            }

           


            msg = string.Empty;
            return 1;
        }

        public Int64? Save_Update_cfs(out string OnError)
        {

            if (this.PreValidations_Update(out OnError) != 1)
            {
                return -1;
            }

            parametros.Clear();
            parametros.Add("ID_PASE", this.ID_PASE);
            parametros.Add("ORDER_ID", this.ORDER_ID);
            parametros.Add("TRACKING_NUMBER", this.TRACKING_NUMBER);
           
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "P2D_ACTUALIZA_PASE_PUERTA_CFS", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }


        public Int64? SaveTransaction_Update_cfs(out string OnError)
        {

            string resultado_otros = null;
            try
            {

              
                OnError = string.Empty;

                using (var scope = new System.Transactions.TransactionScope())
                {

                    //grabar cabecera de la transaccion.
                    var id = this.Save_Update_cfs(out OnError);
                    if (!id.HasValue)
                    {
                        OnError = string.Format("*** Error: al actualizar detalle de pase puerta de carga suelta, {0}  ****", OnError);
                        return 0;
                    }

                    this.ID_PASE = id.Value;

                    //fin de la transaccion
                    scope.Complete();
                    return this.ID_PASE;

                }

            }
            catch (Exception ex)
            {
                resultado_otros = ex.Message; ;
                OnError = string.Format("Ha ocurrido la excepción #{0}", resultado_otros);
                return null;
            }
        }


    }
}
