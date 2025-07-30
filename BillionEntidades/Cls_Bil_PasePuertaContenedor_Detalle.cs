using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
namespace BillionEntidades
{
    public class Cls_Bil_PasePuertaContenedor_Detalle : Cls_Bil_Base
    {
        #region "Variables"
        private Int64 _ID_PPWEB;
       
        private string _MRN = string.Empty;
        private string _MSN = string.Empty;
        private string _HSN = string.Empty;
        private string _FACTURA = string.Empty;
        private string _CARGA = string.Empty;
        private string _AGENTE = string.Empty;
        private string _FACTURADO = string.Empty;
        private bool? _PAGADO;
        private Int64? _GKEY;
        private string _REFERENCIA = string.Empty;
        private string _CONTENEDOR = string.Empty;
        private string _DOCUMENTO = string.Empty;
        private string _CIATRANS = string.Empty;
        private string _CHOFER = string.Empty;
        private string _ID_CIATRANS = string.Empty;
        private string _ID_CHOFER = string.Empty;
        private string _PLACA = string.Empty;
        private DateTime? _CAS;
        private DateTime? _FECHA_SALIDA;
        private DateTime? _FECHA_SALIDA_PASE;
        private DateTime? _FECHA_AUT_PPWEB;
        private string _HORA_AUT_PPWEB = string.Empty;
        private string _TIPO_CNTR = string.Empty;
        private Int64? _ID_TURNO;
        private Int64? _TURNO;
        private string _D_TURNO = string.Empty;
        private double? _ID_PASE;
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

        private DateTime? _TURNO_DESDE ;
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
        private string _TIPO_CONSULTA = string.Empty;
        #endregion
        public int? ID { get; set; }
        public int? BloqueID { get; set; }
        public int? NumeroBloque { get; set; }
        public string NumeroContenedor { get; set; }
        public string Manifiesto { get; set; }
        public string BL { get; set; }
        public string EstadoContenedor { get; set; }
        public string NumeroOrden { get; set; }
        public DateTime? FechaOrdenTrabajo { get; set; }
        public int? TarjaID { get; set; }
        public DateTime? FechaTarja { get; set; }
        public string ruc_importador { get; set; }
        public string vin { get; set; }

        #region "Variables Modifica Pase Puerta"

        private DateTime? _FACTURADO_HASTA;
        private string _TIPO = string.Empty;
        private DateTime? _FECHA_TURNO;
        private string _HTURNO = string.Empty;
        private string _CIATRASNSP = string.Empty;
        private string _CONDUCTOR = string.Empty;
        private string _ID_EMPRESA = string.Empty;
        private Int64? _ID_PLAN;
        private Int64? _ID_SECUENCIA;
        private double? _NUMERO_PASE_N4;
        private Decimal _ID_CARGA;
        private DateTime? _FECHA_EXPIRACION;
        private DateTime? _TINICIA;
        private DateTime? _TFIN;
        private Int64? _TID;

        private DateTime? _FECHA_TURNO_NEW;
        private string _HORA_TURNO_NEW;
        private int? _ID_PLAN_NEW;
        private int? _ID_SECUENCIA_NEW;
        private bool? _SERVICIO;

        private string _IN_OUT;
        #endregion

        #region "Propiedades"

        public Int64 ID_PPWEB { get => _ID_PPWEB; set => _ID_PPWEB = value; } 
        public string MRN { get => _MRN; set => _MRN = value; }
        public string MSN { get => _MSN; set => _MSN = value; }
        public string HSN { get => _HSN; set => _HSN = value; }
        public string FACTURA { get => _FACTURA; set => _FACTURA = value; }
        public string CARGA { get => _CARGA; set => _CARGA = value; }
        public string AGENTE { get => _AGENTE; set => _AGENTE = value; }
        public string FACTURADO { get => _FACTURADO; set => _FACTURADO = value; }
        public bool? PAGADO { get => _PAGADO; set => _PAGADO = value; }
        public Int64? GKEY { get => _GKEY; set => _GKEY = value; }
        public string REFERENCIA { get => _REFERENCIA; set => _REFERENCIA = value; }
        public string CONTENEDOR { get => _CONTENEDOR; set => _CONTENEDOR = value; }
        public string DOCUMENTO { get => _DOCUMENTO; set => _DOCUMENTO = value; }
        public string CIATRANS { get => _CIATRANS; set => _CIATRANS = value; }
        public string CHOFER { get => _CHOFER; set => _CHOFER = value; }
        public string ID_CIATRANS { get => _ID_CIATRANS; set => _ID_CIATRANS = value; }
        public string ID_CHOFER { get => _ID_CHOFER; set => _ID_CHOFER = value; }
        public string PLACA { get => _PLACA; set => _PLACA = value; }
        public DateTime? CAS { get => _CAS; set => _CAS = value; }
        public DateTime? FECHA_SALIDA { get => _FECHA_SALIDA; set => _FECHA_SALIDA = value; }
        public DateTime? FECHA_SALIDA_PASE { get => _FECHA_SALIDA_PASE; set => _FECHA_SALIDA_PASE = value; }
        public DateTime? FECHA_AUT_PPWEB { get => _FECHA_AUT_PPWEB; set => _FECHA_AUT_PPWEB = value; }
        public string HORA_AUT_PPWEB { get => _HORA_AUT_PPWEB; set => _HORA_AUT_PPWEB = value; }
        public string TIPO_CNTR { get => _TIPO_CNTR; set => _TIPO_CNTR = value; }
        public Int64? ID_TURNO { get => _ID_TURNO; set => _ID_TURNO = value; }
        public Int64? TURNO { get => _TURNO; set => _TURNO = value; }
        public string D_TURNO { get => _D_TURNO; set => _D_TURNO = value; }
        public double? ID_PASE { get => _ID_PASE; set => _ID_PASE = value; }
        public Int64? ID_PASE_WEB { get => _ID_PASE_WEB; set => _ID_PASE_WEB = value; }
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

        public bool CNTR_DD { get => _CNTR_DD; set => _CNTR_DD = value; }
        public string AGENTE_DESC { get => _AGENTE_DESC; set => _AGENTE_DESC = value; }
        public string FACTURADO_DESC { get => _FACTURADO_DESC; set => _FACTURADO_DESC = value; }
        public string IMPORTADOR { get => _IMPORTADOR; set => _IMPORTADOR = value; }
        public string IMPORTADOR_DESC { get => _IMPORTADOR_DESC; set => _IMPORTADOR_DESC = value; }

        public string TRANSPORTISTA_DESC { get => _TRANSPORTISTA_DESC; set => _TRANSPORTISTA_DESC = value; }
        public string CHOFER_DESC { get => _CHOFER_DESC; set => _CHOFER_DESC = value; }

        public string MOSTRAR_MENSAJE { get => _MOSTRAR_MENSAJE; set => _MOSTRAR_MENSAJE = value; }
        public string TIPO_CONSULTA { get => _TIPO_CONSULTA; set => _TIPO_CONSULTA = value; }
        #endregion


        #region "Propiedades Modifica Pase Puerta"
        public DateTime? FACTURADO_HASTA { get => _FACTURADO_HASTA; set => _FACTURADO_HASTA = value; }
        public string TIPO { get => _TIPO; set => _TIPO = value; }
        public DateTime? FECHA_TURNO { get => _FECHA_TURNO; set => _FECHA_TURNO = value; }
        public string HTURNO { get => _HTURNO; set => _HTURNO = value; }
        public string CIATRASNSP { get => _CIATRASNSP; set => _CIATRASNSP = value; }
        public string CONDUCTOR { get => _CONDUCTOR; set => _CONDUCTOR = value; }
        public string ID_EMPRESA { get => _ID_EMPRESA; set => _ID_EMPRESA = value; }
        public Int64? ID_PLAN { get => _ID_PLAN; set => _ID_PLAN = value; }
        public Int64? ID_SECUENCIA { get => _ID_SECUENCIA; set => _ID_SECUENCIA = value; }
        public double? NUMERO_PASE_N4 { get => _NUMERO_PASE_N4; set => _NUMERO_PASE_N4 = value; }
        public Decimal ID_CARGA { get => _ID_CARGA; set => _ID_CARGA = value; }
        public DateTime? FECHA_EXPIRACION { get => _FECHA_EXPIRACION; set => _FECHA_EXPIRACION = value; }
        public DateTime? TINICIA { get => _TINICIA; set => _TINICIA = value; }
        public DateTime? TFIN { get => _TFIN; set => _TFIN = value; }
        public Int64? TID { get => _TID; set => _TID = value; }

        public DateTime? FECHA_TURNO_NEW { get => _FECHA_TURNO_NEW; set => _FECHA_TURNO_NEW = value; }
        public string HORA_TURNO_NEW { get => _HORA_TURNO_NEW; set => _HORA_TURNO_NEW = value; }
        public int? ID_PLAN_NEW { get => _ID_PLAN_NEW; set => _ID_PLAN_NEW = value; }
        public int? ID_SECUENCIA_NEW { get => _ID_SECUENCIA_NEW; set => _ID_SECUENCIA_NEW = value; }

        public string ESTADO_PAGO { get => _ESTADO_PAGO; set => _ESTADO_PAGO = value; }
        public bool? SERVICIO { get => _SERVICIO; set => _SERVICIO = value; }

        public string ORDENAMIENTO { get => _ORDENAMIENTO; set => _ORDENAMIENTO = value; }

        public string IN_OUT { get => _IN_OUT; set => _IN_OUT = value; }

        #endregion

        public Cls_Bil_PasePuertaContenedor_Detalle()
        {
            init();
        }


        public Cls_Bil_PasePuertaContenedor_Detalle( Int64 _ID_PPWEB, string _MRN ,string _MSN ,string _HSN , string _FACTURA,string _AGENTE , string _FACTURADO,bool? _PAGADO,
         Int64? _GKEY, string _REFERENCIA ,string _CONTENEDOR, string _DOCUMENTO, string _CIATRANS,string _CHOFER,string _PLACA , DateTime? _CAS,
         DateTime? _FECHA_SALIDA,DateTime? _FECHA_AUT_PPWEB, string _HORA_AUT_PPWEB, string _TIPO_CNTR,int? _ID_TURNO,Int64? _TURNO,string _D_TURNO,
         double? _ID_PASE,string _ESTADO, bool? _ENVIADO, bool? _AUTORIZADO,bool? _VENTANILLA, string _USUARIO_ING, DateTime? _FECHA_ING, string _USUARIO_MOD , DateTime? _FECHA_MOD,
         string _COMENTARIO , string _USUARIO_AUT , DateTime? _FECHA_AUT, bool? _CAMBIO_FECHA, DateTime? _TURNO_DESDE, DateTime? _TURNO_HASTA, bool _VISTO, DateTime? _FECHA_SALIDA_PASE,
         string _ID_CIATRANS , string _ID_CHOFER, bool _CNTR_DD, string _AGENTE_DESC, string _FACTURADO_DESC, string _IMPORTADOR, string _IMPORTADOR_DESC,
         string _TRANSPORTISTA_DESC, string _CHOFER_DESC, DateTime? _FACTURADO_HASTA, string _TIPO, DateTime? _FECHA_TURNO, string _HTURNO,
         string _CIATRASNSP, string _CONDUCTOR, string _ID_EMPRESA, Int64? _ID_PLAN, Int64? _ID_SECUENCIA, double? _NUMERO_PASE_N4,
         Decimal _ID_CARGA, DateTime? _FECHA_EXPIRACION, DateTime? _TINICIA, DateTime? _TFIN, Int64? _TID, string _MOSTRAR_MENSAJE, string _TIPO_CONSULTA)
        {


            this.ID_PPWEB = _ID_PPWEB;
            this.MRN =_MRN; 
            this.MSN = _MSN; 
            this.HSN = _HSN;
            this.FACTURA = _FACTURA;
            this.AGENTE = _AGENTE; 
            this.FACTURADO = _FACTURADO; 
            this.PAGADO = _PAGADO; 
            this.GKEY = _GKEY; 
            this.REFERENCIA = _REFERENCIA; 
            this.CONTENEDOR = _CONTENEDOR; 
            this.DOCUMENTO = _DOCUMENTO;
            this.CIATRANS = _CIATRANS;
            this.CHOFER = _CHOFER; 
            this.PLACA = _PLACA; 
            this.CAS = _CAS;
            this.FECHA_SALIDA = _FECHA_SALIDA; 
            this.FECHA_AUT_PPWEB = _FECHA_AUT_PPWEB; 
            this.HORA_AUT_PPWEB = _HORA_AUT_PPWEB; 
            this.TIPO_CNTR = _TIPO_CNTR;
            this.ID_TURNO = _ID_TURNO; 
            this.TURNO = _TURNO; 
            this.D_TURNO = _D_TURNO; 
            this.ID_PASE = _ID_PASE; 
            this.ESTADO = _ESTADO;
            this.ENVIADO = _ENVIADO; 
            this.AUTORIZADO = _AUTORIZADO;
            this.VENTANILLA = _VENTANILLA; 
            this.USUARIO_ING = _USUARIO_ING;
            this.FECHA_ING = _FECHA_ING; 
            this.USUARIO_MOD =_USUARIO_MOD; 
            this.FECHA_MOD = _FECHA_MOD; 
            this.COMENTARIO= _COMENTARIO; 
            this.USUARIO_AUT = _USUARIO_AUT; 
            this.FECHA_AUT = _FECHA_AUT; 
            this.CAMBIO_FECHA = _CAMBIO_FECHA;
            this.TURNO_DESDE = _TURNO_DESDE;
            this.TURNO_HASTA = _TURNO_HASTA;
            this.VISTO = _VISTO;
            this.FECHA_SALIDA_PASE = _FECHA_SALIDA_PASE;
            this.ID_CIATRANS = _ID_CIATRANS;
            this.ID_CHOFER = _ID_CHOFER;

            this.CNTR_DD = _CNTR_DD;
            this.AGENTE_DESC = _AGENTE_DESC;
            this.FACTURADO_DESC = _FACTURADO_DESC;
            this.IMPORTADOR = _IMPORTADOR;
            this.IMPORTADOR_DESC = _IMPORTADOR_DESC;

            this.TRANSPORTISTA_DESC = _TRANSPORTISTA_DESC;
            this.CHOFER_DESC = _CHOFER_DESC;

            this.FACTURADO_HASTA = _FACTURADO_HASTA;
            this.TIPO = _TIPO;
            this.FECHA_TURNO = _FECHA_TURNO;
            this.HTURNO = _HTURNO;
            this.CIATRASNSP = _CIATRASNSP;
            this.CONDUCTOR = _CONDUCTOR;
            this.ID_EMPRESA = _ID_EMPRESA;
            this.ID_PLAN = _ID_PLAN;
            this.ID_SECUENCIA = _ID_SECUENCIA;
            this.NUMERO_PASE_N4 = _NUMERO_PASE_N4;
            this.ID_CARGA = _ID_CARGA;
            this.FECHA_EXPIRACION = _FECHA_EXPIRACION;
            this.TINICIA = _TINICIA;
            this.TFIN = _TFIN;
            this.TID = _TID;
            this.MOSTRAR_MENSAJE = _MOSTRAR_MENSAJE;
            this.TIPO_CONSULTA = _TIPO_CONSULTA;
        }


        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("PORTAL_BILLION");
        }

        /*lista todas las proformas por rango de fechas*/
        public static List<Cls_Bil_PasePuertaContenedor_Detalle> Listado_Pases(DateTime FECHA_DESDE, DateTime FECHA_HASTA,  string Agente, int Tipo,out string OnError)
         {

            OnInit();
            parametros.Clear();
             parametros.Add("FECHA_DESDE", FECHA_DESDE);
             parametros.Add("FECHA_HASTA", FECHA_HASTA);
             parametros.Add("RUC", Agente);
            parametros.Add("TIPO", Tipo);
            return sql_puntero.ExecuteSelectControl<Cls_Bil_PasePuertaContenedor_Detalle>(sql_puntero.Conexion_Local, 5000, "sp_Bil_Rpt_Listado_Pases_Contenedor", parametros, out OnError);

         }
    }
}
