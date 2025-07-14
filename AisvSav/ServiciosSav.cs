using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControlOPC.Entidades;

namespace ClsAisvSav
{
    public class ServiciosSav : Base
    {
        #region "Variables"

        private string _codigo = string.Empty;
        private string _descripcion = string.Empty;
        private decimal _cantidad = 0;
        private decimal _costo = 0;
        private decimal _vtotal = 0;
        private bool _opcional;
        private bool _aplica;
        private string _nota = string.Empty;
        private string _contenido = string.Empty;
        private string _html = string.Empty;
        private string _tipo = string.Empty;
        private int _num;
        private bool _icont;
        private string _mensaje = string.Empty;
        private decimal _iva = 0;
        #endregion

        #region "Propiedades"

        public string codigo { get => _codigo; set => _codigo = value; }
        public string descripcion { get => _descripcion; set => _descripcion = value; }
        public decimal cantidad { get => _cantidad; set => _cantidad = value; }
        public decimal costo { get => _costo; set => _costo = value; }
        public decimal vtotal { get => _vtotal; set => _vtotal = value; }
        public bool opcional { get => _opcional; set => _opcional = value; }
        public bool aplica { get => _aplica; set => _aplica = value; }
        public string nota { get => _nota; set => _nota = value; }
        public string contenido { get => _contenido; set => _contenido = value; }
        public string html { get => _html; set => _html = value; }
        public string tipo { get => _tipo; set => _tipo = value; }
        public int num { get => _num; set => _num = value; }
        public bool icont { get => _icont; set => _icont = value; }
        public string mensaje { get => _mensaje; set => _mensaje = value; }
        public decimal iva { get => _iva; set => _iva = value; }
        #endregion

        /*conexion proformas*/
        private static void OnInit()
        {
            sql_pointer = (sql_pointer == null) ? SQLHandler.sql_handler.GetInstance() : sql_pointer;
            parametros = new Dictionary<string, object>();
            v_conexion = Extension.Nueva_Conexion("validar");
        }

        public ServiciosSav()
        {
            OnInit();

            this.Detalle = new List<ServiciosSav>();
        }

        public ServiciosSav(string _codigo, string _descripcion, decimal _cantidad, decimal _costo, bool _opcional, bool _aplica, string _nota,
         string _contenido, string _html, string _tipo, int _num, bool _icont, string _mensaje, decimal _iva)
        {
            this.codigo = _codigo;
            this.descripcion = _descripcion;
            this.cantidad = _cantidad;
            this.costo = _costo;
            this.opcional = _opcional;
            this.aplica = _aplica;
            this.nota = _nota;
            this.contenido = _contenido;
            this.html = _html;
            this.tipo = _tipo;
            this.num = _num;
            this.icont = _icont;
            this.mensaje = _mensaje;
            this.iva = _iva;
            OnInit();
            this.Detalle = new List<ServiciosSav>();
        }

        /*carga los servicios*/
        public static List<ServiciosSav> List_Servicios(out string OnError)
        {
            OnInit();

            return sql_pointer.ExecuteSelectControl<ServiciosSav>(v_conexion, 2000, "SP_OBTIENE_SERVICIO_PROFORMA_SAV", null, out OnError);

        }

        public static List<ServiciosSav> List_Servicios_Recontvere(Int64 ID_DEPOT,Int64 ID_LINEA, out string OnError)
        {
            OnInit();
            parametros.Clear();
            parametros.Add("ID_DEPOT", ID_DEPOT);
            parametros.Add("ID_LINEA", ID_LINEA);
            return sql_pointer.ExecuteSelectControl<ServiciosSav>(v_conexion, 2000, "SP_OBTIENE_SERVICIO_PROFORMA_SAV_REC", parametros, out OnError);

        }


        public List<ServiciosSav> Detalle { get; set; }
    }
}
