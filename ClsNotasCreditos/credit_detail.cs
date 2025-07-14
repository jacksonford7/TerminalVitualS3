using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;

namespace ClsNotasCreditos
{
    public class credit_detail : Base
    {
        #region "Variables"

        private Int64 _nc_id;
        private int _sequence;
        private Int64 _codigo_item;
        private string _codigo_servicio = string.Empty;
        private string _unidad_bl = string.Empty;
        private string _desc_servicio = string.Empty;

        private Int64 _id_factura;
        private decimal _cantidad;
        private decimal _precio;
        private decimal _subtotal;
        private decimal _iva;
        private decimal _iva_porcentaje;

        private string _numero_carga = string.Empty;
        private decimal _nc_cantidad;
        private decimal _nc_precio;
        private decimal _nc_subtotal;
        private decimal _nc_iva;


        #endregion

        #region "Propiedades"

        public Int64 nc_id { get => _nc_id; set => _nc_id = value; }
        public int sequence { get => _sequence; set => _sequence = value; }
        public Int64 codigo_item { get => _codigo_item; set => _codigo_item = value; }
        public string codigo_servicio { get => _codigo_servicio; set => _codigo_servicio = value; }
        public string unidad_bl { get => _unidad_bl; set => _unidad_bl = value; }
        public string desc_servicio { get => _desc_servicio; set => _desc_servicio = value; }
        public Int64 id_factura { get => _id_factura; set => _id_factura = value; }

        public decimal cantidad { get => _cantidad; set => _cantidad = value; }
        public decimal precio { get => _precio; set => _precio = value; }
        public decimal subtotal { get => _subtotal; set => _subtotal = value; }
        public decimal iva { get => _iva; set => _iva = value; }
        public decimal iva_porcentaje { get => _iva_porcentaje; set => _iva_porcentaje = value; }

        public string numero_carga { get => _numero_carga; set => _numero_carga = value; }
        public decimal nc_cantidad { get => _nc_cantidad; set => _nc_cantidad = value; }
        public decimal nc_precio { get => _nc_precio; set => _nc_precio = value; }
        public decimal nc_subtotal { get => _nc_subtotal; set => _nc_subtotal = value; }
        public decimal nc_iva { get => _nc_iva; set => _nc_iva = value; }

        #endregion

        public credit_detail()
        {
            init();
        }

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
            v_conexion = Extension.Nueva_Conexion("NOTA_CREDITO");
        }


        public credit_detail(  Int64 _nc_id,int _sequence,Int64 _codigo_item,string _codigo_servicio , string _unidad_bl,string _desc_servicio ,
         Int64 _id_factura, decimal _cantidad,decimal _precio, decimal _subtotal,decimal _iva,string _numero_carga,decimal _nc_cantidad,
         decimal _nc_precio,decimal _nc_subtotal,decimal _nc_iva)
        {
            this.nc_id=_nc_id;
            this.sequence =_sequence;
            this.codigo_item = _codigo_item;
            this.codigo_servicio = _codigo_servicio;
            this.unidad_bl = _unidad_bl;
            this.desc_servicio = _desc_servicio;
            this.id_factura = _id_factura;

            this.cantidad = _cantidad;
            this.precio = _precio;
            this.subtotal = _subtotal;
            this.iva = _iva;

            this.numero_carga=_numero_carga;
            this.nc_cantidad = _nc_cantidad;
            this.nc_precio = _nc_precio;
            this.nc_subtotal = _nc_subtotal;
            this.nc_iva = _nc_iva; 
        }

        private int? PreValidations(out string msg)
        {

            if (this.nc_id <= 0)
            {
                msg = "Especifique el id de la nota de credito";
                return 0;
            }

            if (this.sequence <= 0)
            {
                msg = "Especifique la secuencia del detalle de la transacción";
                return 0;
            }

            if (this.codigo_item <= 0)
            {
                msg = "Especifique el código del producto de la transacción";
                return 0;
            }

            /*if (string.IsNullOrEmpty(this.codigo_servicio))
            {
                msg = "Especifique el código del servicio de la transacción";
                return 0;
            }*/

            if (string.IsNullOrEmpty(this.desc_servicio))
            {
                msg = "Especifique la descripción del servicio de la transacción";
                return 0;
            }

            if (this.id_factura <= 0)
            {
                msg = "Especifique el id de la factura a aplicar la nota de credito";
                return 0;
            }
         
            if (this.nc_cantidad <= 0)
            {
                msg = "la cantidad de la nota de crédito, no puede ser cero";
                return 0;
            }

            if (this.nc_precio <= 0)
            {
                msg = "el precio de la nota de crédito, no puede ser cero";
                return 0;
            }

            if (string.IsNullOrEmpty(this.Create_user))
            {
                msg = "Especifique el usuario que crea la transacción";
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
            parametros.Add("i_nc_id", this.nc_id);
            parametros.Add("i_sequence", this.sequence);
            parametros.Add("i_codigo_item", this.codigo_item);
            parametros.Add("i_codigo_servicio", this.codigo_servicio);
            parametros.Add("i_unidad_bl", this.unidad_bl);
            parametros.Add("i_desc_servicio", this.desc_servicio);
            parametros.Add("i_id_factura", this.id_factura);
            parametros.Add("i_cantidad", this.cantidad);
            parametros.Add("i_precio", this.precio);
            parametros.Add("i_subtotal", this.subtotal);
            parametros.Add("i_iva", this.iva);
            parametros.Add("i_numero_carga", this.numero_carga);
            parametros.Add("i_nc_cantidad", this.nc_cantidad);
            parametros.Add("i_nc_precio", this.nc_precio);
            parametros.Add("i_nc_subtotal", this.nc_subtotal);
            parametros.Add("i_nc_iva", this.nc_iva);
            parametros.Add("i_create_user", this.Create_user);
            parametros.Add("i_action", this.Action);

            var db = sql_pointer.ExecuteInsertUpdateDeleteReturn(v_conexion, 4000, "nc_i_credit_detail", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        /*carga datos del detalle de la factura*/
        public static List<credit_detail> Get_Detalle_Factura(Int64 _id_factura)
        {
            OnInit();
            string msg;
            parametros.Clear();
            parametros.Add("i_id_factura", _id_factura);
            return sql_pointer.ExecuteSelectControl<credit_detail>(v_conexion, 2000, "nc_get_detalle_factura", parametros, out msg);
        }

        /*carga datos del detalle de la nota de credito*/
        public static List<credit_detail> Get_credit_detail(Int64 _nc_id, out string OnError)
        {
            OnInit();      
            parametros.Clear();
            parametros.Add("i_nc_id", _nc_id);
            return sql_pointer.ExecuteSelectControl<credit_detail>(v_conexion, 2000, "nc_get_credit_detail", parametros, out OnError);
        }
    }
}
