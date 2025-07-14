using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Bil_Invoice_Servicios : Cls_Bil_Base
    {

        #region "Variables"

        private Int64 _IV_ID;
        private int _IV_LINEA;
        private string _IV_ID_SERVICIO = string.Empty;
        private string _IV_DESC_SERVICIO = string.Empty;
        private string _IV_CARGA = string.Empty;
        private DateTime? _IV_FECHA = null;
        private string _IV_TIPO_SERVICIO = string.Empty;

        private decimal _IV_CANTIDAD = 0;
        private decimal _IV_PRECIO = 0;
        private decimal _IV_SUBTOTAL = 0;
        private decimal _IV_IVA = 0;

        private string _IV_DRAFT = string.Empty;
        private string _IV_ID_CODIGO = string.Empty;
        #endregion

        #region "Propiedades"

        public Int64 IV_ID { get => _IV_ID; set => _IV_ID = value; }
        public int IV_LINEA { get => _IV_LINEA; set => _IV_LINEA = value; }
        public string IV_ID_SERVICIO { get => _IV_ID_SERVICIO; set => _IV_ID_SERVICIO = value; }
        public string IV_DESC_SERVICIO { get => _IV_DESC_SERVICIO; set => _IV_DESC_SERVICIO = value; }
        public string IV_CARGA { get => _IV_CARGA; set => _IV_CARGA = value; }
        public DateTime? IV_FECHA { get => _IV_FECHA; set => _IV_FECHA = value; }

        public string IV_TIPO_SERVICIO { get => _IV_TIPO_SERVICIO; set => _IV_TIPO_SERVICIO = value; }
        public decimal IV_CANTIDAD { get => _IV_CANTIDAD; set => _IV_CANTIDAD = value; }
        public decimal IV_PRECIO { get => _IV_PRECIO; set => _IV_PRECIO = value; }
        public decimal IV_SUBTOTAL { get => _IV_SUBTOTAL; set => _IV_SUBTOTAL = value; }
        public decimal IV_IVA { get => _IV_IVA; set => _IV_IVA = value; }

        public string IV_DRAFT { get => _IV_DRAFT; set => _IV_DRAFT = value; }
        public string IV_ID_CODIGO { get => _IV_ID_CODIGO; set => _IV_ID_CODIGO = value; }
        #endregion


        public Cls_Bil_Invoice_Servicios()
        {
            init();
        }


        public Cls_Bil_Invoice_Servicios(Int64 _IV_ID, int _IV_LINEA, string _IV_ID_SERVICIO, string _IV_DESC_SERVICIO, string _IV_CARGA,
                DateTime? _IV_FECHA, string _IV_TIPO_SERVICIO, decimal _IV_CANTIDAD, decimal _IV_PRECIO, decimal _IV_SUBTOTAL, decimal _IV_IVA, string _IV_DRAFT, string _IV_ID_CODIGO)
        {
            this.IV_ID = _IV_ID;
            this.IV_LINEA = _IV_LINEA;
            this.IV_ID_SERVICIO = _IV_ID_SERVICIO;
            this.IV_DESC_SERVICIO = _IV_DESC_SERVICIO;
            this.IV_CARGA = _IV_CARGA;
            this.IV_FECHA = _IV_FECHA;
            this.IV_TIPO_SERVICIO = _IV_TIPO_SERVICIO;
            this.IV_CANTIDAD = _IV_CANTIDAD;
            this.IV_PRECIO = _IV_PRECIO;
            this.IV_SUBTOTAL = _IV_SUBTOTAL;
            this.IV_IVA = _IV_IVA;
            this.IV_DRAFT = _IV_DRAFT;
            this.IV_ID_CODIGO = _IV_ID_CODIGO;
        }

        private int? PreValidations(out string msg)
        {

            if (this.IV_ID <= 0)
            {
                msg = "Especifique el id de la transacción";
                return 0;
            }

            if (this.IV_LINEA <= 0)
            {
                msg = "Especifique la fila de la transacción";
                return 0;
            }

            if (string.IsNullOrEmpty(this.IV_ID_SERVICIO))
            {
                msg = "Especifique el código del servicio de la transacción";
                return 0;
            }


            msg = string.Empty;
            return 1;
        }

        public Int64? Save(out string OnError)
        {

            if (this.PreValidations(out OnError) != 1)
            {
                return -1;
            }

            if (this.IV_ID_SERVICIO.Length > 50) { this.IV_ID_SERVICIO = this.IV_ID_SERVICIO.Substring(0, 49); }
            if (this.IV_DESC_SERVICIO.Length > 250) { this.IV_DESC_SERVICIO = this.IV_DESC_SERVICIO.Substring(0, 249); }
            if (this.IV_CARGA.Length > 50) { this.IV_CARGA = this.IV_CARGA.Substring(0, 49); }
            if (this.IV_USUARIO_CREA.Length > 50) { this.IV_USUARIO_CREA = this.IV_USUARIO_CREA.Substring(0, 49); }
            if (this.IV_DRAFT.Length > 50) { this.IV_DRAFT = this.IV_DRAFT.Substring(0, 14); }

            parametros.Clear();
            parametros.Add("IV_ID", this.IV_ID);
            parametros.Add("IV_LINEA", this.IV_LINEA);
            parametros.Add("IV_ID_SERVICIO", this.IV_ID_SERVICIO);
            parametros.Add("IV_DESC_SERVICIO", this.IV_DESC_SERVICIO);
            parametros.Add("IV_CARGA", this.IV_CARGA);
            parametros.Add("IV_FECHA", this.IV_FECHA);
            parametros.Add("IV_TIPO_SERVICIO", this.IV_TIPO_SERVICIO);
            parametros.Add("IV_CANTIDAD", this.IV_CANTIDAD);
            parametros.Add("IV_PRECIO", this.IV_PRECIO);
            parametros.Add("IV_SUBTOTAL", this.IV_SUBTOTAL);
            parametros.Add("IV_IVA", this.IV_IVA);
            parametros.Add("IV_USUARIO_CREA", this.IV_USUARIO_CREA);
            parametros.Add("IV_FECHA_CREA", this.IV_FECHA_CREA);
            parametros.Add("IV_DRAFT", this.IV_DRAFT);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 4000, "sp_Bil_inserta_invoice_servicio", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

    }
}
