using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class BTS_Tarifas : Cls_Bil_Base
    {

        #region "Variables"

        private Int64 _ID_SERVICIO;
        private string _CODIGO_TARIJA_N4 = string.Empty;
        private string _VALUE_TARIJA_N4 = string.Empty;
        private string _DESCRIP_TARIFA_N4 = string.Empty;
        private string _TARIFA = string.Empty;


        #endregion

        #region "Propiedades"

        public Int64 ID_SERVICIO { get => _ID_SERVICIO; set => _ID_SERVICIO = value; }
        public string CODIGO_TARIJA_N4 { get => _CODIGO_TARIJA_N4; set => _CODIGO_TARIJA_N4 = value; }
        public string VALUE_TARIJA_N4 { get => _VALUE_TARIJA_N4; set => _VALUE_TARIJA_N4 = value; }
        public string DESCRIP_TARIFA_N4 { get => _DESCRIP_TARIFA_N4; set => _DESCRIP_TARIFA_N4 = value; }
        public string TARIFA { get => _TARIFA; set => _TARIFA = value; }

        #endregion

        private static String v_mensaje = string.Empty;

        public BTS_Tarifas()
        {
            init();

        }

        public static List<BTS_Tarifas> Carga_Tarifas( out string OnError)
        {
        
            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<BTS_Tarifas>(sql_puntero.Conexion_Local, 6000, "BTS_COMBO_TARIFAS", null, out OnError);

        }


    

    }
}
