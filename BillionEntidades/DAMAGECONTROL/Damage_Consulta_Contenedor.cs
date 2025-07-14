using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class Damage_Consulta_Contenedor : Cls_Bil_Base
    {

        #region "Variables"
        private static Int64? lm = -3;

        private string _CraneName = string.Empty;
        private string _Year = string.Empty;
        private string _Month = string.Empty;
        private string _Day = string.Empty;
        private string _Id = string.Empty;
        private string _Unit = string.Empty;
        private string _View = string.Empty;
        private string _Visit = string.Empty;
        private string _Truck = string.Empty;
        private string _OCRUnit = string.Empty;
        private string _Url = string.Empty;
        private Int64 _Gkey;
        private string _Category = string.Empty;
        private string _Freight_kind = string.Empty;
        private DateTime? _create_date = null;
        private string _Url_Large = string.Empty;


        private Int64 _CNTR_CONSECUTIVO;
        private string _CNTR_CONTAINER = string.Empty;
        private string _CNTR_TYPE = string.Empty;
        private string _CNTR_TYSZ_SIZE = string.Empty;
        private string _CNTR_TYSZ_ISO = string.Empty;
        private string _CNTR_TYSZ_TYPE = string.Empty;
        private string _CNTR_FULL_EMPTY_CODE = string.Empty;
        private string _CNTR_LUFE_STATUS = string.Empty;

        private decimal? _CNTR_GROSS_WEIGHT;
        private decimal? _CNTR_GROSS_WEIGHT_ORIGINAL;
        private decimal? _CNTR_NET_WEIGHT;

        private string _CNTR_SEAL_1 = string.Empty;
        private string _CNTR_SEAL_2 = string.Empty;
        private string _CNTR_SEAL_3 = string.Empty;
        private string _CNTR_SEAL_4 = string.Empty;
        private string _CNTR_TYPE_DOCUMENT = string.Empty;
        private string _CNTR_DOCUMENT = string.Empty;
        private string _CNTR_VEPR_REFERENCE = string.Empty;
        private string _CNTR_CLNT_CUSTOMER_LINE = string.Empty;
        private decimal? _CNTR_TARE;
        private string _CNTR_LCL_FCL = string.Empty;
        private string _CNTR_CATY_CARGO_TYPE = string.Empty;
        private string _CNTR_FREIGHT_KIND = string.Empty;
        private bool? _CNTR_DD;

        private DateTime? _FECHA_CAS = null;
        private string _cntr_bl = string.Empty;
        private string _cntr_aisv = string.Empty;

        private bool _TIENE_SERVICIO;
        private decimal? _DESC_PORCENTAJE;
        private string _MENSAJE = string.Empty;
        private string _Ruta_Fisica = string.Empty;

        #endregion

        #region "Propiedades"
        public string CraneName { get => _CraneName; set => _CraneName = value; }
        public string Year { get => _Year; set => _Year = value; }
        public string Month { get => _Month; set => _Month = value; }
        public string Day { get => _Day; set => _Day = value; }
        public string Id { get => _Id; set => _Id = value; }
        public string Unit { get => _Unit; set => _Unit = value; }
        public string View { get => _View; set => _View = value; }
        public string Visit { get => _Visit; set => _Visit = value; }
        public string Truck { get => _Truck; set => _Truck = value; }
        public string OCRUnit { get => _OCRUnit; set => _OCRUnit = value; }
        public string Url { get => _Url; set => _Url = value; }
        public string Url_Large { get => _Url_Large; set => _Url_Large = value; }

        public Int64 Gkey { get => _Gkey; set => _Gkey = value; }
        public string Category { get => _Category; set => _Category = value; }
        public string Freight_kind { get => _Freight_kind; set => _Freight_kind = value; }
        public DateTime? create_date { get => _create_date; set => _create_date = value; }
        public Int64 CNTR_CONSECUTIVO { get => _CNTR_CONSECUTIVO; set => _CNTR_CONSECUTIVO = value; }
        public string CNTR_CONTAINER { get => _CNTR_CONTAINER; set => _CNTR_CONTAINER = value; }
        public string CNTR_TYPE { get => _CNTR_TYPE; set => _CNTR_TYPE = value; }
        public string CNTR_TYSZ_SIZE { get => _CNTR_TYSZ_SIZE; set => _CNTR_TYSZ_SIZE = value; }
        public string CNTR_TYSZ_ISO { get => _CNTR_TYSZ_ISO; set => _CNTR_TYSZ_ISO = value; }
        public string CNTR_TYSZ_TYPE { get => _CNTR_TYSZ_TYPE; set => _CNTR_TYSZ_TYPE = value; }

        public string CNTR_FULL_EMPTY_CODE { get => _CNTR_FULL_EMPTY_CODE; set => _CNTR_FULL_EMPTY_CODE = value; }
        public string CNTR_LUFE_STATUS { get => _CNTR_LUFE_STATUS; set => _CNTR_LUFE_STATUS = value; }
        public decimal? CNTR_GROSS_WEIGHT { get => _CNTR_GROSS_WEIGHT; set => _CNTR_GROSS_WEIGHT = value; }
        public decimal? CNTR_GROSS_WEIGHT_ORIGINAL { get => _CNTR_GROSS_WEIGHT_ORIGINAL; set => _CNTR_GROSS_WEIGHT_ORIGINAL = value; }
        public decimal? CNTR_NET_WEIGHT { get => _CNTR_NET_WEIGHT; set => _CNTR_NET_WEIGHT = value; }

        public string CNTR_SEAL_1 { get => _CNTR_SEAL_1; set => _CNTR_SEAL_1 = value; }
        public string CNTR_SEAL_2 { get => _CNTR_SEAL_2; set => _CNTR_SEAL_2 = value; }
        public string CNTR_SEAL_3 { get => _CNTR_SEAL_3; set => _CNTR_SEAL_3 = value; }
        public string CNTR_SEAL_4 { get => _CNTR_SEAL_4; set => _CNTR_SEAL_4 = value; }

        public string CNTR_TYPE_DOCUMENT { get => _CNTR_TYPE_DOCUMENT; set => _CNTR_TYPE_DOCUMENT = value; }
        public string CNTR_DOCUMENT { get => _CNTR_DOCUMENT; set => _CNTR_DOCUMENT = value; }
        public string CNTR_VEPR_REFERENCE { get => _CNTR_VEPR_REFERENCE; set => _CNTR_VEPR_REFERENCE = value; }
        public string CNTR_CLNT_CUSTOMER_LINE { get => _CNTR_CLNT_CUSTOMER_LINE; set => _CNTR_CLNT_CUSTOMER_LINE = value; }

        public decimal? CNTR_TARE { get => _CNTR_TARE; set => _CNTR_TARE = value; }

        public string CNTR_LCL_FCL { get => _CNTR_LCL_FCL; set => _CNTR_LCL_FCL = value; }
        public string CNTR_CATY_CARGO_TYPE { get => _CNTR_CATY_CARGO_TYPE; set => _CNTR_CATY_CARGO_TYPE = value; }
        public string CNTR_FREIGHT_KIND { get => _CNTR_FREIGHT_KIND; set => _CNTR_FREIGHT_KIND = value; }

        public bool? CNTR_DD { get => _CNTR_DD; set => _CNTR_DD = value; }

        public DateTime? FECHA_CAS { get => _FECHA_CAS; set => _FECHA_CAS = value; }

        public string cntr_bl { get => _cntr_bl; set => _cntr_bl = value; }

        public string cntr_aisv { get => _cntr_aisv; set => _cntr_aisv = value; }

        public bool TIENE_SERVICIO { get => _TIENE_SERVICIO; set => _TIENE_SERVICIO = value; }
        public decimal? DESC_PORCENTAJE { get => _DESC_PORCENTAJE; set => _DESC_PORCENTAJE = value; }
        public string MENSAJE { get => _MENSAJE; set => _MENSAJE = value; }

        public string Ruta_Fisica { get => _Ruta_Fisica; set => _Ruta_Fisica = value; }

        private static String v_mensaje = string.Empty;


        #endregion

        public List<Damage_Resumen_Contenedor> Contenedores { get; set; }
        public List<Damage_Detalle_Contenedor> Detalle_Contenedores { get; set; }
        public Damage_Consulta_Contenedor()
        {
            init();

            this.Contenedores = new List<Damage_Resumen_Contenedor>();
            this.Detalle_Contenedores = new List<Damage_Detalle_Contenedor>();
        }



        #region "Listado de fotos"
       
        public static List<Damage_Consulta_Contenedor> Consulta_Contenedor(string CONTENEDOR,  string LINEA, out string OnError)
        {
            parametros.Clear();
            parametros.Add("CONTENEDOR", CONTENEDOR);
            parametros.Add("LINEA", LINEA);
            return sql_puntero.ExecuteSelectControl<Damage_Consulta_Contenedor>(sql_puntero.Conexion_Local, 6000, "DAMAGE_INFORMACION_CONTENEDOR", parametros, out OnError);

        }
        #endregion


        #region "Listado de fotos importador"

        public static List<Damage_Consulta_Contenedor> Consulta_Contenedor_Importador(string CONTENEDOR, Int64 GKEY, out string OnError)
        {
            parametros.Clear();
            parametros.Add("CONTENEDOR", CONTENEDOR);
            parametros.Add("GKEY", GKEY);
            return sql_puntero.ExecuteSelectControl<Damage_Consulta_Contenedor>(sql_puntero.Conexion_Local, 6000, "DAMAGE_INFORMACION_CONTENEDOR_IMPORTADOR", parametros, out OnError);

        }
        #endregion

    }
}
