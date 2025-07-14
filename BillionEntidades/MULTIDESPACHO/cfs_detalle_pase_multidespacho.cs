using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;
using PasePuerta;


namespace BillionEntidades
{
    public class cfs_detalle_pase_multidespacho : Cls_Bil_Base
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
        private string _UBICACION = string.Empty;
        private bool _EXPRESS = false;
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
        private static String v_mensaje = string.Empty;

        public string UBICACION { get => _UBICACION; set => _UBICACION = value; }

        public bool EXPRESS { get => _EXPRESS; set => _EXPRESS = value; }

        public string DIRECCION { get; set; }
        public string CIUDAD { get ; set; }
        public string ZONA { get ; set ; }
        public Int64? ID_CIUDAD { get ; set; }
        public Int64? ID_ZONA { get ; set ; }
        #endregion


        public cfs_detalle_pase_multidespacho()
        {
            init();
          
        }



       
    }
}
