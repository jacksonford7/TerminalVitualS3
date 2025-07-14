using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Bil_Cabecera : Cls_Bil_Base
    {

        #region "Variables"

        private Int64 _ID;
        private string _GLOSA = string.Empty;
        private DateTime? _FECHA = null;
        private string _TIPO_CARGA = string.Empty;
        private string _ID_AGENTE = string.Empty;
        private string _ID_UNICO_AGENTE = string.Empty;
        private string _DESC_AGENTE = string.Empty;
        private string _ID_CLIENTE = string.Empty;
        private string _DESC_CLIENTE = string.Empty;
        private string _EMAIL_CLIENTE = string.Empty;
        private string _ID_FACTURADO = string.Empty;
        private string _DESC_FACTURADO = string.Empty;
        private decimal _SUBTOTAL = 0;
        private decimal _IVA = 0;
        private decimal _TOTAL = 0;
        private string _NUMERO_CARGA = string.Empty;
        private string _CONTENEDORES = string.Empty;
        private DateTime? _FECHA_HASTA;

        private string _BL = string.Empty;
        private string _BUQUE = string.Empty;
        private string _VIAJE = string.Empty;
        private string _FECHA_ARRIBO = string.Empty;
        private string _DIR_FACTURADO = string.Empty;
        private string _EMAIL_FACTURADO = string.Empty;
        private string _CIUDAD_FACTURADO = string.Empty;
        private Int64 _DIAS_CREDITO=0;
        private string _SESION =string.Empty;
        private string _HORA_HASTA = string.Empty;
        private string _INVOICE_TYPE = string.Empty;

        private decimal _TOTAL_BULTOS = 0;

        private string _TIPO_CLIENTE = string.Empty;
        private bool _P2D = false;
        private string _RUC_USUARIO = string.Empty;
        private string _DESC_USUARIO = string.Empty;
        #endregion

        #region "Propiedades"

        public Int64 ID { get => _ID; set => _ID = value; }
        public string GLOSA { get => _GLOSA; set => _GLOSA = value; }
        public DateTime? FECHA { get => _FECHA; set => _FECHA = value; }
        public DateTime? FECHA_HASTA { get => _FECHA_HASTA; set => _FECHA_HASTA = value; }
        public string TIPO_CARGA { get => _TIPO_CARGA; set => _TIPO_CARGA = value; }
        public string ID_AGENTE { get => _ID_AGENTE; set => _ID_AGENTE = value; }
        public string ID_UNICO_AGENTE { get => _ID_UNICO_AGENTE; set => _ID_UNICO_AGENTE = value; }
        public string DESC_AGENTE { get => _DESC_AGENTE; set => _DESC_AGENTE = value; }
        public string ID_CLIENTE { get => _ID_CLIENTE; set => _ID_CLIENTE = value; }
        public string DESC_CLIENTE { get => _DESC_CLIENTE; set => _DESC_CLIENTE = value; }
        public string ID_FACTURADO { get => _ID_FACTURADO; set => _ID_FACTURADO = value; }
        public string DESC_FACTURADO { get => _DESC_FACTURADO; set => _DESC_FACTURADO = value; }
        public string NUMERO_CARGA { get => _NUMERO_CARGA; set => _NUMERO_CARGA = value; }
        public string CONTENEDORES { get => _CONTENEDORES; set => _CONTENEDORES = value; }

        public decimal SUBTOTAL { get => _SUBTOTAL; set => _SUBTOTAL = value; }
        public decimal IVA { get => _IVA; set => _IVA = value; }
        public decimal TOTAL { get => _TOTAL; set => _TOTAL = value; }


        public string BL { get => _BL; set => _BL = value; }
        public string BUQUE { get => _BUQUE; set => _BUQUE = value; }
        public string VIAJE { get => _VIAJE; set => _VIAJE = value; }
        public string FECHA_ARRIBO { get => _FECHA_ARRIBO; set => _FECHA_ARRIBO = value; }
        public string DIR_FACTURADO { get => _DIR_FACTURADO; set => _DIR_FACTURADO = value; }
        public string EMAIL_FACTURADO { get => _EMAIL_FACTURADO; set => _EMAIL_FACTURADO = value; }
        public string CIUDAD_FACTURADO { get => _CIUDAD_FACTURADO; set => _CIUDAD_FACTURADO = value; }
        public Int64 DIAS_CREDITO { get => _DIAS_CREDITO; set => _DIAS_CREDITO = value; }
        public string SESION { get => _SESION; set => _SESION = value; }
        public string HORA_HASTA { get => _HORA_HASTA; set => _HORA_HASTA = value; }
        private static String v_mensaje = string.Empty;

        public string INVOICE_TYPE { get => _INVOICE_TYPE; set => _INVOICE_TYPE = value; }

        public decimal TOTAL_BULTOS { get => _TOTAL_BULTOS; set => _TOTAL_BULTOS = value; }
        public string EMAIL_CLIENTE { get => _EMAIL_CLIENTE; set => _EMAIL_CLIENTE = value; }
        public string TIPO_CLIENTE { get => _TIPO_CLIENTE; set => _TIPO_CLIENTE = value; }

        public bool P2D { get => _P2D; set => _P2D = value; }
        public string RUC_USUARIO { get => _RUC_USUARIO; set => _RUC_USUARIO = value; }
        public string DESC_USUARIO { get => _DESC_USUARIO; set => _DESC_USUARIO = value; }

        #endregion



        public List<Cls_Bil_Detalle> Detalle { get; set; }
        public List<Cls_Horas_Reefer> Detalle_Horas_Reefer { get; set; }
        public List<Cls_Horas_Reefer> Detalle_Horas_Errores { get; set; }
        public List<Cls_Bil_DetalleClientes> Detalle_Clientes { get; set; }

        public List<Cls_Bil_PasePuertaCFS_SubItems> DetalleSubItem { get; set; }

        public List<cfs_cargas_pendientes_detalle> Detalle_CargasPendientes { get; set; }

        public Cls_Bil_Cabecera()
        {
            init();

            this.Detalle = new List<Cls_Bil_Detalle>();
            this.Detalle_Horas_Reefer = new List<Cls_Horas_Reefer>();
            this.Detalle_Horas_Errores = new List<Cls_Horas_Reefer>();
            this.Detalle_Clientes = new List<Cls_Bil_DetalleClientes>();
            this.DetalleSubItem = new List<Cls_Bil_PasePuertaCFS_SubItems>();
            this.Detalle_CargasPendientes = new List<cfs_cargas_pendientes_detalle>();

        }

        public Cls_Bil_Cabecera(Int64 _ID, string _GLOSA, DateTime? FECHA, string _TIPO_CARGA, string _ID_AGENTE, string _DESC_AGENTE,
                                      string _ID_CLIENTE, string _DESC_CLIENTE, string _ID_FACTURADO, string _DESC_FACTURADO,
                                      decimal _SUBTOTAL, decimal _IVA, decimal _TOTAL,
                                      string _USUARIO_CREA, DateTime? _FECHA_CREA, string _NUMERO_CARGA, string _CONTENEDORES, DateTime? _FECHA_HASTA,
                                      string _BL, string _BUQUE, string _VIAJE, string _FECHA_ARRIBO, string _DIR_FACTURADO,
                                        string _EMAIL_FACTURADO, string _CIUDAD_FACTURADO, Int64 _DIAS_CREDITO, string _ID_UNICO_AGENTE, string _SESION, 
                                        string _HORA_HASTA, string _INVOICE_TYPE, string _EMAIL_CLIENTE, string _TIPO_CLIENTE)

        {
            this.ID = _ID;
            this._GLOSA = _GLOSA;
            this.FECHA = _FECHA;
            this.TIPO_CARGA = _TIPO_CARGA;
            this.ID_AGENTE = _ID_AGENTE;
            this.DESC_AGENTE = _DESC_AGENTE;
            this.ID_CLIENTE = _ID_CLIENTE;
            this.DESC_CLIENTE = _DESC_CLIENTE;
            this.ID_FACTURADO = _ID_FACTURADO;
            this.DESC_FACTURADO = _DESC_FACTURADO;
            this.SUBTOTAL = _SUBTOTAL;
            this.IVA = _IVA;
            this.TOTAL = _TOTAL;
            this.IV_USUARIO_CREA = _USUARIO_CREA;
            this.IV_FECHA_CREA = _FECHA_CREA;
            this.NUMERO_CARGA = _NUMERO_CARGA;
            this.CONTENEDORES = _CONTENEDORES;
            this.FECHA_HASTA = _FECHA_HASTA;

            this.BL = _BL;
            this.BUQUE = _BUQUE;
            this.VIAJE = _VIAJE;
            this.FECHA_ARRIBO = _FECHA_ARRIBO;
            this.DIR_FACTURADO = _DIR_FACTURADO;
            this.EMAIL_FACTURADO = _EMAIL_FACTURADO;
            this.CIUDAD_FACTURADO = _CIUDAD_FACTURADO;
            this.DIAS_CREDITO = _DIAS_CREDITO;
            this.ID_UNICO_AGENTE = _ID_UNICO_AGENTE;
            this.SESION = _SESION;
            this.HORA_HASTA = _HORA_HASTA;
            this.INVOICE_TYPE = _INVOICE_TYPE;
            this.EMAIL_CLIENTE = _EMAIL_CLIENTE;

            this.TIPO_CLIENTE = _TIPO_CLIENTE;

            this.Detalle = new List<Cls_Bil_Detalle>();
            this.Detalle_Horas_Reefer = new List<Cls_Horas_Reefer>();
            this.Detalle_Horas_Errores = new List<Cls_Horas_Reefer>();
            this.Detalle_Clientes = new List<Cls_Bil_DetalleClientes>();
            this.DetalleSubItem = new List<Cls_Bil_PasePuertaCFS_SubItems>();

            this.Detalle_CargasPendientes = new List<cfs_cargas_pendientes_detalle>();
        }



    }
}
