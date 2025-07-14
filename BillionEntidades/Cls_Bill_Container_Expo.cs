using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BillionEntidades
{
 

    public class Cls_Bill_CabeceraExpo : Cls_Bil_Base
    {

        #region "Variables"
        private string _LLAVE = string.Empty;
        private long _ID = 0;
        private string _CNTR_VEPR_REFERENCE = string.Empty;
        private string _CNTR_BKNG_BOOKING = string.Empty;
        private string _CNTR_CLIENT_ID = string.Empty;
        private string _CNTR_CLIENT_ID2 = string.Empty;
        private string _CNTR_CLIENT = string.Empty;
        private ClientesCombo _CNTR_CLIENTE;
        private List<ClientesCombo> _CNTR_CLIENTES;
        private string _CNTR_INVOICE_TYPE = string.Empty;
        private string _CNTR_INVOICE_TYPE_NAME = string.Empty;
        private string _CNTR_CONTAINERS = string.Empty;
        private string _CNTR_CONTAINERSXML = string.Empty;
        private DateTime? _CNTR_FECHA = null;
        private string _CNTR_ESTADO = string.Empty;
        private string _CNTR_VEPR_VSSL_NAME = string.Empty;
        private string _CNTR_VEPR_VOYAGE = string.Empty;
        private DateTime? _CNTR_VEPR_ACTUAL_ARRIVAL = null;
        private DateTime? _CNTR_VEPR_ACTUAL_DEPART = null;
        private string _CNTR_USUARIO_CREA = string.Empty;
        private bool _VISTO = false;

        private string _BOOKINGLINE = string.Empty;
        private string _CNTR_SIZE = string.Empty;
        private string _CNTR_SIZE_RF = string.Empty;

        //campos a mostrar en grid
        private string _CNTR_CLNT_CUSTOMER_LINE = string.Empty;
        private int _CNTR_CONTENEDOR20 = 0;
        private int _CNTR_CONTENEDOR40 = 0;
        private bool _CNTR_CREDITO = false;
        private bool _CNTR_CONTADO = false;
        private bool _CNTR_PROCESADO = false;
        private int _orden = 0;

        private string _CNTR_TIPO_CLIENTE  = string.Empty;
        private int _CNTR_CLIENTE_DIAS_CREDITO = 0;
        private double? _CNTR_HOURS = 0;
        #endregion

        #region "Propiedades"
        public string LLAVE { get => _LLAVE; set => _LLAVE = value; }
        public long CNTR_ID { get => _ID; set => _ID = value; }
        public string CNTR_VEPR_REFERENCE { get => _CNTR_VEPR_REFERENCE; set => _CNTR_VEPR_REFERENCE = value; }
        public string CNTR_BKNG_BOOKING { get => _CNTR_BKNG_BOOKING; set => _CNTR_BKNG_BOOKING = value; }
        public string CNTR_CLIENT_ID2 { get => _CNTR_CLIENT_ID2; set => _CNTR_CLIENT_ID2 = value; }
        public string CNTR_CLIENT_ID { get => _CNTR_CLIENT_ID; set => _CNTR_CLIENT_ID = value; }
        public string CNTR_CLIENT { get => _CNTR_CLIENT; set => _CNTR_CLIENT = value; }
        public string CNTR_INVOICE_TYPE { get => _CNTR_INVOICE_TYPE; set => _CNTR_INVOICE_TYPE = value; }
        public string CNTR_INVOICE_TYPE_NAME { get => _CNTR_INVOICE_TYPE_NAME; set => _CNTR_INVOICE_TYPE_NAME = value; }

        public string CNTR_CONTAINERS { get => _CNTR_CONTAINERS; set => _CNTR_CONTAINERS = value; }
        public string CNTR_CONTAINERSXML { get => _CNTR_CONTAINERSXML; set => _CNTR_CONTAINERSXML = value; }
        public DateTime? CNTR_FECHA { get => _CNTR_FECHA; set => _CNTR_FECHA = value; }
        public string CNTR_ESTADO { get => _CNTR_ESTADO; set => _CNTR_ESTADO = value; }
        public string CNTR_VEPR_VSSL_NAME { get => _CNTR_VEPR_VSSL_NAME; set => _CNTR_VEPR_VSSL_NAME = value; }
        public string CNTR_VEPR_VOYAGE { get => _CNTR_VEPR_VOYAGE; set => _CNTR_VEPR_VOYAGE = value; }
        public DateTime? CNTR_VEPR_ACTUAL_ARRIVAL { get => _CNTR_VEPR_ACTUAL_ARRIVAL; set => _CNTR_VEPR_ACTUAL_ARRIVAL = value; }
        public DateTime? CNTR_VEPR_ACTUAL_DEPARTED { get => _CNTR_VEPR_ACTUAL_DEPART; set => _CNTR_VEPR_ACTUAL_DEPART = value; }
        public string CNTR_USUARIO_CREA { get => _CNTR_USUARIO_CREA; set => _CNTR_USUARIO_CREA = value; }
        public List<Cls_Bill_Container_Expo> Detalle { get; set; }
        public bool VISTO { get => _VISTO; set => _VISTO = value; }

        public string CNTR_CLNT_CUSTOMER_LINE { get => _CNTR_CLNT_CUSTOMER_LINE; set => _CNTR_CLNT_CUSTOMER_LINE = value; }
        public int CNTR_CONTENEDOR20 { get => _CNTR_CONTENEDOR20; set => _CNTR_CONTENEDOR20 = value; }
        public int CNTR_CONTENEDOR40 { get => _CNTR_CONTENEDOR40; set => _CNTR_CONTENEDOR40 = value; }
        public bool CNTR_CREDITO { get => _CNTR_CREDITO; set => _CNTR_CREDITO = value; }
        public bool CNTR_CONTADO { get => _CNTR_CONTADO; set => _CNTR_CONTADO = value; }
        public bool CNTR_PROCESADO { get => _CNTR_PROCESADO; set => _CNTR_PROCESADO = value; }
        public ClientesCombo CNTR_CLIENTE { get => _CNTR_CLIENTE; set => _CNTR_CLIENTE = value; }
        public List<ClientesCombo> CNTR_CLIENTES { get => _CNTR_CLIENTES; set => _CNTR_CLIENTES = value; }
        public string BOOKINGLINE { get => _BOOKINGLINE; set => _BOOKINGLINE = value; }
        public string CNTR_SIZE { get => _CNTR_SIZE; set => _CNTR_SIZE = value; }
        public int ORDEN { get => _orden; set => _orden = value; }
        public string CNTR_SIZE_RF { get => _CNTR_SIZE_RF; set => _CNTR_SIZE_RF = value; }

        public string CNTR_TIPO_CLIENTE { get => _CNTR_TIPO_CLIENTE; set => _CNTR_TIPO_CLIENTE = value; }
        public int CNTR_CLIENTE_DIAS_CREDITO { get => _CNTR_CLIENTE_DIAS_CREDITO; set => _CNTR_CLIENTE_DIAS_CREDITO = value; }

        public double? CNTR_HOURS { get => _CNTR_HOURS; set => _CNTR_HOURS = value; }
        #endregion

        public Cls_Bill_CabeceraExpo()
        {
            init();

            this.Detalle = new List<Cls_Bill_Container_Expo>();
        }

        public Cls_Bill_CabeceraExpo(long ID_, string CNTR_VEPR_REFERENCE_, string CNTR_BKNG_BOOKING_, string CNTR_CLIENT_ID_, string CNTR_CLIENT_, string CNTR_INVOICE_TYPE_, string CNTR_INVOICE_TYPE_NAME_, string CNTR_CONTAINERS_, DateTime? CNTR_FECHA_, string CNTR_ESTADO_, string CNTR_VEPR_VSSL_NAME_, string CNTR_VEPR_VOYAGE_, DateTime? CNTR_VEPR_ACTUAL_ARRIVAL_, DateTime? CNTR_VEPR_ACTUAL_DEPART_, string CNTR_USUARIO_CREA_)
        {
            this.CNTR_ID = ID_;
            this.CNTR_VEPR_REFERENCE = CNTR_VEPR_REFERENCE_;
            this.CNTR_BKNG_BOOKING = CNTR_BKNG_BOOKING_;
            this.CNTR_CLIENT_ID = CNTR_CLIENT_ID_;
            this.CNTR_CLIENT = CNTR_CLIENT_;
            this.CNTR_INVOICE_TYPE = CNTR_INVOICE_TYPE_;
            this.CNTR_INVOICE_TYPE_NAME = CNTR_INVOICE_TYPE_NAME_;
            this.CNTR_CONTAINERS = CNTR_CONTAINERS_;
            this.CNTR_FECHA = CNTR_FECHA_;
            this.CNTR_ESTADO = CNTR_ESTADO_;
            this.CNTR_VEPR_VSSL_NAME = CNTR_VEPR_VSSL_NAME_;
            this.CNTR_VEPR_VOYAGE = CNTR_VEPR_VOYAGE_;
            this.CNTR_VEPR_ACTUAL_ARRIVAL = CNTR_VEPR_ACTUAL_ARRIVAL_;
            this.CNTR_VEPR_ACTUAL_DEPARTED = CNTR_VEPR_ACTUAL_DEPART_;
            this.CNTR_USUARIO_CREA = CNTR_USUARIO_CREA_;
            this.Detalle = new List<Cls_Bill_Container_Expo>();
        }
    }

    public class Cls_Bill_Container_Expo : Cls_Bil_Base
    {
        #region "Variables"

        private Int64 _CNTR_ID;
        private Int64 _CNTR_CAB_ID;
        private Int64 _CNTR_CONSECUTIVO;
        private string _CNTR_CONTAINER = string.Empty;
        private string _CNTR_TYPE = string.Empty;
        private string _CNTR_TYSZ_SIZE = string.Empty;
        private string _CNTR_TYSZ_ISO = string.Empty;
        private string _CNTR_TYSZ_TYPE = string.Empty;
        private string _CNTR_FULL_EMPTY_CODE = string.Empty;
        private string _CNTR_YARD_STATUS = string.Empty;
        private decimal _CNTR_TEMPERATURE = 0;
        private string _CNTR_TYPE_DOCUMENT = string.Empty;
        private string _CNTR_DOCUMENT = string.Empty;
        private string _CNTR_VEPR_REFERENCE = string.Empty;
        private string _CNTR_CLNT_CUSTOMER_LINE = string.Empty;
        private string _CNTR_LCL_FCL = string.Empty;
        private string _CNTR_CATY_CARGO_TYPE = string.Empty;
        private string _CNTR_FREIGHT_KIND = string.Empty;
        private int _CNTR_DD = 0;
        private string _CNTR_BKNG_BOOKING = string.Empty;
        private DateTime? _FECHA_CAS = null;
        private string _CNTR_AISV = string.Empty;
        private int _CNTR_HOLD = 0;
        private string _CNTR_REEFER_CONT = string.Empty;
        private string _CNTR_VEPR_VSSL_NAME = string.Empty;
        private string _CNTR_VEPR_VOYAGE = string.Empty;
        private DateTime? _CNTR_VEPR_ACTUAL_ARRIVAL = null;
        private DateTime? _CNTR_VEPR_ACTUAL_DEPART = null;
        private bool _ESTADO_ERROR = false;
        private bool _VISTO = false;
        private string _CNTR_PROFORMA = string.Empty;
        private double? _CNTR_HOURS = 0;
        #endregion

        #region "Propiedaes"
        public long CNTR_ID { get => _CNTR_ID; set => _CNTR_ID = value; }
        public long CNTR_CAB_ID { get => _CNTR_CAB_ID; set => _CNTR_CAB_ID = value; }
        public long CNTR_CONSECUTIVO { get => _CNTR_CONSECUTIVO; set => _CNTR_CONSECUTIVO = value; }
        public string CNTR_CONTAINER { get => _CNTR_CONTAINER; set => _CNTR_CONTAINER = value; }
        public string CNTR_TYPE { get => _CNTR_TYPE; set => _CNTR_TYPE = value; }
        public string CNTR_TYSZ_SIZE { get => _CNTR_TYSZ_SIZE; set => _CNTR_TYSZ_SIZE = value; }
        public string CNTR_TYSZ_ISO { get => _CNTR_TYSZ_ISO; set => _CNTR_TYSZ_ISO = value; }
        public string CNTR_TYSZ_TYPE { get => _CNTR_TYSZ_TYPE; set => _CNTR_TYSZ_TYPE = value; }
        public string CNTR_FULL_EMPTY_CODE { get => _CNTR_FULL_EMPTY_CODE; set => _CNTR_FULL_EMPTY_CODE = value; }
        public string CNTR_YARD_STATUS { get => _CNTR_YARD_STATUS; set => _CNTR_YARD_STATUS = value; }
        public decimal CNTR_TEMPERATURE { get => _CNTR_TEMPERATURE; set => _CNTR_TEMPERATURE = value; }
        public string CNTR_TYPE_DOCUMENT { get => _CNTR_TYPE_DOCUMENT; set => _CNTR_TYPE_DOCUMENT = value; }
        public string CNTR_DOCUMENT { get => _CNTR_DOCUMENT; set => _CNTR_DOCUMENT = value; }
        public string CNTR_VEPR_REFERENCE { get => _CNTR_VEPR_REFERENCE; set => _CNTR_VEPR_REFERENCE = value; }
        public string CNTR_CLNT_CUSTOMER_LINE { get => _CNTR_CLNT_CUSTOMER_LINE; set => _CNTR_CLNT_CUSTOMER_LINE = value; }
        public string CNTR_LCL_FCL { get => _CNTR_LCL_FCL; set => _CNTR_LCL_FCL = value; }
        public string CNTR_CATY_CARGO_TYPE { get => _CNTR_CATY_CARGO_TYPE; set => _CNTR_CATY_CARGO_TYPE = value; }
        public string CNTR_FREIGHT_KIND { get => _CNTR_FREIGHT_KIND; set => _CNTR_FREIGHT_KIND = value; }
        public int CNTR_DD { get => _CNTR_DD; set => _CNTR_DD = value; }
        public string CNTR_BKNG_BOOKING { get => _CNTR_BKNG_BOOKING; set => _CNTR_BKNG_BOOKING = value; }
        public DateTime? FECHA_CAS { get => _FECHA_CAS; set => _FECHA_CAS = value; }
        public string CNTR_AISV { get => _CNTR_AISV; set => _CNTR_AISV = value; }
        public int CNTR_HOLD { get => _CNTR_HOLD; set => _CNTR_HOLD = value; }
        public string CNTR_REEFER_CONT { get => _CNTR_REEFER_CONT; set => _CNTR_REEFER_CONT = value; }
        public string CNTR_VEPR_VSSL_NAME { get => _CNTR_VEPR_VSSL_NAME; set => _CNTR_VEPR_VSSL_NAME = value; }
        public string CNTR_VEPR_VOYAGE { get => _CNTR_VEPR_VOYAGE; set => _CNTR_VEPR_VOYAGE = value; }
        public DateTime? CNTR_VEPR_ACTUAL_ARRIVAL { get => _CNTR_VEPR_ACTUAL_ARRIVAL; set => _CNTR_VEPR_ACTUAL_ARRIVAL = value; }
        public DateTime? CNTR_VEPR_ACTUAL_DEPARTED { get => _CNTR_VEPR_ACTUAL_DEPART; set => _CNTR_VEPR_ACTUAL_DEPART = value; }
        public bool ESTADO_ERROR { get => _ESTADO_ERROR; set => _ESTADO_ERROR = value; }
        public bool VISTO { get => _VISTO; set => _VISTO = value; }
        public string CNTR_PROFORMA { get => _CNTR_PROFORMA; set => _CNTR_PROFORMA = value; }
        public double? CNTR_HOURS  { get => _CNTR_HOURS; set => _CNTR_HOURS = value; }
        #endregion

        public Cls_Bill_Container_Expo()
        {
            init();
        }

        public Cls_Bill_Container_Expo(Int64 _CNTR_CAB_ID, Int64 _CNTR_CONSECUTIVO, string _CNTR_CONTAINER,
                                       string _CNTR_TYPE,
                                       string _CNTR_TYSZ_SIZE,
                                       string _CNTR_TYSZ_ISO,
                                       string _CNTR_TYSZ_TYPE,
                                       string _CNTR_FULL_EMPTY_CODE,
                                       string _CNTR_YARD_STATUS,
                                       decimal _CNTR_TEMPERATURE,
                                       string _CNTR_TYPE_DOCUMENT,
                                       string _CNTR_DOCUMENT,
                                       string _CNTR_VEPR_REFERENCE,
                                       string _CNTR_CLNT_CUSTOMER_LINE,
                                       string _CNTR_LCL_FCL,
                                       string _CNTR_CATY_CARGO_TYPE,
                                       string _CNTR_FREIGHT_KIND,
                                       int _CNTR_DD,
                                       string _CNTR_BKNG_BOOKING,
                                       DateTime? _FECHA_CAS,
                                       string _CNTR_AISV,
                                       int _CNTR_HOLD,
                                       string _CNTR_REEFER_CONT,
                                       string _CNTR_VEPR_VSSL_NAME,
                                       string _CNTR_VEPR_VOYAGE,
                                       DateTime? _CNTR_VEPR_ACTUAL_ARRIVAL)
        {
            CNTR_CAB_ID = _CNTR_CAB_ID;
            CNTR_CONSECUTIVO = _CNTR_CONSECUTIVO;
            CNTR_CONTAINER = _CNTR_CONTAINER;
            CNTR_TYPE = _CNTR_TYPE;
            CNTR_TYSZ_SIZE = _CNTR_TYSZ_SIZE;
            CNTR_TYSZ_ISO = _CNTR_TYSZ_ISO;
            CNTR_TYSZ_TYPE = _CNTR_TYSZ_TYPE;
            CNTR_FULL_EMPTY_CODE = _CNTR_FULL_EMPTY_CODE;
            CNTR_YARD_STATUS = _CNTR_YARD_STATUS;
            CNTR_TEMPERATURE = _CNTR_TEMPERATURE;
            CNTR_TYPE_DOCUMENT = _CNTR_TYPE_DOCUMENT;
            CNTR_DOCUMENT = _CNTR_DOCUMENT;
            CNTR_VEPR_REFERENCE = _CNTR_VEPR_REFERENCE;
            CNTR_CLNT_CUSTOMER_LINE = _CNTR_CLNT_CUSTOMER_LINE;
            CNTR_LCL_FCL = _CNTR_LCL_FCL;
            CNTR_CATY_CARGO_TYPE = _CNTR_CATY_CARGO_TYPE;
            CNTR_FREIGHT_KIND = _CNTR_FREIGHT_KIND;
            CNTR_DD = _CNTR_DD;
            CNTR_BKNG_BOOKING = _CNTR_BKNG_BOOKING;
            FECHA_CAS = _FECHA_CAS;
            CNTR_AISV = _CNTR_AISV;
            CNTR_HOLD = _CNTR_HOLD;
            CNTR_REEFER_CONT = _CNTR_REEFER_CONT;
            CNTR_VEPR_VSSL_NAME = _CNTR_VEPR_VSSL_NAME;
            CNTR_VEPR_VOYAGE = _CNTR_VEPR_VOYAGE;
            CNTR_VEPR_ACTUAL_ARRIVAL = _CNTR_VEPR_ACTUAL_ARRIVAL;
        }
    }

    public class ClientesCombo
    {
        private int _orden;
        private string _tipo;
        private string _ruc;
        private string _cliente;
        private string _booking;
        private bool _ClienteContado;
       
        private N4.Entidades.Cliente _datoCliente;

        public int Orden { get => _orden; set => _orden = value; }
        public string Tipo { get => _tipo; set => _tipo = value; }
        public string Ruc { get => _ruc; set => _ruc = value; }
        public string Cliente { get => _cliente; set => _cliente = value; }
        public string Booking { get => _booking; set => _booking = value; }
        public bool ClienteContado { get => _ClienteContado; set => _ClienteContado = value; }
       
        public N4.Entidades.Cliente DatoCliente { get => _datoCliente; set => _datoCliente = value; }
    }

    public class Cls_Bill_Container_Expo_Det_Validacion : Cls_Bil_Base
    {
        private long _ID = 0;
        private long _CNTR_CONSECUTIVO = 0;
        private long _CNTR_CAB_ID = 0;
        private string _BOOKING = string.Empty;
        private string _SERVICIOS_ACTUALES = string.Empty;
        private int _TOTAL = 0;
        private string _MSG_DUPLICADO = string.Empty;
        private string _MSG_FALTANTE = string.Empty;
        private string _MSG_OTROS = string.Empty;
        private string _FALTANTES = string.Empty;
        private DateTime? _FECHA_REGISTRO = null;
        private string _USUARIO_REGISTRA = string.Empty;
        private bool _ESTADO_REGISTRO = false;
        private bool _ERROR = false;
        private string _REFERENCIA = string.Empty;


        public long ID { get => _ID; set => _ID = value; }
        public long CNTR_CONSECUTIVO { get => _CNTR_CONSECUTIVO; set => _CNTR_CONSECUTIVO = value; }
        public long CNTR_CAB_ID { get => _CNTR_CAB_ID; set => _CNTR_CAB_ID = value; }
        public string BOOKING { get => _BOOKING; set => _BOOKING = value; }
        public string SERVICIOS_ACTUALES { get => _SERVICIOS_ACTUALES; set => _SERVICIOS_ACTUALES = value; }
        public int TOTAL { get => _TOTAL; set => _TOTAL = value; }
        public string MSG_DUPLICADO { get => _MSG_DUPLICADO; set => _MSG_DUPLICADO = value; }
        public string MSG_FALTANTE { get => _MSG_FALTANTE; set => _MSG_FALTANTE = value; }
        public string MSG_OTROS { get => _MSG_OTROS; set => _MSG_OTROS = value; }
        public string FALTANTES { get => _FALTANTES; set => _FALTANTES = value; }
        public DateTime? FECHA_REGISTRO { get => _FECHA_REGISTRO; set => _FECHA_REGISTRO = value; }
        public string USUARIO_REGISTRA { get => _USUARIO_REGISTRA; set => _USUARIO_REGISTRA = value; }
        public bool ESTADO_REGISTRO { get => _ESTADO_REGISTRO; set => _ESTADO_REGISTRO = value; }
        public bool ERROR { get => _ERROR; set => _ERROR = value; }
        public string REFERENCIA { get => _REFERENCIA; set => _REFERENCIA = value; }

        public Cls_Bill_Container_Expo_Det_Validacion()
        {
            init();
        }

        public Cls_Bill_Container_Expo_Det_Validacion(long _ID
                                                        , long _CNTR_CONSECUTIVO
                                                        , long _CNTR_CAB_ID
                                                        , string _BOOKING
                                                        , string _SERVICIOS_ACTUALES
                                                        , int _TOTAL
                                                        , string _MSG_DUPLICADO
                                                        , string _MSG_FALTANTE
                                                        , string _MSG_OTROS
                                                        , float _FALTANTES
                                                        , DateTime? _FECHA_REGISTRO
                                                        , string _USUARIO_REGISTRA
                                                        , bool _ESTADO_REGISTRO)
        {

            CNTR_CONSECUTIVO = _CNTR_CONSECUTIVO;
            CNTR_CAB_ID = _CNTR_CAB_ID;

        }
    }

    public class Cls_FacturasExpo
    {
        private string _referencia;
        private long _codigo; 
        private string _numeroFactura;
        private string _tipoFactura;
        private DateTime _fechaFactura;
        private decimal _monto;
        private string _facturadoA;
        private string _mail;
        private bool _enviado;

        public Cls_FacturasExpo() { }

        public string Referencia { get => _referencia; set => _referencia = value; }
        public string NumeroFactura { get => _numeroFactura; set => _numeroFactura = value; }
        public string TipoFactura { get => _tipoFactura; set => _tipoFactura = value; }
        public DateTime FechaFactura { get => _fechaFactura; set => _fechaFactura = value; }
        public decimal Monto { get => _monto; set => _monto = value; }
        public string FacturadoA { get => _facturadoA; set => _facturadoA = value; }
        public string Mail { get => _mail; set => _mail = value; }
        public bool Enviado { get => _enviado; set => _enviado = value; }
        public long Codigo { get => _codigo; set => _codigo = value; }
    }
}
