using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClsProformas;
using ControlOPC.Entidades;


namespace ClsProformas
{
    public class Pases_Saldos : Base
    {
        #region "Variables"

        private string _ruc = string.Empty;
        private Int64 _id_pase = 0;
        private string _booking = string.Empty;
        private string _referencia = string.Empty;
        private string _fecha_salida = string.Empty;
        private string _turno = string.Empty;
        private string _placa = string.Empty;
        private string _chofer = string.Empty;
        private string _liquidacion = string.Empty;
        private string _estado_pago = string.Empty;
        private string _estado_pase = string.Empty;
        private Int64 _id_proforma = 0;
        private int _fila = 0;
        private decimal _valor_pase = 0;
        private decimal _saldo_pase = 0;

        #endregion

        #region "Propiedades"

        public string ruc { get => _ruc; set => _ruc = value; }
        public Int64 id_pase { get => _id_pase; set => _id_pase = value; }
        public string booking { get => _booking; set => _booking = value; }
        public string referencia { get => _referencia; set => _referencia = value; }
        public string fecha_salida { get => _fecha_salida; set => _fecha_salida = value; }
        public string turno { get => _turno; set => _turno = value; }
        public string placa { get => _placa; set => _placa = value; }
        public string chofer { get => _chofer; set => _chofer = value; }
        public string liquidacion { get => _liquidacion; set => _liquidacion = value; }
        public string estado_pago { get => _estado_pago; set => _estado_pago = value; }
        public string estado_pase { get => _estado_pase; set => _estado_pase = value; }
        public Int64 id_proforma { get => _id_proforma; set => _id_proforma = value; }
        public int fila { get => _fila; set => _fila = value; }
        public decimal valor_pase { get => _valor_pase; set => _valor_pase = value; }
        public decimal saldo_pase { get => _saldo_pase; set => _saldo_pase = value; }

        #endregion

        private static void OnInit_N4()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
            v_conexion = Extension.Nueva_Conexion("PORTAL_N4");
        }


        public Pases_Saldos()
        {
            OnInit_N4();
            this.Detalle = new List<Pases_Saldos>();
        }

        /*busca un nivel en especifico de la base de datos de nota de credito*/
        public static List<Pases_Saldos> Detalle_Pases(string _id_ruc, long _id_deposito)
        {
            OnInit_N4();
            string msg;
            parametros.Clear();
            parametros.Add("i_ruc", _id_ruc);
            parametros.Add("deposito", _id_deposito);
            return sql_pointer.ExecuteSelectControl<Pases_Saldos>(v_conexion, 4000, "SP_GET_DET_PASE_ZAL_SALDOS", parametros, out msg);
        }

        public List<Pases_Saldos> Detalle { get; set; }



    }
}
