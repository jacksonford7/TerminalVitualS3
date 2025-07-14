using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
using PasePuerta;

namespace BillionEntidades
{
  public  class Cls_Actualiza_Referencias : Cls_Bil_Base
    {
        #region "Variables"

        private Int64 _IV_ID;
    
        private string _REFERENCIA = string.Empty;
       
        #endregion

        #region "Propiedades"

        public Int64 IV_ID { get => _IV_ID; set => _IV_ID = value; }
       
        public string REFERENCIA { get => _REFERENCIA; set => _REFERENCIA = value; }
       
        #endregion

        public Cls_Actualiza_Referencias()
        {
            init();
        }

        private int? PreValidations_Update(out string msg)
        {
           

            if (string.IsNullOrEmpty(this.REFERENCIA))
            {
                msg = "Especifique la referencia";
                return 0;
            }

           

            msg = string.Empty;
            return 1;
        }


        #region "Actualiza "
        public Int64? Save_Update(out string OnError)
        {

            if (this.PreValidations_Update(out OnError) != 1)
            {
                return -1;
            }

            parametros.Clear();
            parametros.Add("REFERENCIA", this.REFERENCIA);
           
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "[Bill].[actualiza_referencias]", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_Update(out string OnError)
        {

            string resultado_otros = null;
            try
            {

                //this.IV_ID = 0;
                OnError = string.Empty;

                //using (var scope = new System.Transactions.TransactionScope())
                //{

                    //grabar cabecera de la transaccion.
                    var id = this.Save_Update(out OnError);
                    if (!id.HasValue)
                    {
                        OnError = string.Format("*** Error: al actualizar referencias, {0}  ****", OnError);
                        return 0;
                    }

                    this.IV_ID = id.Value;

                    //fin de la transaccion
                    //scope.Complete();
                    return this.IV_ID;

                //}

            }
            catch (Exception ex)
            {
                resultado_otros = ex.Message; ;
                OnError = string.Format("Ha ocurrido la excepción #{0}", resultado_otros);
                return null;
            }
        }

        #endregion


    }
}
