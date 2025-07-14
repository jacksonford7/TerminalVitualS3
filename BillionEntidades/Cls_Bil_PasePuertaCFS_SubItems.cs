using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Cls_Bil_PasePuertaCFS_SubItems : Cls_Bil_Base
    {
        #region "Variables"
 
        private string _MRN = string.Empty;
        private string _MSN = string.Empty;
        private string _HSN = string.Empty;
        private string _CARGA = string.Empty;       
        private Int64? _CONSECUTIVO;      
        private string _CIATRANS = string.Empty;
        private string _CHOFER = string.Empty;
        private string _ID_CIATRANS = string.Empty;
        private string _ID_CHOFER = string.Empty;
        private string _PLACA = string.Empty;
        private string _TRANSPORTISTA_DESC = string.Empty;
        private string _CHOFER_DESC = string.Empty;
        private bool _VISTO;
        private int _CANTIDAD = 0;
        private string _ESTADO_PAGO = string.Empty;
        private string _MARCADO_SUBITEMS = string.Empty;

        private decimal? _P2D_ALTO = 0;
        private decimal? _P2D_ANCHO = 0;
        private decimal? _P2D_LARGO = 0;
        private decimal? _P2D_VOLUMEN = 0;
        private decimal? _PESO = 0;
        private string _IMO = string.Empty;
        private string _NUMERO_CERTIFICADO = string.Empty;
        #endregion

        #region "Propiedades"

        public string MRN { get => _MRN; set => _MRN = value; }
        public string MSN { get => _MSN; set => _MSN = value; }
        public string HSN { get => _HSN; set => _HSN = value; }      
        public string CARGA { get => _CARGA; set => _CARGA = value; }
        public Int64? CONSECUTIVO { get => _CONSECUTIVO; set => _CONSECUTIVO = value; }
        public string CIATRANS { get => _CIATRANS; set => _CIATRANS = value; }
        public string CHOFER { get => _CHOFER; set => _CHOFER = value; }
        public string ID_CIATRANS { get => _ID_CIATRANS; set => _ID_CIATRANS = value; }
        public string ID_CHOFER { get => _ID_CHOFER; set => _ID_CHOFER = value; }
        public string PLACA { get => _PLACA; set => _PLACA = value; }
        public string ESTADO_PAGO { get => _ESTADO_PAGO; set => _ESTADO_PAGO = value; }
        public bool VISTO { get => _VISTO; set => _VISTO = value; }
        public int CANTIDAD { get => _CANTIDAD; set => _CANTIDAD = value; }
        
        public string TRANSPORTISTA_DESC { get => _TRANSPORTISTA_DESC; set => _TRANSPORTISTA_DESC = value; }
        public string CHOFER_DESC { get => _CHOFER_DESC; set => _CHOFER_DESC = value; }
        public string MARCADO_SUBITEMS { get => _MARCADO_SUBITEMS; set => _MARCADO_SUBITEMS = value; }

        public decimal? P2D_ALTO { get => _P2D_ALTO; set => _P2D_ALTO = value; }
        public decimal? P2D_ANCHO { get => _P2D_ANCHO; set => _P2D_ANCHO = value; }
        public decimal? P2D_LARGO { get => _P2D_LARGO; set => _P2D_LARGO = value; }
        public decimal? P2D_VOLUMEN { get => _P2D_VOLUMEN; set => _P2D_VOLUMEN = value; }
        public decimal? PESO { get => _PESO; set => _PESO = value; }
        public string IMO { get => _IMO; set => _IMO = value; }
        public string NUMERO_CERTIFICADO { get => _NUMERO_CERTIFICADO; set => _NUMERO_CERTIFICADO = value; }
        #endregion


        public Cls_Bil_PasePuertaCFS_SubItems()
        {
            init();
        }

        private static void OnInit()
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion("PORTAL_BILLION");
        }


        public Cls_Bil_PasePuertaCFS_SubItems( string _MRN , string _MSN,string _HSN, string _CARGA, Int64? _CONSECUTIVO,string _CIATRANS,
         string _CHOFER , string _ID_CIATRANS,string _ID_CHOFER , string _PLACA , string _TRANSPORTISTA_DESC ,string _CHOFER_DESC,bool _VISTO,int _CANTIDAD, string _MARCADO_SUBITEMS,
            decimal? _P2D_ALTO,
         decimal? _P2D_ANCHO,
         decimal? _P2D_LARGO ,
         decimal? _P2D_VOLUMEN ,
         decimal? _PESO,
          string _IMO
         )
        {

            this.MRN = _MRN;
            this.MRN = _MRN;
            this.MSN = _MSN;
            this.HSN = _HSN;      
            this.CARGA = _CARGA;
            this.CONSECUTIVO = _CONSECUTIVO;         
            this.CIATRANS = _CIATRANS;
            this.CHOFER = _CHOFER;
            this.PLACA = _PLACA;
            this.TRANSPORTISTA_DESC = _TRANSPORTISTA_DESC;
            this.CHOFER_DESC = _CHOFER_DESC;
            this.VISTO = _VISTO;
            this.CANTIDAD = _CANTIDAD;
            this.MARCADO_SUBITEMS = _MARCADO_SUBITEMS;

            this.P2D_ALTO = _P2D_ALTO;
            this.P2D_ANCHO = _P2D_ANCHO;
            this.P2D_LARGO = _P2D_LARGO;
            this.P2D_VOLUMEN = _P2D_VOLUMEN;
            this.PESO = _PESO;
            this.IMO = _IMO;
        }


      

    }
}
