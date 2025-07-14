using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Bil_PasePuertaBRBK_Temporal : Cls_Bil_Base
    {
        #region "Variables"

      
        private Int64 _IDX_ROW = 0;
        private string _MRN = string.Empty;
        private string _MSN = string.Empty;
        private string _HSN = string.Empty;
        private string _USUARIO_CREA = string.Empty;
        private Int64 _SECUENCIA = 0;

        private Int64 _ID_PASE = 0;
        #endregion

        #region "Propiedades"

        public Int64 IDX_ROW { get => _IDX_ROW; set => _IDX_ROW = value; }
    
        public string MRN { get => _MRN; set => _MRN = value; }
        public string MSN { get => _MSN; set => _MSN = value; }
        public string HSN { get => _HSN; set => _HSN = value; }

        private static String v_mensaje = string.Empty;

      
        public string USUARIO_CREA { get => _USUARIO_CREA; set => _USUARIO_CREA = value; }
        public Int64 SECUENCIA { get => _SECUENCIA; set => _SECUENCIA = value; }

        public Int64 ID_PASE { get => _ID_PASE; set => _ID_PASE = value; }
        #endregion

        public Cls_Bil_PasePuertaBRBK_Temporal()
        {
            init();

        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("N4Middleware");
        }

        private int? PreValidationsTransaction(out string msg)
        {

            if (this._IDX_ROW == 0)
            {

                msg = "Debe seleccionar un plan/turno";
                return 0;
            }

           
            msg = string.Empty;
            return 1;
        }

        private Int64? Save(out string OnError)
        {
             //OnInit();

            parametros.Clear();
            parametros.Add("IDX_ROW", this.IDX_ROW);
            parametros.Add("MRN", this.MRN);
            parametros.Add("MSN", this.MSN);
            parametros.Add("HSN", this.HSN);
            parametros.Add("USUARIO_CREA", this.USUARIO_CREA);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "[Brbk].[brbk_nuevo_turno_temporal]", parametros, out OnError);
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
                        OnError = "*** Error: al grabar detalle de reserva de turnos temporales Break Bulk ****";
                        return 0;
                    }

                    //fin de la transaccion
                    scope.Complete();

                    return id;
                }
            }
            catch (Exception ex)
            {
                resultado_otros = ex.Message; ;
                OnError = string.Format("Ha ocurrido la excepción #{0}", resultado_otros);
                return null;
            }
        }

        public bool Delete(out string OnError)
        {
            parametros.Clear();
            parametros.Add("IDX_ROW", this.IDX_ROW);
            parametros.Add("SECUENCIA", this.SECUENCIA);
            using (var scope = new System.Transactions.TransactionScope())
            {
                var db = sql_puntero.ExecuteInsertUpdateDelete(sql_puntero.Conexion_Local, 6000, "[Brbk].[ELIMINA_TURNO_TEMPORAL]", parametros, out OnError);
                if (!db.HasValue || db.Value < 0)
                {
                    return false;
                }
                scope.Complete();
            }
            return true;
        }


        #region "Agregar turno temporal, cuando es actualizacion de pase"
        private Int64? Save_Update(out string OnError)
        {
            //OnInit();

            parametros.Clear();
            parametros.Add("IDX_ROW", this.IDX_ROW);
            parametros.Add("MRN", this.MRN);
            parametros.Add("MSN", this.MSN);
            parametros.Add("HSN", this.HSN);
            parametros.Add("ID_PASE", this.ID_PASE);
            parametros.Add("USUARIO_CREA", this.USUARIO_CREA);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "[Brbk].[brbk_nuevo_turno_temporal_update]", parametros, out OnError);
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
                //validaciones previas
                if (this.PreValidationsTransaction(out OnError) != 1)
                {
                    return 0;
                }
                using (var scope = new System.Transactions.TransactionScope())
                {
                    //grabar cabecera de la transaccion.
                    var id = this.Save_Update(out OnError);
                    if (!id.HasValue)
                    {
                        OnError = "*** Error: al grabar detalle de reserva de turnos temporales Break Bulk ****";
                        return 0;
                    }

                    //fin de la transaccion
                    scope.Complete();

                    return id;
                }
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
