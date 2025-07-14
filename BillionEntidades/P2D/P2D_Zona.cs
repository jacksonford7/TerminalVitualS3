using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlConexion;

namespace BillionEntidades
{
    public class P2D_Zona : Cls_Bil_Base
    {
        #region "Variables"

        private Int64 _ID_ZONA = 0;
        private string _ZONA = string.Empty;


        #endregion

        #region "Propiedades"
        public Int64 ID_ZONA { get => _ID_ZONA; set => _ID_ZONA = value; }
        public string ZONA { get => _ZONA; set => _ZONA = value; }

        #endregion


        public P2D_Zona()
        {
            init();
        }

        public static List<P2D_Zona> CboZona(out string OnError)
        {
            parametros.Clear();
            return sql_puntero.ExecuteSelectControl<P2D_Zona>(sql_puntero.Conexion_Local, 6000, "P2D_CBO_ZONA", null, out OnError);
        }
    }
}
