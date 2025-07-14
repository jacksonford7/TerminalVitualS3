using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
namespace BillionEntidades
{
    public class Cls_Bil_Sesion : Cls_Bil_Base
    {
        #region "Variables"

      
        private string _USUARIO_CREA = string.Empty;
     
        #endregion

        #region "Propiedades"

        public string USUARIO_CREA { get => _USUARIO_CREA; set => _USUARIO_CREA = value; }
   
        private static String v_mensaje = string.Empty;

        #endregion


        public Cls_Bil_Sesion()
        {
            init();

          
        }

        private int? PreValidationsTransaction(out string msg)
        {

         
            if (string.IsNullOrEmpty(this.USUARIO_CREA))
            {
                msg = "Debe especificar el usuario ";
                return 0;

            }

          
            msg = string.Empty;
            return 1;
        }

        private Int64? Save(out string OnError)
        {

            parametros.Clear();
            parametros.Add("USUARIO_CREA", this.USUARIO_CREA);
         
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 4000, "sp_Bil_inserta_sesion", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction(out string OnError)
        {

            string resultado_otros = null;
            try
            {
                //validaciones previas
                if (this.PreValidationsTransaction(out OnError) != 1)
                {
                    return 0;
                }
                using (var scope = new System.Transactions.TransactionScope())
                {
                    //grabar cabecera de la transaccion.
                    var id = this.Save(out OnError);
                    if (!id.HasValue)
                    {
                        return 0;
                    }

                    //fin de la transaccion
                    scope.Complete();



                    return id.Value;
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
