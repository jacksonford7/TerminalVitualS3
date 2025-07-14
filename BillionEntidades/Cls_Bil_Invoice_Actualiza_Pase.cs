using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
using PasePuerta;
namespace BillionEntidades
{
    public class Cls_Bil_Invoice_Actualiza_Pase : Cls_Bil_Base
    {

        #region "Variables"

        private Int64 _IV_ID;
        private Int64 _IV_GKEY;
        private string _IV_MRN = string.Empty;
        private string _IV_MSN = string.Empty;
        private string _IV_HSN = string.Empty;
        private string _IV_CONTENEDOR = string.Empty;      
        private DateTime? _IV_FECHA_ULTIMA = null;  
        private DateTime? _IV_FECHA_HASTA;
        private string _IV_MODULO= string.Empty;
        #endregion

        #region "Propiedades"

        public Int64 IV_ID { get => _IV_ID; set => _IV_ID = value; }
        public Int64 IV_GKEY { get => _IV_GKEY; set => _IV_GKEY = value; }
        public string IV_MRN { get => _IV_MRN; set => _IV_MRN = value; }
        public string IV_MSN { get => _IV_MSN; set => _IV_MSN = value; }
        public string IV_HSN { get => _IV_HSN; set => _IV_HSN = value; }
        public string IV_CONTENEDOR { get => _IV_CONTENEDOR; set => _IV_CONTENEDOR = value; }    
        public DateTime? IV_FECHA_ULTIMA { get => _IV_FECHA_ULTIMA; set => _IV_FECHA_ULTIMA = value; }   
        public DateTime? IV_FECHA_HASTA { get => _IV_FECHA_HASTA; set => _IV_FECHA_HASTA = value; }
        public string IV_MODULO { get => _IV_MODULO; set => _IV_MODULO = value; }
        #endregion

        public Cls_Bil_Invoice_Actualiza_Pase()
        {
            init();
        }

        public Cls_Bil_Invoice_Actualiza_Pase(Int64 _IV_ID, Int64 _IV_GKEY, string _IV_MRN, string _IV_MSN, string _IV_HSN, string _IV_CONTENEDOR,
                     DateTime? _IV_FECHA_ULTIMA,   DateTime? _IV_FECHA_HASTA, string _IV_MODULO)
        {
            this.IV_ID = _IV_ID;
            this.IV_GKEY = _IV_GKEY;
            this.IV_MRN = _IV_MRN;
            this.IV_MSN = _IV_MSN;
            this.IV_HSN = _IV_HSN;
            this.IV_HSN = _IV_HSN;
            this.IV_CONTENEDOR = _IV_CONTENEDOR;        
            this.IV_FECHA_ULTIMA = _IV_FECHA_ULTIMA;        
            this.IV_FECHA_HASTA = _IV_FECHA_HASTA;
            this.IV_MODULO = _IV_MODULO;
        }

        private int? PreValidations_Update(out string msg)
        {
            /*
            if (this.IV_GKEY <= 0)
            {
                msg = "Especifique el id del contenedor o la carga";
                return 0;
            }*/

            if (string.IsNullOrEmpty(this.IV_MRN))
            {
                msg = "Especifique el MRN de la carga";
                return 0;
            }

            if (string.IsNullOrEmpty(this.IV_MSN))
            {
                msg = "Especifique el MSN de la carga";
                return 0;
            }

            if (string.IsNullOrEmpty(this.IV_HSN))
            {
                msg = "Especifique el HSN de la carga";
                return 0;
            }

            if (string.IsNullOrEmpty(this.IV_USUARIO_CREA))
            {
                msg = "Especifique el usuario que crea la transacción";
                return 0;
            }


            msg = string.Empty;
            return 1;
        }

        #region "Actualiza pase dias libres contenedor"
        public Int64? Save_Update(out string OnError)
        {

            if (this.PreValidations_Update(out OnError) != 1)
            {
                return -1;
            }

            parametros.Clear();
            parametros.Add("IV_ID", this.IV_ID);
            parametros.Add("IV_MRN", this.IV_MRN);
            parametros.Add("IV_MSN", this.IV_MSN);
            parametros.Add("IV_HSN", this.IV_HSN);
            parametros.Add("IV_CONTENEDOR", this.IV_CONTENEDOR);
            parametros.Add("IV_GKEY", this.IV_GKEY);
            parametros.Add("IV_FECHA_ULTIMA", this.IV_FECHA_ULTIMA);
            parametros.Add("IV_USUARIO_CREA", this.IV_USUARIO_CREA);
            parametros.Add("IV_FECHA_HASTA", this.IV_FECHA_HASTA);
            parametros.Add("IV_MODULO", this.IV_MODULO);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "sp_Bil_Actualiza_invoice_det", parametros, out OnError);
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

                using (var scope = new System.Transactions.TransactionScope())
                {

                    //grabar cabecera de la transaccion.
                    var id = this.Save_Update(out OnError);
                    if (!id.HasValue)
                    {
                        OnError = string.Format("*** Error: al actualizar detalle de pase puerta contenedor, {0}  ****", OnError);
                        return 0;
                    }

                    this.IV_ID = id.Value;

                    //fin de la transaccion
                    scope.Complete();
                    return this.IV_ID;

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

        #region "Actualiza pase dias libres carga suelta CFS"
        public Int64? Save_Update_cfs(out string OnError)
        {

            if (this.PreValidations_Update(out OnError) != 1)
            {
                return -1;
            }

            parametros.Clear();
            parametros.Add("IV_ID", this.IV_ID);
            parametros.Add("IV_MRN", this.IV_MRN);
            parametros.Add("IV_MSN", this.IV_MSN);
            parametros.Add("IV_HSN", this.IV_HSN);
            parametros.Add("IV_FECHA_ULTIMA", this.IV_FECHA_ULTIMA);
            parametros.Add("IV_USUARIO_CREA", this.IV_USUARIO_CREA);
            parametros.Add("IV_FECHA_HASTA", this.IV_FECHA_HASTA);
            parametros.Add("IV_MODULO", this.IV_MODULO);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "sp_Bil_Actualiza_invoice_det_cfs", parametros, out OnError);
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

                //this.IV_ID = 0;
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

                    this.IV_ID = id.Value;

                    //fin de la transaccion
                    scope.Complete();
                    return this.IV_ID;

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

        #region "Actualiza pase dias libres carga Break Bulk"
        public Int64? Save_Update_BreakBulk(out string OnError)
        {

            if (this.PreValidations_Update(out OnError) != 1)
            {
                return -1;
            }

            parametros.Clear();
            parametros.Add("IV_ID", this.IV_ID);
            parametros.Add("IV_MRN", this.IV_MRN);
            parametros.Add("IV_MSN", this.IV_MSN);
            parametros.Add("IV_HSN", this.IV_HSN);
            parametros.Add("IV_FECHA_ULTIMA", this.IV_FECHA_ULTIMA);
            parametros.Add("IV_USUARIO_CREA", this.IV_USUARIO_CREA);
            parametros.Add("IV_FECHA_HASTA", this.IV_FECHA_HASTA);
            parametros.Add("IV_MODULO", this.IV_MODULO);
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "sp_Bil_Actualiza_invoice_det_breakbulk", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_Update_BreakBulk(out string OnError)
        {

            string resultado_otros = null;
            try
            {

                //this.IV_ID = 0;
                OnError = string.Empty;

                using (var scope = new System.Transactions.TransactionScope())
                {

                    //grabar cabecera de la transaccion.
                    var id = this.Save_Update_BreakBulk(out OnError);
                    if (!id.HasValue)
                    {
                        OnError = string.Format("*** Error: al actualizar detalle de pase puerta de carga Break Bulk, {0}  ****", OnError);
                        return 0;
                    }

                    this.IV_ID = id.Value;

                    //fin de la transaccion
                    scope.Complete();
                    return this.IV_ID;

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
