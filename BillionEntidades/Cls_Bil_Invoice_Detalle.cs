using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Bil_Invoice_Detalle : Cls_Bil_Base
    {
        #region "Variables"

        private Int64 _IV_ID;
        private Int64 _IV_GKEY;
        private string _IV_MRN = string.Empty;
        private string _IV_MSN = string.Empty;
        private string _IV_HSN = string.Empty;
        private string _IV_CONTENEDOR = string.Empty;
        private string _IV_REFERENCIA = string.Empty;
        private string _IV_TRAFICO = string.Empty;
        private string _IV_TAMANO = string.Empty;
        private string _IV_TIPO = string.Empty;
        private DateTime? _IV_CAS = null;
        private string _IV_BOOKING = string.Empty;
        private string _IV_IMDT = string.Empty;
        private bool _IV_BLOQUEO = false;
        private DateTime? _IV_FECHA_ULTIMA = null;
        private string _IV_IN_OUT = string.Empty;
        private string _IV_FULL_VACIO = string.Empty;
        private string _IV_AISV = string.Empty;
        private string _IV_REEFER = string.Empty;

        private bool _IV_VISTO = false;
        private string _IV_DOCUMENTO = string.Empty;
        private string _IV_DES_BLOQUEO = string.Empty;
        private string _IV_CONECTADO = string.Empty;

        private string _IV_FACTURA = string.Empty;
        private string _IV_ID_AGENTE = string.Empty;
        private string _IV_ID_FACTURADO = string.Empty;
        private DateTime? _IV_FECHA_HASTA;
        private bool _IV_PAGADO = false;
        private bool _IV_CNTR_DD = false;
        private string _IV_DESC_AGENTE = string.Empty;
        private string _IV_DESC_FACTURADO = string.Empty;
        private string _IV_ID_IMPORTADOR= string.Empty;
        private string _IV_DESC_IMPORTADOR = string.Empty;

        private DateTime? _IV_FECHA_TOPE_DLIBRE = null;
        private DateTime? _IV_CNTR_DESCARGA = null;
        private string _IV_MODULO = string.Empty;

        private DateTime? _IV_CNTR_DEPARTED = null;

        private bool _IV_TIENE_SERVICIOS = false;
        private string _IV_LINEA = string.Empty;

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


        private decimal _IV_CANTIDAD = 0;
        private decimal _IV_PESO = 0;
        private string _IV_OPERACION = string.Empty;
        private string _IV_DESCRIPCION = string.Empty;
        private string _IV_EXPORTADOR = string.Empty;
        private string _IV_AGENCIA = string.Empty;
        private string _IV_AUTORIZADO = string.Empty;
        private Int64? _ID_UNIDAD;

        private bool _IV_CERTIFICADO = false;
        private string _IV_TIENE_CERTIFICADO = string.Empty;

        #endregion

        #region "Propiedades"

        public Int64 IV_ID { get => _IV_ID; set => _IV_ID = value; }
        public Int64 IV_GKEY { get => _IV_GKEY; set => _IV_GKEY = value; }
        public string IV_MRN { get => _IV_MRN; set => _IV_MRN = value; }
        public string IV_MSN { get => _IV_MSN; set => _IV_MSN = value; }
        public string IV_HSN { get => _IV_HSN; set => _IV_HSN = value; }
        public string IV_CONTENEDOR { get => _IV_CONTENEDOR; set => _IV_CONTENEDOR = value; }
        public string IV_REFERENCIA { get => _IV_REFERENCIA; set => _IV_REFERENCIA = value; }
        public string IV_TRAFICO { get => _IV_TRAFICO; set => _IV_TRAFICO = value; }
        public string IV_TAMANO { get => _IV_TAMANO; set => _IV_TAMANO = value; }
        public string IV_TIPO { get => _IV_TIPO; set => _IV_TIPO = value; }
        public DateTime? IV_CAS { get => _IV_CAS; set => _IV_CAS = value; }
        public string IV_BOOKING { get => _IV_BOOKING; set => _IV_BOOKING = value; }
        public string IV_IMDT { get => _IV_IMDT; set => _IV_IMDT = value; }
        public bool IV_BLOQUEO { get => _IV_BLOQUEO; set => _IV_BLOQUEO = value; }
        public DateTime? IV_FECHA_ULTIMA { get => _IV_FECHA_ULTIMA; set => _IV_FECHA_ULTIMA = value; }
        public string IV_IN_OUT { get => _IV_IN_OUT; set => _IV_IN_OUT = value; }
        public string IV_FULL_VACIO { get => _IV_FULL_VACIO; set => _IV_FULL_VACIO = value; }
        public string IV_AISV { get => _IV_AISV; set => _IV_AISV = value; }
        public string IV_REEFER { get => _IV_REEFER; set => _IV_REEFER = value; }
        public bool IV_VISTO { get => _IV_VISTO; set => _IV_VISTO = value; }
        public string IV_DOCUMENTO { get => _IV_DOCUMENTO; set => _IV_DOCUMENTO = value; }
        public string IV_DES_BLOQUEO { get => _IV_DES_BLOQUEO; set => _IV_DES_BLOQUEO = value; }
        public string IV_CONECTADO { get => _IV_CONECTADO; set => _IV_CONECTADO = value; }

        public string IV_FACTURA { get => _IV_FACTURA; set => _IV_FACTURA = value; }
        public string IV_ID_AGENTE { get => _IV_ID_AGENTE; set => _IV_ID_AGENTE = value; }
        public string IV_ID_FACTURADO { get => _IV_ID_FACTURADO; set => _IV_ID_FACTURADO = value; }
        public DateTime? IV_FECHA_HASTA { get => _IV_FECHA_HASTA; set => _IV_FECHA_HASTA = value; }
        public bool IV_PAGADO { get => _IV_PAGADO; set => _IV_PAGADO = value; }
        public bool IV_CNTR_DD { get => _IV_CNTR_DD; set => _IV_CNTR_DD = value; }

        public string IV_DESC_AGENTE { get => _IV_DESC_AGENTE; set => _IV_DESC_AGENTE = value; }
        public string IV_DESC_FACTURADO { get => _IV_DESC_FACTURADO; set => _IV_DESC_FACTURADO = value; }
        public string IV_ID_IMPORTADOR { get => _IV_ID_IMPORTADOR; set => _IV_ID_IMPORTADOR = value; }
        public string IV_DESC_IMPORTADOR { get => _IV_DESC_IMPORTADOR; set => _IV_DESC_IMPORTADOR = value; }

        public DateTime? IV_FECHA_TOPE_DLIBRE { get => _IV_FECHA_TOPE_DLIBRE; set => _IV_FECHA_TOPE_DLIBRE = value; }
        public DateTime? IV_CNTR_DESCARGA { get => _IV_CNTR_DESCARGA; set => _IV_CNTR_DESCARGA = value; }
        public string IV_MODULO { get => _IV_MODULO; set => _IV_MODULO = value; }
        public DateTime? IV_CNTR_DEPARTED { get => _IV_CNTR_DEPARTED; set => _IV_CNTR_DEPARTED = value; }

        public bool IV_TIENE_SERVICIOS { get => _IV_TIENE_SERVICIOS; set => _IV_TIENE_SERVICIOS = value; }

        public string IV_LINEA { get => _IV_LINEA; set => _IV_LINEA= value; }

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

        public decimal IV_CANTIDAD { get => _IV_CANTIDAD; set => _IV_CANTIDAD = value; }
        public decimal IV_PESO { get => _IV_PESO; set => _IV_PESO = value; }
        public string IV_OPERACION { get => _IV_OPERACION; set => _IV_OPERACION = value; }
        public string IV_DESCRIPCION { get => _IV_DESCRIPCION; set => _IV_DESCRIPCION = value; }
        public string IV_EXPORTADOR { get => _IV_EXPORTADOR; set => _IV_EXPORTADOR = value; }
        public string IV_AGENCIA { get => _IV_AGENCIA; set => _IV_AGENCIA = value; }
        public string IV_AUTORIZADO { get => _IV_AUTORIZADO; set => _IV_AUTORIZADO = value; }
        public Int64? ID_UNIDAD { get => _ID_UNIDAD; set => _ID_UNIDAD = value; }

        public bool IV_CERTIFICADO { get => _IV_CERTIFICADO; set => _IV_CERTIFICADO = value; }
        public string IV_TIENE_CERTIFICADO { get => _IV_TIENE_CERTIFICADO; set => _IV_TIENE_CERTIFICADO = value; }

        #endregion

        public Cls_Bil_Invoice_Detalle()
        {
            init();
        }


        public Cls_Bil_Invoice_Detalle(Int64 _IV_ID,  Int64 _IV_GKEY, string _IV_MRN, string _IV_MSN, string _IV_HSN, string _IV_CONTENEDOR,
        string _IV_REFERENCIA, string _IV_TRAFICO, string _IV_TAMANO, string _IV_TIPO, DateTime? _IV_CAS, 
        string _IV_BOOKING, string _IV_IMDT, bool _IV_BLOQUEO, DateTime? _IV_FECHA_ULTIMA, string _IV_IN_OUT, string _IV_FULL_VACIO,
        string _IV_AISV, string _IV_REEFER, bool _IV_VISTO, string _IV_DOCUMENTO, string _IV_DES_BLOQUEO, string _IV_CONECTADO,
        string _IV_FACTURA, string _IV_ID_AGENTE, string _IV_ID_FACTURADO, bool _IV_PAGADO, bool _IV_CNTR_DD,
        string _IV_DESC_AGENTE, string _IV_DESC_FACTURADO, string _IV_ID_IMPORTADOR, string _IV_DESC_IMPORTADOR,
        DateTime? _IV_FECHA_TOPE_DLIBRE, DateTime? _IV_CNTR_DESCARGA, string _IV_MODULO, 
        DateTime? _IV_CNTR_DEPARTED, bool _IV_TIENE_SERVICIOS, string _IV_LINEA, Int64? _ID_UNIDAD,
        bool _IV_CERTIFICADO, string _IV_TIENE_CERTIFICADO
        )
        {
            this.IV_ID = _IV_ID;
            this.IV_GKEY = _IV_GKEY;
            this.IV_MRN = _IV_MRN;
            this.IV_MSN = _IV_MSN;
            this.IV_HSN = _IV_HSN;
            this.IV_HSN = _IV_HSN;
            this.IV_CONTENEDOR = _IV_CONTENEDOR;
            this.IV_TRAFICO = _IV_TRAFICO;

            this.IV_TAMANO = _IV_TAMANO;
            this.IV_TIPO = _IV_TIPO;
            this.IV_CAS = _IV_CAS;
            this._IV_BOOKING = _IV_BOOKING;

            this.IV_IMDT = _IV_IMDT;
            this.IV_BLOQUEO = _IV_BLOQUEO;
            this.IV_FECHA_ULTIMA = _IV_FECHA_ULTIMA;
            this.IV_IN_OUT = _IV_IN_OUT;
            this.IV_FULL_VACIO = _IV_FULL_VACIO;
            this.IV_AISV = _IV_AISV;
            this.IV_REEFER = _IV_REEFER;
            this.IV_DOCUMENTO = _IV_DOCUMENTO;
            this.IV_DES_BLOQUEO = _IV_DES_BLOQUEO;
            this.IV_CONECTADO = _IV_CONECTADO;
            this.IV_VISTO = _IV_VISTO;

            this.IV_FACTURA = _IV_FACTURA;
            this.IV_ID_AGENTE = _IV_ID_AGENTE;
            this.IV_ID_FACTURADO = _IV_ID_FACTURADO;
            this.IV_PAGADO = _IV_PAGADO;
            this.IV_CNTR_DD = _IV_CNTR_DD;
            this.IV_DESC_AGENTE = _IV_DESC_AGENTE;
            this.IV_DESC_FACTURADO = _IV_DESC_FACTURADO;
            this.IV_ID_IMPORTADOR = _IV_ID_IMPORTADOR;
            this.IV_DESC_IMPORTADOR = _IV_DESC_IMPORTADOR;

            this.IV_FECHA_TOPE_DLIBRE = _IV_FECHA_TOPE_DLIBRE;
            this.IV_CNTR_DESCARGA = _IV_CNTR_DESCARGA;
            this.IV_MODULO = _IV_MODULO;
            this.IV_CNTR_DEPARTED = _IV_CNTR_DEPARTED;

            this.IV_TIENE_SERVICIOS = _IV_TIENE_SERVICIOS;
            this.IV_LINEA = _IV_LINEA;
            this.ID_UNIDAD = _ID_UNIDAD;

            this.IV_CERTIFICADO = _IV_CERTIFICADO;
            this.IV_TIENE_CERTIFICADO = _IV_TIENE_CERTIFICADO;
        }

        private int? PreValidations(out string msg)
        {

            if (this.IV_GKEY <= 0)
            {
                msg = "Especifique el id del contenedor o la carga";
                return 0;
            }

            if (this.IV_ID <= 0)
            {
                msg = "Especifique el ID de la cabecera de la transacción";
                return 0;
            }

            if (string.IsNullOrEmpty(this.IV_MRN))
            {
                msg = "Especifique el MRN de la carga";
                return 0;
            }

            if (string.IsNullOrEmpty(this.IV_MSN))
            {
                msg = "Especifique el MSN de la carga";
                return 0;
            }

            if (string.IsNullOrEmpty(this.IV_HSN))
            {
                msg = "Especifique el HSN de la carga";
                return 0;
            }

           if (string.IsNullOrEmpty(this.IV_USUARIO_CREA))
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

            //MM-dd-yyyy
            if (this.IV_CAS == null)
            {
                this.IV_CAS = DateTime.Parse("01/01/1900");
            }
            if (this.IV_FECHA_ULTIMA == null)
            {
                this.IV_FECHA_ULTIMA = DateTime.Parse("01/01/1900");
            }

            if (this.IV_CONTENEDOR.Length > 30) { this.IV_CONTENEDOR = this.IV_CONTENEDOR.Substring(0, 29); }
            if (this.IV_REFERENCIA.Length > 50) { this.IV_REFERENCIA = this.IV_REFERENCIA.Substring(0, 49); }
            if (this.IV_TRAFICO.Length > 15) { this.IV_TRAFICO = this.IV_TRAFICO.Substring(0, 14); }
            if (this.IV_TAMANO.Length > 5) { this.IV_TAMANO = this.IV_TAMANO.Substring(0, 5); }
            if (this.IV_TIPO.Length > 10) { this.IV_TIPO = this.IV_TIPO.Substring(0, 9); }
            if (this.IV_BOOKING.Length > 50) { this.IV_BOOKING = this.IV_BOOKING.Substring(0, 49); }
            if (this.IV_IMDT.Length > 150) { this.IV_IMDT = this.IV_IMDT.Substring(0, 149); }
            if (this.IV_IN_OUT.Length > 5) { this.IV_IN_OUT = this.IV_IN_OUT.Substring(0, 4); }
            if (this.IV_FULL_VACIO.Length > 5) { this.IV_FULL_VACIO = this.IV_FULL_VACIO.Substring(0, 4); }
            if (this.IV_AISV.Length > 50) { this.IV_AISV = this.IV_AISV.Substring(0, 49); }
            if (this.IV_REEFER.Length > 5) { this.IV_REEFER = this.IV_REEFER.Substring(0, 4); }
            if (this.IV_DOCUMENTO.Length > 50) { this.IV_DOCUMENTO = this.IV_DOCUMENTO.Substring(0, 49); }
            if (this.IV_USUARIO_CREA.Length > 50) { this.IV_USUARIO_CREA = this.IV_USUARIO_CREA.Substring(0, 49); }
            if (this.IV_FACTURA.Length > 25) { this.IV_FACTURA = this.IV_FACTURA.Substring(0, 24); }
            if (this.IV_ID_AGENTE.Length > 20) { this.IV_ID_AGENTE = this.IV_ID_AGENTE.Substring(0, 19); }
            if (this.IV_ID_FACTURADO.Length > 20) { this.IV_ID_FACTURADO = this.IV_ID_FACTURADO.Substring(0, 19); }
            if (this.IV_DESC_AGENTE.Length > 200) { this.IV_DESC_AGENTE = this.IV_DESC_AGENTE.Substring(0, 199); }
            if (this.IV_DESC_FACTURADO.Length > 200) { this.IV_DESC_FACTURADO = this.IV_DESC_FACTURADO.Substring(0, 199); }
            if (this.IV_ID_IMPORTADOR.Length > 20) { this.IV_ID_IMPORTADOR = this.IV_ID_IMPORTADOR.Substring(0, 19); }
            if (this.IV_DESC_IMPORTADOR.Length > 200) { this.IV_DESC_IMPORTADOR = this.IV_DESC_IMPORTADOR.Substring(0, 199); }

            parametros.Clear();
            parametros.Add("IV_ID", this.IV_ID);
            parametros.Add("IV_MRN", this.IV_MRN);
            parametros.Add("IV_MSN", this.IV_MSN);
            parametros.Add("IV_HSN", this.IV_HSN);
            parametros.Add("IV_CONTENEDOR", this.IV_CONTENEDOR);
            parametros.Add("IV_GKEY", this.IV_GKEY);
            parametros.Add("IV_REFERENCIA", this.IV_REFERENCIA);
            parametros.Add("IV_TRAFICO", this.IV_TRAFICO);
            parametros.Add("IV_TAMANO", this.IV_TAMANO);
            parametros.Add("IV_TIPO", this.IV_TIPO);
            parametros.Add("IV_CAS", this.IV_CAS);
            parametros.Add("IV_BOOKING", this.IV_BOOKING);
            parametros.Add("IV_IMDT", this.IV_IMDT);
            parametros.Add("IV_BLOQUEO", this.IV_BLOQUEO);
            parametros.Add("IV_FECHA_ULTIMA", this.IV_FECHA_ULTIMA);
            parametros.Add("IV_IN_OUT", this.IV_IN_OUT);
            parametros.Add("IV_FULL_VACIO", this.IV_FULL_VACIO);
            parametros.Add("IV_AISV", this.IV_AISV);
            parametros.Add("IV_REEFER", this.IV_REEFER);
            parametros.Add("IV_USUARIO_CREA", this.IV_USUARIO_CREA);
            parametros.Add("IV_FECHA_CREA", this.IV_FECHA_CREA);
            parametros.Add("IV_DOCUMENTO", this.IV_DOCUMENTO);

            parametros.Add("IV_FACTURA", this.IV_FACTURA);
            parametros.Add("IV_ID_AGENTE", this.IV_ID_AGENTE);
            parametros.Add("IV_ID_FACTURADO", this.IV_ID_FACTURADO);
            parametros.Add("IV_PAGADO", this.IV_PAGADO);
            parametros.Add("IV_FECHA_HASTA", this.IV_FECHA_HASTA);
            parametros.Add("IV_CNTR_DD", this.IV_CNTR_DD);
            parametros.Add("IV_DESC_AGENTE", this.IV_DESC_AGENTE);
            parametros.Add("IV_DESC_FACTURADO", this.IV_DESC_FACTURADO);
            parametros.Add("IV_ID_IMPORTADOR", this.IV_ID_IMPORTADOR);
            parametros.Add("IV_DESC_IMPORTADOR", this.IV_DESC_IMPORTADOR);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 4000, "sp_Bil_inserta_invoice_det", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

     

    }
}
