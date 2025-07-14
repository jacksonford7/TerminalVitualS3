using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class P2D_Valida_Proforma : Cls_Bil_Base
    {
        #region "Variables"

        private Int64 _ID_PROFORMA;
        private DateTime _FECHA_EMISION;
        private string _NUMERO_CARGA = string.Empty;
        private string _ZONA = string.Empty;
        private string _CIUDAD = string.Empty;
        private string _DIRECCION = string.Empty;

        private int _CANT_BULTOS = 0;
        private decimal _TOTAL_M3 = 0;
        private decimal _TOTAL_TN = 0;
        private decimal _TOTAL_PAGAR = 0;
        private bool _ESTADO = false;

        private string _USUARIO_CREA = string.Empty;
        private string _USUARIO_MOD = string.Empty;

        private string _RUC = string.Empty;
        private string _MRN = string.Empty;
        private string _MSN = string.Empty;
        private string _HSN = string.Empty;

        private Int64 _gkey;
        private string _contenedor = string.Empty;
        private int _servicio;

        private decimal _CANT_CALCULAR = 0;

        private Int64? _ID_CIUDAD;
        private Int64? _ID_ZONA;

        private bool _APILABLE = false;
        private bool _EXPRESS = false;

        private decimal? _LATITUD;
        private decimal? _LONGITUD;

        #endregion

        #region "Propiedades"

        public Int64 ID_PROFORMA { get => _ID_PROFORMA; set => _ID_PROFORMA = value; }
        public DateTime FECHA_EMISION { get => _FECHA_EMISION; set => _FECHA_EMISION = value; }
        public string NUMERO_CARGA { get => _NUMERO_CARGA; set => _NUMERO_CARGA = value; }
        public string ZONA { get => _ZONA; set => _ZONA = value; }
        public string CIUDAD { get => _CIUDAD; set => _CIUDAD = value; }
        public string DIRECCION { get => _DIRECCION; set => _DIRECCION = value; }

        public int CANT_BULTOS { get => _CANT_BULTOS; set => _CANT_BULTOS = value; }

        public decimal TOTAL_M3 { get => _TOTAL_M3; set => _TOTAL_M3 = value; }
        public decimal TOTAL_TN { get => _TOTAL_TN; set => _TOTAL_TN = value; }
        public decimal TOTAL_PAGAR { get => _TOTAL_PAGAR; set => _TOTAL_PAGAR = value; }
        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }

        public string USUARIO_CREA { get => _USUARIO_CREA; set => _USUARIO_CREA = value; }
        public string USUARIO_MOD { get => _USUARIO_MOD; set => _USUARIO_MOD = value; }

        private static String v_mensaje = string.Empty;

        public string RUC { get => _RUC; set => _RUC = value; }
        public string MRN { get => _MRN; set => _MRN = value; }
        public string MSN { get => _MSN; set => _MSN = value; }
        public string HSN { get => _HSN; set => _HSN = value; }

        public Int64 gkey { get => _gkey; set => _gkey = value; }
        public string contenedor { get => _contenedor; set => _contenedor = value; }
        public int servicio { get => _servicio; set => _servicio = value; }

        public decimal CANT_CALCULAR { get => _CANT_CALCULAR; set => _CANT_CALCULAR = value; }

        public Int64? ID_CIUDAD { get => _ID_CIUDAD; set => _ID_CIUDAD = value; }
        public Int64? ID_ZONA { get => _ID_ZONA; set => _ID_ZONA = value; }

        public bool APILABLE { get => _APILABLE; set => _APILABLE = value; }

        public decimal? LATITUD { get => _LATITUD; set => _LATITUD = value; }

        public decimal? LONGITUD { get => _LONGITUD; set => _LONGITUD  = value; }

        public bool EXPRESS { get => _EXPRESS; set => _EXPRESS = value; }

        #endregion


        public P2D_Valida_Proforma()
        {
            init();
        }

        private static void OnInit(string Base)
        {
            sql_puntero = (sql_puntero == null) ? Cls_Conexion.Conexion() : sql_puntero;
            parametros = new Dictionary<string, object>();
            nueva_conexion = Cls_Conexion.Nueva_Conexion(Base);
        }

        /*poblar */
        public bool PopulateMyData(out string OnError)
        {
          
            parametros.Clear();
            parametros.Add("RUC", this.RUC);
            parametros.Add("MRN", this.MRN);
            parametros.Add("MSN", this.MSN);
            parametros.Add("HSN", this.HSN);

            var t = sql_puntero.ExecuteSelectOnly<P2D_Valida_Proforma>(sql_puntero.Conexion_Local, 6000, "P2D_VALIDA_EXISTE_PROFORMA", parametros);
            if (t == null)
            {
                OnError = "";
                return false;
            }
            else
            {
                OnError = string.Format("La carga ingresada {0}-{1}-{2}, ya se tiene una proforma activa # {3}, debe anular la misma si desea generar una nueva", this.MRN, this.MSN, this.HSN, t.ID_PROFORMA);
            }
            this.ID_PROFORMA = t.ID_PROFORMA;
            this.FECHA_EMISION = t.FECHA_EMISION;//LICENCIA
            this.NUMERO_CARGA = t.NUMERO_CARGA;
            this.ZONA = t.ZONA;
            this.CIUDAD = t.CIUDAD;
            this.DIRECCION = t.DIRECCION;
            this.CANT_BULTOS = t.CANT_BULTOS;
            this.TOTAL_TN = t.TOTAL_TN;
            this.TOTAL_M3 = t.TOTAL_M3;
            this.TOTAL_PAGAR = t.TOTAL_PAGAR;
            this.CANT_CALCULAR = t.CANT_CALCULAR;
            this.ID_CIUDAD = t.ID_CIUDAD;
            this.ID_ZONA = t.ID_ZONA;

            this.LATITUD = t.LATITUD;
            this.LONGITUD = t.LONGITUD;
            this.EXPRESS = t.EXPRESS;
            this.APILABLE = t.APILABLE;
            //OnError = string.Empty;
            return true;
        }

        public static List<P2D_Valida_Proforma> Validacion_Servicio_Transporte_Cfs(string XmlContenedor, out string OnError)
        {
            OnInit("N5");

            parametros.Clear();
            parametros.Add("XmlContenedor", XmlContenedor);
            return sql_puntero.ExecuteSelectControl<P2D_Valida_Proforma>(nueva_conexion, 6000, "P2D_VALIDA_SERVICIO_TRANSPORTE", parametros, out OnError);

        }

        public static List<P2D_Valida_Proforma> Validacion_Servicio_Transporte_Cfs_Express(string XmlContenedor, out string OnError)
        {
            OnInit("N5");

            parametros.Clear();
            parametros.Add("XmlContenedor", XmlContenedor);
            return sql_puntero.ExecuteSelectControl<P2D_Valida_Proforma>(nueva_conexion, 6000, "P2D_VALIDA_SERVICIO_TRANSPORTE_EXPRESS", parametros, out OnError);

        }

        public static List<P2D_Valida_Proforma> Validacion_Servicio_MultiDespacho(string XmlContenedor, out string OnError)
        {
            OnInit("N5");

            parametros.Clear();
            parametros.Add("XmlContenedor", XmlContenedor);
            return sql_puntero.ExecuteSelectControl<P2D_Valida_Proforma>(nueva_conexion, 6000, "[Bill].[Validacion_MultiDespacho_CFS]", parametros, out OnError);

        }

    }
}
