using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Damage_Contenedor_Cliente : Cls_Bil_Base
    {

        #region "Variables"
        private static Int64? lm = -3;

        private Int64 _ID_SERVICIO;
        private Int64? _LIN_ID;
        private Int64? _DESC_ID;
        private decimal _DESC_PORCENTAJE = 0;
        private DateTime? _FECHA_ACEPTA = null;
        private string _USUARIO_ACEPTA = string.Empty;
        private string _LINEA = string.Empty;
        private string _CONTENEDOR = string.Empty;
        private Int64? _GKEY;
        private DateTime? _FECHA_FACTURA = null;
        private string _NUMERO_FACTURA = string.Empty;
        private string _MENSAJE_N4 = string.Empty;
        private string _DESC_USER_CREA = string.Empty;
        #endregion

        #region "Propiedades"

        public Int64 ID_SERVICIO { get => _ID_SERVICIO; set => _ID_SERVICIO = value; }
        public Int64? LIN_ID { get => _LIN_ID; set => _LIN_ID = value; }
        public Int64? DESC_ID { get => _DESC_ID; set => _DESC_ID = value; }
        public decimal DESC_PORCENTAJE { get => _DESC_PORCENTAJE; set => _DESC_PORCENTAJE = value; }
        public DateTime? FECHA_ACEPTA { get => _FECHA_ACEPTA; set => _FECHA_ACEPTA = value; }
       
        public string USUARIO_ACEPTA { get => _USUARIO_ACEPTA; set => _USUARIO_ACEPTA = value; }
        public string LINEA { get => _LINEA; set => _LINEA = value; }
        public string CONTENEDOR { get => _CONTENEDOR; set => _CONTENEDOR = value; }
        public Int64? GKEY { get => _GKEY; set => _GKEY = value; }
        public DateTime? FECHA_FACTURA { get => _FECHA_FACTURA; set => _FECHA_FACTURA = value; }
        public string NUMERO_FACTURA { get => _NUMERO_FACTURA; set => _NUMERO_FACTURA = value; }
        public string MENSAJE_N4 { get => _MENSAJE_N4; set => _MENSAJE_N4 = value; }
        public string DESC_USER_CREA { get => _DESC_USER_CREA; set => _DESC_USER_CREA = value; }

        private static String v_mensaje = string.Empty;

  
        #endregion


        public Damage_Contenedor_Cliente()
        {
            init();
  
        }

     
        private int? PreValidationsTransaction(out string msg)
        {

            if (string.IsNullOrEmpty(this.LINEA))
            {

                msg = "Debe especificar la Línea Naviera";
                return 0;
            }

            if (string.IsNullOrEmpty(this.CONTENEDOR))
            {

                msg = "Debe especificar el Contenedor";
                return 0;
            }

            if (string.IsNullOrEmpty(this.DESC_USER_CREA))
            {
                msg = "Debe especificar el estado que crea la transacción";
                return 0;
            }


        
            msg = string.Empty;
            return 1;
        }


        private Int64? Save(out string OnError)
        {
         
            parametros.Clear();
            parametros.Add("LINEA", this.LINEA);
            parametros.Add("CONTENEDOR", this.CONTENEDOR);
            parametros.Add("DESC_USER_CREA", this.DESC_USER_CREA);
         
           
            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "DAMAGE_REGISTRAR_CONTENEDOR_CLIENTE", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }


        public Int64? SaveTransaction(out string OnError)
        {

         
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

                    this.ID_SERVICIO = id.Value;
                   
                    //fin de la transaccion
                    scope.Complete();



                    return this.ID_SERVICIO;
                }
            }
            catch (Exception ex)
            {
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction), "SaveTransaction", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }

    }
}
