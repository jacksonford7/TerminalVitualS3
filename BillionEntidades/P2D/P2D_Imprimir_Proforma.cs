using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;


namespace BillionEntidades
{
   public class P2D_Imprimir_Proforma : Cls_Bil_Base
    {
        #region "Variables"
        
        private string _ID_PROFORMA;
        private string _FECHA ;
        private string _RUC_USUARIO = string.Empty;
        private string _NOMBRES = string.Empty;
        private string _APELLIDOS = string.Empty;
        private string _MRN = string.Empty;
        private string _MSN = string.Empty;
        private string _HSN = string.Empty;
        private int _CANT_BULTOS = 0;
        private Int64? _ID_CIUDAD;
        private string _DESC_CIUDAD = string.Empty;
        private Int64? _ID_ZONA;
        private string _ZONA = string.Empty;
        private Int64? _ID_TARIFA;
        private int? _ID_TARIFA_SECUEN;
        private string _DIR_ENTREGA = string.Empty;
        private decimal _TOTAL_M3 = 0;
        private decimal _TOTAL_TN = 0;
        private decimal _TOTAL_PAGAR = 0;
        private bool _ESTADO = false;
        private decimal _CANT_CALCULAR = 0;
        private string _USUARIO_CREA = string.Empty;
        private string _USUARIO_MOD = string.Empty;
        private bool _EXPRESS = false;

        #endregion

        #region "Propiedades"

        public string ID_PROFORMA { get => _ID_PROFORMA; set => _ID_PROFORMA = value; }
        public string FECHA { get => _FECHA; set => _FECHA = value; }
        public string RUC_USUARIO { get => _RUC_USUARIO; set => _RUC_USUARIO = value; }
        public string NOMBRES { get => _NOMBRES; set => _NOMBRES = value; }
        public string APELLIDOS { get => _APELLIDOS; set => _APELLIDOS = value; }
        public string MRN { get => _MRN; set => _MRN = value; }
        public string MSN { get => _MSN; set => _MSN = value; }
        public string HSN { get => _HSN; set => _HSN = value; }
        public int CANT_BULTOS { get => _CANT_BULTOS; set => _CANT_BULTOS = value; }
        public Int64? ID_CIUDAD { get => _ID_CIUDAD; set => _ID_CIUDAD = value; }
        public string DESC_CIUDAD { get => _DESC_CIUDAD; set => _DESC_CIUDAD = value; }
        public Int64? ID_ZONA { get => _ID_ZONA; set => _ID_ZONA = value; }
        public string ZONA { get => _ZONA; set => _ZONA = value; }
        public Int64? ID_TARIFA { get => _ID_TARIFA; set => _ID_TARIFA = value; }
        public int? ID_TARIFA_SECUEN { get => _ID_TARIFA_SECUEN; set => _ID_TARIFA_SECUEN = value; }

        public string DIR_ENTREGA { get => _DIR_ENTREGA; set => _DIR_ENTREGA = value; }


        public decimal TOTAL_M3 { get => _TOTAL_M3; set => _TOTAL_M3 = value; }
        public decimal TOTAL_TN { get => _TOTAL_TN; set => _TOTAL_TN = value; }
        public decimal TOTAL_PAGAR { get => _TOTAL_PAGAR; set => _TOTAL_PAGAR = value; }
        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }
        public decimal CANT_CALCULAR { get => _CANT_CALCULAR; set => _CANT_CALCULAR = value; }

        public string USUARIO_CREA { get => _USUARIO_CREA; set => _USUARIO_CREA = value; }
        public string USUARIO_MOD { get => _USUARIO_MOD; set => _USUARIO_MOD = value; }
        public bool EXPRESS { get => _EXPRESS; set => _EXPRESS = value; }

        private static String v_mensaje = string.Empty;

      
        #endregion


        public P2D_Imprimir_Proforma()
        {
            init();

           

        }

        /*carga todas las proforma*/
        public static List<P2D_Imprimir_Proforma> Datos_Proforma(Int64 ID_PROFORMA, out string OnError)
        {
            parametros.Clear();
            parametros.Add("ID_PROFORMA", ID_PROFORMA);
            return sql_puntero.ExecuteSelectControl<P2D_Imprimir_Proforma>(sql_puntero.Conexion_Local, 6000, "P2D_RPT_IMPRIMIR_PROFORMA", parametros, out OnError);

        }

        /*carga todas las proforma*/
        public static List<P2D_Imprimir_Proforma> Datos_Simulador(Int64 ID_PROFORMA, out string OnError)
        {
            parametros.Clear();
            parametros.Add("ID_PROFORMA", ID_PROFORMA);
            return sql_puntero.ExecuteSelectControl<P2D_Imprimir_Proforma>(sql_puntero.Conexion_Local, 6000, "P2D_RPT_IMPRIMIR_SIMULADOR", parametros, out OnError);

        }

    }
}
