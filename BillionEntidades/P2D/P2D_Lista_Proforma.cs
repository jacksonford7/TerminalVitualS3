using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class P2D_Lista_Proforma : Cls_Bil_Base
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
        private decimal _CANT_CALCULAR = 0;
        private bool _ESTADO = false;

        private string _USUARIO_CREA = string.Empty;
        private string _USUARIO_MOD = string.Empty;


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
        public decimal CANT_CALCULAR { get => _CANT_CALCULAR; set => _CANT_CALCULAR = value; }

        public bool ESTADO { get => _ESTADO; set => _ESTADO = value; }

        public string USUARIO_CREA { get => _USUARIO_CREA; set => _USUARIO_CREA = value; }
        public string USUARIO_MOD { get => _USUARIO_MOD; set => _USUARIO_MOD = value; }

        private static String v_mensaje = string.Empty;

        #endregion


        public P2D_Lista_Proforma()
        {
            init();
        }

        public static List<P2D_Lista_Proforma> Listado_Cotizaciones(string RUC, DateTime FECHA_DESDE, DateTime FECHA_HASTA, out string OnError)
        {
            parametros.Clear();
            parametros.Add("RUC", RUC);
            parametros.Add("FECHA_DESDE", FECHA_DESDE);
            parametros.Add("FECHA_HASTA", FECHA_HASTA);

            return sql_puntero.ExecuteSelectControl<P2D_Lista_Proforma>(sql_puntero.Conexion_Local, 4000, "P2D_LISTA_PROFORMA", parametros, out OnError);

        }

    }
}
