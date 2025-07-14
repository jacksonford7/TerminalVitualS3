using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
   public  class Cls_Bil_Detalle : Cls_Bil_Base
    {

        #region "Variables"

        private Int64 _ID;
        private Int64 _GKEY;
        private string _MRN = string.Empty;
        private string _MSN = string.Empty;
        private string _HSN = string.Empty;
        private string _CONTENEDOR = string.Empty;
        private string _REFERENCIA = string.Empty;
        private string _TRAFICO = string.Empty;
        private string _TAMANO = string.Empty;
        private string _TIPO = string.Empty;
        private DateTime? _CAS = null;
        private string _BOOKING = string.Empty;
        private string _IMDT = string.Empty;
        private bool _BLOQUEO = false;
        private DateTime? _FECHA_ULTIMA = null;
        private string _IN_OUT = string.Empty;
        private string _FULL_VACIO = string.Empty;
        private string _AISV = string.Empty;
        private string _REEFER = string.Empty;
        private bool _VISTO = false;
        private string _DOCUMENTO = string.Empty;
        private string _DES_BLOQUEO = string.Empty;
        private string _CONECTADO = string.Empty;
        private string _NUMERO_FACTURA = string.Empty;
        private bool _CNTR_DD = false;
        private Int16 _SECUENCIA;
        private DateTime? _FECHA_HASTA = null;
        private string _ESTADO_RDIT = string.Empty;
        private DateTime? _FECHA_TOPE_DLIBRE = null;
        private DateTime? _CNTR_DESCARGA = null;
        private DateTime? _CNTR_DEPARTED = null;
        private string _IDPLAN = string.Empty;
        private string _TURNO = string.Empty;
        private string _MODULO = string.Empty;
        private string _NUMERO_PASE_N4 = string.Empty;
        private DateTime? _FECHA_ARRIBO = null;
        private string _LINEA = string.Empty;

        private string _CNTR_TYSZ_ISO = string.Empty;
        private decimal _CNTR_TEMPERATURE = 0;
        private string _CNTR_TYPE_DOCUMENT = string.Empty;
        private string _CNTR_LCL_FCL = string.Empty;
        private string _CNTR_CATY_CARGO_TYPE = string.Empty;
        private string _CNTR_FREIGHT_KIND = string.Empty;
        private string _CNTR_BKNG_BOOKING = string.Empty;
        private string _CNTR_VEPR_VOYAGE = string.Empty;
        private string _CNTR_VEPR_VSSL_NAME = string.Empty;
        private string _CNTR_PROFORMA = string.Empty;

        private decimal _CANTIDAD = 0;
        private decimal _PESO = 0;
        private decimal _TEMPERATURA = 0;
        private string _OPERACION = string.Empty;
        private string _DESCRIPCION = string.Empty;
        private string _EXPORTADOR = string.Empty;
        private string _AGENCIA = string.Empty;
        private string _AUTORIZADO = string.Empty;
        private Int64? _ID_UNIDAD;

        private bool _CERTIFICADO = false;
        private string _TIENE_CERTIFICADO = string.Empty;
        private string _TIME_IN = string.Empty;
        private double? _VOLUMEN;

        #endregion

        #region "Propiedades"

        public Int64 ID { get => _ID; set => _ID = value; }
        public Int64 GKEY { get => _GKEY; set => _GKEY = value; }
        public string MRN { get => _MRN; set => _MRN = value; }
        public string MSN { get => _MSN; set => _MSN = value; }
        public string HSN { get => _HSN; set => _HSN = value; }
        public string CONTENEDOR { get => _CONTENEDOR; set => _CONTENEDOR = value; }
        public string REFERENCIA { get => _REFERENCIA; set => _REFERENCIA = value; }
        public string TRAFICO { get => _TRAFICO; set => _TRAFICO = value; }
        public string TAMANO { get => _TAMANO; set => _TAMANO = value; }
        public string TIPO { get => _TIPO; set => _TIPO = value; }
        public DateTime? CAS { get => _CAS; set => _CAS = value; }
        public string BOOKING { get => _BOOKING; set => _BOOKING = value; }
        public string IMDT { get => _IMDT; set => _IMDT = value; }
        public bool BLOQUEO { get => _BLOQUEO; set => _BLOQUEO = value; }
        public DateTime? FECHA_ULTIMA { get => _FECHA_ULTIMA; set => _FECHA_ULTIMA = value; }
        public string IN_OUT { get => _IN_OUT; set => _IN_OUT = value; }
        public string FULL_VACIO { get => _FULL_VACIO; set => _FULL_VACIO = value; }
        public string AISV { get => _AISV; set => _AISV = value; }
        public string REEFER { get => _REEFER; set => _REEFER = value; }
        public bool VISTO { get => _VISTO; set => _VISTO = value; }
        public string DOCUMENTO { get => _DOCUMENTO; set => _DOCUMENTO = value; }
        public string DES_BLOQUEO { get => _DES_BLOQUEO; set => _DES_BLOQUEO = value; }
        public string CONECTADO { get => _CONECTADO; set => _CONECTADO = value; }
        public string NUMERO_FACTURA { get => _NUMERO_FACTURA; set => _NUMERO_FACTURA = value; }
        public bool CNTR_DD { get => _CNTR_DD; set => _CNTR_DD = value; }
        public Int16 SECUENCIA { get => _SECUENCIA; set => _SECUENCIA = value; }
        public DateTime? FECHA_HASTA { get => _FECHA_HASTA; set => _FECHA_HASTA = value; }
        public string ESTADO_RDIT { get => _ESTADO_RDIT; set => _ESTADO_RDIT = value; }
        public DateTime? FECHA_TOPE_DLIBRE { get => _FECHA_TOPE_DLIBRE; set => _FECHA_TOPE_DLIBRE = value; }
        public DateTime? CNTR_DESCARGA { get => _CNTR_DESCARGA; set => _CNTR_DESCARGA = value; }
        public DateTime? CNTR_DEPARTED { get => _CNTR_DEPARTED; set => _CNTR_DEPARTED = value; }
        public DateTime? FECHA_ARRIBO { get => _FECHA_ARRIBO; set => _FECHA_ARRIBO = value; }

        public string IDPLAN { get => _IDPLAN; set => _IDPLAN = value; }
        public string TURNO { get => _TURNO; set => _TURNO = value; }
        public string MODULO { get => _MODULO; set => _MODULO = value; }
        public string NUMERO_PASE_N4 { get => _NUMERO_PASE_N4; set => _NUMERO_PASE_N4 = value; }
        public string LINEA { get => _LINEA; set => _LINEA = value; }

        public string CNTR_TYSZ_ISO { get => _CNTR_TYSZ_ISO; set => _CNTR_TYSZ_ISO = value; }
        public decimal CNTR_TEMPERATURE { get => _CNTR_TEMPERATURE; set => _CNTR_TEMPERATURE = value; }
        public string CNTR_TYPE_DOCUMENT { get => _CNTR_TYPE_DOCUMENT; set => _CNTR_TYPE_DOCUMENT = value; }
        public string CNTR_LCL_FCL { get => _CNTR_LCL_FCL; set => _CNTR_LCL_FCL = value; }
        public string CNTR_CATY_CARGO_TYPE { get => _CNTR_CATY_CARGO_TYPE; set => _CNTR_CATY_CARGO_TYPE = value; }
        public string CNTR_FREIGHT_KIND { get => _CNTR_FREIGHT_KIND; set => _CNTR_FREIGHT_KIND = value; }
        public string CNTR_BKNG_BOOKING { get => _CNTR_BKNG_BOOKING; set => _CNTR_BKNG_BOOKING = value; }
        public string CNTR_VEPR_VOYAGE { get => _CNTR_VEPR_VOYAGE; set => _CNTR_VEPR_VOYAGE = value; }
        public string CNTR_VEPR_VSSL_NAME { get => _CNTR_VEPR_VSSL_NAME; set => _CNTR_VEPR_VSSL_NAME = value; }
        public string CNTR_PROFORMA { get => _CNTR_PROFORMA; set => _CNTR_PROFORMA = value; }

        public decimal CANTIDAD { get => _CANTIDAD; set => _CANTIDAD = value; }
        public decimal PESO { get => _PESO; set => _PESO = value; }
        public decimal TEMPERATURA { get => _TEMPERATURA; set => _TEMPERATURA = value; }
        public string OPERACION { get => _OPERACION; set => _OPERACION = value; }
        public string DESCRIPCION { get => _DESCRIPCION; set => _DESCRIPCION = value; }
        public string EXPORTADOR { get => _EXPORTADOR; set => _EXPORTADOR = value; }
        public string AGENCIA { get => _AGENCIA; set => _AGENCIA = value; }
        public string AUTORIZADO { get => _AUTORIZADO; set => _AUTORIZADO = value; }
        public Int64? ID_UNIDAD { get => _ID_UNIDAD; set => _ID_UNIDAD = value; }

        public bool CERTIFICADO { get => _CERTIFICADO; set => _CERTIFICADO = value; }
        public string TIENE_CERTIFICADO { get => _TIENE_CERTIFICADO; set => _TIENE_CERTIFICADO = value; }

        public string TIME_IN { get => _TIME_IN; set => _TIME_IN = value; }
        public double? VOLUMEN { get => _VOLUMEN; set => _VOLUMEN = value; }

        public string NUMERO_CARGA { get ; set ; }
        public string P2D_ID_AGENTE { get ; set; }
        public string P2D_DESC_AGENTE { get; set; }
        public string P2D_ID_CLIENTE { get; set ; }
        public string P2D_DESC_CLIENTE { get; set ; }
        public string P2D_ID_FACTURADO { get ; set ; }
        public string P2D_DESC_FACTURADO { get ; set; }
        public string P2D_DIRECCION { get; set; }
        #endregion

        public Cls_Bil_Detalle()
        {
            init();
        }


        public Cls_Bil_Detalle(Int64 _ID, Int64 _GKEY, string _MRN, string _MSN, string _HSN, string _CONTENEDOR,
        string _REFERENCIA, string _TRAFICO, string _TAMANO, string _TIPO, DateTime? _CAS,
        string _BOOKING, string _IMDT, bool _BLOQUEO, DateTime? _FECHA_ULTIMA, string _IN_OUT, string _FULL_VACIO,
        string _AISV, string _REEFER, bool _VISTO, string _DOCUMENTO, string _DES_BLOQUEO, string _CONECTADO, 
        string _NUMERO_FACTURA, bool _CNTR_DD, Int16 _SECUENCIA, DateTime? _FECHA_HASTA, string _ESTADO_RDIT, DateTime? _FECHA_TOPE_DLIBRE,
        DateTime? _CNTR_DESCARGA, string _IDPLAN, string _TURNO, string _MODULO, DateTime?  _CNTR_DEPARTED, string _NUMERO_PASE_N4, string _LINEA, 
        Int64? _ID_UNIDAD, bool _CERTIFICADO, string _TIENE_CERTIFICADO)
        {
            this.ID = _ID;
            this.GKEY = _GKEY;
            this.MRN = _MRN;
            this.MSN = _MSN;
            this.HSN = _HSN;

            this.CONTENEDOR = _CONTENEDOR;
            this.TRAFICO = _TRAFICO;

            this.TAMANO = _TAMANO;
            this.TIPO = _TIPO;
            this.CAS = _CAS;
            this.BOOKING = _BOOKING;

            this.IMDT = _IMDT;
            this.BLOQUEO = _BLOQUEO;
            this.FECHA_ULTIMA = _FECHA_ULTIMA;
            this.IN_OUT = _IN_OUT;
            this.FULL_VACIO = _FULL_VACIO;
            this.AISV = _AISV;
            this.REEFER = _REEFER;
            this.DOCUMENTO = _DOCUMENTO;
            this.DES_BLOQUEO = _DES_BLOQUEO;
            this.CONECTADO = _CONECTADO;
            this.NUMERO_FACTURA = _NUMERO_FACTURA;
            this.CNTR_DD = _CNTR_DD;
            this.SECUENCIA = _SECUENCIA;
            this.FECHA_HASTA = _FECHA_HASTA;
            this.ESTADO_RDIT = _ESTADO_RDIT;
            this.FECHA_TOPE_DLIBRE = _FECHA_TOPE_DLIBRE;
            this.CNTR_DESCARGA = _CNTR_DESCARGA;

            this.IDPLAN = _IDPLAN;
            this.TURNO = _TURNO;
            this.MODULO = _MODULO;

            this.CNTR_DEPARTED = _CNTR_DEPARTED;
            this.NUMERO_PASE_N4 = _NUMERO_PASE_N4;
            this.LINEA = _LINEA;
            this.ID_UNIDAD = _ID_UNIDAD;
            this.CERTIFICADO = _CERTIFICADO;
            this.TIENE_CERTIFICADO = _TIENE_CERTIFICADO;
        }


    }
}
