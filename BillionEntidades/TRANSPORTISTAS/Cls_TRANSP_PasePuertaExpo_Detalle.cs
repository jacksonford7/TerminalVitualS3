using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_TRANSP_PasePuertaExpo_Detalle : Cls_Bil_Base
    {

        #region "Variables"

        private DateTime? _aisv_fecha_ing;
        private int _item ;
        private string _aisv = string.Empty;
        private string _tipo = string.Empty;
        private string _movi = string.Empty;
        private string _boking = string.Empty;
        private string _referencia = string.Empty;
        private string _fk = string.Empty;
        private string _agencia = string.Empty;
        private string _pod = string.Empty;

        private string _dae = string.Empty;
        private string _carga = string.Empty;

        private DateTime? _fecha;
        private string _estado = string.Empty;
        private string _cntr;
        private DateTime? _aisv_fecha_llegada_turno;
       // private DateTime? _aisv_hora_llegada_turno;

        private string _aisv_cedul_chof = string.Empty;
        private string _aisv_nombr_chof = string.Empty;
        private string _aisv_placa_vehi = string.Empty;

        private string _aisv_tran_cia = string.Empty;
        private string _aisv_tran_ruc = string.Empty;

        private DateTime? _FECHA_VALIDA;
        private string _turno = string.Empty;
        private string _chofer = string.Empty;
        #endregion




        #region "Propiedades"

        public DateTime? aisv_fecha_ing { get => _aisv_fecha_ing; set => _aisv_fecha_ing = value; }
        public int item { get => _item; set => _item = value; }
        public string aisv { get => _aisv; set => _aisv = value; }
        public string tipo { get => _tipo; set => _tipo = value; }
        public string movi { get => _movi; set => _movi = value; }
        public string boking { get => _boking; set => _boking = value; }
        public string referencia { get => _referencia; set => _referencia = value; }
        public string fk { get => _fk; set => _fk = value; }
        public string agencia { get => _agencia; set => _agencia = value; }
        public string pod { get => _pod; set => _pod = value; }
        public string dae { get => _dae; set => _dae = value; }
        public string carga { get => _carga; set => _carga = value; }
        public DateTime? fecha { get => _fecha; set => _fecha = value; }

        public string estado { get => _estado; set => _estado = value; }
        public string cntr { get => _cntr; set => _cntr = value; }

        public DateTime? aisv_fecha_llegada_turno { get => _aisv_fecha_llegada_turno; set => _aisv_fecha_llegada_turno = value; }
        //public DateTime? aisv_hora_llegada_turno { get => _aisv_hora_llegada_turno; set => _aisv_hora_llegada_turno = value; }

        public string aisv_cedul_chof { get => _aisv_cedul_chof; set => _aisv_cedul_chof = value; }    
        public string aisv_nombr_chof { get => _aisv_nombr_chof; set => _aisv_nombr_chof = value; }
        public string aisv_placa_vehi { get => _aisv_placa_vehi; set => _aisv_placa_vehi = value; }
     
    
        public string aisv_tran_cia { get => _aisv_tran_cia; set => _aisv_tran_cia = value; }
        public string aisv_tran_ruc { get => _aisv_tran_ruc; set => _aisv_tran_ruc = value; }
        public DateTime? FECHA_VALIDA { get => _FECHA_VALIDA; set => _FECHA_VALIDA = value; }

        public string turno { get => _turno; set => _turno = value; }
        public string chofer { get => _chofer; set => _chofer = value; }
        #endregion





        public Cls_TRANSP_PasePuertaExpo_Detalle()
        {
            init();
        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("SERVICE");
        }


        public static List<Cls_TRANSP_PasePuertaExpo_Detalle> Listado_Pases_Expo(string ID_EMPRESA, out string OnError)
        {

            OnInit();
            parametros.Clear();
            parametros.Add("ID_EMPRESA", ID_EMPRESA);
            return sql_puntero.ExecuteSelectControl<Cls_TRANSP_PasePuertaExpo_Detalle>(nueva_conexion, 5000, "TRANSP_LISTADO_PASEPUERTA_EXPO", parametros, out OnError);

        }

    }
}
