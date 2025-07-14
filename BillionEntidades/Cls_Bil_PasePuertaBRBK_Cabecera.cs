using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
using PasePuerta;


namespace BillionEntidades
{
    public class Cls_Bil_PasePuertaBRBK_Cabecera : Cls_Bil_Base
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
        private string _MARCA= string.Empty;
        private int? _CANTIDAD = 0;

        private string _CIATRANS = string.Empty;
        private string _CHOFER = string.Empty;
        private string _ID_CIATRANS = string.Empty;
        private string _ID_CHOFER = string.Empty;
        private string _PLACA = string.Empty;

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
        private Int64? _ID_UNIDAD;

        private double _CANTIDAD_EMITIDOS;
        private double _CNTR_CANTIDAD_SALDO;
        private decimal _CNTR_PESO;
        private double _CNTR_CANTIDAD_TOTAL;
        private string _CNTR_UBICACION;
        private int? _idProducto;
        private int? _idItem;
        private string _Tipo_Producto;
        private Int64? _ID_TIPO_TURNO;
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

        public string CIATRANS { get => _CIATRANS; set => _CIATRANS = value; }
        public string CHOFER { get => _CHOFER; set => _CHOFER = value; }
        public string ID_CIATRANS { get => _ID_CIATRANS; set => _ID_CIATRANS = value; }
        public string ID_CHOFER { get => _ID_CHOFER; set => _ID_CHOFER = value; }
        public string PLACA { get => _PLACA; set => _PLACA = value; }
     
        public DateTime? FECHA_SALIDA { get => _FECHA_SALIDA; set => _FECHA_SALIDA = value; }
        public DateTime? FECHA_SALIDA_PASE { get => _FECHA_SALIDA_PASE; set => _FECHA_SALIDA_PASE = value; }
        public DateTime? FECHA_AUT_PPWEB { get => _FECHA_AUT_PPWEB; set => _FECHA_AUT_PPWEB = value; }
        public string HORA_AUT_PPWEB { get => _HORA_AUT_PPWEB; set => _HORA_AUT_PPWEB = value; }
        public DateTime? FECHA_ULT_FAC { get => _FECHA_ULT_FAC; set => _FECHA_ULT_FAC = value; }

        public string TIPO_CNTR { get => _TIPO_CNTR; set => _TIPO_CNTR = value; }
        public Int64? ID_TURNO { get => _ID_TURNO; set => _ID_TURNO = value; }
        public Int64? TURNO { get => _TURNO; set => _TURNO = value; }
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
        public Int64? ID_UNIDAD { get => _ID_UNIDAD; set => _ID_UNIDAD = value; }

        public Int64? ID_TIPO_TURNO { get => _ID_TIPO_TURNO; set => _ID_TIPO_TURNO = value; }



        /*
            private double _CANTIDAD_EMITIDOS;
        private double _CNTR_CANTIDAD_SALDO;
        private decimal _CNTR_PESO;
        private double _CNTR_CANTIDAD_TOTAL;
        private string _CNTR_UBICACION;

             */

        public double CANTIDAD_EMITIDOS { get => _CANTIDAD_EMITIDOS; set => _CANTIDAD_EMITIDOS = value; }
        public double CNTR_CANTIDAD_SALDO { get => _CNTR_CANTIDAD_SALDO; set => _CNTR_CANTIDAD_SALDO = value; }
        public decimal CNTR_PESO { get => _CNTR_PESO; set => _CNTR_PESO = value; }
        public double CNTR_CANTIDAD_TOTAL { get => _CNTR_CANTIDAD_TOTAL; set => _CNTR_CANTIDAD_TOTAL = value; }
        public string CNTR_UBICACION { get => _CNTR_UBICACION; set => _CNTR_UBICACION = value; }

        public int? idProducto { get => _idProducto; set => _idProducto = value; }
        public int? idItem { get => _idItem; set => _idItem = value; }
        public string Tipo_Producto { get => _Tipo_Producto; set => _Tipo_Producto = value; }

     

        private static String v_mensaje = string.Empty;

        #endregion

        public List<Cls_Bil_PasePuertaBRBK_SubItems> DetalleSubItem { get; set; }
        public List<Cls_Bil_PasePuertaBRBK_Detalle> Detalle { get; set; }
        public List<Cls_Bil_CasManual> Detalle_Cas { get; set; }

        public Cls_Bil_PasePuertaBRBK_Cabecera()
        {
            init();
            this.DetalleSubItem = new List<Cls_Bil_PasePuertaBRBK_SubItems>();
            this.Detalle = new List<Cls_Bil_PasePuertaBRBK_Detalle>();
            this.Detalle_Cas = new List<Cls_Bil_CasManual>();
        }



        public Cls_Bil_PasePuertaBRBK_Cabecera(DateTime? _FECHA,string _MRN,string _MSN,string _HSN, string _SESION,string _FACTURA,
         string _CARGA, string _AGENTE,string _FACTURADO, bool? _PAGADO, Int64? _GKEY, string _REFERENCIA,string _CONTENEDOR,string _DOCUMENTO,
         string _PRIMERA,string _MARCA, int _CANTIDAD,string _CIATRANS, string _CHOFER,string _ID_CIATRANS,string _ID_CHOFER,string _PLACA,
         DateTime? _FECHA_SALIDA,DateTime? _FECHA_SALIDA_PASE,DateTime? _FECHA_AUT_PPWEB,string _HORA_AUT_PPWEB,DateTime? _FECHA_ULT_FAC, string _TIPO_CNTR,
         Int64? _ID_TURNO,Int64? _TURNO, string _D_TURNO , double? _ID_PASE, Int64? _ID_PPWEB, string _ESTADO ,bool? _ENVIADO, bool? _AUTORIZADO, bool? _VENTANILLA,
         string _USUARIO_ING,DateTime? _FECHA_ING,string _USUARIO_MOD, DateTime? _FECHA_MOD, string _COMENTARIO,string _USUARIO_AUT,DateTime? _FECHA_AUT, bool? _CAMBIO_FECHA,
         DateTime? _TURNO_DESDE, DateTime? _TURNO_HASTA, bool _VISTO, bool _CNTR_DD, string _AGENTE_DESC,string _FACTURADO_DESC,string _IMPORTADOR, string _IMPORTADOR_DESC,
         string _TRANSPORTISTA_DESC, string _CHOFER_DESC,string _ESTADO_PAGO, string _MOSTRAR_MENSAJE, string _ORDENAMIENTO , Int64? _ID_UNIDAD)

        {
            this.MRN=_MRN;
            this.MSN=_MSN;
            this.HSN=_HSN; 
            this.SESION=_SESION; 
            this.FECHA=_FECHA;

            this.FACTURA=_FACTURA; 
            this.CARGA=_CARGA; 
            this.AGENTE=_AGENTE; 
            this.FACTURADO=_FACTURADO; 
            this.PAGADO=_PAGADO; 
            this.GKEY=_GKEY; 
            this.REFERENCIA=_REFERENCIA;
            this.CONTENEDOR=_CONTENEDOR; 
            this.DOCUMENTO=_DOCUMENTO; 
            this.PRIMERA=_PRIMERA; 
            this.MARCA=_MARCA;
            this.CANTIDAD=_CANTIDAD; 
            this.CIATRANS=_CIATRANS; 
            this.CHOFER=_CHOFER; 
            this.ID_CIATRANS =_ID_CIATRANS;
            this.ID_CHOFER=_ID_CHOFER; 
            this.PLACA=_PLACA; 
            this.FECHA_SALIDA=_FECHA_SALIDA; 
            this.FECHA_SALIDA_PASE=_FECHA_SALIDA_PASE; 
            this.FECHA_AUT_PPWEB=_FECHA_AUT_PPWEB; 
            this.HORA_AUT_PPWEB=_HORA_AUT_PPWEB; 
            this.FECHA_ULT_FAC=_FECHA_ULT_FAC; 

            this.TIPO_CNTR=_TIPO_CNTR; 
            this.ID_TURNO=_ID_TURNO;
            this.TURNO=_TURNO; 
            this.D_TURNO=_D_TURNO; 
            this.ID_PASE=_ID_PASE; 
            this.ID_PPWEB=_ID_PPWEB; 
            this.ESTADO=_ESTADO; 
            this.ENVIADO=_ENVIADO; 
            this.AUTORIZADO=_AUTORIZADO;
            this.VENTANILLA=_VENTANILLA; 
            this.USUARIO_ING=_USUARIO_ING; 
            this.FECHA_ING=_FECHA_ING; 
            this.USUARIO_MOD=_USUARIO_MOD; 
            this.FECHA_MOD=_FECHA_MOD; 
            this.COMENTARIO=_COMENTARIO; 
            this.USUARIO_AUT=_USUARIO_AUT; 
            this.FECHA_AUT =_FECHA_AUT; 
            this.CAMBIO_FECHA =_CAMBIO_FECHA; 
            this.TURNO_DESDE=_TURNO_DESDE; 
            this.TURNO_HASTA=_TURNO_HASTA; 
            this.VISTO=_VISTO; 

            this.CNTR_DD=_CNTR_DD; 
            this.AGENTE_DESC=_AGENTE_DESC; 
            this.FACTURADO_DESC=_FACTURADO_DESC; 
            this.IMPORTADOR=_IMPORTADOR; 
            this.IMPORTADOR_DESC=_IMPORTADOR_DESC; 

            this.TRANSPORTISTA_DESC=_TRANSPORTISTA_DESC; 
            this.CHOFER_DESC=_CHOFER_DESC; 

            this.MOSTRAR_MENSAJE=_MOSTRAR_MENSAJE; 
            this.ESTADO_PAGO=_ESTADO_PAGO; 
            this.ORDENAMIENTO=_ORDENAMIENTO;
            this.ID_UNIDAD = _ID_UNIDAD;

            this.DetalleSubItem = new List<Cls_Bil_PasePuertaBRBK_SubItems>();
           this.Detalle = new List<Cls_Bil_PasePuertaBRBK_Detalle>();

        }
    }
}
