using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;


namespace BillionEntidades
{
    public class Cls_Bil_Proforma_Servicios : Cls_Bil_Base
    {

        #region "Variables"

        private Int64 _PF_ID;
        private int _PF_LINEA;
        private string _PF_ID_SERVICIO = string.Empty;
        private string _PF_DESC_SERVICIO = string.Empty;
        private string _PF_CARGA = string.Empty;
        private DateTime? _PF_FECHA = null;
        private string _PF_TIPO_SERVICIO = string.Empty;

        private decimal _PF_CANTIDAD = 0;
        private decimal _PF_PRECIO = 0;
        private decimal _PF_SUBTOTAL = 0;
        private decimal _PF_IVA = 0;

        #endregion

        #region "Propiedades"

        public Int64 PF_ID { get => _PF_ID; set => _PF_ID = value; }
        public int PF_LINEA { get => _PF_LINEA; set => _PF_LINEA = value; }
        public string PF_ID_SERVICIO { get => _PF_ID_SERVICIO; set => _PF_ID_SERVICIO = value; }
        public string PF_DESC_SERVICIO { get => _PF_DESC_SERVICIO; set => _PF_DESC_SERVICIO = value; }
        public string PF_CARGA { get => _PF_CARGA; set => _PF_CARGA = value; }
        public DateTime? PF_FECHA { get => _PF_FECHA; set => _PF_FECHA = value; }

        public string PF_TIPO_SERVICIO { get => _PF_TIPO_SERVICIO; set => _PF_TIPO_SERVICIO = value; }
        public decimal PF_CANTIDAD { get => _PF_CANTIDAD; set => _PF_CANTIDAD = value; }
        public decimal PF_PRECIO { get => _PF_PRECIO; set => _PF_PRECIO = value; }
        public decimal PF_SUBTOTAL { get => _PF_SUBTOTAL; set => _PF_SUBTOTAL = value; }
        public decimal PF_IVA { get => _PF_IVA; set => _PF_IVA = value; }

        #endregion


        public Cls_Bil_Proforma_Servicios()
        {
            init();
        }


        public Cls_Bil_Proforma_Servicios(Int64 _PF_ID, int _PF_LINEA, string _PF_ID_SERVICIO, string _PF_DESC_SERVICIO, string _PF_CARGA,
                DateTime? _PF_FECHA, string _PF_TIPO_SERVICIO, decimal _PF_CANTIDAD, decimal _PF_PRECIO, decimal _PF_SUBTOTAL, decimal _PF_IVA)
        {
            this.PF_ID = _PF_ID;
            this.PF_LINEA = _PF_LINEA;
            this.PF_ID_SERVICIO = _PF_ID_SERVICIO;
            this.PF_DESC_SERVICIO = _PF_DESC_SERVICIO;
            this.PF_CARGA = _PF_CARGA;
            this.PF_FECHA = _PF_FECHA;
            this.PF_TIPO_SERVICIO = _PF_TIPO_SERVICIO;
            this.PF_CANTIDAD = _PF_CANTIDAD;
            this.PF_PRECIO = _PF_PRECIO;
            this.PF_SUBTOTAL = _PF_SUBTOTAL;
            this.PF_IVA = _PF_IVA;
        }

        private int? PreValidations(out string msg)
        {

            if (this.PF_ID <= 0)
            {
                msg = "Especifique el id de la transacción";
                return 0;
            }

            if (this.PF_LINEA <= 0)
            {
                msg = "Especifique la fila de la transacción";
                return 0;
            }

            if (string.IsNullOrEmpty(this.PF_ID_SERVICIO))
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
            parametros.Clear();
            parametros.Add("PF_ID", this.PF_ID);
            parametros.Add("PF_LINEA", this.PF_LINEA);
            parametros.Add("PF_ID_SERVICIO", this.PF_ID_SERVICIO);
            parametros.Add("PF_DESC_SERVICIO", this.PF_DESC_SERVICIO);
            parametros.Add("PF_CARGA", this.PF_CARGA);
            parametros.Add("PF_FECHA", this.PF_FECHA);
            parametros.Add("PF_TIPO_SERVICIO", this.PF_TIPO_SERVICIO);
            parametros.Add("PF_CANTIDAD", this.PF_CANTIDAD);
            parametros.Add("PF_PRECIO", this.PF_PRECIO);
            parametros.Add("PF_SUBTOTAL", this.PF_SUBTOTAL);
            parametros.Add("PF_IVA", this.PF_IVA);
            parametros.Add("PF_USUARIO_CREA", this.IV_USUARIO_CREA);
            parametros.Add("PF_FECHA_CREA", this.IV_FECHA_CREA);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 4000, "sp_Bil_inserta_proforma_servicio", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }


    }
}
