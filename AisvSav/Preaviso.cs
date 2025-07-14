using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;


namespace ClsAisvSav
{
    public class Preaviso : Base
    {

        #region "Variables"

        private Int64 _id = 0;
        private Int64 _unidad_key = 0;
        private string _turno_id = string.Empty;
        private DateTime _turno_fecha ;
        private string _turno_hora = string.Empty;
        private string _unidad_id= string.Empty;
        private string _unidad_tamano = string.Empty;
        private string _unidad_linea = string.Empty;
        private string _unidad_booking = string.Empty;
        private string _unidad_referencia = string.Empty;
        private string _unidad_estatus = string.Empty;

        private string _chofer_licencia = string.Empty;
        private string _chofer_nombre = string.Empty;
        private string _vehiculo_placa = string.Empty;
        private string _vehiculo_desc = string.Empty;
        private bool _active = false;
        private string _documento_id = string.Empty;
        private string _documento_estado = string.Empty;
        private DateTime? _documento_fecha;

        private string _liquidacion = string.Empty;
        private string _estado_pase = string.Empty;
        private string _turno_fecha_dos = string.Empty;
        private string _estado_pago = string.Empty;
        private Int64 _id_proforma = 0;
        private decimal _valor_pase = 0;
        private decimal _saldo_pase = 0;
        private int _fila = 0;
        #endregion

        #region "Propiedades"
        public Int64 id { get => _id; set => _id = value; }
        public Int64 unidad_key { get => _unidad_key; set => _unidad_key = value; }
        public string turno_id { get => _turno_id; set => _turno_id = value; }
        public DateTime turno_fecha { get => _turno_fecha; set => _turno_fecha = value; }
        public string turno_hora { get => _turno_hora; set => _turno_hora = value; }
        public string unidad_id { get => _unidad_id; set => _unidad_id = value; }
        public string unidad_tamano { get => _unidad_tamano; set => _unidad_tamano = value; }
        public string unidad_linea { get => _unidad_linea; set => _unidad_linea = value; }
        public string unidad_booking { get => _unidad_booking; set => _unidad_booking = value; }
        public string unidad_referencia { get => _unidad_referencia; set => _unidad_referencia = value; }
        public string unidad_estatus { get => _unidad_estatus; set => _unidad_estatus = value; }
        public string chofer_licencia { get => _chofer_licencia; set => _chofer_licencia = value; }
        public string chofer_nombre { get => _chofer_nombre; set => _chofer_nombre = value; }
        public string vehiculo_placa { get => _vehiculo_placa; set => _vehiculo_placa = value; }
        public string vehiculo_desc { get => _vehiculo_desc; set => _vehiculo_desc = value; }
        public bool active { get => _active; set => _active = value; }
        public string documento_id { get => _documento_id; set => _documento_id = value; }
        public string documento_estado { get => _documento_estado; set => _documento_estado = value; }
        public DateTime? documento_fecha { get => _documento_fecha; set => _documento_fecha = value; }

        public string liquidacion { get => _liquidacion; set => _liquidacion = value; }
        public string estado_pase { get => _estado_pase; set => _estado_pase = value; }
        public string turno_fecha_dos { get => _turno_fecha_dos; set => _turno_fecha_dos = value; }
        public string estado_pago { get => _estado_pago; set => _estado_pago = value; }
        public Int64 id_proforma { get => _id_proforma; set => _id_proforma = value; }
        public decimal valor_pase { get => _valor_pase; set => _valor_pase = value; }
        public decimal saldo_pase { get => _saldo_pase; set => _saldo_pase = value; }
        public int fila { get => _fila; set => _fila = value; }
        #endregion

        private static String v_mensaje = string.Empty;
        public List<Preaviso> Detalle { get; set; }

        public Preaviso()
        {
            init();

            this.Detalle = new List<Preaviso>();

        }

        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();

            v_conexion = Extension.Nueva_Conexion("SERVICES");
        }


        public static List<Preaviso> Cargar_Preaviso(int id,  out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("id", id);
            return sql_pointer.ExecuteSelectControl<Preaviso>(v_conexion, 6000, "sav_carga_preaviso", parametros, out OnError);
        }

        public static List<Preaviso> Detalle_Pases_Saldo(string _i_ruc, out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("i_ruc", _i_ruc);
            return sql_pointer.ExecuteSelectControl<Preaviso>(v_conexion, 6000, "sav_listar_pases_saldos", parametros, out OnError);
        }

        public static List<Preaviso> Detalle_Pases_Saldo_Repcontver(string _i_ruc, Int64 deposito_id, out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("i_ruc", _i_ruc);
            parametros.Add("deposito_id", deposito_id);
            return sql_pointer.ExecuteSelectControl<Preaviso>(v_conexion, 6000, "sav_listar_pases_saldos_repcontver", parametros, out OnError);
        }
    }
}
