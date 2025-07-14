using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
using PasePuerta;


namespace BillionEntidades
{
    public  class Cls_Bil_Invoice_Cabecera : Cls_Bil_Base
    {

        #region "Variables"
        private static Int64? lm = -3;
        private Int64 _IV_ID;
        private string _IV_GLOSA = string.Empty;
        private DateTime? _IV_FECHA = null;
        private string _IV_TIPO_CARGA = string.Empty;
        private string _IV_ID_AGENTE = string.Empty;
        private string _IV_DESC_AGENTE = string.Empty;
        private string _IV_ID_CLIENTE = string.Empty;
        private string _IV_DESC_CLIENTE = string.Empty;
        private string _IV_EMAIL_CLIENTE = string.Empty;
        private string _IV_ID_FACTURADO = string.Empty;
        private string _IV_DESC_FACTURADO = string.Empty;
        private decimal _IV_SUBTOTAL = 0;
        private decimal _IV_IVA = 0;
        private decimal _IV_TOTAL = 0;
        private string _IV_NUMERO_CARGA = string.Empty;
        private string _IV_CONTENEDORES = string.Empty;
        private DateTime? _IV_FECHA_HASTA;
        private string _IV_DRAFT = string.Empty;
        private string _IV_FACTURA = string.Empty;

        private string _IV_BL = string.Empty;
        private string _IV_BUQUE = string.Empty;
        private string _IV_VIAJE = string.Empty;
        private string _IV_FECHA_ARRIBO = string.Empty;
        private string _IV_DIR_FACTURADO = string.Empty;
        private string _IV_EMAIL_FACTURADO = string.Empty;
        private string _IV_CIUDAD_FACTURADO = string.Empty;
        private Int64 _IV_DIAS_CREDITO = 0;
        private string _IV_HORA_HASTA = string.Empty;
        private string _IV_IP = string.Empty;
        private string _INVOICE_TYPE = string.Empty;

        private decimal _IV_TOTAL_BULTOS = 0;

        private string _IV_TIPO_CLIENTE = string.Empty;

        private bool _IV_P2D = false;
        private string _IV_RUC_USUARIO = string.Empty;
        private string _IV_DESC_USUARIO = string.Empty;

        #endregion

        #region "Propiedades"

        public Int64 IV_ID { get => _IV_ID; set => _IV_ID = value; }
        public string IV_GLOSA { get => _IV_GLOSA; set => _IV_GLOSA = value; }
        public DateTime? IV_FECHA { get => _IV_FECHA; set => _IV_FECHA = value; }
        public string IV_TIPO_CARGA { get => _IV_TIPO_CARGA; set => _IV_TIPO_CARGA = value; }
        public string IV_ID_AGENTE { get => _IV_ID_AGENTE; set => _IV_ID_AGENTE = value; }
        public string IV_DESC_AGENTE { get => _IV_DESC_AGENTE; set => _IV_DESC_AGENTE = value; }
        public string IV_ID_CLIENTE { get => _IV_ID_CLIENTE; set => _IV_ID_CLIENTE = value; }
        public string IV_DESC_CLIENTE { get => _IV_DESC_CLIENTE; set => _IV_DESC_CLIENTE = value; }
        public string IV_EMAIL_CLIENTE { get => _IV_EMAIL_CLIENTE; set => _IV_EMAIL_CLIENTE = value; }
        public string IV_ID_FACTURADO { get => _IV_ID_FACTURADO; set => _IV_ID_FACTURADO = value; }
        public string IV_DESC_FACTURADO { get => _IV_DESC_FACTURADO; set => _IV_DESC_FACTURADO = value; }

 
        public decimal IV_SUBTOTAL { get => _IV_SUBTOTAL; set => _IV_SUBTOTAL = value; }
        public decimal IV_IVA { get => _IV_IVA; set => _IV_IVA = value; }
        public decimal IV_TOTAL { get => _IV_TOTAL; set => _IV_TOTAL = value; }


        public DateTime? IV_FECHA_HASTA { get => _IV_FECHA_HASTA; set => _IV_FECHA_HASTA = value; }
        public string IV_NUMERO_CARGA { get => _IV_NUMERO_CARGA; set => _IV_NUMERO_CARGA = value; }
        public string IV_CONTENEDORES { get => _IV_CONTENEDORES; set => _IV_CONTENEDORES = value; }

        public string IV_DRAFT { get => _IV_DRAFT; set => _IV_DRAFT = value; }
        public string IV_FACTURA { get => _IV_FACTURA; set => _IV_FACTURA = value; }

        public string IV_BL { get => _IV_BL; set => _IV_BL = value; }
        public string IV_BUQUE { get => _IV_BUQUE; set => _IV_BUQUE = value; }
        public string IV_VIAJE { get => _IV_VIAJE; set => _IV_VIAJE = value; }
        public string IV_FECHA_ARRIBO { get => _IV_FECHA_ARRIBO; set => _IV_FECHA_ARRIBO = value; }
        public string IV_DIR_FACTURADO { get => _IV_DIR_FACTURADO; set => _IV_DIR_FACTURADO = value; }
        public string IV_EMAIL_FACTURADO { get => _IV_EMAIL_FACTURADO; set => _IV_EMAIL_FACTURADO = value; }
        public string IV_CIUDAD_FACTURADO { get => _IV_CIUDAD_FACTURADO; set => _IV_CIUDAD_FACTURADO = value; }
        public Int64  IV_DIAS_CREDITO { get => _IV_DIAS_CREDITO; set => _IV_DIAS_CREDITO = value; }
        public string IV_HORA_HASTA { get => _IV_HORA_HASTA; set => _IV_HORA_HASTA = value; }
        public string IV_IP { get => _IV_IP; set => _IV_IP = value; }
        private static String v_mensaje = string.Empty;
        public string INVOICE_TYPE { get => _INVOICE_TYPE; set => _INVOICE_TYPE = value; }

        public decimal IV_TOTAL_BULTOS { get => _IV_TOTAL_BULTOS; set => _IV_TOTAL_BULTOS = value; }

        public string IV_TIPO_CLIENTE { get => _IV_TIPO_CLIENTE; set => _IV_TIPO_CLIENTE = value; }

        public bool IV_P2D { get => _IV_P2D; set => _IV_P2D = value; }
        public string IV_RUC_USUARIO { get => _IV_RUC_USUARIO; set => _IV_RUC_USUARIO = value; }
        public string IV_DESC_USUARIO { get => _IV_DESC_USUARIO; set => _IV_DESC_USUARIO = value; }

        #endregion

        public List<Cls_Bil_Invoice_Detalle> Detalle { get; set; }
        public List<Cls_Bil_Invoice_Servicios> DetalleServicios { get; set; }
        public List<Cls_Bil_DetalleClientes> Detalle_Clientes { get; set; }

        public Cls_Bil_Invoice_Cabecera()
        {
            init();
            this.Detalle = new List<Cls_Bil_Invoice_Detalle>();
            this.DetalleServicios = new List<Cls_Bil_Invoice_Servicios>();
            this.Detalle_Clientes = new List<Cls_Bil_DetalleClientes>();

        }

        public Cls_Bil_Invoice_Cabecera(Int64 _IV_ID, string _IV_GLOSA, DateTime? IV_FECHA, string _IV_TIPO_CARGA, string _IV_ID_AGENTE, string _IV_DESC_AGENTE,
                                        string _IV_ID_CLIENTE, string _IV_DESC_CLIENTE, string _IV_ID_FACTURADO, string _IV_DESC_FACTURADO,
                                        decimal _IV_SUBTOTAL, decimal _IV_IVA, decimal _IV_TOTAL,
                                        string _IV_USUARIO_CREA, DateTime? _IV_FECHA_CREA, string _IV_NUMERO_CARGA, string _IV_CONTENEDORES, DateTime? _IV_FECHA_HASTA,
                                        string _IV_DRAFT, string _IV_FACTURA, string _IV_BL, string _IV_BUQUE, string _IV_VIAJE, string _IV_FECHA_ARRIBO, string _IV_DIR_FACTURADO,
                                        string _IV_EMAIL_FACTURADO, string _IV_CIUDAD_FACTURADO, Int64 _IV_DIAS_CREDITO, 
                                        string _IV_HORA_HASTA, string _IV_IP, string _INVOICE_TYPE, string _IV_EMAIL_CLIENTE, string IV_TIPO_CLIENTE)

        {
            this.IV_ID = _IV_ID;
            this._IV_GLOSA = _IV_GLOSA;
            this.IV_FECHA = _IV_FECHA;
            this.IV_TIPO_CARGA = _IV_TIPO_CARGA;
            this.IV_ID_AGENTE = _IV_ID_AGENTE;
            this.IV_DESC_AGENTE = _IV_DESC_AGENTE;
            this.IV_ID_CLIENTE = _IV_ID_CLIENTE;
            this.IV_DESC_CLIENTE = _IV_DESC_CLIENTE;
            this.IV_ID_FACTURADO = _IV_ID_FACTURADO;
            this.IV_DESC_FACTURADO = _IV_DESC_FACTURADO;
            this.IV_SUBTOTAL = _IV_SUBTOTAL;
            this.IV_IVA = _IV_IVA;
            this.IV_TOTAL = _IV_TOTAL;
            this.IV_USUARIO_CREA = _IV_USUARIO_CREA;
            this.IV_FECHA_CREA = _IV_FECHA_CREA;
            this.IV_NUMERO_CARGA = _IV_NUMERO_CARGA;
            this.IV_CONTENEDORES = _IV_CONTENEDORES;
            this.IV_FECHA_HASTA = _IV_FECHA_HASTA;
            this.IV_DRAFT = _IV_DRAFT;
            this.IV_FACTURA = _IV_FACTURA;

            this.IV_BL = _IV_BL;
            this.IV_BUQUE = _IV_BUQUE;
            this.IV_VIAJE = _IV_VIAJE;
            this.IV_FECHA_ARRIBO = _IV_FECHA_ARRIBO;
            this.IV_DIR_FACTURADO = _IV_DIR_FACTURADO;
            this.IV_EMAIL_FACTURADO = _IV_EMAIL_FACTURADO;
            this.IV_CIUDAD_FACTURADO = _IV_CIUDAD_FACTURADO;
            this.IV_DIAS_CREDITO = _IV_DIAS_CREDITO;
            this.IV_HORA_HASTA = _IV_HORA_HASTA;
            this.IV_IP = _IV_IP;
            this.INVOICE_TYPE = _INVOICE_TYPE;
            this.IV_EMAIL_CLIENTE = _IV_EMAIL_CLIENTE;
            this.IV_TIPO_CLIENTE = _IV_TIPO_CLIENTE;

            this.Detalle = new List<Cls_Bil_Invoice_Detalle>();
            this.DetalleServicios = new List<Cls_Bil_Invoice_Servicios>();
            this.Detalle_Clientes = new List<Cls_Bil_DetalleClientes>();
        }


        private int? PreValidationsTransaction(out string msg)
        {
           
            if (!this.IV_FECHA.HasValue)
            {

                msg = "La fecha de la transacción no es valida";
                return 0;
            }

            if (string.IsNullOrEmpty(this.IV_TIPO_CARGA))
            {
                msg = "Debe especificar el tipo de carga (CONTENEDOR, CFS, BRBK) ";
                return 0;

            }

            if (string.IsNullOrEmpty(this.IV_ID_AGENTE))
            {
                msg = "Debe especificar el ID del agente de aduana ";
                return 0;

            }
            if (string.IsNullOrEmpty(this.IV_DESC_AGENTE))
            {
                msg = "Debe especificar la descripción del agente de aduana ";
                return 0;

            }
            if (string.IsNullOrEmpty(this.IV_ID_CLIENTE))
            {
                msg = "Debe especificar el ID del cliente ";
                return 0;

            }
            if (string.IsNullOrEmpty(this.IV_DESC_CLIENTE))
            {
                msg = "Debe especificar la descripción del cliente ";
                return 0;

            }
            if (this.IV_TOTAL <= 0)
            {
                msg = "Especifique los totales de la transacción";
                return 0;
            }

            if (string.IsNullOrEmpty(this.IV_USUARIO_CREA))
            {
                msg = "Debe especificar el usuario que crea la transacción";
                return 0;
            }


            //cuenta solo los activos
            var nRegistros = this.Detalle.Where(d => d.IV_GKEY != 0).Count();

            if (this.Detalle == null || nRegistros <= 0)
            {
                msg = "No se puede agregar el detalle de la transaccion, seleccione las cargas a facturar";
                return 0;
            }

          
            nRegistros = this.DetalleServicios.Where(d => d.IV_SUBTOTAL != 0).Count();
            if (this.DetalleServicios == null || nRegistros <= 0)
            {
                msg = "No se puede agregar el detalle de servicios, no existen rubros a facturar";
                return 0;
            }
           

            msg = string.Empty;
            return 1;
        }

        private int? PreValidationsTransaction_Traza(out string msg)
        {

            if (!this.IV_FECHA.HasValue)
            {

                msg = "La fecha de la transacción no es valida";
                return 0;
            }

            if (string.IsNullOrEmpty(this.IV_TIPO_CARGA))
            {
                msg = "Debe especificar el tipo de carga (CONTENEDOR, CFS, BRBK) ";
                return 0;

            }

            if (string.IsNullOrEmpty(this.IV_ID_AGENTE))
            {
                msg = "Debe especificar el ID del agente de aduana ";
                return 0;

            }
            if (string.IsNullOrEmpty(this.IV_DESC_AGENTE))
            {
                msg = "Debe especificar la descripción del agente de aduana ";
                return 0;

            }
            if (string.IsNullOrEmpty(this.IV_ID_CLIENTE))
            {
                msg = "Debe especificar el ID del cliente ";
                return 0;

            }
            if (string.IsNullOrEmpty(this.IV_DESC_CLIENTE))
            {
                msg = "Debe especificar la descripción del cliente ";
                return 0;

            }
            if (this.IV_TOTAL <= 0)
            {
                msg = "Especifique los totales de la transacción";
                return 0;
            }

            if (string.IsNullOrEmpty(this.IV_USUARIO_CREA))
            {
                msg = "Debe especificar el usuario que crea la transacción";
                return 0;
            }


            msg = string.Empty;
            return 1;
        }

        private Int64? Save(out string OnError)
        {

            parametros.Clear();
            this.IV_ID = 0;
           
            if (this.IV_TIPO_CARGA.Length > 10) {
                this.IV_TIPO_CARGA = this.IV_TIPO_CARGA.Substring(0,10);

            }
            if (this.IV_ID_AGENTE.Length > 20) { this.IV_ID_AGENTE = this.IV_ID_AGENTE.Substring(0, 19);  }
            if (this.IV_DESC_AGENTE.Length > 200) { this.IV_DESC_AGENTE = this.IV_DESC_AGENTE.Substring(0, 199);  }
            if (this.IV_ID_CLIENTE.Length > 20) { this.IV_ID_CLIENTE = this.IV_ID_CLIENTE.Substring(0, 19); }
            if (this.IV_DESC_CLIENTE.Length > 200) { this.IV_DESC_CLIENTE = this.IV_DESC_CLIENTE.Substring(0, 199); }
            if (this.IV_ID_FACTURADO.Length > 20) { this.IV_ID_FACTURADO = this.IV_ID_FACTURADO.Substring(0, 19); }
            if (this.IV_DESC_FACTURADO.Length > 200) { this.IV_DESC_FACTURADO = this.IV_DESC_FACTURADO.Substring(0, 199); }
            if (this.IV_DRAFT.Length > 15) { this.IV_DRAFT = this.IV_DRAFT.Substring(0, 14); }
            if (this.IV_FACTURA.Length > 25) { this.IV_FACTURA = this.IV_FACTURA.Substring(0, 24); }
            if (this.IV_NUMERO_CARGA.Length > 50) { this.IV_NUMERO_CARGA = this.IV_NUMERO_CARGA.Substring(0, 49); }
            if (this.IV_BL.Length > 50) { this.IV_BL = this.IV_BL.Substring(0, 49); }
            if (this.IV_BUQUE.Length > 50) { this.IV_BUQUE = this.IV_BUQUE.Substring(0, 49); }
            if (this.IV_VIAJE.Length > 50) { this.IV_VIAJE = this.IV_VIAJE.Substring(0, 49); }
            if (this.IV_FECHA_ARRIBO.Length > 10) { this.IV_FECHA_ARRIBO = this.IV_FECHA_ARRIBO.Substring(0, 10); }
            if (this.IV_DIR_FACTURADO.Length > 200) { this.IV_DIR_FACTURADO = this.IV_DIR_FACTURADO.Substring(0, 199); }
            if (this.IV_EMAIL_FACTURADO.Length > 150) { this.IV_EMAIL_FACTURADO = this.IV_EMAIL_FACTURADO.Substring(0, 149); }
            if (this.IV_CIUDAD_FACTURADO.Length > 25) { this.IV_CIUDAD_FACTURADO = this.IV_CIUDAD_FACTURADO.Substring(0, 24); }
            if (this.IV_USUARIO_CREA.Length > 50) { this.IV_USUARIO_CREA = this.IV_USUARIO_CREA.Substring(0, 49); }
            if (this.IV_HORA_HASTA.Length > 10) { this.IV_HORA_HASTA = this.IV_HORA_HASTA.Substring(0, 10); }
            if (this.IV_IP.Length > 20) { this.IV_IP = this.IV_IP.Substring(0, 19); }

            parametros.Add("IV_ID", this.IV_ID);
            parametros.Add("IV_GLOSA", this.IV_GLOSA);
            parametros.Add("IV_FECHA", this.IV_FECHA);
            parametros.Add("IV_TIPO_CARGA", this.IV_TIPO_CARGA);
            parametros.Add("IV_ID_AGENTE", this.IV_ID_AGENTE);
            parametros.Add("IV_DESC_AGENTE", this.IV_DESC_AGENTE);
            parametros.Add("IV_ID_CLIENTE", this.IV_ID_CLIENTE);
            parametros.Add("IV_DESC_CLIENTE", this.IV_DESC_CLIENTE);
            parametros.Add("IV_ID_FACTURADO", this.IV_ID_FACTURADO);
            parametros.Add("IV_DESC_FACTURADO", this.IV_DESC_FACTURADO);
            parametros.Add("IV_SUBTOTAL", this.IV_SUBTOTAL);
            parametros.Add("IV_IVA", this.IV_IVA);
            parametros.Add("IV_TOTAL", this.IV_TOTAL);
            parametros.Add("IV_DRAFT", this.IV_DRAFT);
            parametros.Add("IV_FACTURA", this.IV_FACTURA);
            parametros.Add("IV_NUMERO_CARGA", this.IV_NUMERO_CARGA);
            parametros.Add("IV_FECHA_HASTA", this.IV_FECHA_HASTA);

            parametros.Add("IV_BL", this.IV_BL);
            parametros.Add("IV_BUQUE", this.IV_BUQUE);
            parametros.Add("IV_VIAJE", this.IV_VIAJE);
            parametros.Add("IV_FECHA_ARRIBO", this.IV_FECHA_ARRIBO);
            parametros.Add("IV_DIR_FACTURADO", this.IV_DIR_FACTURADO);
            parametros.Add("IV_EMAIL_FACTURADO", this.IV_EMAIL_FACTURADO);
            parametros.Add("IV_CIUDAD_FACTURADO", this.IV_CIUDAD_FACTURADO);
            parametros.Add("IV_DIAS_CREDITO", this.IV_DIAS_CREDITO);

            parametros.Add("IV_USUARIO_CREA", this.IV_USUARIO_CREA);
            parametros.Add("IV_FECHA_CREA", this.IV_FECHA_CREA);

            parametros.Add("IV_HORA_HASTA", this.IV_HORA_HASTA);
            parametros.Add("IV_IP", this.IV_IP);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 4000, "sp_Bil_inserta_invoice_cab", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        private Int64? Save_Traza(out string OnError)
        {

            parametros.Clear();
            this.IV_ID = 0;

            if (string.IsNullOrEmpty(this.IV_GLOSA)) { this.IV_GLOSA = "";  }
            if (string.IsNullOrEmpty(this.IV_TIPO_CARGA)) { this.IV_TIPO_CARGA = ""; }
            if (string.IsNullOrEmpty(this.IV_ID_AGENTE)) { this.IV_ID_AGENTE = ""; }
            if (string.IsNullOrEmpty(this.IV_DESC_AGENTE)) { this.IV_DESC_AGENTE = ""; }
            if (string.IsNullOrEmpty(this.IV_ID_CLIENTE)) { this.IV_ID_CLIENTE = ""; }
            if (string.IsNullOrEmpty(this.IV_DESC_CLIENTE)) { this.IV_DESC_CLIENTE = ""; }
            if (string.IsNullOrEmpty(this.IV_ID_FACTURADO)) { this.IV_ID_FACTURADO = ""; }
            if (string.IsNullOrEmpty(this.IV_DESC_FACTURADO)) { this.IV_DESC_FACTURADO = ""; }
            if (string.IsNullOrEmpty(this.IV_NUMERO_CARGA)) { this.IV_NUMERO_CARGA = ""; }
            if (string.IsNullOrEmpty(this.IV_BL)) { this.IV_BL = ""; }
            if (string.IsNullOrEmpty(this.IV_BUQUE)) { this.IV_BUQUE = ""; }
            if (string.IsNullOrEmpty(this.IV_VIAJE)) { this.IV_VIAJE = ""; }
            if (string.IsNullOrEmpty(this.IV_FECHA_ARRIBO)) { this.IV_FECHA_ARRIBO = ""; }
            if (string.IsNullOrEmpty(this.IV_DIR_FACTURADO)) { this.IV_DIR_FACTURADO = ""; }
            if (string.IsNullOrEmpty(this.IV_EMAIL_FACTURADO)) { this.IV_EMAIL_FACTURADO = ""; }
            if (string.IsNullOrEmpty(this.IV_CIUDAD_FACTURADO)) { this.IV_CIUDAD_FACTURADO = ""; }
            if (string.IsNullOrEmpty(this.IV_HORA_HASTA)) { this.IV_HORA_HASTA = ""; }
            if (string.IsNullOrEmpty(this.IV_IP)) { this.IV_IP = ""; }

            if (this.IV_TIPO_CARGA.Length > 10){ this.IV_TIPO_CARGA = this.IV_TIPO_CARGA.Substring(0, 10);}
            if (this.IV_ID_AGENTE.Length > 20) { this.IV_ID_AGENTE = this.IV_ID_AGENTE.Substring(0, 19); }
            if (this.IV_DESC_AGENTE.Length > 200) { this.IV_DESC_AGENTE = this.IV_DESC_AGENTE.Substring(0, 199); }
            if (this.IV_ID_CLIENTE.Length > 20) { this.IV_ID_CLIENTE = this.IV_ID_CLIENTE.Substring(0, 19); }
            if (this.IV_DESC_CLIENTE.Length > 200) { this.IV_DESC_CLIENTE = this.IV_DESC_CLIENTE.Substring(0, 199); }
            if (this.IV_ID_FACTURADO.Length > 20) { this.IV_ID_FACTURADO = this.IV_ID_FACTURADO.Substring(0, 19); }
            if (this.IV_DESC_FACTURADO.Length > 200) { this.IV_DESC_FACTURADO = this.IV_DESC_FACTURADO.Substring(0, 199); }
            
            if (this.IV_NUMERO_CARGA.Length > 50) { this.IV_NUMERO_CARGA = this.IV_NUMERO_CARGA.Substring(0, 49); }
            if (this.IV_BL.Length > 50) { this.IV_BL = this.IV_BL.Substring(0, 49); }
            if (this.IV_BUQUE.Length > 50) { this.IV_BUQUE = this.IV_BUQUE.Substring(0, 49); }
            if (this.IV_VIAJE.Length > 50) { this.IV_VIAJE = this.IV_VIAJE.Substring(0, 49); }
            if (this.IV_FECHA_ARRIBO.Length > 10) { this.IV_FECHA_ARRIBO = this.IV_FECHA_ARRIBO.Substring(0, 10); }
            if (this.IV_DIR_FACTURADO.Length > 200) { this.IV_DIR_FACTURADO = this.IV_DIR_FACTURADO.Substring(0, 199); }
            if (this.IV_EMAIL_FACTURADO.Length > 150) { this.IV_EMAIL_FACTURADO = this.IV_EMAIL_FACTURADO.Substring(0, 149); }
            if (this.IV_CIUDAD_FACTURADO.Length > 25) { this.IV_CIUDAD_FACTURADO = this.IV_CIUDAD_FACTURADO.Substring(0, 24); }
            if (this.IV_USUARIO_CREA.Length > 50) { this.IV_USUARIO_CREA = this.IV_USUARIO_CREA.Substring(0, 49); }
            if (this.IV_HORA_HASTA.Length > 10) { this.IV_HORA_HASTA = this.IV_HORA_HASTA.Substring(0, 10); }
            if (this.IV_IP.Length > 20) { this.IV_IP = this.IV_IP.Substring(0, 19); }



            parametros.Add("IV_GLOSA", this.IV_GLOSA);
            parametros.Add("IV_FECHA", this.IV_FECHA);
            parametros.Add("IV_TIPO_CARGA", this.IV_TIPO_CARGA);
            parametros.Add("IV_ID_AGENTE", this.IV_ID_AGENTE);
            parametros.Add("IV_DESC_AGENTE", this.IV_DESC_AGENTE);
            parametros.Add("IV_ID_CLIENTE", this.IV_ID_CLIENTE);
            parametros.Add("IV_DESC_CLIENTE", this.IV_DESC_CLIENTE);
            parametros.Add("IV_ID_FACTURADO", this.IV_ID_FACTURADO);
            parametros.Add("IV_DESC_FACTURADO", this.IV_DESC_FACTURADO);
            parametros.Add("IV_SUBTOTAL", this.IV_SUBTOTAL);
            parametros.Add("IV_IVA", this.IV_IVA);
            parametros.Add("IV_TOTAL", this.IV_TOTAL);
            parametros.Add("IV_NUMERO_CARGA", this.IV_NUMERO_CARGA);
            parametros.Add("IV_FECHA_HASTA", this.IV_FECHA_HASTA);
            parametros.Add("IV_BL", this.IV_BL);
            parametros.Add("IV_BUQUE", this.IV_BUQUE);
            parametros.Add("IV_VIAJE", this.IV_VIAJE);
            parametros.Add("IV_FECHA_ARRIBO", this.IV_FECHA_ARRIBO);
            parametros.Add("IV_DIR_FACTURADO", this.IV_DIR_FACTURADO);
            parametros.Add("IV_EMAIL_FACTURADO", this.IV_EMAIL_FACTURADO);
            parametros.Add("IV_CIUDAD_FACTURADO", this.IV_CIUDAD_FACTURADO);
            parametros.Add("IV_DIAS_CREDITO", this.IV_DIAS_CREDITO);
            parametros.Add("IV_USUARIO_CREA", this.IV_USUARIO_CREA);
            parametros.Add("IV_HORA_HASTA", this.IV_HORA_HASTA);
            parametros.Add("IV_IP", this.IV_IP);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "sp_Bil_inserta_invoice_cab_traza", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        private Int64? Save_Traza_cfs(out string OnError)
        {

            parametros.Clear();
            this.IV_ID = 0;

            if (string.IsNullOrEmpty(this.IV_GLOSA)) { this.IV_GLOSA = ""; }
            if (string.IsNullOrEmpty(this.IV_TIPO_CARGA)) { this.IV_TIPO_CARGA = ""; }
            if (string.IsNullOrEmpty(this.IV_ID_AGENTE)) { this.IV_ID_AGENTE = ""; }
            if (string.IsNullOrEmpty(this.IV_DESC_AGENTE)) { this.IV_DESC_AGENTE = ""; }
            if (string.IsNullOrEmpty(this.IV_ID_CLIENTE)) { this.IV_ID_CLIENTE = ""; }
            if (string.IsNullOrEmpty(this.IV_DESC_CLIENTE)) { this.IV_DESC_CLIENTE = ""; }
            if (string.IsNullOrEmpty(this.IV_ID_FACTURADO)) { this.IV_ID_FACTURADO = ""; }
            if (string.IsNullOrEmpty(this.IV_DESC_FACTURADO)) { this.IV_DESC_FACTURADO = ""; }
            if (string.IsNullOrEmpty(this.IV_NUMERO_CARGA)) { this.IV_NUMERO_CARGA = ""; }
            if (string.IsNullOrEmpty(this.IV_BL)) { this.IV_BL = ""; }
            if (string.IsNullOrEmpty(this.IV_BUQUE)) { this.IV_BUQUE = ""; }
            if (string.IsNullOrEmpty(this.IV_VIAJE)) { this.IV_VIAJE = ""; }
            if (string.IsNullOrEmpty(this.IV_FECHA_ARRIBO)) { this.IV_FECHA_ARRIBO = ""; }
            if (string.IsNullOrEmpty(this.IV_DIR_FACTURADO)) { this.IV_DIR_FACTURADO = ""; }
            if (string.IsNullOrEmpty(this.IV_EMAIL_FACTURADO)) { this.IV_EMAIL_FACTURADO = ""; }
            if (string.IsNullOrEmpty(this.IV_CIUDAD_FACTURADO)) { this.IV_CIUDAD_FACTURADO = ""; }
            if (string.IsNullOrEmpty(this.IV_HORA_HASTA)) { this.IV_HORA_HASTA = ""; }
            if (string.IsNullOrEmpty(this.IV_IP)) { this.IV_IP = ""; }

            if (this.IV_TIPO_CARGA.Length > 10) { this.IV_TIPO_CARGA = this.IV_TIPO_CARGA.Substring(0, 10); }
            if (this.IV_ID_AGENTE.Length > 20) { this.IV_ID_AGENTE = this.IV_ID_AGENTE.Substring(0, 19); }
            if (this.IV_DESC_AGENTE.Length > 200) { this.IV_DESC_AGENTE = this.IV_DESC_AGENTE.Substring(0, 199); }
            if (this.IV_ID_CLIENTE.Length > 20) { this.IV_ID_CLIENTE = this.IV_ID_CLIENTE.Substring(0, 19); }
            if (this.IV_DESC_CLIENTE.Length > 200) { this.IV_DESC_CLIENTE = this.IV_DESC_CLIENTE.Substring(0, 199); }
            if (this.IV_ID_FACTURADO.Length > 20) { this.IV_ID_FACTURADO = this.IV_ID_FACTURADO.Substring(0, 19); }
            if (this.IV_DESC_FACTURADO.Length > 200) { this.IV_DESC_FACTURADO = this.IV_DESC_FACTURADO.Substring(0, 199); }

            if (this.IV_NUMERO_CARGA.Length > 50) { this.IV_NUMERO_CARGA = this.IV_NUMERO_CARGA.Substring(0, 49); }
            if (this.IV_BL.Length > 50) { this.IV_BL = this.IV_BL.Substring(0, 49); }
            if (this.IV_BUQUE.Length > 50) { this.IV_BUQUE = this.IV_BUQUE.Substring(0, 49); }
            if (this.IV_VIAJE.Length > 50) { this.IV_VIAJE = this.IV_VIAJE.Substring(0, 49); }
            if (this.IV_FECHA_ARRIBO.Length > 10) { this.IV_FECHA_ARRIBO = this.IV_FECHA_ARRIBO.Substring(0, 10); }
            if (this.IV_DIR_FACTURADO.Length > 200) { this.IV_DIR_FACTURADO = this.IV_DIR_FACTURADO.Substring(0, 199); }
            if (this.IV_EMAIL_FACTURADO.Length > 150) { this.IV_EMAIL_FACTURADO = this.IV_EMAIL_FACTURADO.Substring(0, 149); }
            if (this.IV_CIUDAD_FACTURADO.Length > 25) { this.IV_CIUDAD_FACTURADO = this.IV_CIUDAD_FACTURADO.Substring(0, 24); }
            if (this.IV_USUARIO_CREA.Length > 50) { this.IV_USUARIO_CREA = this.IV_USUARIO_CREA.Substring(0, 49); }
            if (this.IV_HORA_HASTA.Length > 10) { this.IV_HORA_HASTA = this.IV_HORA_HASTA.Substring(0, 10); }
            if (this.IV_IP.Length > 20) { this.IV_IP = this.IV_IP.Substring(0, 19); }



            parametros.Add("IV_GLOSA", this.IV_GLOSA);
            parametros.Add("IV_FECHA", this.IV_FECHA);
            parametros.Add("IV_TIPO_CARGA", this.IV_TIPO_CARGA);
            parametros.Add("IV_ID_AGENTE", this.IV_ID_AGENTE);
            parametros.Add("IV_DESC_AGENTE", this.IV_DESC_AGENTE);
            parametros.Add("IV_ID_CLIENTE", this.IV_ID_CLIENTE);
            parametros.Add("IV_DESC_CLIENTE", this.IV_DESC_CLIENTE);
            parametros.Add("IV_ID_FACTURADO", this.IV_ID_FACTURADO);
            parametros.Add("IV_DESC_FACTURADO", this.IV_DESC_FACTURADO);
            parametros.Add("IV_SUBTOTAL", this.IV_SUBTOTAL);
            parametros.Add("IV_IVA", this.IV_IVA);
            parametros.Add("IV_TOTAL", this.IV_TOTAL);
            parametros.Add("IV_NUMERO_CARGA", this.IV_NUMERO_CARGA);
            parametros.Add("IV_FECHA_HASTA", this.IV_FECHA_HASTA);
            parametros.Add("IV_BL", this.IV_BL);
            parametros.Add("IV_BUQUE", this.IV_BUQUE);
            parametros.Add("IV_VIAJE", this.IV_VIAJE);
            parametros.Add("IV_FECHA_ARRIBO", this.IV_FECHA_ARRIBO);
            parametros.Add("IV_DIR_FACTURADO", this.IV_DIR_FACTURADO);
            parametros.Add("IV_EMAIL_FACTURADO", this.IV_EMAIL_FACTURADO);
            parametros.Add("IV_CIUDAD_FACTURADO", this.IV_CIUDAD_FACTURADO);
            parametros.Add("IV_DIAS_CREDITO", this.IV_DIAS_CREDITO);
            parametros.Add("IV_USUARIO_CREA", this.IV_USUARIO_CREA);
            parametros.Add("IV_HORA_HASTA", this.IV_HORA_HASTA);
            parametros.Add("IV_IP", this.IV_IP);
            parametros.Add("IV_TOTAL_BULTOS", this.IV_TOTAL_BULTOS);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "sp_Bil_inserta_invoice_cab_traza_cfs", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction(out string OnError)
        {
          
            string resultado_otros = null;
            Int64 ID = 0;
            try
            {
                //validaciones previas
                if (this.PreValidationsTransaction(out OnError) != 1)
                {
                    return 0;
                }
                using (var scope = new System.Transactions.TransactionScope())
                {
                    //grabar cabecera de la transaccion.
                    var id = this.Save(out OnError);
                    if (!id.HasValue)
                    {
                        return 0;
                    }

                    this.IV_ID = id.Value;
                    ID = id.Value;
                    var nContador = 1;

                    //si no falla la cabecera entonces añado detalle de contenedores
                    foreach (var i in this.Detalle)
                    {
                        i.IV_ID = ID;
                        i.IV_USUARIO_CREA = this.IV_USUARIO_CREA;
                        i.IV_FECHA_CREA = this.IV_FECHA_CREA;
                        i.IV_FACTURA = this.IV_FACTURA;
                        i.IV_ID_AGENTE = this.IV_ID_AGENTE;
                        i.IV_ID_FACTURADO = this.IV_ID_FACTURADO;
                        i.IV_DESC_AGENTE = this.IV_DESC_AGENTE;
                        i.IV_DESC_FACTURADO = this.IV_DESC_FACTURADO;
                        i.IV_ID_IMPORTADOR = this.IV_ID_CLIENTE;
                        i.IV_DESC_IMPORTADOR = this.IV_DESC_CLIENTE;

                        if (this.IV_DIAS_CREDITO != 0)
                        {
                            i.IV_PAGADO = true;
                        }
                        else
                        {
                            i.IV_PAGADO = false;
                        }
                        //i.IV_FECHA_HASTA = this.IV_FECHA_HASTA;

                        var IdRetorno = i.Save(out OnError);
                        if (!IdRetorno.HasValue || IdRetorno.Value <= 0)
                        {
                            OnError = "*** Error: al grabar detalle de factura y pase a puerta ****";
                            return 0;
                        }

                        //i.IV_ID = IdRetorno.Value;
                        nContador = nContador + 1;

                    }

                    nContador = 1;
                    //si no falla la cabecera entonces añada los items
                    foreach (var dn in this.DetalleServicios)
                    {
                        dn.IV_ID = ID;
                        dn.IV_LINEA = nContador;
                        dn.IV_USUARIO_CREA = this.IV_USUARIO_CREA;
                        dn.IV_FECHA_CREA = this.IV_FECHA_CREA;
                       
                        var IdRetorno = dn.Save(out OnError);
                        if (!IdRetorno.HasValue || IdRetorno.Value <= 0)
                        {
                            if (OnError == string.Empty)
                            {
                                OnError = "*** Error: No existen servicios disponibles ****";
                            }
                            return 0;
                        }

                        //dn.IV_ID = IdRetorno.Value;
                        nContador = nContador + 1;
                    }
                  
                    //fin de la transaccion
                    scope.Complete();



                    return ID;
                }
            }
            catch (Exception ex)
            {
                resultado_otros = ex.Message; ;
                OnError = string.Format("Ha ocurrido la excepción #{0}", resultado_otros);

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction), "SaveTransaction", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));


                return null;
            }
        }

        public Int64? SaveTransaction_Traza(out string OnError)
        {
 
            Int64 ID = 0;
            try
            {
                //validaciones previas
                if (this.PreValidationsTransaction_Traza(out OnError) != 1)
                {
                    return 0;
                }
                using (var scope = new System.Transactions.TransactionScope())
                {
                    //grabar cabecera de la transaccion.
                    var id = this.Save_Traza(out OnError);
                    if (!id.HasValue)
                    {
                        OnError = "*** Error: al grabar traza de factura ****";
                        return 0;
                    }
                    ID = id.Value;
                    
                    //fin de la transaccion
                    scope.Complete();

                    return ID;
                }
            }
            catch (Exception ex)
            {
               
                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Traza), "SaveTransaction_Traza", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }

        public Int64? SaveTransaction_Traza_cfs(out string OnError)
        {

            Int64 ID = 0;
            try
            {
                //validaciones previas
                if (this.PreValidationsTransaction_Traza(out OnError) != 1)
                {
                    return 0;
                }
                using (var scope = new System.Transactions.TransactionScope())
                {
                    //grabar cabecera de la transaccion.
                    var id = this.Save_Traza_cfs(out OnError);
                    if (!id.HasValue)
                    {
                        OnError = "*** Error: al grabar traza de factura de carga suelta ****";
                        return 0;
                    }
                    ID = id.Value;

                    //fin de la transaccion
                    scope.Complete();

                    return ID;
                }
            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Traza), "SaveTransaction_Traza_cfs", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }

        #region "Traza Break Bulk"
        private Int64? Save_Traza_brbk(out string OnError)
        {

            parametros.Clear();
            this.IV_ID = 0;

            if (string.IsNullOrEmpty(this.IV_GLOSA)) { this.IV_GLOSA = ""; }
            if (string.IsNullOrEmpty(this.IV_TIPO_CARGA)) { this.IV_TIPO_CARGA = ""; }
            if (string.IsNullOrEmpty(this.IV_ID_AGENTE)) { this.IV_ID_AGENTE = ""; }
            if (string.IsNullOrEmpty(this.IV_DESC_AGENTE)) { this.IV_DESC_AGENTE = ""; }
            if (string.IsNullOrEmpty(this.IV_ID_CLIENTE)) { this.IV_ID_CLIENTE = ""; }
            if (string.IsNullOrEmpty(this.IV_DESC_CLIENTE)) { this.IV_DESC_CLIENTE = ""; }
            if (string.IsNullOrEmpty(this.IV_ID_FACTURADO)) { this.IV_ID_FACTURADO = ""; }
            if (string.IsNullOrEmpty(this.IV_DESC_FACTURADO)) { this.IV_DESC_FACTURADO = ""; }
            if (string.IsNullOrEmpty(this.IV_NUMERO_CARGA)) { this.IV_NUMERO_CARGA = ""; }
            if (string.IsNullOrEmpty(this.IV_BL)) { this.IV_BL = ""; }
            if (string.IsNullOrEmpty(this.IV_BUQUE)) { this.IV_BUQUE = ""; }
            if (string.IsNullOrEmpty(this.IV_VIAJE)) { this.IV_VIAJE = ""; }
            if (string.IsNullOrEmpty(this.IV_FECHA_ARRIBO)) { this.IV_FECHA_ARRIBO = ""; }
            if (string.IsNullOrEmpty(this.IV_DIR_FACTURADO)) { this.IV_DIR_FACTURADO = ""; }
            if (string.IsNullOrEmpty(this.IV_EMAIL_FACTURADO)) { this.IV_EMAIL_FACTURADO = ""; }
            if (string.IsNullOrEmpty(this.IV_CIUDAD_FACTURADO)) { this.IV_CIUDAD_FACTURADO = ""; }
            if (string.IsNullOrEmpty(this.IV_HORA_HASTA)) { this.IV_HORA_HASTA = ""; }
            if (string.IsNullOrEmpty(this.IV_IP)) { this.IV_IP = ""; }

            if (this.IV_TIPO_CARGA.Length > 10) { this.IV_TIPO_CARGA = this.IV_TIPO_CARGA.Substring(0, 10); }
            if (this.IV_ID_AGENTE.Length > 20) { this.IV_ID_AGENTE = this.IV_ID_AGENTE.Substring(0, 19); }
            if (this.IV_DESC_AGENTE.Length > 200) { this.IV_DESC_AGENTE = this.IV_DESC_AGENTE.Substring(0, 199); }
            if (this.IV_ID_CLIENTE.Length > 20) { this.IV_ID_CLIENTE = this.IV_ID_CLIENTE.Substring(0, 19); }
            if (this.IV_DESC_CLIENTE.Length > 200) { this.IV_DESC_CLIENTE = this.IV_DESC_CLIENTE.Substring(0, 199); }
            if (this.IV_ID_FACTURADO.Length > 20) { this.IV_ID_FACTURADO = this.IV_ID_FACTURADO.Substring(0, 19); }
            if (this.IV_DESC_FACTURADO.Length > 200) { this.IV_DESC_FACTURADO = this.IV_DESC_FACTURADO.Substring(0, 199); }

            if (this.IV_NUMERO_CARGA.Length > 50) { this.IV_NUMERO_CARGA = this.IV_NUMERO_CARGA.Substring(0, 49); }
            if (this.IV_BL.Length > 50) { this.IV_BL = this.IV_BL.Substring(0, 49); }
            if (this.IV_BUQUE.Length > 50) { this.IV_BUQUE = this.IV_BUQUE.Substring(0, 49); }
            if (this.IV_VIAJE.Length > 50) { this.IV_VIAJE = this.IV_VIAJE.Substring(0, 49); }
            if (this.IV_FECHA_ARRIBO.Length > 10) { this.IV_FECHA_ARRIBO = this.IV_FECHA_ARRIBO.Substring(0, 10); }
            if (this.IV_DIR_FACTURADO.Length > 200) { this.IV_DIR_FACTURADO = this.IV_DIR_FACTURADO.Substring(0, 199); }
            if (this.IV_EMAIL_FACTURADO.Length > 150) { this.IV_EMAIL_FACTURADO = this.IV_EMAIL_FACTURADO.Substring(0, 149); }
            if (this.IV_CIUDAD_FACTURADO.Length > 25) { this.IV_CIUDAD_FACTURADO = this.IV_CIUDAD_FACTURADO.Substring(0, 24); }
            if (this.IV_USUARIO_CREA.Length > 50) { this.IV_USUARIO_CREA = this.IV_USUARIO_CREA.Substring(0, 49); }
            if (this.IV_HORA_HASTA.Length > 10) { this.IV_HORA_HASTA = this.IV_HORA_HASTA.Substring(0, 10); }
            if (this.IV_IP.Length > 20) { this.IV_IP = this.IV_IP.Substring(0, 19); }



            parametros.Add("IV_GLOSA", this.IV_GLOSA);
            parametros.Add("IV_FECHA", this.IV_FECHA);
            parametros.Add("IV_TIPO_CARGA", this.IV_TIPO_CARGA);
            parametros.Add("IV_ID_AGENTE", this.IV_ID_AGENTE);
            parametros.Add("IV_DESC_AGENTE", this.IV_DESC_AGENTE);
            parametros.Add("IV_ID_CLIENTE", this.IV_ID_CLIENTE);
            parametros.Add("IV_DESC_CLIENTE", this.IV_DESC_CLIENTE);
            parametros.Add("IV_ID_FACTURADO", this.IV_ID_FACTURADO);
            parametros.Add("IV_DESC_FACTURADO", this.IV_DESC_FACTURADO);
            parametros.Add("IV_SUBTOTAL", this.IV_SUBTOTAL);
            parametros.Add("IV_IVA", this.IV_IVA);
            parametros.Add("IV_TOTAL", this.IV_TOTAL);
            parametros.Add("IV_NUMERO_CARGA", this.IV_NUMERO_CARGA);
            parametros.Add("IV_FECHA_HASTA", this.IV_FECHA_HASTA);
            parametros.Add("IV_BL", this.IV_BL);
            parametros.Add("IV_BUQUE", this.IV_BUQUE);
            parametros.Add("IV_VIAJE", this.IV_VIAJE);
            parametros.Add("IV_FECHA_ARRIBO", this.IV_FECHA_ARRIBO);
            parametros.Add("IV_DIR_FACTURADO", this.IV_DIR_FACTURADO);
            parametros.Add("IV_EMAIL_FACTURADO", this.IV_EMAIL_FACTURADO);
            parametros.Add("IV_CIUDAD_FACTURADO", this.IV_CIUDAD_FACTURADO);
            parametros.Add("IV_DIAS_CREDITO", this.IV_DIAS_CREDITO);
            parametros.Add("IV_USUARIO_CREA", this.IV_USUARIO_CREA);
            parametros.Add("IV_HORA_HASTA", this.IV_HORA_HASTA);
            parametros.Add("IV_IP", this.IV_IP);
            parametros.Add("IV_TOTAL_BULTOS", this.IV_TOTAL_BULTOS);

            var db = sql_puntero.ExecuteInsertUpdateDeleteReturn(sql_puntero.Conexion_Local, 6000, "sp_Bil_inserta_invoice_cab_traza_brbk", parametros, out OnError);
            if (!db.HasValue || db.Value < 0)
            {
                return null;
            }
            OnError = string.Empty;
            return db.Value;

        }

        public Int64? SaveTransaction_Traza_brbk(out string OnError)
        {

            Int64 ID = 0;
            try
            {
                //validaciones previas
                if (this.PreValidationsTransaction_Traza(out OnError) != 1)
                {
                    return 0;
                }
                using (var scope = new System.Transactions.TransactionScope())
                {
                    //grabar cabecera de la transaccion.
                    var id = this.Save_Traza_brbk(out OnError);
                    if (!id.HasValue)
                    {
                        OnError = "*** Error: al grabar traza de factura de carga break bulk ****";
                        return 0;
                    }
                    ID = id.Value;

                    //fin de la transaccion
                    scope.Complete();

                    return ID;
                }
            }
            catch (Exception ex)
            {

                lm = SqlConexion.Cls_Conexion.LogEvent<Exception>("SQL", nameof(SaveTransaction_Traza), "SaveTransaction_Traza_brbk", false, null, null, ex.StackTrace, ex);
                OnError = string.Format("{0} {1}", string.Format("Excepcion no.{0}", lm), string.Format("Reporte el número el siguiente ticket de servicio:{0}", lm.HasValue ? lm : -2));

                return null;
            }
        }
        #endregion

    }

}
