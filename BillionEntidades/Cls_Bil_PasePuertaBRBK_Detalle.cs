using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Bil_PasePuertaBRBK_Detalle : Cls_Bil_Base
    {

        #region "Variables"


        private DateTime? _FECHA = null;
        private string _MRN = string.Empty;
        private string _MSN = string.Empty;
        private string _HSN = string.Empty;
        private string _SESION = string.Empty;

        private string _FACTURA = string.Empty;
        private string _CARGA = string.Empty;
        private string _AGENTE = string.Empty;
        private string _FACTURADO = string.Empty;
        private bool? _PAGADO;
        private Int64? _GKEY;
        private string _REFERENCIA = string.Empty;
        private string _CONTENEDOR = string.Empty;
        private string _DOCUMENTO = string.Empty;
        private string _PRIMERA = string.Empty;
        private string _MARCA = string.Empty;
        private int? _CANTIDAD = 0;
        private int? _CANTIDAD_CARGA = 0;
        private int? _BULTOS_HORARIOS = 0;
        private string _CIATRANS = string.Empty;
        private string _CHOFER = string.Empty;
        private string _ID_CIATRANS = string.Empty;
        private string _ID_CHOFER = string.Empty;
        private string _PLACA = string.Empty;
        private string _ID_EMPRESA = string.Empty;
        private string _ID_PLACA = string.Empty;

        private DateTime? _FECHA_SALIDA;
        private DateTime? _FECHA_SALIDA_PASE;
        private DateTime? _FECHA_AUT_PPWEB;
        private string _HORA_AUT_PPWEB = string.Empty;
        private DateTime? _FECHA_ULT_FAC;
        private string _TIPO_CNTR = string.Empty;
        private Int64? _ID_TURNO;
        private Int64? _TURNO;
        private string _D_TURNO = string.Empty;
        private double? _ID_PASE;
        private Int64? _ID_PPWEB;
        private Int64? _ID_PASE_WEB;
        private string _ESTADO = string.Empty;
        private bool? _ENVIADO;
        private bool? _AUTORIZADO;
        private bool? _VENTANILLA;
        private string _USUARIO_ING = string.Empty;
        private DateTime? _FECHA_ING;
        private string _USUARIO_MOD = string.Empty;
        private DateTime? _FECHA_MOD;
        private string _COMENTARIO = string.Empty;
        private string _USUARIO_AUT = string.Empty;
        private DateTime? _FECHA_AUT;
        private bool? _CAMBIO_FECHA;
        private DateTime? _FECHA_PASE;
        private DateTime? _TURNO_DESDE;
        private DateTime? _TURNO_HASTA;
        private bool _VISTO;

        private bool _CNTR_DD = false;
        private string _AGENTE_DESC = string.Empty;
        private string _FACTURADO_DESC = string.Empty;
        private string _IMPORTADOR = string.Empty;
        private string _IMPORTADOR_DESC = string.Empty;
        private string _TRANSPORTISTA_DESC = string.Empty;
        private string _CHOFER_DESC = string.Empty;

        private string _ESTADO_PAGO = string.Empty;

        private string _MOSTRAR_MENSAJE = string.Empty;
        private string _ORDENAMIENTO = string.Empty;
        private string _SUB_SECUENCIA = string.Empty;
        private string _LLAVE = string.Empty;
        private string _NUMERO_PASE_N4 = string.Empty;

        private bool? _SERVICIO;
        private string _IN_OUT;
        private string _CAMBIO_TURNO;
        private bool _ESTADO_TRANSACCION;
        private Int64? _ID_UNIDAD;

        private bool? _P2D;
        private bool? _ENVIADO_LIFTIF;
        private string _ORDER_ID;
        private string _ORDEN;
        private Int64? _ID_CIUDAD;
        private Int64? _ID_ZONA;
        private string _DIRECCION;
        private string _TRACKING_NUMBER;
        private decimal? _LATITUD;
        private decimal? _LONGITUD;
        private string _CONTACTO;
        private string _CLIENTE;
        private string _TELEFONOS;
        private string _EMAIL;
        private string _ID_CLIENTE;
        private string _PRODUCTO;
        private decimal? _PESO;
        private decimal? _VOLUMEN;
        private string _DIRECCION_CLIENTE;

        private int? _CANTIDAD_VEHICULOS = 0;
        private int? _CANTIDAD_BULTOS = 0;

        private string _ESTADO_PASE = string.Empty;
        private Int64 _PASE_DESDE_SOLICITUD;
        private string _PASE_EXPIRADO = string.Empty;
        private Int64? _ID_SOL;
        private Int64? _SECUENCIA_SOL;
        private Int64? _ID_TIPO_TURNO;
        private string _TIPO_TURNO = string.Empty;
        #endregion

        #region "Propiedades"


        public string MRN { get => _MRN; set => _MRN = value; }
        public string MSN { get => _MSN; set => _MSN = value; }
        public string HSN { get => _HSN; set => _HSN = value; }
        public string SESION { get => _SESION; set => _SESION = value; }
        public DateTime? FECHA { get => _FECHA; set => _FECHA = value; }


        public string FACTURA { get => _FACTURA; set => _FACTURA = value; }
        public string CARGA { get => _CARGA; set => _CARGA = value; }
        public string AGENTE { get => _AGENTE; set => _AGENTE = value; }
        public string FACTURADO { get => _FACTURADO; set => _FACTURADO = value; }
        public bool? PAGADO { get => _PAGADO; set => _PAGADO = value; }
        public Int64? GKEY { get => _GKEY; set => _GKEY = value; }
        public string REFERENCIA { get => _REFERENCIA; set => _REFERENCIA = value; }
        public string CONTENEDOR { get => _CONTENEDOR; set => _CONTENEDOR = value; }
        public string DOCUMENTO { get => _DOCUMENTO; set => _DOCUMENTO = value; }
        public string PRIMERA { get => _PRIMERA; set => _PRIMERA = value; }
        public string MARCA { get => _MARCA; set => _MARCA = value; }
        public int? CANTIDAD { get => _CANTIDAD; set => _CANTIDAD = value; }
        public int? CANTIDAD_CARGA { get => _CANTIDAD_CARGA; set => _CANTIDAD_CARGA = value; }
        public int? BULTOS_HORARIOS { get => _BULTOS_HORARIOS; set => _BULTOS_HORARIOS = value; }

        public string CIATRANS { get => _CIATRANS; set => _CIATRANS = value; }
        public string CHOFER { get => _CHOFER; set => _CHOFER = value; }
        public string ID_CIATRANS { get => _ID_CIATRANS; set => _ID_CIATRANS = value; }
        public string ID_CHOFER { get => _ID_CHOFER; set => _ID_CHOFER = value; }
        public string PLACA { get => _PLACA; set => _PLACA = value; }
        public string ID_EMPRESA { get => _ID_EMPRESA; set => _ID_EMPRESA = value; }
        public string ID_PLACA { get => _ID_PLACA; set => _ID_PLACA = value; }

        public DateTime? FECHA_SALIDA { get => _FECHA_SALIDA; set => _FECHA_SALIDA = value; }
        public DateTime? FECHA_SALIDA_PASE { get => _FECHA_SALIDA_PASE; set => _FECHA_SALIDA_PASE = value; }
        public DateTime? FECHA_AUT_PPWEB { get => _FECHA_AUT_PPWEB; set => _FECHA_AUT_PPWEB = value; }
        public string HORA_AUT_PPWEB { get => _HORA_AUT_PPWEB; set => _HORA_AUT_PPWEB = value; }
        public DateTime? FECHA_ULT_FAC { get => _FECHA_ULT_FAC; set => _FECHA_ULT_FAC = value; }

        public string TIPO_CNTR { get => _TIPO_CNTR; set => _TIPO_CNTR = value; }
        public Int64? ID_TURNO { get => _ID_TURNO; set => _ID_TURNO = value; }
        public Int64? TURNO { get => _TURNO; set => _TURNO = value; }
        public Int64? ID_PASE_WEB { get => _ID_PASE_WEB; set => _ID_PASE_WEB = value; }
        public string D_TURNO { get => _D_TURNO; set => _D_TURNO = value; }
        public double? ID_PASE { get => _ID_PASE; set => _ID_PASE = value; }
        public Int64? ID_PPWEB { get => _ID_PPWEB; set => _ID_PPWEB = value; }
        public string ESTADO { get => _ESTADO; set => _ESTADO = value; }
        public bool? ENVIADO { get => _ENVIADO; set => _ENVIADO = value; }
        public bool? AUTORIZADO { get => _AUTORIZADO; set => _AUTORIZADO = value; }
        public bool? VENTANILLA { get => _VENTANILLA; set => _VENTANILLA = value; }
        public string USUARIO_ING { get => _USUARIO_ING; set => _USUARIO_ING = value; }
        public DateTime? FECHA_ING { get => _FECHA_ING; set => _FECHA_ING = value; }
        public string USUARIO_MOD { get => _USUARIO_MOD; set => _USUARIO_MOD = value; }
        public DateTime? FECHA_MOD { get => _FECHA_MOD; set => _FECHA_MOD = value; }
        public string COMENTARIO { get => _COMENTARIO; set => _COMENTARIO = value; }
        public string USUARIO_AUT { get => _USUARIO_AUT; set => _USUARIO_AUT = value; }
        public DateTime? FECHA_AUT { get => _FECHA_AUT; set => _FECHA_AUT = value; }
        public bool? CAMBIO_FECHA { get => _CAMBIO_FECHA; set => _CAMBIO_FECHA = value; }
        public DateTime? TURNO_DESDE { get => _TURNO_DESDE; set => _TURNO_DESDE = value; }
        public DateTime? TURNO_HASTA { get => _TURNO_HASTA; set => _TURNO_HASTA = value; }
        public bool VISTO { get => _VISTO; set => _VISTO = value; }
        public DateTime? FECHA_PASE { get => _FECHA_PASE; set => _FECHA_PASE = value; }
        public bool CNTR_DD { get => _CNTR_DD; set => _CNTR_DD = value; }
        public string AGENTE_DESC { get => _AGENTE_DESC; set => _AGENTE_DESC = value; }
        public string FACTURADO_DESC { get => _FACTURADO_DESC; set => _FACTURADO_DESC = value; }
        public string IMPORTADOR { get => _IMPORTADOR; set => _IMPORTADOR = value; }
        public string IMPORTADOR_DESC { get => _IMPORTADOR_DESC; set => _IMPORTADOR_DESC = value; }

        public string TRANSPORTISTA_DESC { get => _TRANSPORTISTA_DESC; set => _TRANSPORTISTA_DESC = value; }
        public string CHOFER_DESC { get => _CHOFER_DESC; set => _CHOFER_DESC = value; }

        public string MOSTRAR_MENSAJE { get => _MOSTRAR_MENSAJE; set => _MOSTRAR_MENSAJE = value; }
        public string ESTADO_PAGO { get => _ESTADO_PAGO; set => _ESTADO_PAGO = value; }
        public string ORDENAMIENTO { get => _ORDENAMIENTO; set => _ORDENAMIENTO = value; }
        public string SUB_SECUENCIA { get => _SUB_SECUENCIA; set => _SUB_SECUENCIA = value; }
        public string LLAVE { get => _LLAVE; set => _LLAVE = value; }
        public string NUMERO_PASE_N4 { get => _NUMERO_PASE_N4; set => _NUMERO_PASE_N4 = value; }
        private static String v_mensaje = string.Empty;

        public string IN_OUT { get => _IN_OUT; set => _IN_OUT = value; }
        public bool? SERVICIO { get => _SERVICIO; set => _SERVICIO = value; }
        public string CAMBIO_TURNO { get => _CAMBIO_TURNO; set => _CAMBIO_TURNO = value; }
        public bool ESTADO_TRANSACCION { get => _ESTADO_TRANSACCION; set => _ESTADO_TRANSACCION = value; }
        public Int64? ID_UNIDAD { get => _ID_UNIDAD; set => _ID_UNIDAD = value; }

        public bool? P2D { get => _P2D; set => _P2D = value; }
        public bool? ENVIADO_LIFTIF { get => _ENVIADO_LIFTIF; set => _ENVIADO_LIFTIF = value; }
        public string ORDER_ID { get => _ORDER_ID; set => _ORDER_ID = value; }
        public string ORDEN { get => _ORDEN; set => _ORDEN = value; }

        public Int64? ID_CIUDAD { get => _ID_CIUDAD; set => _ID_CIUDAD = value; }
        public Int64? ID_ZONA { get => _ID_ZONA; set => _ID_ZONA = value; }
        public string DIRECCION { get => _DIRECCION; set => _DIRECCION = value; }
        public string TRACKING_NUMBER { get => _TRACKING_NUMBER; set => _TRACKING_NUMBER = value; }
        public decimal? LATITUD { get => _LATITUD; set => _LATITUD = value; }
        public decimal? LONGITUD { get => _LONGITUD; set => _LONGITUD = value; }
        public string CONTACTO { get => _CONTACTO; set => _CONTACTO = value; }
        public string CLIENTE { get => _CLIENTE; set => _CLIENTE = value; }
        public string TELEFONOS { get => _TELEFONOS; set => _TELEFONOS = value; }
        public string EMAIL { get => _EMAIL; set => _EMAIL = value; }
        public string ID_CLIENTE { get => _ID_CLIENTE; set => _ID_CLIENTE = value; }
        public string PRODUCTO { get => _PRODUCTO; set => _PRODUCTO = value; }
        public decimal? PESO { get => _PESO; set => _PESO = value; }
        public decimal? VOLUMEN { get => _VOLUMEN; set => _VOLUMEN = value; }
        public string DIRECCION_CLIENTE { get => _DIRECCION_CLIENTE; set => _DIRECCION_CLIENTE = value; }

        public int? CANTIDAD_VEHICULOS { get => _CANTIDAD_VEHICULOS; set => _CANTIDAD_VEHICULOS = value; }
        public int? CANTIDAD_BULTOS { get => _CANTIDAD_BULTOS; set => _CANTIDAD_BULTOS = value; }

        public string ESTADO_PASE { get => _ESTADO_PASE; set => _ESTADO_PASE = value; }
        public Int64 PASE_DESDE_SOLICITUD { get => _PASE_DESDE_SOLICITUD; set => _PASE_DESDE_SOLICITUD = value; }

        public string PASE_EXPIRADO { get => _PASE_EXPIRADO; set => _PASE_EXPIRADO = value; }

        public Int64? ID_SOL { get => _ID_SOL; set => _ID_SOL = value; }
        public Int64? SECUENCIA_SOL { get => _SECUENCIA_SOL; set => _SECUENCIA_SOL = value; }

        public Int64? ID_TIPO_TURNO { get => _ID_TIPO_TURNO; set => _ID_TIPO_TURNO = value; }
        public string TIPO_TURNO { get => _TIPO_TURNO; set => _TIPO_TURNO = value; }

        #endregion

        public Cls_Bil_PasePuertaBRBK_Detalle()
        {
            init();
        }

        public Cls_Bil_PasePuertaBRBK_Detalle(DateTime? _FECHA, string _MRN, string _MSN, string _HSN, string _SESION, string _FACTURA,
        string _CARGA, string _AGENTE, string _FACTURADO, bool? _PAGADO, Int64? _GKEY, string _REFERENCIA, string _CONTENEDOR, string _DOCUMENTO,
        string _PRIMERA, string _MARCA, int _CANTIDAD, string _CIATRANS, string _CHOFER, string _ID_CIATRANS, string _ID_CHOFER, string _PLACA,
        DateTime? _FECHA_SALIDA, DateTime? _FECHA_SALIDA_PASE, DateTime? _FECHA_AUT_PPWEB, string _HORA_AUT_PPWEB, DateTime? _FECHA_ULT_FAC, string _TIPO_CNTR,
        Int64? _ID_TURNO, Int64? _TURNO, string _D_TURNO, double? _ID_PASE, Int64? _ID_PPWEB, string _ESTADO, bool? _ENVIADO, bool? _AUTORIZADO, bool? _VENTANILLA,
        string _USUARIO_ING, DateTime? _FECHA_ING, string _USUARIO_MOD, DateTime? _FECHA_MOD, string _COMENTARIO, string _USUARIO_AUT, DateTime? _FECHA_AUT, bool? _CAMBIO_FECHA,
        DateTime? _TURNO_DESDE, DateTime? _TURNO_HASTA, bool _VISTO, bool _CNTR_DD, string _AGENTE_DESC, string _FACTURADO_DESC, string _IMPORTADOR, string _IMPORTADOR_DESC,
        string _TRANSPORTISTA_DESC, string _CHOFER_DESC, string _ESTADO_PAGO, string _MOSTRAR_MENSAJE, string _ORDENAMIENTO, string _SUB_SECUENCIA, string _LLAVE, Int64? _ID_UNIDAD,
         bool? _P2D, bool? _ENVIADO_LIFTIF, string _ORDER_ID, string _ORDEN, int? _CANTIDAD_BULTOS, int? _CANTIDAD_VEHICULOS
        )

        {
            this.MRN = _MRN;
            this.MSN = _MSN;
            this.HSN = _HSN;
            this.SESION = _SESION;
            this.FECHA = _FECHA;

            this.FACTURA = _FACTURA;
            this.CARGA = _CARGA;
            this.AGENTE = _AGENTE;
            this.FACTURADO = _FACTURADO;
            this.PAGADO = _PAGADO;
            this.GKEY = _GKEY;
            this.REFERENCIA = _REFERENCIA;
            this.CONTENEDOR = _CONTENEDOR;
            this.DOCUMENTO = _DOCUMENTO;
            this.PRIMERA = _PRIMERA;
            this.MARCA = _MARCA;
            this.CANTIDAD = _CANTIDAD;
            this.CIATRANS = _CIATRANS;
            this.CHOFER = _CHOFER;
            this.ID_CIATRANS = _ID_CIATRANS;
            this.ID_CHOFER = _ID_CHOFER;
            this.PLACA = _PLACA;
            this.FECHA_SALIDA = _FECHA_SALIDA;
            this.FECHA_SALIDA_PASE = _FECHA_SALIDA_PASE;
            this.FECHA_AUT_PPWEB = _FECHA_AUT_PPWEB;
            this.HORA_AUT_PPWEB = _HORA_AUT_PPWEB;
            this.FECHA_ULT_FAC = _FECHA_ULT_FAC;

            this.TIPO_CNTR = _TIPO_CNTR;
            this.ID_TURNO = _ID_TURNO;
            this.TURNO = _TURNO;
            this.D_TURNO = _D_TURNO;
            this.ID_PASE = _ID_PASE;
            this.ID_PPWEB = _ID_PPWEB;
            this.ESTADO = _ESTADO;
            this.ENVIADO = _ENVIADO;
            this.AUTORIZADO = _AUTORIZADO;
            this.VENTANILLA = _VENTANILLA;
            this.USUARIO_ING = _USUARIO_ING;
            this.FECHA_ING = _FECHA_ING;
            this.USUARIO_MOD = _USUARIO_MOD;
            this.FECHA_MOD = _FECHA_MOD;
            this.COMENTARIO = _COMENTARIO;
            this.USUARIO_AUT = _USUARIO_AUT;
            this.FECHA_AUT = _FECHA_AUT;
            this.CAMBIO_FECHA = _CAMBIO_FECHA;
            this.TURNO_DESDE = _TURNO_DESDE;
            this.TURNO_HASTA = _TURNO_HASTA;
            this.VISTO = _VISTO;

            this.CNTR_DD = _CNTR_DD;
            this.AGENTE_DESC = _AGENTE_DESC;
            this.FACTURADO_DESC = _FACTURADO_DESC;
            this.IMPORTADOR = _IMPORTADOR;
            this.IMPORTADOR_DESC = _IMPORTADOR_DESC;

            this.TRANSPORTISTA_DESC = _TRANSPORTISTA_DESC;
            this.CHOFER_DESC = _CHOFER_DESC;

            this.MOSTRAR_MENSAJE = _MOSTRAR_MENSAJE;
            this.ESTADO_PAGO = _ESTADO_PAGO;
            this.ORDENAMIENTO = _ORDENAMIENTO;
            this.SUB_SECUENCIA = _SUB_SECUENCIA;
            this.LLAVE = _LLAVE;
            this.ID_UNIDAD = _ID_UNIDAD;

            this.P2D = _P2D;
            this.ENVIADO_LIFTIF = _ENVIADO_LIFTIF;
            this.ORDER_ID = _ORDER_ID;
            this.ORDEN = _ORDEN;

            this.CANTIDAD_BULTOS = _CANTIDAD_BULTOS;
            this.CANTIDAD_VEHICULOS = _CANTIDAD_VEHICULOS;

        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("PORTAL_BILLION");
        }

        /*lista todas las proformas por rango de fechas*/
        public static List<Cls_Bil_PasePuertaBRBK_Detalle> Listado_Pases(DateTime FECHA_DESDE, DateTime FECHA_HASTA, string Agente, int Tipo, out string OnError)
        {

            OnInit();
            parametros.Clear();
            parametros.Add("FECHA_DESDE", FECHA_DESDE);
            parametros.Add("FECHA_HASTA", FECHA_HASTA);
            parametros.Add("RUC", Agente);
            parametros.Add("TIPO", Tipo);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_PasePuertaBRBK_Detalle>(sql_puntero.Conexion_Local, 6000, "sp_Bil_Rpt_Listado_Pases_Break_Bulk", parametros, out OnError);

        }

    }
}
